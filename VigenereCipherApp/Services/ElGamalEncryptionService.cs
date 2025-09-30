using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class ElGamalEncryptionService : IAsymmetricCryptoService
    {
        public (string publicKey, string privateKey) GenerateKeys()
        {
            // Use a large safe prime for secure real-world use
            BigInteger p = BigInteger.Parse("30803"); // Safe prime for demo
            BigInteger g = 2;

            var rng = RandomNumberGenerator.Create();
            byte[] privateBytes = new byte[4];
            rng.GetBytes(privateBytes);

            // Ensure non-negative private key (x) by prepending a 0-byte
            BigInteger x = new BigInteger(new byte[] { 0 }.Concat(privateBytes).ToArray());
            x = x % (p - 2) + 1; // Private key x in range [1, p-2]

            BigInteger y = BigInteger.ModPow(g, x, p); // y = g^x mod p

            string publicKey = $"{p}|{g}|{y}";
            string privateKey = $"{p}|{g}|{x}";
            return (publicKey, privateKey);
        }

        public string Encrypt(string plainText, string publicKey)
        {
            var parts = publicKey.Split('|');
            BigInteger p = BigInteger.Parse(parts[0]);
            BigInteger g = BigInteger.Parse(parts[1]);
            BigInteger y = BigInteger.Parse(parts[2]);

            var rng = RandomNumberGenerator.Create();
            byte[] kBytes = new byte[4];
            rng.GetBytes(kBytes);

            // Ensure non-negative k
            BigInteger k = new BigInteger(new byte[] { 0 }.Concat(kBytes).ToArray());
            k = k % (p - 2) + 1;

            BigInteger a = BigInteger.ModPow(g, k, p);
            BigInteger m = new BigInteger(new byte[] { 0 }.Concat(Encoding.UTF8.GetBytes(plainText)).ToArray());

            BigInteger b = (BigInteger.ModPow(y, k, p) * m) % p;

            return $"{a}|{b}";
        }

        public string Decrypt(string cipherText, string privateKey)
        {
            var parts = privateKey.Split('|');
            BigInteger p = BigInteger.Parse(parts[0]);
            BigInteger g = BigInteger.Parse(parts[1]);
            BigInteger x = BigInteger.Parse(parts[2]);

            var cipherParts = cipherText.Split('|');
            BigInteger a = BigInteger.Parse(cipherParts[0]);
            BigInteger b = BigInteger.Parse(cipherParts[1]);

            BigInteger s = BigInteger.ModPow(a, x, p);
            BigInteger m = (b * ModInverse(s, p)) % p;

            return Encoding.UTF8.GetString(m.ToByteArray().SkipWhile(bi => bi == 0).ToArray());
        }

        private BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;
            if (m == 1) return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;

                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;
            return x1;
        }
    }
}
