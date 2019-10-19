using System;

namespace Lottery.Storage.Contract
{
    public class HistoricalData
    {
        public HistoricalData(HistoricalDataType historicalDataType, params HistoricalDataEntry[] entries)
        {
            HistoricalDataType = historicalDataType;
            Entries = entries;
        }

        public HistoricalDataType HistoricalDataType { get; }

        public HistoricalDataEntry[] Entries { get; }

        public override string ToString()
        {
            return $"{HistoricalDataType} ({Entries.Length} entries)";
        }
    }

    public class HistoricalDataEntry
    {
        public HistoricalDataEntry(DateTime timestamp, int[] numbers)
        {
            Timestamp = timestamp;
            Numbers = numbers;
        }

        public DateTime Timestamp { get; set; }

        public int[] Numbers { get; set; }
    }
}
