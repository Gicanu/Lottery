namespace Lottery.Engine
{
    public static class NumberExtensions
    {
        public static Number ToNumber(this int value, string category = null)
        {
            return new Number(value, category);
        }
    }
}
