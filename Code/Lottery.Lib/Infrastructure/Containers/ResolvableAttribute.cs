using System;

namespace Lottery.Infrastructure.Containers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ResolvableAttribute : Attribute
    {        
        public ResolvableAttribute()
        {
        }

        public string Name { get; internal set; }
    }
}
