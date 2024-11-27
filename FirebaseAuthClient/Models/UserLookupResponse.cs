using System.Text.Json.Serialization;

namespace FirebaseAuthClient.Models;

public sealed record UserLookupResponse(
    [property: JsonPropertyName("users")] UserDetails[] Users
);