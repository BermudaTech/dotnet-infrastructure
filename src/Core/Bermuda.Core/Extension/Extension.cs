using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class Extension
{
    public static bool xToBool(this object value)
    {
        bool defaultValue = default(bool);

        try
        {
            defaultValue = Convert.ToBoolean(value);
        }
        catch { }

        return defaultValue;
    }

    public static bool? xToBoolDefault(this object value)
    {
        bool? nullableBool = null;

        try
        {
            nullableBool = (bool)value;
        }
        catch { }

        return nullableBool;
    }

    public static int xToInt(this object value)
    {
        int defaultValue = default(int);

        try
        {
            defaultValue = Convert.ToInt32(value);
        }
        catch { }

        return defaultValue;
    }

    public static int? xToIntDefault(this object value)
    {
        int? nullableInt = null;

        try
        {
            nullableInt = (int)value;
        }
        catch { }

        return nullableInt;
    }

    public static decimal xToDecimal(this object value)
    {
        decimal defaultValue = default(decimal);

        try
        {
            defaultValue = Convert.ToDecimal(value);
        }
        catch { }

        return defaultValue;
    }

    public static decimal? xToDecimalDefault(this object value)
    {
        decimal? nullableDecimal = null;

        try
        {
            nullableDecimal = (decimal)value;
        }
        catch { }

        return nullableDecimal;
    }

    public static long xToLong(this object value)
    {
        long defaultValue = default(long);

        try
        {
            defaultValue = Convert.ToInt64(value);
        }
        catch { }

        return defaultValue;
    }

    public static long? xToLongDefault(this object value)
    {
        long? nullableLong = null;

        try
        {
            nullableLong = (long)value;
        }
        catch { }

        return nullableLong;
    }

    public static DateTime xToDateTime(this object value)
    {
        DateTime defaultValue = default(DateTime);

        try
        {
            defaultValue = Convert.ToDateTime(value);
        }
        catch { }

        return defaultValue;
    }

    public static DateTime? xToDateTimeDefault(this object value)
    {
        DateTime? nullableDateTime = null;

        try
        {
            nullableDateTime = (DateTime)value;
        }
        catch { }

        return nullableDateTime;
    }

    public static string GetURLFromText(this string text, string extension = "")
    {
        text = text.Trim().ToLower();
        text = Regex.Replace(text, "ş", "s");
        text = Regex.Replace(text, "ı", "i");
        text = Regex.Replace(text, "ö", "o");
        text = Regex.Replace(text, "ü", "u");
        text = Regex.Replace(text, "ç", "c");
        text = Regex.Replace(text, "ğ", "g");
        text = Regex.Replace(text, @"[^a-z0-9]", " ");
        text = Regex.Replace(text, @"\s+", " ");
        text = text.Trim();
        text = Regex.Replace(text, " ", "-");

        if (!string.IsNullOrEmpty(extension))
        {
            text = text + extension;
        }

        return text;
    }

    public static string GetEnCharFromText(this string text)
    {
        text = Regex.Replace(text, "ş", "s");
        text = Regex.Replace(text, "ı", "i");
        text = Regex.Replace(text, "ö", "o");
        text = Regex.Replace(text, "ü", "u");
        text = Regex.Replace(text, "ç", "c");
        text = Regex.Replace(text, "ğ", "g");
        text = Regex.Replace(text, "Ş", "S");
        text = Regex.Replace(text, "İ", "I");
        text = Regex.Replace(text, "Ö", "O");
        text = Regex.Replace(text, "Ü", "U");
        text = Regex.Replace(text, "Ç", "C");
        text = Regex.Replace(text, "Ğ", "G");
        text = Regex.Replace(text, "Ğ", "G");
        return text;
    }

    public static string GetClearTextFromHtml(this string text)
    {
        return Regex.Replace(text, @"<[^>]*>", " ");
    }

    public static string GetFirstSentence(this string paragraph)
    {
        for (int i = 0; i < paragraph.Length; i++)
        {
            switch (paragraph[i])
            {
                case '.':
                    if (i < (paragraph.Length - 1) &&
                        char.IsWhiteSpace(paragraph[i + 1]))
                    {
                        goto case '!';
                    }
                    break;
                case '?':
                case '!':
                    return paragraph.Substring(0, i + 1);
            }
        }
        return paragraph;
    }

    public static string GenerateOidWithDate(int suffixLength, string dateFormat = null)
    {
        string date = DateTime.Now.ToString(dateFormat ?? "yyyyMMddHHmmss");

        return $"{date}{GenerateUniqueRandomString(suffixLength)}".ToUpper();
    }

    public static string GenerateUniqueRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
        if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
        if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

        const int byteSize = 0x100;
        var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
        if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

        // Guid.NewGuid and System.Random are not particularly random. By using a
        // cryptographically-secure random number generator, the caller is always
        // protected, regardless of use.
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            var result = new StringBuilder();
            var buf = new byte[128];
            while (result.Length < length)
            {
                rng.GetBytes(buf);
                for (var i = 0; i < buf.Length && result.Length < length; ++i)
                {
                    // Divide the byte into allowedCharSet-sized groups. If the
                    // random value falls into the last group and the last group is
                    // too small to choose from the entire allowedCharSet, ignore
                    // the value in order to avoid biasing the result.
                    var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                    if (outOfRangeStart <= buf[i]) continue;
                    result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                }
            }

            return result.ToString();
        }
    }

    public static string PhoneNumberClear(this string phoneNumber, string replaceWithEmpty = null)
    {

        try
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").TrimStart().TrimEnd();
            }

            if (!string.IsNullOrEmpty(replaceWithEmpty))
            {
                phoneNumber = phoneNumber.Replace(replaceWithEmpty, "");
            }
        }
        catch
        {
        }

        return phoneNumber;
    }

    public static string ToBase64Encode(this string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string ToBase64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string PasswordToSHA256(this string password, string salt)
    {
        byte[] newsalt = Encoding.ASCII.GetBytes(salt);

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: newsalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    public static bool IsValidEmail(this string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(this string number, string areaCode)
    {
        try
        {
            if (number.Substring(0, 2) != areaCode)
            {
                number = (areaCode + number);
            }

            var phoneNumber = number.Trim()
                        .Replace(" ", "")
                        .Replace("-", "")
                        .Replace("(", "")
                        .Replace(")", "");

            return Regex.Match(phoneNumber, @"^\+\d{5,15}$").Success;
        }
        catch
        {
            return false;
        }
    }

    public static string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }

        return res.ToString();
    }

    public static int CalculateAgeCorrect(this DateTime birthDate, DateTime now)
    {
        int age = now.Year - birthDate.Year;

        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            age--;

        return age;
    }
}