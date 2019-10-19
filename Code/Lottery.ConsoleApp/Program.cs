using Lottery.Storage.Contract;
using System;
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

            IUnityContainer unityContainer = new UnityContainer().RegisterAllLibTypes();

            IHistoricalDataReader reader = unityContainer.Resolve<IHistoricalDataReader>(historicalDataType.ToString());
            HistoricalData historicalData = reader.ReadAll(arguments.InputFilePath);

            Console.WriteLine(historicalData);
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
    }
}
