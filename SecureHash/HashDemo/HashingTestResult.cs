namespace HashDemo;

public class HashingTestResult
{
    public string AlgorithmName { get; set; }
    public TimeSpan HashingTime { get; set; }
    public TimeSpan VerificationTime { get; set; }
    public bool IsVerified { get; set; }
}