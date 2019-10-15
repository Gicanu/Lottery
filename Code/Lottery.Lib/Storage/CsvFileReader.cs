using System;
using System.IO;
using System.Linq;

namespace Lottery.Lib.Storage
{
    public class CsvFileReader : IDisposable
    {
        private readonly char separator;
        private StreamReader reader;

        public CsvFileReader(StreamReader reader, char separator = ',')
        {
            this.reader = reader;
            this.separator = separator;
        }

        public string[] ReadLine()
        {
            string line = reader.ReadLine();

            return line?.Split(separator).Select(value => value.Trim()).ToArray();
        }

        ~CsvFileReader()
        {
            Dispose(false);
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                    reader?.Close();
            }
            finally
            {
                reader = null;
            }
        }
    }
}
