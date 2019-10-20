using Lottery.Engine.Contract;
using Lottery.Storage.Contract;
using System.Collections.Generic;
using System.Linq;

namespace Lottery.Engine
{
    class DataProcessor : IDataProcessor
    {
        private readonly IScoreProcessor[] scoreProcessors;

        public DataProcessor(IScoreProcessor[] scoreProcessors)
        {
            this.scoreProcessors = scoreProcessors;
        }

        public ProcessingResult Process(HistoricalData historicalData)
        {
            LinkedList<ProcessingResultGroup> groups = new LinkedList<ProcessingResultGroup>();

            foreach (IScoreProcessor scoreProcessor in scoreProcessors)
                groups.AddLast(scoreProcessor.Process(historicalData.Entries));

            return new ProcessingResult(historicalData, groups.ToArray());
        }
    }
}
