using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.XPath;

namespace Gsu.Lottery.RetrieveData.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            Console.WriteLine("Retrieving data ...");

            HttpWebRequest request = WebRequest.CreateHttp("https://www.loto49.ro/arhiva-joker.php");

            WebResponse response = request.GetResponse();

            string pageData = "";

            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                pageData = streamReader.ReadToEnd();
            }

            Console.WriteLine("Data retrieved.");

            Console.WriteLine("Processing data ...");

            int startIndex = pageData.IndexOf("<table", StringComparison.InvariantCultureIgnoreCase);
            int endIndex = pageData.IndexOf("</table>", startIndex, StringComparison.InvariantCultureIgnoreCase);

            string rawData = pageData.Substring(startIndex, endIndex - startIndex + 8);
            
            XPathDocument document;

            using (var rawDataReader = new StringReader(rawData))
            {
                document = new XPathDocument(rawDataReader); 
            }

            var navigator = document.CreateNavigator();

            var valuesList = new List<(DateTime, int[])>();

            foreach (var node in navigator.Select("/table/tbody/tr").OfType<XPathNavigator>().Skip(1))
            {
                string[] rawValues = node.Select("td").OfType<XPathNavigator>().Select(node => node.Value).ToArray();

                DateTime date = DateTime.ParseExact(rawValues[0], "yyyy-MM-d", CultureInfo.InvariantCulture);
                int[] numbers = rawValues.Skip(1).Select(value => int.Parse(value.Trim('+', ' '))).ToArray();

                valuesList.Add((date, numbers));
            }

            Console.WriteLine("Data processed.");

            Console.WriteLine("Saving data ...");

            if (File.Exists(args[0]))
                File.Delete(args[0]);

            using (var writer = new StreamWriter(File.OpenWrite(args[0])))
            {
                foreach ((DateTime date, int[] numbers) in valuesList)
                {
                    string line = string.Join(", ", new[] { $"{date:yyyy-MM-dd}" }.Concat(numbers.Select(number => $"{number:N2}")));

                    writer.WriteLine(line);
                }
            }

            Console.WriteLine("Data saved.");
        }
    }
}
