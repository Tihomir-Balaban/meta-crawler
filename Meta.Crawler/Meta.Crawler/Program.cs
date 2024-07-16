using Meta.Crawler.Service;
using Meta.Crawler.Utilities;

class Program
{
    static async Task Main(string[] args)
    {
        Logger.Init();

        var service = new CrawlService();

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a command.");
            Logger.LogWarning("Please provide a command.");
            return;
        }

        switch (args[0])
        {
            case "list":
                var channels = await service.GetChannelsAsync();
                foreach (var channel in channels)
                {
                    Console.WriteLine($"{channel.Number} {channel.Name}");
                }
                break;

            case "channel":
                if (args.Length < 2 || !int.TryParse(args[1], out int channelNumber))
                {
                    Console.WriteLine("Please provide a valid channel number.");
                    return;
                }
                var listings = await service.GetChannelListingsAsync(channelNumber);
                foreach (var listing in listings)
                {
                    Console.WriteLine($"{listing.Timestamp} {listing.Title}");
                }
                break;

            case "json":
                if (args.Length < 2 || !int.TryParse(args[1], out int jsonChannelNumber))
                {
                    Console.WriteLine("Please provide a valid channel number.");
                    return;
                }
                await service.SaveChannelListingsToJsonAsync(jsonChannelNumber);
                Console.WriteLine($"{jsonChannelNumber}.json");
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
}
