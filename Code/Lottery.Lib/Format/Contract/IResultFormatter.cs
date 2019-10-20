using Lottery.Engine.Contract;
using System.Collections.Generic;

namespace Lottery.Format.Contract
{
    public interface IResultFormatter
    {
        IEnumerable<string[]> Format(ProcessingResult processingResult);
    }
}
