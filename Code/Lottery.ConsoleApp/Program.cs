using Lottery.Lib.Engine;
using Lottery.Lib.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lottery.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            INumbersScoreEngine engine = new JokerNumbersScoreEngine();

            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "..", "..", "Data", "joker.csv");

            List<Number[]> inputData = new List<Number[]>();
            
            using (var reader = new CsvFileReader(new StreamReader(File.OpenRead(filePath))))
            {
                string[] values = reader.ReadLine();

                if (values != null)
                    values = reader.ReadLine();

                while (values != null)
                {
                    List<Number> numberSeries = new List<Number>();

                    numberSeries.Add(values.Skip(1).Select(value => int.Parse(value)).First().ToJoker());
                    numberSeries.AddRange(values.Skip(2).Select(value => int.Parse(value)).Select(value => value.ToNumber()));

                    inputData.Add(numberSeries.ToArray());

                    values = reader.ReadLine();
                }
            }

            foreach (Number[] numberSeries in inputData.Skip(inputData.Count - 6))
                Console.WriteLine(string.Join(", ", numberSeries.Select(number => number.ToString().PadLeft(3))));

            var scores = engine.Process(inputData);

            foreach (var score in scores)
                Console.WriteLine(score.Item1);

            Console.WriteLine();
        }
    }
}
