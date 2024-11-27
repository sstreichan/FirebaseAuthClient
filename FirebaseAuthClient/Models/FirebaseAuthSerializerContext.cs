using FirebaseAuthClient.Models;
using System.Text.Json.Serialization;

namespace FirebaseAuthClient;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(SignInRequest))]
[JsonSerializable(typeof(UserLookupRequest))]
[JsonSerializable(typeof(SignInResponse))]
[JsonSerializable(typeof(UserLookupResponse))]
[JsonSerializable(typeof(UserDetails))]
[JsonSerializable(typeof(TokenRefreshResponse))]
public partial class FirebaseAuthSerializerContext : JsonSerializerContext
{
}