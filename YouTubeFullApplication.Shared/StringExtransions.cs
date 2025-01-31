using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace YouTubeFullApplication.Shared
{
    public static partial class StringExtransions
    {
        public static string FirstUpper(this string value)
        {
            if (value == string.Empty) return value;
            return string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1).ToString());
        }

        public static string FirstLower(this string value)
        {
            if (value == string.Empty) return value;
            return string.Concat(value[0].ToString().ToLower(), value.AsSpan(1).ToString());
        }

        public static string AllFirstUpper(this string value)
        {
            if (value == string.Empty) return value;
            return string.Join(" ", value.Split(' ').Select(i => i.FirstUpper()));
        }

        public static string? EmptyToNull(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        public static string OnlyNumbers(this string value)
        {
            var array = value.ToCharArray().Where(c => c >= '0' && c <= '9').ToArray();
            return new string(array);
        }

        public static bool IsEmail(this string value)
        {
            return EmailRegEx().Match(value).Success;
        }

        public static bool IsPartitaIva(this string value)
        {
            return PartitaIvaRegex().Match(value).Success;
        }

        public static bool IsCodiceFiscale(this string value)
        {
            return CodiceFiscaleRegex().Match(value).Success || value.IsPartitaIva();
        }

        [GeneratedRegex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$")]
        private static partial Regex EmailRegEx();
        [GeneratedRegex("^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$")]
        private static partial Regex CodiceFiscaleRegex();
        [GeneratedRegex("^[[0-9]{11}$")]
        private static partial Regex PartitaIvaRegex();
    }
}
