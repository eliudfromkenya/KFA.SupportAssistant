using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.DataEncryption;

namespace KFA.SupportAssistant.Infrastructure.Data;
internal class EncryptionProvider : IEncryptionProvider
{
  public byte[]? Encrypt(byte[]? input)
  {
    if (input == null) return null;
    return AesEncryptor.Encrypt(input!);
  }

  public byte[]? Decrypt(byte[]? input)
  {
    if (input == null) return null;
    return AesEncryptor.Decrypt(input!);
  }
}



static class AesEncryptor
{

  static readonly byte[] _key = [];
  static readonly byte[] _vector = [];

  /// <summary>
  /// Decrypts the specified byte array to plain text.
  /// </summary>
  /// <param name="encryptedBytes">The encrypted byte array</param>
  /// <param name="key">The encryption key</param>
  /// <param name="vector">The initialization vector</param>
  /// <returns>The decrypted text as a string</returns>
  internal static byte[] Decrypt(byte[] encryptedBytes)
  {
    using var aesAlgorithm = Aes.Create();
    using var decryptor = aesAlgorithm.CreateDecryptor(_key, _vector);
    using var memoryStream = new MemoryStream(encryptedBytes);
    using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
    using var binaryReader = new BinaryReader(cryptoStream, Encoding.UTF8);
    return binaryReader.ReadBytes((int)cryptoStream.Length);
  }

  /// <summary>
  /// Encrypts the specified text and returns an encrypted byte array.
  /// </summary>
  /// <param name="plainText">The text to encrypt</param>
  /// <param name="key">The encryption key</param>
  /// <param name="vector">The initialization vector</param>
  /// <returns>The encrypted text as a byte array</returns>
  internal static byte[] Encrypt(byte[] plainText)
  {
    using var aesAlgorithm = Aes.Create();
    using var encryptor = aesAlgorithm.CreateEncryptor(_key, _vector);
    using var memoryStream = new MemoryStream();
    using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    using var streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8);
    streamWriter.Write(plainText);
    return memoryStream.ToArray();
  }
  static AesEncryptor()
  {
    var aesAlgorithm = Aes.Create();
    aesAlgorithm.GenerateIV();

    _key= aesAlgorithm.IV;
    aesAlgorithm.GenerateIV();

    _vector = aesAlgorithm.IV;
    var mm = Encoding.ASCII.GetString(_key);
    var mm2 = Encoding.ASCII.GetString(_vector);
    //_vector = Encoding.ASCII.GetBytes("#$fghfghf$%y9uupnyuoyomkjpouyh79y79oy9yuu[piumpoiuhpmoipuioYgft5GTR^%UTHHF fgh fdgsdfg hsdfgs dfgfghghjghj 766 yhdfgehdGFFHTY%^%%^%TGFGRG%jlbt7 t866 gkhbgkhkl g78o68o76i kb yutbkjhfiu6 i7 868 gjhkbkh giuyongkjiy ty 768oy iu780yjnh kjgbiouy h iu oiu po houy uypo ioupo8g iu gjhk iyhop7ioblk b8o7").Take(256).ToArray();
    //_key = Encoding.ASCII.GetBytes("$%GFghgr67#$d#$436 fghfg ggdfg 45t5t5tgsg trgwt 45tw45tw56tfgfdgioyoi itguytkyjhniy658oynjlhghgbio7ioiggoiuhjghkjhiouhnhy7y0897898797696noihib7t*&^&*(^&*()(nngb8bybgoi7^(&rb987npyhnouoiuyoYNITb87b6yb89tyuiogb*^B87^*&^bodfgsdfgsddfg sdfgs df#%hxfhc fgzfg $#$ gdfhgf#%%").Take(256).ToArray();
  }
}
