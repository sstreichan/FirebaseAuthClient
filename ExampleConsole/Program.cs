using FirebaseAuthClient;

public class Program
{
    static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
    public static async Task MainAsync(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: ExampleConsole.exe <api-key> <email> <password>");
            Console.WriteLine("  api-key    : Firebase API key");
            Console.WriteLine("  email      : Email to sign in with");
            Console.WriteLine("  password   : Password to sign in with");
            Console.WriteLine("  Example    : ExampleConsole.exe NVmnFlPF_XgJVMvQ59OuAvdsfSH8pfAOC_cp0dD user@gmail.com password");
            return;
        }

        var apiKey = args[0];
        var email = args[1];
        var password = args[2];

        var client = new FirebaseAuthenticationClient(apiKey);

        try
        {
            // Sign In
            var signInResponse = await client.SignInWithPasswordAsync(
                email,
                password
            );
            Console.WriteLine($"signIn successful IdToken: {signInResponse.IdToken}");
            // Lookup User
            var lookupResponse = await client.LookupUserAsync(signInResponse.IdToken);
            Console.WriteLine($"lookupResponse Email: {lookupResponse.Users[0].Email}");
            // Refresh Token
            var refreshResponse = await client.RefreshTokenAsync(signInResponse.RefreshToken);
            Console.WriteLine($"refreshResponse AccessToken: {refreshResponse.AccessToken}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
}