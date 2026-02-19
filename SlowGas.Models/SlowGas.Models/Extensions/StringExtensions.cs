using System.Text.RegularExpressions;

namespace SlowGas.Models.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out _);
        }

        public static bool IsValidPhone(this string phone)
        {
            if (IsEmpty(phone)) return false;
            return Regex.IsMatch(phone, @"^(\+7|8)[0-9]{10}$");
        }

        // ДОБАВЬ ЭТОТ МЕТОД
        public static bool IsValidEmail(this string email)
        {
            if (IsEmpty(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}