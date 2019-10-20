using Lottery.Engine.Contract;
using Lottery.Format.Contract;
using Lottery.Storage.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.Format
{
    class TabularResultFormatter : IResultFormatter
    {
        public string[] Format(ProcessingResult processingResult)
        {
            switch (processingResult.DataType)
            {
                case HistoricalDataType.Joker:
                    return GenerateRows(GenerateJokerHeader(processingResult.ResultGroups).Concat(GenerateJokerContent(processingResult.ResultGroups)));

                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<string[]> GenerateJokerHeader(ProcessingResultGroup[] groups)
        {
            yield return groups
                .Aggregate(new LinkedList<string>(), (result, group) => result.AddLast(group.Key, string.Empty))
                .ToArray();

            yield return groups
                .Aggregate(new LinkedList<string>(), (result, _) => result.AddLast("---", string.Empty))
                .ToArray();
        }

        private IEnumerable<string[]> GenerateJokerContent(ProcessingResultGroup[] groups)
        {
            List<string[]> itemsList = new List<string[]>(groups.Length * 2);

            for (int index = 0; index < groups.Length; index++)
            {
                itemsList.Add(groups[index].Entries
                    .Where(entry => entry.Type == NumberType.Joker)
                    .OrderByDescending(entry => entry.Score)
                    .Select((entry, index) => $"{index + 1}. {entry}")
                    .ToArray());

                itemsList.Add(groups[index].Entries
                    .Where(entry => entry.Type == NumberType.Regular)
                    .OrderByDescending(entry => entry.Score)
                    .Select((entry, index) => $"{index + 1}. {entry}")
                    .ToArray());
            }

            for (int index = 0; index < itemsList.Select(items => items.Length).Max(); index++)
            {
                List<string> rowItems = new List<string>(itemsList.Count);

                foreach (string[] items in itemsList)
                    rowItems.Add(index < items.Length ? items[index] : string.Empty);

                yield return rowItems.ToArray();
            }
        }

        private string[] GenerateRows(IEnumerable<string[]> rowItems)
        {
            int maxItemLength = rowItems.SelectMany(item => item).Select(item => item.Length).Max();

            return rowItems
                .Select(items => items.Aggregate(string.Empty, (result, item) => string.Concat(result, item, new string(' ', maxItemLength + 1 - item.Length))))
                .ToArray();
        }
    }

    static class LinkedListExtensions
    {
        public static LinkedList<T> AddLast<T>(this LinkedList<T> list, params T[] items)
        {
            foreach (T item in items)
                list.AddLast(item);

            return list;
        }
    }
}
