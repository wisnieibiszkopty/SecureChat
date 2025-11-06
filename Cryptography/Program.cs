using System.Text;
using Cryptography.Crypto;
using Cryptography.PKI.Services;

ICryptoService cryptoService = new CryptoService();

string message = "Hello Bob!";

//SYMMETRIC CRYPTOGRAPHY
using var key = cryptoService.GenerateSymmetricKey();

var encryptedSymmetric = cryptoService.EncryptSymmetric(message, key);
var decryptedSymmetric = cryptoService.DecryptSymmetric(encryptedSymmetric, key);

WriteTitle("Szyfrowanie symetryczne");

Console.WriteLine("Tekst do zaszyfrowania: " + message);
Console.WriteLine("Zaszyfrowane dane (base64): " + Convert.ToBase64String(encryptedSymmetric));
Console.WriteLine("Odszyfrowane dane: " + decryptedSymmetric);

//ASYMMETRIC CRYPTOGRAPHY
using var aliceKeyPair = cryptoService.GenerateAsymmetricKeyPair();
using var bobKeyPair = cryptoService.GenerateAsymmetricKeyPair();

string messageToBob = "Message to Bob";
string messageToAlice = "Message to Alice";

var encryptedAsymmetricToBob = cryptoService
    .EncryptAsymmetric(messageToBob, bobKeyPair.PublicKey, aliceKeyPair.PrivateKey);
var decryptedAsymmetricToBob = cryptoService
    .DecryptAsymmetric(encryptedAsymmetricToBob, aliceKeyPair.PublicKey, bobKeyPair.PrivateKey);

WriteTitle("Szyfrowanie Asymetryczne");

Console.WriteLine($"Tekst do zaszyfrowania: {messageToBob}");
Console.WriteLine($"Zaszyfrowane dane (base64): {Convert.ToBase64String(encryptedAsymmetricToBob)}");
Console.WriteLine($"Odszyfrowane dane: {decryptedAsymmetricToBob}");

var encryptedAsymmetricToAlice = cryptoService
    .EncryptAsymmetric(messageToAlice, aliceKeyPair.PublicKey, bobKeyPair.PrivateKey);
var decryptedAsymmetricToAlice = cryptoService
    .DecryptAsymmetric(encryptedAsymmetricToAlice, bobKeyPair.PublicKey, aliceKeyPair.PrivateKey);
    
Console.WriteLine($"Tekst do zaszyfrowania: {messageToAlice}");
Console.WriteLine($"Zaszyfrowane dane (base64): {Convert.ToBase64String(encryptedAsymmetricToAlice)}");
Console.WriteLine($"Odszyfrowane dane: {decryptedAsymmetricToAlice}");

//HYBRID CRYPTOGRAPHY
string filepath = "Crypto/Resources/message.txt";
Console.WriteLine("Ścieżka pliku do zaszyfrowania: " + filepath);

var encryptedFilePath = cryptoService.EncryptFileSecretStream(filepath, bobKeyPair.PublicKey, aliceKeyPair.PrivateKey);
Console.WriteLine("Ścieżka zaszyfrowanego pliku: " + encryptedFilePath);

var decryptedFilePath = cryptoService.DecryptFileSecretStream(encryptedFilePath, aliceKeyPair.PublicKey, bobKeyPair.PrivateKey);
Console.WriteLine("Ścieżka odszyfrowanego pliku: " + decryptedFilePath);

// PKI SIGNATURE
WriteTitle("PKI Signature");

var pkiService = new PKIService();
using var keyPair = pkiService.GenerateSigningKeyPair();

var pkiMessage = "Test PKI message";
var messageBytes = Encoding.UTF8.GetBytes(pkiMessage);

Console.WriteLine($"Message: {pkiMessage}");

var signature = pkiService.SignData(messageBytes, keyPair.PrivateKey);

Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

var isValid = pkiService.VerifySignature(messageBytes, signature, keyPair.PublicKey);

Console.WriteLine($"Is signature valid? {isValid}");

void WriteTitle(string text)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"=== {text} ===");
    Console.ForegroundColor = ConsoleColor.White;
}