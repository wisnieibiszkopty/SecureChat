using System.Security.Cryptography;

namespace HashDemo;

public static class PepperAndSaltGenerator
{
    public static string GeneratePepper(int size = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(bytes);
    }
    
    public static string GenerateSalt(int size = 16)
    {
        var bytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(bytes);
    }
}