namespace MFAWebApp.Services.Authentication;

public class PasswordHasherBcrypt(IConfiguration config, int workFactor = 12) : IPasswordHasher
{
    private readonly string _pepper =
        config["Security:PasswordPepper"]
        ?? throw new InvalidOperationException("Missing PasswordPepper in configuration");
    
    private readonly int _workFactor = workFactor;
    
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: _workFactor);
    }

    public bool Verify(string password, string storedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedPassword);
    }
}