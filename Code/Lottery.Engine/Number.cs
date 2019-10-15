using System;
using System.Diagnostics.CodeAnalysis;

namespace Lottery.Engine
{
    public class Number : IEquatable<Number>
    {
        public Number(int value, string category = null)
        {
            if (value <= 0)
                throw new ArgumentException("Number value must be greater than zero.");

            Value = value;
            Category = category ?? string.Empty;
        }

        public int Value { get; }

        public string Category { get; }

        public override string ToString()
        {
            return string.Concat(Value, Category[0]);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            else if (obj is Number)
                return Equals((Number)obj);
            else
                return false;
        }

        public bool Equals([AllowNull]Number other)
        {
            if (other == null)
                return false;

            return 
                Value == other.Value &&
                string.Equals(Category, other.Category, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}
