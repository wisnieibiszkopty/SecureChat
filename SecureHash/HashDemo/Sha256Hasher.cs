using System.Security.Cryptography;
using System.Text;

namespace HashDemo;

public class Sha256Hasher
{
    // TODO refactor so it implements IPasswordHasher
    
    public string Hash(string password, string salt, string pepper)
    {
        using var sha = SHA256.Create();
        var combined = password + salt + pepper;
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
    
    public bool Verify(string password, string storedHash, string salt, string pepper)
    {
        var hashToVerify = Hash(password, salt, pepper);
        return storedHash.Equals(hashToVerify, StringComparison.OrdinalIgnoreCase);
    }
}