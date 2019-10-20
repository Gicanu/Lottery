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
            Dictionary<int, int> jokerFrequencies = CalculateFrequencies(dataEntries, 5, 1, 20);
            Dictionary<int, int> numberFrequencies = CalculateFrequencies(dataEntries, 0, 5, 45);

            List<ProcessingResultEntry> jokerResultEntries = ConvertToResult(jokerFrequencies, isJoker: true);
            List<ProcessingResultEntry> numberResultEntries = ConvertToResult(numberFrequencies, isJoker: false);

            return new ProcessingResultGroup("Frequency", jokerResultEntries.Concat(numberResultEntries).ToArray());
        }

        private Dictionary<int, int> CalculateFrequencies(HistoricalDataEntry[] dataEntries, int from, int count, int endRange)
        {
            Dictionary<int, int> frequencies = Enumerable.Range(1, endRange).ToDictionary(number => number, _ => 0);

            foreach (HistoricalDataEntry dataEntry in dataEntries)
            {
                foreach (int number in dataEntry.Numbers.Skip(from).Take(count))
                    frequencies[number]++;
            }

            return frequencies;
        }

        private List<ProcessingResultEntry> ConvertToResult(Dictionary<int, int> frequencies, bool isJoker)
        {
            int minIndex = frequencies.Values.Min();

            return frequencies
                .OrderBy(pair => pair.Key)
                .Select(pair => new ProcessingResultEntry(pair.Key, isJoker ? NumberType.Joker : NumberType.Regular, pair.Value - minIndex))
                .ToList();
        }
    }
}
