using Lottery.Engine.Contract;
using Lottery.Storage.Contract;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.Engine
{
    class JokerLastOccurenceScoreProcessor : IScoreProcessor
    {
        public ProcessingResultGroup Process(HistoricalDataEntry[] dataEntries)
        {
            Dictionary<int, int> jokerLastOccurenceIndexes = CalculateLastOccurenceIndexes(dataEntries, 0, 1, 20);
            Dictionary<int, int> numberLastOccurenceIndexes = CalculateLastOccurenceIndexes(dataEntries, 1, 5, 40);

            List<ProcessingResultEntry> jokerResultEntries = ConvertToResult(jokerLastOccurenceIndexes, isJoker: true);
            List<ProcessingResultEntry> numberResultEntries = ConvertToResult(numberLastOccurenceIndexes, isJoker: false);

            return new ProcessingResultGroup("LastOccurence", jokerResultEntries.Concat(numberResultEntries).ToArray());
        }

        private Dictionary<int, int> CalculateLastOccurenceIndexes(HistoricalDataEntry[] dataEntries, int from, int count, int endRange)
        {
            Dictionary<int, int> lastOccurenceIndexes = Enumerable.Range(1, endRange).ToDictionary(number => number, _ => -1);
                
            for (int index = 0; index < dataEntries.Length; index++)
            {
                foreach (int number in dataEntries[index].Numbers.Skip(from).Take(count))
                {
                    lastOccurenceIndexes[number] = index;
                }
            }

            return lastOccurenceIndexes;
        }

        private List<ProcessingResultEntry> ConvertToResult(Dictionary<int, int> lastOccurenceIndexes, bool isJoker)
        {
            int minIndex = lastOccurenceIndexes.Values.Min();

            return lastOccurenceIndexes
                .OrderBy(pair => pair.Key)
                .Select(pair => new ProcessingResultEntry(pair.Key, isJoker ? NumberType.Joker : NumberType.Regular, pair.Value - minIndex))
                .ToList();
        }
    }
}
