using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Lottery.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            Console.WriteLine("Reading data ...");

            var extractions = new List<Extraction>();

            using (var reader = new StreamReader(File.OpenRead(args[0])))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    string[] rawValues = line.Split(", ");

                    DateTime date = DateTime.ParseExact(rawValues[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int[] numbers = rawValues.Skip(1).Select(rawValue => int.Parse(rawValue)).ToArray();

                    extractions.Add(new Extraction { Date = date, Numbers = numbers });

                    line = reader.ReadLine();
                }
            }

            Console.WriteLine("Data read.");

            Console.WriteLine("Processing data ...");

            var jokerStatistics = GenerateDefaultDictionary<Statistics>(20);
            var numberStatistics = GenerateDefaultDictionary<Statistics>(45);

            DateTime today = DateTime.Now.Date;
            
            Extraction[] orderedExtractions = extractions.OrderBy(extraction => extraction.Date).ToArray();

            for (int i = 0; i < orderedExtractions.Length; i++)
            {
                Extraction extraction = orderedExtractions[i];

                for (int j = 0; j < extraction.Numbers.Length; j++)
                {
                    Statistics statistics;

                    if (j < 5)
                        statistics = numberStatistics[extraction.Numbers[j]];
                    else
                        statistics = jokerStatistics[extraction.Numbers[j]];

                    statistics.Count++;
                    statistics.LastOccurance = orderedExtractions.Length - i - 1;
                }
            }

            FillPercentages(jokerStatistics, orderedExtractions.Length, extractions.Count);
            FillPercentages(numberStatistics, orderedExtractions.Length, extractions.Count * 5);

            var jokerScore1 = jokerStatistics.Select(pair => (pair.Key, pair.Value.Count)).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            var jokerScore2 = jokerStatistics.Select(pair => (pair.Key, pair.Value.LastOccurance)).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            
            var numberScore1 = numberStatistics.Select(pair => (pair.Key, pair.Value.Count)).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            var numberScore2 = numberStatistics.Select(pair => (pair.Key, pair.Value.LastOccurance)).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            Console.WriteLine("Data processed.");

            Console.WriteLine($"Total: {extractions.Count}");
            Console.WriteLine();

            string[] jokerTexts1 = jokerScore1.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:F0}").ToArray();
            string[] jokerTexts2 = jokerScore2.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:F0}").ToArray();

            string[] numberTexts1 = numberScore1.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:F0}").ToArray();
            string[] numberTexts2 = numberScore2.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:F0}").ToArray();

            DisplayResults(jokerTexts1, jokerTexts2, numberTexts1, numberTexts2);
        }

        private static void FillPercentages(Dictionary<int, Statistics> jokerStatistics, double totalDays, double totalCount)
        {
            foreach (var pair in jokerStatistics)
            {
                pair.Value.CountPerTotal = 100.0 * pair.Value.Count / totalCount;
                pair.Value.LastOccurancePerTotal = 100.0 * pair.Value.LastOccurance / totalDays;
            }
        }

        private static Dictionary<int, T> GenerateDefaultDictionary<T>(int maxKeyValue) where T : new()
        {
            var dictionary = new Dictionary<int, T>(maxKeyValue);

            for (int keyValue = 1; keyValue <= maxKeyValue; keyValue++)
                dictionary.Add(keyValue, new T());

            return dictionary;
        }

        private static void DisplayResults(params string[][] textLists)
        {
            int maxCount = textLists.Max(textList => textList.Length);
            int cellSize = 20;

            var result = new StringBuilder();

            for (int index = 0; index < maxCount; index++)
            {
                foreach (string[] textList in textLists)
                {
                    if (index >= textList.Length)
                    {
                        result.Append(new string(' ', cellSize));
                    }
                    else
                    {
                        result.Append(textList[index]);

                        if (textList[index].Length < cellSize)
                            result.Append(new string(' ', cellSize - textList[index].Length));
                    }
                }

                result.AppendLine();
            }

            Console.WriteLine(result);
        }
    }
}
