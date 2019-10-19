using Lottery.Engine.Contract;
using Lottery.Storage.Contract;

namespace Lottery.Engine
{
    class NumbersProcessor : INumbersProcessor
    {
        public ProcessingResult Process(HistoricalData historicalData)
        {
            return new ProcessingResult(historicalData);
        }
    }
}
