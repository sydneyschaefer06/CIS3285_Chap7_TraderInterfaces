using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class URLTradeDataProvider : ITradeDataProvider
{
    private readonly string url;        // URL to fetch trade data from
    private readonly ILogger logger;    //  Logger to log messages

    public URLTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from URL: " + url);
        // -------------- Add your code here to fetch trade data from the URL and return it as an IEnumerable<string> --------------
        List<string> tradeData = new List<string>();
        using (HttpClient client = new HttpClient())
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }
            // set up a Stream and StreamReader to access the data
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (StreamReader reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    tradeData.Add(line);
                };
                // Read each line of the text file using reader.ReadLine()
                // Read until the reader returns a null line
                // Add each line to the tradeData list
            }
        }
        return tradeData;
    }
}
