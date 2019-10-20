namespace Lottery.Engine.Contract
{
    public class ProcessingResultEntry
    {
        public ProcessingResultEntry(int number, double score, NumberType type = NumberType.Regular)
        {
            Number = number;
            Score = score;
            Type = type;
        }

        public int Number { get; }

        public double Score { get; }

        public NumberType Type { get; }

        public override string ToString()
        {
            return $"{Number}{(Type == NumberType.Joker ? "J" : string.Empty)} ({Score})";
        }
    }

    public enum NumberType
    {
        None = 0,
        Regular = 1,
        Joker = 2,
    }
}
