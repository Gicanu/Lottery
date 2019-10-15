using System.Collections.Generic;

namespace Lottery.Lib.Engine
{
    public interface INumbersScoreEngine
    {
        IEnumerable<(Number, NumberScore)> Process(IEnumerable<Number[]> data);
    }
}
