using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityProject.Pages
{
    public static class UserHelper
    {

        public static string GetUserImageFolderPath(string userName, string action, string passwordHash)
        {
            string userFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", userName, "logs");

            // Ensure the directory exists
            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            // Define the path for the text file
            string filePath = Path.Combine(userFolderPath, "log.txt");

            // The new line to append
            string newLine = $"Korisnik: {userName} Action: {action} Time: {DateTime.Now}";

            // Generate key and IV from the password hash
            var keyIv = GenerateKeyAndIV(passwordHash);

            // Encrypt the new line
            byte[] encryptedNewLine = EncryptStringToBytes_Aes(newLine, keyIv.Key, keyIv.IV);

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read all lines from the file and decrypt them
                var encryptedLines = File.ReadAllLines(filePath).ToList();
                var lines = encryptedLines.Select(line => DecryptStringFromBytes_Aes(Convert.FromBase64String(line), keyIv.Key, keyIv.IV)).ToList();

                // Check if the file has reached 1000 lines
                if (lines.Count >= 1000)
                {
                    // Remove the oldest line
                    lines.RemoveAt(0);
                }

                // Append the new line
                lines.Add(Convert.ToBase64String(encryptedNewLine));

                // Encrypt and write all lines back to the file
                File.WriteAllLines(filePath, lines.Select(line => Convert.ToBase64String(EncryptStringToBytes_Aes(line, keyIv.Key, keyIv.IV))).ToArray());
            }
            else
            {
                // If the file doesn't exist, create it and write the new line
                File.WriteAllText(filePath, Convert.ToBase64String(encryptedNewLine));
            }

            return newLine;
        }

        private static (byte[] Key, byte[] IV) GenerateKeyAndIV(string passwordHash)
        {
            // Use a salt to add additional security
            byte[] salt = Encoding.UTF8.GetBytes("SomeSaltValue");

            using (var keyDerivationFunction = new Rfc2898DeriveBytes(passwordHash, salt, 10000))
            {
                byte[] key = keyDerivationFunction.GetBytes(32); // AES-256 key size
                byte[] iv = keyDerivationFunction.GetBytes(16);  // AES block size
                return (key, iv);
            }
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
