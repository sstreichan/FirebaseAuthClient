using System.Text.Json.Serialization;

namespace FirebaseAuthClient.Models;

public sealed record UserDetails(
    [property: JsonPropertyName("localId")] string LocalId,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("emailVerified")] bool EmailVerified
);