using Lottery.Infrastructure.Containers;
using System.Windows;

namespace Lottery.Wpf
{
    public partial class App : Application
    {
        public IContainer Container { get; }

        public App()
        {
            Container = new Container();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Container.Resolve<IContainer>();
        }
    }
}
