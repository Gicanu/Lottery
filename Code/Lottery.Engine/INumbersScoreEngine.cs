using System.Collections.Generic;

namespace Lottery.Engine
{
    public interface INumbersScoreEngine
    {
        IEnumerable<(Number, NumberScore)> Process(IEnumerable<Number[]> data);
    }
}
