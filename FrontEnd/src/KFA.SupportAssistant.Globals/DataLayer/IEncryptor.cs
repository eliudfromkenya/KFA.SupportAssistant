namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IEncryptor
{
  string StringObfuscate(string text);

  string StringDeObfuscate(string text);

  string GenerateSha256String(string text);

  string GenerateSha512String(string text);

  string EncryptString(string text, string password);

  string DecryptString(string text, string password);

  string EncryptString(string text);

  string DecryptString(string text);

  byte[] HashPassword(string password, out byte[] salt);

  string HashPassword(string password, out string salt);

  bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);

  bool VerifyPassword(string passwordToVerify, string password, string salt);
}
