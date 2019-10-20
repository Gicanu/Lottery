using Lottery.Engine.Contract;
using Lottery.Storage.Contract;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.Engine
{
    class JokerFrequencyScoreProcessor : IScoreProcessor
    {
        public ProcessingResultGroup Process(HistoricalDataEntry[] dataEntries)
        {
            Dictionary<int, int> jokerFrequencies = CalculateFrequencies(dataEntries, 0, 1, 20);
            Dictionary<int, int> numberFrequencies = CalculateFrequencies(dataEntries, 1, 5, 40);

            List<ProcessingResultEntry> jokerResultEntries = Enumerable.Range(1, 20).Select(number => new ProcessingResultEntry(number, jokerFrequencies[number], NumberType.Joker)).ToList();
            List<ProcessingResultEntry> numberResultEntries = Enumerable.Range(1, 40).Select(number => new ProcessingResultEntry(number, numberFrequencies[number], NumberType.Regular)).ToList();

            return new ProcessingResultGroup("Frequency", jokerResultEntries.Concat(numberResultEntries).ToArray());
        }

        private Dictionary<int, int> CalculateFrequencies(HistoricalDataEntry[] dataEntries, int from, int count, int endRange)
        {
            Dictionary<int, int> frequencies = dataEntries
                .SelectMany(dataEntry => dataEntry.Numbers.Skip(from).Take(count))
                .ToLookup(number => number)
                .ToDictionary(group => group.Key, group => group.Count());

            for (int number = 1; number <= endRange; number++)
            {
                if (!frequencies.ContainsKey(number))
                    frequencies.Add(number, 0);
            }

            return frequencies;
        }
    }
}
