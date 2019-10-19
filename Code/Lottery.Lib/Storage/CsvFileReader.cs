using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lottery.Storage
{
    interface ICsvFileReader
    {
        IEnumerable<string[]> ReadContent(string filePath);
    }

    class CsvFileReader : ICsvFileReader
    {
        public IEnumerable<string[]> ReadContent(string filePath)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
            {
                string line = reader.ReadLine();

                if (line != null)
                    line = reader.ReadLine();

                while (line != null)
                {
                    yield return line.Split(',').Select(value => value.Trim()).ToArray();

                    line = reader.ReadLine();
                }
            }
        }
    }
}
