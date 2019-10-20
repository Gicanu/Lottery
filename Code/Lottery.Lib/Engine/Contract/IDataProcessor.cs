using Lottery.Storage.Contract;

namespace Lottery.Engine.Contract
{
    public interface IDataProcessor
    {
        ProcessingResult Process(HistoricalData historicalData);
    }
}
