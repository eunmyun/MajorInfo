

namespace courseCrwaler
{
    using HtmlAgilityPack;

    /// <summary>
    /// This program grabs the title and desc of the course
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //link for informatics
            string Url = "https://www.washington.edu/students/crscat/info.html";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            var root = doc.DocumentNode.SelectNodes("//p/a");

            foreach (var node in root)
            {
                var courseName = node.SelectSingleNode("//b").InnerText;
                var desc = node.SelectSingleNode("//br").InnerText; //not working
                string other = node.InnerText;

            }
        }
    }
}
