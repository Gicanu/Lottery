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
                .RegisterType<ICsvFileReader, CsvFileReader>()
                .RegisterType<IHistoricalDataReader, JokerHistoricalDataReader>(HistoricalDataType.Joker.ToString());

            return container
                .RegisterFactory<IHistoricalDataReader>(HistoricalDataType.Joker.ToString(), _ => storageContainer.Resolve<IHistoricalDataReader>(HistoricalDataType.Joker.ToString()));
        }

        public static IUnityContainer RegisterAllEngineTypes(this IUnityContainer container)
        {
            IUnityContainer engineContainer = container
                .CreateChildContainer()
                .RegisterType<INumbersProcessor, NumbersProcessor>();

            return container
                .RegisterFactory<INumbersProcessor>(_ => engineContainer.Resolve<INumbersProcessor>());
        }
    }
}
