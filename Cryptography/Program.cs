using Cryptography.Crypto;

ICryptoService cryptoService = new CryptoService();

string message = "Hello Bob!";

//SYMMETRIC CRYPTOGRAPHY
using var key = cryptoService.GenerateSymmetricKey();

var encryptedSymmetric = cryptoService.EncryptSymmetric(message, key);
var decryptedSymmetric = cryptoService.DecryptSymmetric(encryptedSymmetric, key);

Console.WriteLine("=== Szyfrowanie symetryczne ===");

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

Console.WriteLine("=== Szyfrowanie Asymetryczne ===");

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

//SZYFROWANIE HYBRYDOWE
string filepath = "Crypto/Resources/message.txt";
Console.WriteLine("Ścieżka pliku do zaszyfrowania: " + filepath);

var encryptedFilePath = cryptoService.EncryptFileSecretStream(filepath, bobKeyPair.PublicKey, aliceKeyPair.PrivateKey);
Console.WriteLine("Ścieżka zaszyfrowanego pliku: " + encryptedFilePath);

var decryptedFilePath = cryptoService.DecryptFileSecretStream(encryptedFilePath, aliceKeyPair.PublicKey, bobKeyPair.PrivateKey);
Console.WriteLine("Ścieżka odszyfrowanego pliku: " + decryptedFilePath);

