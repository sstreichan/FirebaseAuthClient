using System.Text.Json.Serialization;

namespace FirebaseAuthClient.Models;

public sealed record UserLookupRequest(
    [property: JsonPropertyName("idToken")] string IdToken
);