﻿using HtmlAgilityPack;
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

        public static NewsModel ParseNews(HtmlNode newsHtml, ScrapingAndParsingMethods.NewsModelCategory newsCategory = 0)
        {
            string dateOfPublicationString = newsHtml.Descendants("small")
                                                     .FirstOrDefault()?.InnerHtml ?? "";
            
            string dateOfPublicationStringParsed = dateOfPublicationString.Split("by")
                                                                          .FirstOrDefault()?.Trim() ?? "";

            DateTime.TryParseExact(dateOfPublicationStringParsed,
                           "dd'-'MM'-'yyyy' 'HH':'mm':'ss",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out DateTime dateOfPublication);

            var headingHtmlElement = newsHtml.Descendants("a")
                                             .FirstOrDefault();

            var headingElementString = headingHtmlElement?.InnerHtml ?? "";

            var newsURL = headingHtmlElement?.Attributes.FirstOrDefault(x => x.Name == "href")?.Value ?? "";

            NewsModel newsModel = new()
            {
                Date = dateOfPublication,
                Heading = headingElementString.Trim(),
                NewsUrl = "https://www.opcinailidza.ba" + newsURL.Trim(),
                CategoryId = (int)newsCategory
            };

            return newsModel;
        }
    }
}
