namespace Lottery.Infrastructure.Containers
{
    public interface IContainer
    {
        TInterface Resolve<TInterface>();
    }
}
