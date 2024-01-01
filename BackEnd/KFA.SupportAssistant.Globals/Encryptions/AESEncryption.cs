using System;
using System.Security.Cryptography;
using System.Text;

namespace KFA.SupportAssistant.Globals.Encryptions;

public class AESEncryption
{
  public string Encrypt(string text, string IV, string key)
  {
    var cipher = CreateCipher(key);
    cipher.IV = Convert.FromBase64String(IV);

    var cryptTransform = cipher.CreateEncryptor();
    var plaintext = Encoding.UTF8.GetBytes(text);
    var cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

    return Convert.ToBase64String(cipherText);
  }

  public string Decrypt(string encryptedText, string IV, string key)
  {
    var cipher = CreateCipher(key);
    cipher.IV = Convert.FromBase64String(IV);

    var cryptTransform = cipher.CreateDecryptor();
    var encryptedBytes = Convert.FromBase64String(encryptedText);
    var plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

    return Encoding.UTF8.GetString(plainBytes);
  }

  public (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
  {
    var key = GetEncodedRandomString(32); // 256
    var cipher = CreateCipher(key);
    var IVBase64 = Convert.ToBase64String(cipher.IV);
    return (key, IVBase64);
  }

  string GetEncodedRandomString(int length)
  {
    var base64 = Convert.ToBase64String(GenerateRandomBytes(length));
    return base64;
  }

  Aes CreateCipher(string keyBase64)
  {
    // Default values: Keysize 256, Padding PKC27
    var cipher = Aes.Create();
    cipher.Mode = CipherMode.CBC;  // Ensure the integrity of the ciphertext if using CBC

    cipher.Padding = PaddingMode.ISO10126;
    cipher.Key = Convert.FromBase64String(keyBase64);

    return cipher;
  }

  byte[] GenerateRandomBytes(int length)
  {
    var byteArray = new byte[length];
    RandomNumberGenerator.Fill(byteArray);
    return byteArray;
  }
}
