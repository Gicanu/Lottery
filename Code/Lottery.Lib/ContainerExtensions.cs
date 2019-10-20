using Lottery.Engine;
using Lottery.Engine.Contract;
using Lottery.Format;
using Lottery.Format.Contract;
using Lottery.Storage;
using Lottery.Storage.Contract;
using Unity;

namespace Lottery
{
    public static class ContainerExtensions
    {
        public static IUnityContainer RegisterAllTypes(this IUnityContainer container)
        {
            RegisterAllStorageTypes(container);
            RegisterAllEngineTypes(container);
            RegisterAllFormatTypes(container);

            return container;
        }

        private static void RegisterAllStorageTypes(IUnityContainer container)
        {
            IUnityContainer storageContainer = container
                .CreateChildContainer()
                .RegisterType<ICsvFileReader, CsvFileReader>();

            IUnityContainer jokerStorageContainer = storageContainer
                .CreateChildContainer()
                .RegisterType<IHistoricalDataReader, JokerHistoricalDataReader>();

            container
                .RegisterFactory<IHistoricalDataReader>(HistoricalDataType.Joker.ToString(), _ => jokerStorageContainer.Resolve<IHistoricalDataReader>());
        }

        public static void RegisterAllEngineTypes(IUnityContainer container)
        {
            IUnityContainer engineContainer = container
                .CreateChildContainer()
                .RegisterType<IDataProcessor, DataProcessor>();

            IUnityContainer jokerEngineContainer = engineContainer
                .CreateChildContainer()
                .RegisterType<IScoreProcessor, JokerFrequencyScoreProcessor>("Joker|Frequency")
                .RegisterType<IScoreProcessor, JokerLastOccurenceScoreProcessor>("Joker|LastOccurence");

            container
                .RegisterFactory<IDataProcessor>(HistoricalDataType.Joker.ToString(), _ => jokerEngineContainer.Resolve<IDataProcessor>());
        }

        public static void RegisterAllFormatTypes(IUnityContainer container)
        {
            container
                .RegisterType<IResultFormatter, TabularResultFormatter>("Tabular");
        }
    }
}
