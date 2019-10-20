using Lottery.Storage.Contract;

namespace Lottery.Engine.Contract
{
    public class ProcessingResult
    {
        public ProcessingResult(HistoricalData historicalData, params ProcessingResultGroup[] resultGroups)
        {
            DataType = historicalData.HistoricalDataType;
            Source = historicalData.Entries;
            ResultGroups = resultGroups;
        }

        public HistoricalDataType DataType { get; }

        public HistoricalDataEntry[] Source { get; }

        public ProcessingResultGroup[] ResultGroups { get; }

        public override string ToString()
        {
            return $"{DataType} ({Source.Length} source entries) - {ResultGroups.Length} result groups";
        }
    }
}
