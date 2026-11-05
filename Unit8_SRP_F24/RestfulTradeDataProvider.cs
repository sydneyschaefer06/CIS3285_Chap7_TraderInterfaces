using System;
using System.Text.Json;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class RestfulTradeDataProvider : ITradeDataProvider
{
    string url;
    ILogger logger;

    public RestfulTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from Restful API: " + url);

        using (HttpClient client = new HttpClient())
        using (HttpResponseMessage response = await client.GetAsync(url))
        {
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(content)
                ?? throw new InvalidOperationException("The trade service returned an empty response.");
        }
    }
}
