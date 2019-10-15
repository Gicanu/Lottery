using System.Collections.Generic;
using System.Linq;

namespace Lottery.Lib.Engine
{
    public class JokerNumbersScoreEngine : INumbersScoreEngine
    {
        public IEnumerable<(Number, NumberScore)> Process(IEnumerable<Number[]> data)
        {
            Dictionary<Number, NumberScore> scores = new Dictionary<Number, NumberScore>();

            return scores.Select(score => (score.Key, score.Value)).OrderBy(score => score.Key.Value);
        }
    }
}
