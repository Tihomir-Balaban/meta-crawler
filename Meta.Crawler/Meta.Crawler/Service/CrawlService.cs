using HtmlAgilityPack;
using Meta.Crawler.Models;
using Meta.Crawler.Utilities;
using Newtonsoft.Json;
using System.Threading.Channels;
using System.Xml.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Meta.Crawler.Service;

internal sealed class CrawlService
{
    private static readonly HttpClient client = new();
    private static HtmlDocument doc = new();
    private Task<string> response;
    private LoadingAnimation loadingAnimation = new();

    internal CrawlService()
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");

        response = client.GetStringAsync("https://www.telered.com.ar/buscador-grilla");
    }

    internal async Task<List<TvChannel>> GetChannelsAsync()
    {
        Logger.LogInformation("Getting a list of all Channels");
        Logger.LogInformation("Started Loading the HTML file to be parsed");
        loadingAnimation.Start();

        doc.LoadHtml(await response);

        loadingAnimation.Stop();
        Logger.LogInformation("Loaded the HTML file and Parsing");

        var channels = new List<TvChannel>();

        var channelNodes = doc.DocumentNode.SelectNodes("//div[@class='chtitle']//div[@class='infoch']");

        if (channelNodes != null)
        {
            var node = channelNodes
                .Skip(1)
                .FirstOrDefault();

            var cellNumber = node.SelectNodes("//div[@class='vermas']//span[@class='chnum']");
            var cellName = node.SelectNodes("//span[@class='chname']");

            if (cellNumber != null && cellName != null)
            {
                for (int i = 0; i < cellNumber.Count(); i++)
                {
                    var channel = new TvChannel
                    {
                        Number = int.Parse(cellNumber[i].InnerText.Trim()),
                        Name = cellName[i].InnerText.Trim()
                    };

                    channels.Add(channel);
                }
            }
        }

        Logger.LogInformation($"{channels.Count()} channels found.");
        return channels;
    }

    internal async Task<List<Listing>> GetChannelListingsAsync(int channelNumber)
    {
        Logger.LogInformation("Getting listings by Channel");
        Logger.LogInformation("Started Loading the HTML file to be parsed");
        loadingAnimation.Start();

        var listings = new List<Listing>();

        doc.LoadHtml(await response);

        loadingAnimation.Stop();
        Logger.LogInformation("Loaded the HTML file and Parsing");


        var listingNodes = doc.DocumentNode.SelectNodes("//li[contains(concat(' ',normalize-space(@class),' '),' chrow ')]");

        if (listingNodes != null)
        {
            var node = listingNodes
                .Skip(1)
                .FirstOrDefault();

            var channelNodes = node
                .SelectNodes("//span[@class='chnum']")
                .Where(a => a.InnerText.Equals($"{channelNumber}"))
                .FirstOrDefault()
                .ParentNode.ParentNode.ParentNode.NextSibling.NextSibling.ChildNodes;

            foreach (var channelNode in channelNodes.Skip(2).SkipLast(1))
            {
                var listing = new Listing
                {
                    Timestamp = channelNode.ChildNodes[1].ChildNodes[3].InnerText.Trim(),
                    Title = channelNode.ChildNodes[1].ChildNodes[1].InnerText.Trim()
                };

                listings.Add(listing);
            }
        }

        Logger.LogInformation($"{listings.Count()} listings found.");
        return listings;
    }

    internal async Task SaveChannelListingsToJsonAsync(int channelNumber)
    {
        var listings = await GetChannelListingsAsync(channelNumber);

        Logger.LogInformation("Converting listings to json");
        var json = JsonConvert.SerializeObject(listings, Formatting.Indented);

        Logger.LogInformation("Saving them to the root of this exe directory");
        await File.WriteAllTextAsync($"{channelNumber}.json", json);
    }
}
