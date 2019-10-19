using Lottery.Storage.Contract;

namespace Lottery.Engine.Contract
{
    public class ProcessingResult
    {
        public ProcessingResult(HistoricalData historicalData, params ProcessingResultEntry[] resultEntries)
        {
            DataType = historicalData.HistoricalDataType;
            Source = historicalData.Entries;
            Result = resultEntries;
        }

        public HistoricalDataType DataType { get; }

        public HistoricalDataEntry[] Source { get; }

        public ProcessingResultEntry[] Result { get; }

        public override string ToString()
        {
            return $"{DataType} ({Source.Length} source entries) - {Result.Length} result entries";
        }
    }

    public class ProcessingResultEntry
    {
        public int Number { get; }

        public NumberType Type { get; }

        public double Score { get; }
    }

    public enum NumberType
    {
        None = 0,
        Regular = 1,
        Joker = 2,
    }
}
