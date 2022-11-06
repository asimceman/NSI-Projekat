using NSI_Prototip;

var urlsIlidza = new Dictionary<int, string>()
{
    { 4, "https://www.opcinailidza.ba/news/notice?page=" },
    { 6, "https://www.opcinailidza.ba/news?page="},
    { 3, "https://www.opcinailidza.ba/news/invitation?page="}
};

DateTime dateTimeLimit = DateTime.UtcNow.AddDays(-30);

List<NewsModel> listOfIlidzaNews = new();
foreach(var url in urlsIlidza)
{
    bool controlVariable = true;
    int pageNumber = 1;

    while(controlVariable)
    {
        string response = await ScrapingAndParsingMethods.CallUrl(url.Value + pageNumber++);
        var newsElements = Ilidza.ParseHtml(response);

        controlVariable = newsElements.Count() > 0;

        foreach (var newsElement in newsElements)
        {
            var element = await Ilidza.ParseNews(newsElement, (ScrapingAndParsingMethods.NewsModelCategory)url.Key);
            await Task.Delay(500);

            if(element.Date < dateTimeLimit)
            {
                controlVariable = false;
                break;
            }

            listOfIlidzaNews.Add(element);
        }
    }
}


string urlRad = "https://www.rad.com.ba/aktuelno.htm";
string responseRad =ScrapingAndParsingMethods.CallUrl(urlRad).Result;
var newsElementsRad = KJKPRad.ParseHtml(responseRad);
var listOfRadNews = newsElementsRad.Select(x => KJKPRad.ParseNews(x))
                                   .Where(x => x.CategoryId > 0)
                                   .ToList();



