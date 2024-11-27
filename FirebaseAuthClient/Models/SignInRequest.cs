using System.Text.Json.Serialization;

namespace FirebaseAuthClient.Models;

public sealed record SignInRequest(
    [property: JsonPropertyName("returnSecureToken")] bool ReturnSecureToken,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("password")] string Password,
    [property: JsonPropertyName("clientType")] string ClientType
);