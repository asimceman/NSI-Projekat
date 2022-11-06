using HtmlAgilityPack;
using System.Globalization;

namespace NSI_Prototip
{
    public class KJKPRad
    {
        public static IEnumerable<HtmlNode> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);

            var mainNewsDiv = htmlDoc.DocumentNode.Descendants("div")
                                                  .Where(node => node.GetAttributeValue("class", "") == "padding-left")
                                                  .FirstOrDefault();

            var splittedMainNewsDiv = mainNewsDiv?.InnerHtml.Split("<hr>")
                                                            .Where(x => x.Trim().Length > 0)
                                                            .Select(s => HtmlNode.CreateNode($"<div>{s}</div>"));

            return splittedMainNewsDiv ?? new List<HtmlNode>();
        }

        public static NewsModel ParseNews(HtmlNode newsHtml)
        {
            var dateOfPublicationHtmlElement = newsHtml.Elements("p").FirstOrDefault();
            var dateOfPublicationString = dateOfPublicationHtmlElement?.InnerText ?? string.Empty;

            DateTime? dateOfPublication = DateTime.TryParseExact(dateOfPublicationString,
                                                                "dd'.'MM'.'yyyy'.'",
                                                                CultureInfo.InvariantCulture,
                                                                DateTimeStyles.None,
                                                                out DateTime DOP) ? DOP : null;

            var headings = newsHtml.Elements("h5").Select(el => el.InnerHtml);

            NewsModel newsModel = new()
            {
                Date = ScrapingAndParsingMethods.ConvertLocalDatetimeToUtc(dateOfPublication),
                CategoryId = GetNewsCategoryId(headings.First()),
                Heading = string.Join(" ", headings).ToUpper(),
                NewsUrl = "https://www.rad.com.ba/aktuelno.htm",
                Content = ""
            };

            return newsModel;
        }

        public static int GetNewsCategoryId(string categoryString)
        {
            string categoryStringLowerCase = categoryString.ToLower();

            if (categoryStringLowerCase.Contains("konkurs"))
            {
                return (int)ScrapingAndParsingMethods.NewsModelCategory.Konkurs;
            }
            else if (categoryStringLowerCase.Contains("oglas"))
            {
                return (int)ScrapingAndParsingMethods.NewsModelCategory.JavniOglas;
            }
            else if (categoryStringLowerCase.Contains("poziv"))
            {
                return (int)ScrapingAndParsingMethods.NewsModelCategory.JavniPoziv;
            }
            else if (categoryStringLowerCase.Contains("čestitka"))
            {
                return (int)ScrapingAndParsingMethods.NewsModelCategory.Cestitka;
            }

            return (int)ScrapingAndParsingMethods.NewsModelCategory.Obavjestenje;
        }
    }
}
