public interface IAsymmetricCryptoService
{
    string Encrypt(string plainText, string publicKey);
    string Decrypt(string cipherText, string privateKey);
    (string publicKey, string privateKey) GenerateKeys();
}
