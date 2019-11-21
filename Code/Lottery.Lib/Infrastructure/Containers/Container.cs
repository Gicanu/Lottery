using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace Lottery.Infrastructure.Containers
{
    public class Container : IContainer
    {
        private readonly Lazy<IUnityContainer> unityContainer;

        public Container()
        {
            unityContainer = new Lazy<IUnityContainer>(() => Initialize(typeof(Container).Assembly), isThreadSafe: true);
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface)unityContainer.Value.Resolve(typeof(TInterface));
        }

        private IUnityContainer Initialize(Assembly assembly)
        {
            UnityContainer container = new UnityContainer();

            Dictionary<Type, ResolvableAttribute> resolvableTypes = new Dictionary<Type, ResolvableAttribute>();

            foreach (Type type in assembly.GetTypes())
            {
                ResolvableAttribute attribute = type.GetCustomAttribute<ResolvableAttribute>();

                if (attribute != null)
                    resolvableTypes.Add(type, attribute);
            }

            foreach (KeyValuePair<Type, ResolvableAttribute> pair in resolvableTypes.Where(pair => pair.Key.IsClass && !pair.Key.IsAbstract))
            {
                Type @class = pair.Key;
                ResolvableAttribute @attribute = pair.Value;

                List<Type> interfaces = @class.GetInterfaces().Where(type => resolvableTypes.ContainsKey(type)).ToList();

                if (interfaces.Count == 0)
                    continue;

                if (interfaces.Count == 1)
                    container.RegisterType(interfaces[0], @class);
                else
                    interfaces.ForEach(@interface => container.RegisterType(@interface, @class, @attribute.Name));
            }

            container.RegisterInstance<IContainer>(this);

            return container;
        }
    }
}
