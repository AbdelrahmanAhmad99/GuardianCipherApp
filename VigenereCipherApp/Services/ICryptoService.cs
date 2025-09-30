namespace VigenereCipherApp.Services
{
    public interface ICryptoService
    {
        string Encrypt(string plaintext, string key);
        string Decrypt(string ciphertext, string key);
    }
}
