using System;
using System.Text;
using System.Collections.Generic;

namespace VigenereCipherApp.Services
{
    public class PlayfairCipherService : ICryptoService
    {
        private char[,] matrix = new char[5, 5];

        public string Encrypt(string plainText, string key)
        {
            if (!IsValidInput(plainText))
                throw new ArgumentException("Playfair Cipher only supports alphabetic characters (A-Z, a-z).");

            BuildMatrix(key);
            plainText = FormatInput(plainText, true);
            StringBuilder result = new();

            for (int i = 0; i < plainText.Length; i += 2)
            {
                char a = plainText[i];
                char b = plainText[i + 1];
                GetPosition(a, out int row1, out int col1);
                GetPosition(b, out int row2, out int col2);

                if (row1 == row2)
                {
                    result.Append(matrix[row1, (col1 + 1) % 5]);
                    result.Append(matrix[row2, (col2 + 1) % 5]);
                }
                else if (col1 == col2)
                {
                    result.Append(matrix[(row1 + 1) % 5, col1]);
                    result.Append(matrix[(row2 + 1) % 5, col2]);
                }
                else
                {
                    result.Append(matrix[row1, col2]);
                    result.Append(matrix[row2, col1]);
                }
            }

            return result.ToString();
        }

        public string Decrypt(string cipherText, string key)
        {
            if (!IsValidInput(cipherText))
                throw new ArgumentException("Playfair Cipher only supports alphabetic characters (A-Z, a-z).");

            BuildMatrix(key);
            cipherText = FormatInput(cipherText, false);
            StringBuilder result = new();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                char a = cipherText[i];
                char b = cipherText[i + 1];
                GetPosition(a, out int row1, out int col1);
                GetPosition(b, out int row2, out int col2);

                if (row1 == row2)
                {
                    result.Append(matrix[row1, (col1 + 4) % 5]);
                    result.Append(matrix[row2, (col2 + 4) % 5]);
                }
                else if (col1 == col2)
                {
                    result.Append(matrix[(row1 + 4) % 5, col1]);
                    result.Append(matrix[(row2 + 4) % 5, col2]);
                }
                else
                {
                    result.Append(matrix[row1, col2]);
                    result.Append(matrix[row2, col1]);
                }
            }

            return result.ToString().TrimEnd('X');
        }

        private void BuildMatrix(string key)
        {
            string cleanedKey = RemoveDuplicates(key.ToUpper().Replace("J", "I"));
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            HashSet<char> used = new();
            StringBuilder fullKey = new(cleanedKey);

            foreach (char c in alphabet)
                if (!used.Contains(c) && !fullKey.ToString().Contains(c))
                    fullKey.Append(c);

            int index = 0;
            for (int row = 0; row < 5; row++)
                for (int col = 0; col < 5; col++)
                    matrix[row, col] = fullKey[index++];
        }

        private string RemoveDuplicates(string input)
        {
            StringBuilder result = new();
            HashSet<char> seen = new();

            foreach (char c in input)
                if (!seen.Contains(c) && char.IsLetter(c))
                {
                    seen.Add(c);
                    result.Append(c);
                }

            return result.ToString();
        }

        private void GetPosition(char c, out int row, out int col)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (matrix[i, j] == c)
                    {
                        row = i;
                        col = j;
                        return;
                    }
            throw new ArgumentException("Character not found in matrix");
        }

        private string FormatInput(string input, bool forEncrypt)
        {
            input = input.ToUpper().Replace("J", "I");
            StringBuilder formatted = new();

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsLetter(input[i])) continue;

                formatted.Append(input[i]);

                if (i + 1 < input.Length && input[i] == input[i + 1])
                    formatted.Append('X');
            }

            if (formatted.Length % 2 != 0)
                formatted.Append('X');

            return formatted.ToString();
        }

        private bool IsValidInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }
    }
}
