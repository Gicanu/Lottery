using System;

namespace Lottery.Infrastructure.Containers
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ResolvableAttribute : Attribute
    {        
        public ResolvableAttribute()
        {
        }

        public string Name { get; internal set; }
    }
}
