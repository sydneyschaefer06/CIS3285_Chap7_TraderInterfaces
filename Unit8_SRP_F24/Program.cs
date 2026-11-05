using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;


class Program
{
    static async Task Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();
        ITradeValidator tradeValidator = new SimpleTradeValidator(logger);
        ITradeDataProvider tradeDataProvider = new RestfulTradeDataProvider(
            "https://unit9trader.azurewebsites.net/api/TradeData", logger);
        ITradeMapper tradeMapper = new SimpleTradeMapper();
        ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
        ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

        var tradeProcessor = new TradeProcessor(tradeDataProvider, tradeParser, tradeStorage);
        await tradeProcessor.ProcessTradesAsync();

        //Console.ReadKey();
    }
}
