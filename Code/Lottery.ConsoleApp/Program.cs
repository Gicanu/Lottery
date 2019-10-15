using Lottery.Engine;
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
            
            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                string row = reader.ReadLine();

                if (row != null)
                    row = reader.ReadLine();

                while (row != null)
                {
                    string[] values = row.Split(',');
                    Number[] numbers = values.Skip(1).Select(value => int.Parse(value)).Select((value, index) => new Number(value, index == values.Length - 1 ? "Joker" : "")).ToArray();

                    inputData.Add(numbers);

                    row = reader.ReadLine();
                }
            }

            var scores = engine.Process(inputData);

            foreach (var score in scores)
                Console.WriteLine(score.Item1);

            Console.WriteLine();
        }
    }
}
