// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using HashDemo;
using MFAWebApp.Services.Authentication;
using Microsoft.Extensions.Configuration;

var pepper = PepperAndSaltGenerator.GeneratePepper();
var salt = PepperAndSaltGenerator.GenerateSalt();

Console.WriteLine($"Pepper: {pepper}");
Console.WriteLine($"Salt: {salt}");

var memoryConfig = new Dictionary<string, string>
{
    { "Security:PasswordPepper", pepper }
};

var inMemoryConfig = new ConfigurationBuilder()
    .AddInMemoryCollection(memoryConfig)
    .Build();

string retrievedPepper = inMemoryConfig["Security:PasswordPepper"];
Console.WriteLine($"Retrieved Pepper: {retrievedPepper}");

var stopwatch = new Stopwatch();

var password = "Password123Test";

var bcryptHasher = new PasswordHasherBcrypt(inMemoryConfig);
var scryptHasher = new PasswordHasherScrypt(inMemoryConfig);

// TODO refactor Sha256
var sha256TestResult = Sha256Test();
var bcryptTestResult = PerformanceTest(bcryptHasher, "Bcrypt");
var scryptTestResult = PerformanceTest(scryptHasher, "Scrypt");

var testsResults = new List<HashingTestResult>
{
    sha256TestResult,
    bcryptTestResult,
    scryptTestResult
};

// TODO print result and add comment
Console.WriteLine($"Hashing tests: {testsResults.Count}");
Console.WriteLine(bcryptTestResult.HashingTime);

HashingTestResult PerformanceTest(IPasswordHasher hasher, string algorithmName)
{
    stopwatch.Restart();
    stopwatch.Start();
    
    var hash = hasher.Hash(password);
    
    stopwatch.Stop();
    var hashingTime = stopwatch.Elapsed;
    
    stopwatch.Restart();
    stopwatch.Start();
    
    var isVerified = hasher.Verify(password, hash);
    
    stopwatch.Stop();
    var verificationTime = stopwatch.Elapsed;

    return new HashingTestResult
    {
        AlgorithmName = algorithmName,
        HashingTime = hashingTime,
        VerificationTime = verificationTime,
        IsVerified = isVerified
    };
}

HashingTestResult Sha256Test()
{
    var hasher = new Sha256Hasher();
 
    stopwatch.Restart();
    stopwatch.Start();
    
    var hash = hasher.Hash(password, salt, pepper);
    
    stopwatch.Stop();
    var sha256HashingTime = stopwatch.Elapsed;

    Console.WriteLine($"Hashed: {hash}");

    stopwatch.Restart();
    stopwatch.Start();
    
    var isVerified = hasher.Verify(password, hash, salt, pepper);

    stopwatch.Stop();
    var sha256VerifyingTime = stopwatch.Elapsed;
    
    Console.WriteLine($"Is verified: {isVerified}");

    return new HashingTestResult
    {
        AlgorithmName = "SHA256",
        HashingTime = sha256HashingTime,
        VerificationTime = sha256VerifyingTime,
        IsVerified = isVerified,
    };
}

void BcryptTest()
{
    var bcryptHasher = new PasswordHasherBcrypt(inMemoryConfig);
    var bcryptHash = bcryptHasher.Hash(password);

    Console.WriteLine($"Hashed: {bcryptHash}");

    var isVerifiedBcrypt = bcryptHasher.Verify(password, bcryptHash);

    Console.WriteLine($"Is verified: {isVerifiedBcrypt}");
    
}

void ScryptTest()
{
    var scryptHasher = new PasswordHasherScrypt(inMemoryConfig);
    var scryptHash = scryptHasher.Hash(password);

    Console.WriteLine($"Hashed: {scryptHash}");

    var isVerifiedScrypt = scryptHasher.Verify(password, scryptHash);

    Console.WriteLine($"Is verified: {isVerifiedScrypt}");
}