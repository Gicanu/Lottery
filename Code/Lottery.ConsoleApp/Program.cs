using Lottery.Engine.Contract;
using Lottery.Storage.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Lottery.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramArguments arguments = ParseArguments(args);

            if (arguments == null)
            {
                PrintUsageMessage();
                return;
            }

            HistoricalDataType historicalDataType = GetHistoricalDataType(arguments);

            if (historicalDataType == HistoricalDataType.None)
            {
                PrintUsageMessage();
                return;
            }

            IUnityContainer unityContainer = new UnityContainer()
                .RegisterAllStorageTypes()
                .RegisterAllEngineTypes();

            IHistoricalDataReader reader = unityContainer.Resolve<IHistoricalDataReader>(historicalDataType.ToString());
            HistoricalData historicalData = reader.ReadAll(arguments.InputFilePath);
            
            IDataProcessor dataProcessor = unityContainer.Resolve<IDataProcessor>(HistoricalDataType.Joker.ToString());
            ProcessingResult result = dataProcessor.Process(historicalData);

            PrintResult(result);
        }

        private static HistoricalDataType GetHistoricalDataType(ProgramArguments arguments)
        {
            switch (arguments.InputType)
            {
                case "Joker":
                    return HistoricalDataType.Joker;

                default:
                    return HistoricalDataType.None;
            }
        }

        private static ProgramArguments ParseArguments(string[] args)
        {
            if (args.Length != 3)
                return null;

            string inputTypeArgument = args.FirstOrDefault(argument => argument.StartsWith("-InputType", StringComparison.InvariantCultureIgnoreCase));
            string inputTypeArgumentValue = null;
            string inputFilePathArgumentValue = null;

            bool inputTypeArgumentValueExpected = false;

            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg.Equals("-InputType", StringComparison.InvariantCultureIgnoreCase))
                    {
                        inputTypeArgumentValueExpected = true;
                        continue;
                    }
                }
                else
                {
                    if (inputTypeArgumentValueExpected)
                    {
                        if (string.IsNullOrEmpty(inputTypeArgumentValue))
                        {
                            inputTypeArgumentValue = arg;
                            inputTypeArgumentValueExpected = false;
                            continue;
                        }
                    }
                    else if (string.IsNullOrEmpty(inputFilePathArgumentValue))
                    {
                        inputFilePathArgumentValue = arg;
                        continue;
                    }
                }

                return null;
            }

            return new ProgramArguments
            {
                InputType = inputTypeArgumentValue,
                InputFilePath = inputFilePathArgumentValue,
            };
        }

        private static void PrintUsageMessage()
        {
            Console.WriteLine("Usage: Lottery -InputType 6of49|Joker|5of40 InputFilePath ");
        }

        private static void PrintResult(ProcessingResult processingResult)
        {
            foreach (ProcessingResultGroup group in processingResult.ResultGroups)
            {
                ProcessingResultEntry[] jokerEntries = group.Entries.Where(entry => entry.Type == NumberType.Joker).OrderByDescending(entry => entry.Score).ToArray();
                ProcessingResultEntry[] numberEntries = group.Entries.Where(entry => entry.Type == NumberType.Regular).OrderByDescending(entry => entry.Score).ToArray();

                List<(string, string)> rows = new List<(string, string)>();

                Console.WriteLine(group.Key);
                Console.WriteLine();

                for (int i = 0; i < Math.Max(jokerEntries.Length, numberEntries.Length); i++)
                {
                    string value1 = string.Empty;

                    if (i < jokerEntries.Length)
                        value1 = $"{jokerEntries[i].ToString()} ";

                    string value2 = string.Empty;

                    if (i < numberEntries.Length)
                        value2 = numberEntries[i].ToString();

                    string separator = new string(' ', 12 - value1.Length);

                    Console.WriteLine(string.Concat(value1, separator, value2));
                    Console.WriteLine();
                }
            }
        }
    }
}
