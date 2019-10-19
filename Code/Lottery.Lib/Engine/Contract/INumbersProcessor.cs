using Lottery.Storage.Contract;

namespace Lottery.Engine.Contract
{
    public interface INumbersProcessor
    {
        ProcessingResult Process(HistoricalData historicalData);
    }
}
