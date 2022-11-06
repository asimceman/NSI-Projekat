using System.Net;

namespace NSI_Prototip
{
    public class ScrapingAndParsingMethods
    {
        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetStringAsync(fullUrl);
           
            return response;
        }

        public static DateTime? ConvertLocalDatetimeToUtc(DateTime? localDateTime)
        {
            if (localDateTime is null)
                return null;

            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
           
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime.Value, tst);
        }

        public enum NewsModelCategory
        {
            Unknown,
            Konkurs,
            JavniOglas,
            JavniPoziv,
            Obavjestenje,
            Cestitka,
            Press
        }
    }
}
