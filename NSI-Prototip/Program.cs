using NSI_Prototip;

var urlsIlidza = new List<string>()
{
    "https://www.opcinailidza.ba/news/notice?page=1",
    "https://www.opcinailidza.ba/news",
    "https://www.opcinailidza.ba/news/invitation"
};

List<NewsModel> listOfIlidzaNews = new();
foreach(var url in urlsIlidza)
{
    string response = ScrapingAndParsingMethods.CallUrl(url).Result;
    var newsElements = Ilidza.ParseHtml(response);

    listOfIlidzaNews.AddRange(newsElements.Select(x => Ilidza.ParseNews(x))
                                          .ToList());
}


string urlRad = "https://www.rad.com.ba/aktuelno.htm";
string responseRad =ScrapingAndParsingMethods.CallUrl(urlRad).Result;
var newsElementsRad = KJKPRad.ParseHtml(responseRad);
var listOfRadNews = newsElementsRad.Select(x => KJKPRad.ParseNews(x))
                                   .Where(x => x.CategoryId > 0)
                                   .ToList();




Console.WriteLine("a");


