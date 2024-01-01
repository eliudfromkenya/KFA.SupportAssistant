using Microsoft.VisualBasic;

namespace KFA.SupportAssistant.Globals.Encryptions;

public static class StringObfuscate
{
  // Calls to subroutine using a shorted name!
  public static string Os(string sInput) => ObfuscateString(sInput);

  // Calls to subroutine using a shorted name!
  public static string Us(string sInput) => UnObfuscateString(sInput);

  // Simple string encryption,
  // encryption just enough to make the string not easily searchable,
  // but fast enough not to impose to much of an overhead.

  public static string ObfuscateString(string sInput)
  {
    var sTemp = "";
    int iLoop;

    var iLen = sInput.Length;

    for (iLoop = iLen; iLoop >= 1; iLoop -= 2)
      sTemp += Strings.Mid(sInput, iLoop, 1);

    for (iLoop = iLen - 1; iLoop >= 1; iLoop -= 2)
      sTemp += Strings.Mid(sInput, iLoop, 1);

    return sTemp;
  }

  public static string UnObfuscateString(string sInput)
  {
    var sTemp = "";
    int iLoop;

    var iLen = sInput.Length;
    var iRemainder = iLen % 2;
    var iMiddle = iLen / 2;

    for (iLoop = iMiddle + iRemainder; iLoop >= 1; iLoop--)
    {
      if (iRemainder == 0)
        sTemp += Strings.Mid(sInput, iLoop + iMiddle, 1);

      sTemp += Strings.Mid(sInput, iLoop, 1);

      if (iRemainder == 1 && iLoop != 1)
        sTemp += Strings.Mid(sInput, iLoop + iMiddle, 1);
    }

    return sTemp;
  }
}
