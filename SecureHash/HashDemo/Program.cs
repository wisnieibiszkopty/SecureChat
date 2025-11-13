// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using ConsoleTables;
using HashDemo;
using MFAWebApp.Services.Authentication;
using Microsoft.Extensions.Configuration;

var pepper = PepperAndSaltGenerator.GeneratePepper();
var salt = PepperAndSaltGenerator.GenerateSalt();

var memoryConfig = new Dictionary<string, string>
{
    { "Security:PasswordPepper", pepper },
    { "Security:PasswordSalt", salt }
};

var inMemoryConfig = new ConfigurationBuilder()
    .AddInMemoryCollection(memoryConfig)
    .Build();

string retrievedPepper = inMemoryConfig["Security:PasswordPepper"];
string retrievedSalt = inMemoryConfig["Security:PasswordSalt"];
Console.WriteLine($"Retrieved Pepper: {retrievedPepper}");
Console.WriteLine($"Retrieved Pepper: {retrievedSalt}");

var stopwatch = new Stopwatch();

var password = "Password123Test";

var sha256Hasher = new Sha256Hasher(inMemoryConfig);
var bcryptHasher = new PasswordHasherBcrypt(inMemoryConfig);
var scryptHasher = new PasswordHasherScrypt(inMemoryConfig);

var testsResults = new List<HashingTestResult>
{
    PerformanceTest(sha256Hasher, "SHA256"),
    PerformanceTest(bcryptHasher, "Bcrypt"),
    PerformanceTest(scryptHasher, "Scrypt")
};

PrintResultsAsTable(testsResults);
// Time in ms
// + ------------- + ----------- + ---------------- + ---------- +
// | AlgorithmName | HashingTime | VerificationTime | IsVerified |
// + ------------- + ----------- + ---------------- + ---------- +
// | SHA256        | 00.002711   | 00.0002879       | True       |
// + ------------- + ----------- + ---------------- + ---------- +
// | Bcrypt        | 00.585103   | 00.4764247       | True       |
// + ------------- + ----------- + ---------------- + ---------- +
// | Scrypt        | 00.252355   | 00.0786659       | True       |
// + ------------- + ----------- + ---------------- + ---------- +


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
        HashingTime = hashingTime.ToString(@"ss\.ffffff"),
        VerificationTime = verificationTime.ToString(@"ss\.fffffff"),
        IsVerified = isVerified
    };
}

void PrintResultsAsTable(List<HashingTestResult> rows)
{
    ConsoleTable.From(rows)
        .Write(Format.Alternative);
}