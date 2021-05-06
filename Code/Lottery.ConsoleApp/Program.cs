using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lottery.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            Console.WriteLine("Reading data ...");

            var valuesList = new List<(DateTime, int[])>();

            using (var reader = new StreamReader(File.OpenRead(args[0])))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    string[] rawValues = line.Split(", ");

                    DateTime date = DateTime.ParseExact(rawValues[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int[] numbers = rawValues.Skip(1).Select(rawValue => int.Parse(rawValue)).ToArray();

                    valuesList.Add((date, numbers));

                    line = reader.ReadLine();
                }
            }

            Console.WriteLine("Data read.");

            Console.WriteLine("Processing data ...");

            var jokerOccurences = new Dictionary<int, int>();
            var numbersOccurences = new Dictionary<int, int>();

            foreach ((DateTime date, int[] numbers) in valuesList)
            {
                if (!jokerOccurences.ContainsKey(numbers[5]))
                    jokerOccurences.Add(numbers[5], 1);
                else
                    jokerOccurences[numbers[5]]++;

                foreach (int number in numbers.Take(5))
                {
                    if (!numbersOccurences.ContainsKey(number))
                        numbersOccurences.Add(number, 1);
                    else
                        numbersOccurences[number]++;
                }
            }

            Console.WriteLine("Data processed.");

            string[] jokerOccureancesTexts = jokerOccurences.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:D3}").ToArray();
            string[] numberOccureancesTexts = numbersOccurences.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key:D2} - {pair.Value:D3}").ToArray();

            Console.WriteLine("Joker    \tNumbers");

            for (int i = 0; i < numberOccureancesTexts.Length; i++)
            {
                if (i < jokerOccureancesTexts.Length)
                    Console.WriteLine(string.Concat(jokerOccureancesTexts[i], "\t", numberOccureancesTexts[i]));
                else
                    Console.WriteLine(string.Concat(new string(' ', 8), "\t", numberOccureancesTexts[i]));
            }
        }
    }
}
