namespace Lottery.Storage.Contract
{
    public interface IHistoricalDataReader
    {
        HistoricalData ReadAll(string sourceFilePath);
    }
}
