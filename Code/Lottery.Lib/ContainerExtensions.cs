using Lottery.Storage;
using Lottery.Storage.Contract;
using Unity;
using Unity.Injection;

namespace Lottery
{
    public static class ContainerExtensions
    {
        public static IUnityContainer RegisterAllLibTypes(this IUnityContainer container)
        {
            IUnityContainer storageContainer = container
                .CreateChildContainer()
                .RegisterType<ICsvFileReader, CsvFileReader>()
                .RegisterType<IHistoricalDataReader, JokerHistoricalDataReader>(HistoricalDataType.Joker.ToString());

            return container
                .RegisterFactory<IHistoricalDataReader>(HistoricalDataType.Joker.ToString(), _ => storageContainer.Resolve<IHistoricalDataReader>(HistoricalDataType.Joker.ToString()));
        }
    }
}
