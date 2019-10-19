using Lottery.Storage.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.Storage
{
    class JokerHistoricalDataReader : IHistoricalDataReader
    {
        private readonly ICsvFileReader csvFileReader;

        public JokerHistoricalDataReader(ICsvFileReader csvFileReader)
        {
            this.csvFileReader = csvFileReader;
        }

        public HistoricalData ReadAll(string sourceFilePath)
        {
            List<HistoricalDataEntry> entries = new List<HistoricalDataEntry>();

            foreach (string[] rawValues in csvFileReader.ReadContent(sourceFilePath))
            {
                DateTime timestamp = DateTime.Parse(rawValues.First());
                int[] numbers = rawValues.Skip(1).Select(rawValue => int.Parse(rawValue)).ToArray();

                entries.Add(new HistoricalDataEntry(timestamp, numbers));
            }

            return new HistoricalData(HistoricalDataType.Joker, entries.ToArray());
        }
    }
}
