using System.Text.RegularExpressions;

namespace CentralTask.Core.Extensions;

public static class StringExtensions
{
    public static string ApenasNumeros(this string str)
    {
        if (!string.IsNullOrEmpty(str))
            return new string(str.Where(char.IsDigit).ToArray());

        return string.Empty;
    }
    public static bool ValidaExisteApenasLetras(this string str)
    {
        return string.IsNullOrEmpty(str) ? false : str.Any(char.IsLetter);
    }
    public static bool ValidaExisteApenasNumeros(this string str)
    {
        return string.IsNullOrEmpty(str) ? false : str.Any(char.IsNumber);
    }
    public static string ApenasLetrasNumeros(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var rgx = new Regex("[^a-zA-Z0-9]");
        return rgx.Replace(str, "");
    }
}