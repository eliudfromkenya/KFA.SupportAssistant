﻿namespace Pilgrims.Systems.Assistant.Globals.Classes;

using System;

/// <summary>
/// Defines the <see cref="NumberToWordsConverter" />.
/// </summary>
public static class NumberToWordsConverter
{
  // converts any number between 0 & INT_MAX (2,147,483,647)
  /// <summary>
  /// The ConvertNumberToString.
  /// </summary>
  /// <param name="n">The n<see cref="long"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  public static string ConvertNumberToString(long n)
  {
    if (n < 0)
      throw new NotSupportedException("negative numbers not supported");
    if (n == 0) return "zero";
    if (n < 10) return ConvertDigitToString(n);
    if (n < 20) return ConvertTeensToString(n);
    if (n < 100) return ConvertHighTensToString(n);
    if (n < 1000)
      return ConvertBigNumberToString(n, (long)1e2, "hundred");
    if (n < 1e6)
      return ConvertBigNumberToString(n, (int)1e3, "thousand");
    if (n < 1e9)
      return ConvertBigNumberToString(n, (int)1e6, "million");
    if (n < 1e12)
      return ConvertBigNumberToString(n, (int)1e9, "billion");

    return ConvertBigNumberToString(n, (long)1e12, "trillion");
  }

  /// <summary>
  /// The ConvertDigitToString.
  /// </summary>
  /// <param name="i">The i<see cref="long"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private static string ConvertDigitToString(long i)
  {
    switch (i)
    {
      case 0: return "";
      case 1: return "one";
      case 2: return "two";
      case 3: return "three";
      case 4: return "four";
      case 5: return "five";
      case 6: return "six";
      case 7: return "seven";
      case 8: return "eight";
      case 9: return "nine";
      default:
        throw new IndexOutOfRangeException(string.Format("{0} not a digit", i));
    }
  }

  // assumes a number between 10 & 19
  /// <summary>
  /// The ConvertTeensToString.
  /// </summary>
  /// <param name="n">The n<see cref="long"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private static string ConvertTeensToString(long n)
  {
    switch (n)
    {
      case 10: return "ten";
      case 11: return "eleven";
      case 12: return "twelve";
      case 13: return "thirteen";
      case 14: return "fourteen";
      case 15: return "fiveteen";
      case 16: return "sixteen";
      case 17: return "seventeen";
      case 18: return "eighteen";
      case 19: return "nineteen";
      default:
        throw new IndexOutOfRangeException(string.Format("{0} not a teen", n));
    }
  }

  // assumes a number between 20 and 99
  /// <summary>
  /// The ConvertHighTensToString.
  /// </summary>
  /// <param name="n">The n<see cref="long"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private static string ConvertHighTensToString(long n)
  {
    var tensDigit = (int)Math.Floor(n / 10.0);

    string tensStr;
    switch (tensDigit)
    {
      case 2:
        tensStr = "twenty";
        break;

      case 3:
        tensStr = "thirty";
        break;

      case 4:
        tensStr = "forty";
        break;

      case 5:
        tensStr = "fifty";
        break;

      case 6:
        tensStr = "sixty";
        break;

      case 7:
        tensStr = "seventy";
        break;

      case 8:
        tensStr = "eighty";
        break;

      case 9:
        tensStr = "ninety";
        break;

      default:
        throw new IndexOutOfRangeException(string.Format("{0} not in range 20-99", n));
    }

    if (n % 10 == 0) return tensStr;
    var onesStr = ConvertDigitToString(n - tensDigit * 10);
    return tensStr + "-" + onesStr;
  }

  // Use this to convert any integer bigger than 99
  /// <summary>
  /// The ConvertBigNumberToString.
  /// </summary>
  /// <param name="n">The n<see cref="long"/>.</param>
  /// <param name="baseNum">The baseNum<see cref="long"/>.</param>
  /// <param name="baseNumStr">The baseNumStr<see cref="string"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private static string ConvertBigNumberToString(long n, long baseNum, string baseNumStr)
  {
    // special case: use commas to separate portions of the number, unless we are in the hundreds
    var separator = baseNumStr != "hundred" ? ", " : " ";

    // Strategy: translate the first portion of the number, then recursively translate the remaining sections.
    // Step 1: strip off first portion, and convert it to string:
    var bigPart = (long)Math.Floor((double)n / baseNum);
    var bigPartStr = ConvertNumberToString(bigPart) + " " + baseNumStr;
    // Step 2: check to see whether we're done:
    if (n % baseNum == 0) return bigPartStr;
    // Step 3: concatenate 1st part of string with recursively generated remainder:
    var restOfNumber = n - bigPart * baseNum;
    return bigPartStr + separator + ConvertNumberToString(restOfNumber);
  }
}
