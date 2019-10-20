using System.Linq;

namespace Lottery.Engine.Contract
{
    public class ProcessingResultGroup
    {
        public ProcessingResultGroup(string key, params ProcessingResultEntry[] entries)
        {
            Key = key;
            Entries = entries;
        }

        public string Key { get; }

        public ProcessingResultEntry[] Entries { get; }

        public override string ToString()
        {
            return  $"{Key}: {string.Join(", ", Entries.Select(entry => entry.ToString()))}";
        }
    }
}
