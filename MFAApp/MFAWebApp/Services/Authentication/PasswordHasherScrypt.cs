using Scrypt;

namespace MFAWebApp.Services.Authentication;

public class PasswordHasherScrypt : IPasswordHasher
{
    private ScryptEncoder _encoder;
    private string _pepper;

    public PasswordHasherScrypt(IConfiguration config)
    {
        _pepper = config["Security:PasswordPepper"] ??
                  throw new InvalidOperationException("Missing PasswordPepper in configuration");
        _encoder = new ScryptEncoder(16384, 8, 1);
    }
    
    public string Hash(string password)
    {
        return _encoder.Encode(password + _pepper);
    }

    public bool Verify(string password, string storedPassword)
    {
        return _encoder.Compare(password + _pepper, storedPassword);
    }
}