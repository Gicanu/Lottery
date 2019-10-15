using System.Collections.Generic;

namespace Lottery.Lib
{
    public interface INumbersScoreEngine
    {
        IEnumerable<(Number, NumberScore)> Process(IEnumerable<Number[]> data);
    }
}
