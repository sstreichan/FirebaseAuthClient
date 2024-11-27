using System.Text.Json.Serialization;

namespace FirebaseAuthClient.Models;

public sealed record SignInResponse(
    [property: JsonPropertyName("idToken")] string IdToken,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("refreshToken")] string RefreshToken,
    [property: JsonPropertyName("expiresIn")] string ExpiresIn,
    [property: JsonPropertyName("localId")] string LocalId
);