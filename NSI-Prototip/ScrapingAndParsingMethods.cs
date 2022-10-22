using System.Net;

namespace NSI_Prototip
{
    public class ScrapingAndParsingMethods
    {
        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        public enum NewsModelCategory
        {
            Unknown,
            Konkurs,
            JavniOglas,
            JavniPoziv,
            Obavjestenje,
            Cestitka
        }
    }

    public class NewsModel
    {
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public string Heading { get; set; }
        public string NewsUrl { get; set; }
    }
}
