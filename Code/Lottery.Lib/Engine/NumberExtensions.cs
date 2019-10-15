namespace Lottery.Lib.Engine
{
    public static class NumberExtensions
    {
        public static Number ToNumber(this int value)
        {
            return new Number(value);
        }

        public static Number ToJoker(this int value)
        {
            return new Number(value, "Joker");
        }
    }
}
