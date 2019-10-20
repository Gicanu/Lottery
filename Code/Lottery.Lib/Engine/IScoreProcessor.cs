using Lottery.Engine.Contract;
using Lottery.Storage.Contract;

namespace Lottery.Engine
{
    public interface IScoreProcessor
    {
        ProcessingResultGroup Process(HistoricalDataEntry[] dataEntries);
    }
}
