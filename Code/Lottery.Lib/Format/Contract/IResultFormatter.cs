using Lottery.Engine.Contract;

namespace Lottery.Format.Contract
{
    public interface IResultFormatter
    {
        string[] Format(ProcessingResult processingResult);
    }
}
