using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Net.Http.Json;
using FirebaseAuthClient.Models;

namespace FirebaseAuthClient;

public sealed class FirebaseAuthenticationClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the FirebaseAuthenticationClient class.
    /// </summary>
    /// <param name="apiKey">The API key to use for authentication.</param>
    /// <remarks>
    /// This constructor sets up the necessary dependencies for the client, including the HTTP client and JSON serializer options.
    /// </remarks>
    public FirebaseAuthenticationClient(string apiKey)
    {
        _httpClient = new();
        _apiKey = apiKey;
        _jsonOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = FirebaseAuthSerializerContext.Default
        };
    }
    /// <summary>
    /// Initializes a new instance of the FirebaseAuthenticationClient class with a custom HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance to use for authentication requests.</param>
    /// <param name="apiKey">The API key to use for authentication.</param>
    /// <remarks>
    /// This constructor sets up the necessary dependencies for the client, including the JSON serializer options.
    /// </remarks>
    public FirebaseAuthenticationClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _jsonOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = FirebaseAuthSerializerContext.Default
        };
    }

    /// <summary>
    /// Signs in a user with the provided email and password.
    /// </summary>
    /// <param name="email">The email address of the user to sign in.</param>
    /// <param name="password">The password of the user to sign in.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="SignInResponse"/> containing the result of the sign-in operation.</returns>
    public async ValueTask<SignInResponse> SignInWithPasswordAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        var request = new SignInRequest(
            ReturnSecureToken: true,
            Email: email,
            Password: password,
            ClientType: "CLIENT_TYPE_WEB"
        );

        var response = await _httpClient.PostAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}", 
            JsonContent.Create(request, FirebaseAuthSerializerContext.Default.SignInRequest), 
            cancellationToken
        );

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return JsonSerializer.Deserialize(stream, FirebaseAuthSerializerContext.Default.SignInResponse) 
            ?? throw new InvalidOperationException("Response deserialization failed");
    }

    /// <summary>
    /// Asynchronously looks up a user using their ID token.
    /// </summary>
    /// <param name="idToken">The ID token of the user to look up.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response from the server, which contains information about the looked-up user.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response from the server cannot be deserialized.</exception>
    public async ValueTask<UserLookupResponse> LookupUserAsync(
        string idToken,
        CancellationToken cancellationToken = default)
    {
        // Create a new UserLookupRequest with the provided ID token
        var request = new UserLookupRequest(IdToken: idToken);

        // Send a POST request to the Firebase Identity Toolkit API with the request object as JSON content
        var response = await _httpClient.PostAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={_apiKey}",
            JsonContent.Create(request, FirebaseAuthSerializerContext.Default.UserLookupRequest),
            cancellationToken
        );

        // Read the response content as a stream
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        // Deserialize the stream into a UserLookupResponse object
        return JsonSerializer.Deserialize(stream, FirebaseAuthSerializerContext.Default.UserLookupResponse)
               ?? throw new InvalidOperationException("Response deserialization failed");
    }

    /// <summary>
    /// Asynchronously refreshes a Firebase authentication token using a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for the token refresh.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="TokenRefreshResponse"/> containing the refreshed token.</returns>
    public async ValueTask<TokenRefreshResponse> RefreshTokenAsync(
        string refreshToken, 
        CancellationToken cancellationToken = default)
    {
        var content = new FormUrlEncodedContent(
        [
            new("grant_type", "refresh_token"),
            new("refresh_token", refreshToken)
        ]);

        var response = await _httpClient.PostAsync(
            $"https://securetoken.googleapis.com/v1/token?key={_apiKey}", 
            content, 
            cancellationToken
        );

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return JsonSerializer.Deserialize(stream, FirebaseAuthSerializerContext.Default.TokenRefreshResponse) 
            ?? throw new InvalidOperationException("Response deserialization failed");
    }
}