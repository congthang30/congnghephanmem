using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace EasyBuy.Method
{
    public static class SessionExtensions
    {
        public static void SetDecimal(this ISession session, string key, decimal value)
        {
            session.SetString(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static decimal? GetDecimal(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return null;
        }
    }
}
