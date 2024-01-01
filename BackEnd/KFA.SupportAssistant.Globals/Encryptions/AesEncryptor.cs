//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;

//namespace KFA.SupportAssistant.Globals.Encryptions;

//public static class AesEncryptor
//{
//  public static byte[] SaltBytes { get; set; } = [23, 105, 212, 190, 123, 167, 5, 247];

//  public static string EncryptString(string text, string password)
//  {
//    var baPwd = Encoding.UTF8.GetBytes(password);

//    // Hash the password with SHA256
//    var baPwdHash = SHA256.HashData(baPwd);

//    var baText = Encoding.UTF8.GetBytes(text);

//    var baSalt = SaltBytes;//  GetRandomBytes();
//    var baEncrypted = new byte[baSalt.Length + baText.Length];

//    // Combine Salt + Text
//    for (var i = 0; i < baSalt.Length; i++)
//      baEncrypted[i] = baSalt[i];

//    for (var i = 0; i < baText.Length; i++)
//      baEncrypted[i + baSalt.Length] = baText[i];

//    baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

//    var result = Convert.ToBase64String(baEncrypted);
//    return result;
//  }

//  public static string DecryptString(string text, string password)
//  {
//    var baPwd = Encoding.UTF8.GetBytes(password);

//    // Hash the password with SHA256
//    var baPwdHash = SHA256.HashData(baPwd);

//    var baText = Convert.FromBase64String(text);

//    var baDecrypted = AES_Decrypt(baText, baPwdHash);

//    // Remove salt
//    var saltLength = GetSaltLength();
//    var baResult = new byte[baDecrypted.Length - saltLength];

//    for (var i = 0; i < baResult.Length; i++)
//      baResult[i] = baDecrypted[i + saltLength];

//    var result = Encoding.UTF8.GetString(baResult);
//    return result;
//  }

//  public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
//  {
//    byte[] encryptedBytes;

//    // Set your salt here, change it to meet your flavor:
//    // The salt bytes must be at least 8 bytes.
//    var saltBytes = SaltBytes;

//    using var ms = new MemoryStream();
//    using var aes = Aes.Create();
//      aes.KeySize = 256;
//      aes.BlockSize = 128;

//      var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
//      aes.Key = key.GetBytes(aes.KeySize / 8);
//      aes.IV = key.GetBytes(aes.BlockSize / 8);

//      aes.Mode = CipherMode.CBC;

//      using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
//      {
//        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
//        cs.Close();
//      }

//      encryptedBytes = ms.ToArray();
//    }

//    return encryptedBytes;
//  }

//  public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
//  {
//    byte[] decryptedBytes;

//    // Set your salt here, change it to meet your flavor:
//    // The salt bytes must be at least 8 bytes.
//    var saltBytes = SaltBytes;

//    using (var ms = new MemoryStream())
//    {
//      using (var aes = new RijndaelManaged())
//      {
//        aes.KeySize = 256;
//        aes.BlockSize = 128;

//        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
//        aes.Key = key.GetBytes(aes.KeySize / 8);
//        aes.IV = key.GetBytes(aes.BlockSize / 8);

//        aes.Mode = CipherMode.CBC;

//        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
//        {
//          cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
//          cs.Close();
//        }

//        decryptedBytes = ms.ToArray();
//      }
//    }

//    return decryptedBytes;
//  }

//  // public byte[] GetRandomBytes()
//  // {
//  //    var saltLength = GetSaltLength();
//  //    var ba = new byte[saltLength];
//  //    RandomNumberGenerator.Create().GetBytes(ba);
//  //    return ba;
//  // }

//  public static int GetSaltLength() => 8;
//}
