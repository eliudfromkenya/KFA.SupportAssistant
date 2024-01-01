using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KFA.SupportAssistant.Globals.Encryptions;
public class ShaEncryptor
{
  public static string GenerateSha256String(string input)
  {
    var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(input);
    var hash = sha256.ComputeHash(bytes);
    return GetStringFromHash(hash);
  }

  public static string GenerateSha512String(string input)
  {
    var sha512 = SHA512.Create();
    var bytes = Encoding.UTF8.GetBytes(input);
    var hash = sha512.ComputeHash(bytes);
    return GetStringFromHash(hash);
  }

  static string GetStringFromHash(IEnumerable<byte> hash)
  {
    return string.Join("", hash.Select(x => x.ToString("X2")));
  }
}

