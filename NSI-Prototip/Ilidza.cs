using HtmlAgilityPack;
using System.Globalization;

namespace NSI_Prototip
{
    public class Ilidza
    {
        public static IEnumerable<HtmlNode> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);

            var newsDiv = htmlDoc.DocumentNode.Descendants("div")
                                              .Where(node => node.GetAttributeValue("class", "").Contains("news-row"));

            return newsDiv ?? new List<HtmlNode>();
        }

        public static async Task<string> ScrapeAndParseNewsContent(string newsUrl)
        {
            string responseHtml = await ScrapingAndParsingMethods.CallUrl(newsUrl);

            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(responseHtml);

            var newsDiv = htmlDoc.DocumentNode.Descendants("div")
                                              .Where(node => node.GetAttributeValue("class", "").Contains("central padding"))
                                              .FirstOrDefault()?.Descendants("div")
                                                                .Where(node => node.GetAttributeValue("class", "").Contains("caption"))
                                                                .FirstOrDefault();

            var content = newsDiv is not null ? string.Join("\n", newsDiv.Descendants("p")
                                                      .Select(x => x.InnerHtml)) : "";
            return content;

        }

        public static async Task<NewsModel> ParseNews(HtmlNode newsHtml, ScrapingAndParsingMethods.NewsModelCategory newsCategory = 0)
        {
            string dateOfPublicationString = newsHtml.Descendants("small")
                                                     .FirstOrDefault()?.InnerHtml ?? "";
            
            string dateOfPublicationStringParsed = dateOfPublicationString.Split("by")
                                                                          .FirstOrDefault()?.Trim() ?? "";

            DateTime? dateOfPublication = DateTime.TryParseExact(dateOfPublicationStringParsed,
                                                       "dd'-'MM'-'yyyy' 'HH':'mm':'ss",
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.None,
                                                       out DateTime DOP) ? DOP : null;

            var headingHtmlElement = newsHtml.Descendants("a")
                                             .FirstOrDefault();

            var headingElementString = headingHtmlElement?.InnerHtml ?? "";

            var newsURL = headingHtmlElement?.Attributes.FirstOrDefault(x => x.Name == "href")?.Value ?? "";

            var content = await ScrapeAndParseNewsContent("https://www.opcinailidza.ba" + newsURL.Trim());

            Console.WriteLine("Gotova ParseNews");

            NewsModel newsModel = new()
            {
                Date = ScrapingAndParsingMethods.ConvertLocalDatetimeToUtc(dateOfPublication),
                Heading = headingElementString.Trim(),
                NewsUrl = "https://www.opcinailidza.ba" + newsURL.Trim(),
                CategoryId = (int)newsCategory,
                Content = content
            };

            return newsModel;
        }
    }
}
