

namespace courseEvalParser
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(@"C:\Users\5TJ\Desktop\MajorInfo\courseEval\", "*.html", SearchOption.TopDirectoryOnly);
            var csv = new StringBuilder();
            foreach (string fileName in filePaths)
            {
                StreamReader reader = new StreamReader(fileName);
                string html = "";
                html = reader.ReadToEnd();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                Regex regex = new Regex(@"^[A-Z]+(\d)+[A-Z]+.*");
                Regex regex1 = new Regex(@"^[A-Z]+");
                Regex regex2 = new Regex(@"^(\d)+");
                string courseAbbr;
                foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    string[] array = hrefValue.Split('/', '.');
                    if (array.Length == 3)
                    {
                        string courseName = array[1];
                        Match match = regex.Match(courseName);
                        if (match.Success)
                        {
                            Match match1 = regex1.Match(courseName);
                            if (match1.Success)
                            {
                                courseAbbr = match1.Value;
                                Console.WriteLine("Name : " + match1.Value);

                                string number = courseName.Substring(courseAbbr.Length);
                                Match match2 = regex2.Match(number);
                                if (match2.Success)
                                {
                                    Console.WriteLine("number : " + match2.Value);
                                    Console.WriteLine("link : " + hrefValue);
                                    var newLine = string.Format("{0},{1},{2},{3}", match1.Value, match2.Value, "https://www.washington.edu/cec/" + hrefValue, link.InnerText);
                                    csv.AppendLine(newLine);
                                }
                            }
                        }
                    }

                }
            }
            File.WriteAllText(@"C:\Users\5TJ\Desktop\MajorInfo\courseEval\combine.csv", csv.ToString());
            Console.Read();
        }
    }
}
