using System;
using System.Reflection;

using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class StreamTradeDataProviderTests
{

    private int countStrings(IEnumerable<string> collectionOfStrings)
    {
        // count the trades
        int count = 0;
        foreach (var trade in collectionOfStrings)
        {
            count++;
        }
        return count;
    }


    [TestMethod()]
    public async Task TestLocaFile()
    {
        //Arrange
        ILogger logger = new ConsoleLogger();
        Stream tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.trades_1good.txt")
            ?? throw new InvalidOperationException("Embedded resource 'Unit8_SRP_F24Tests.trades_1good.txt' was not found.");

        ITradeDataProvider tradeProvider = new StreamTradeDataProvider(tradeStream, logger);

        //Act
        IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

        //Assert
        Assert.AreEqual(countStrings(trades), 1);
    }

    [TestMethod()]
    public async Task TestLocalFileFiveTrades()
    {
        //Arrange
        ILogger logger = new ConsoleLogger();
        Stream tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.trades_5good.txt")
            ?? throw new InvalidOperationException("Embedded resource 'Unit8_SRP_F24Tests.trades_5good.txt' was not found.");

        ITradeDataProvider tradeProvider = new StreamTradeDataProvider(tradeStream, logger);

        //Act
        IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

        //Assert
        Assert.AreEqual(5, countStrings(trades));
    }

}
