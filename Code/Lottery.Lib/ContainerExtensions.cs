using Lottery.Engine;
using Lottery.Engine.Contract;
using Lottery.Storage;
using Lottery.Storage.Contract;
using Unity;

namespace Lottery
{
    public static class ContainerExtensions
    {
        public static IUnityContainer RegisterAllStorageTypes(this IUnityContainer container)
        {
            IUnityContainer storageContainer = container
                .CreateChildContainer()
                .RegisterType<ICsvFileReader, CsvFileReader>();

            IUnityContainer jokerStorageContainer = storageContainer
                .CreateChildContainer()
                .RegisterType<IHistoricalDataReader, JokerHistoricalDataReader>();

            return container
                .RegisterFactory<IHistoricalDataReader>(HistoricalDataType.Joker.ToString(), _ => jokerStorageContainer.Resolve<IHistoricalDataReader>());
        }

        public static IUnityContainer RegisterAllEngineTypes(this IUnityContainer container)
        {
            IUnityContainer engineContainer = container
                .CreateChildContainer()
                .RegisterType<IDataProcessor, DataProcessor>();

            IUnityContainer jokerEngineContainer = engineContainer
                .CreateChildContainer()
                .RegisterType<IScoreProcessor, FrequencyScoreJokerProcessor>("Frequency_Joker");

            return container
                .RegisterFactory<IDataProcessor>(HistoricalDataType.Joker.ToString(), _ => jokerEngineContainer.Resolve<IDataProcessor>());
        }
    }
}
