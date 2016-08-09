using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace courseCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var csv = new StringBuilder();
            string url = "https://www.washington.edu/students/crscat/";
            //List<string> majors = new List<string>();
            //<string> majorNames = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Regex courseNameReg = new Regex(@"^[A-Za-z]+( [A-Z]+)?");
            Regex courseNumReg = new Regex(@"^(\d)+");
            Regex courseTitleReg = new Regex(@"^([A-Za-z0-9-,])+( [A-Za-z0-9:,]+)*");
            Regex majorNameReg = new Regex(@"^[A-Za-z-']+( [A-Za-z]+)*");

            var newLine = string.Format("{0},{1},{2},{3},\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"", "major", "courseAbbr_courseNum", "courseAbbr", "courseNum", "courseTitle", "courseDesc", "credits", "my_plan_link", "type", "prerequisite", "courseAbbr_without_space");
            csv.AppendLine(newLine);

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            string courseAbbr;
            string courseNum;
            string courseTitleOfficial;
            var root = doc.DocumentNode.SelectNodes("//li/a");
            string majorHtml;
            string majorName;

            foreach (var node in root)
            {
                majorHtml = node.Attributes["href"].Value;
                if (!dictionary.ContainsKey(majorHtml) && !majorHtml.Contains("/") && majorHtml.Contains(".html"))
                {
                    //majors.Add(major);
                    majorName = node.InnerText;
                    Match match = majorNameReg.Match(majorName);
                    if (match.Success) {
                        dictionary.Add(majorHtml, match.Value);
                        //majorNames.Add(match.Value);
                    }
                }
            }


            //using (StreamWriter writer = new StreamWriter(@"C:\Users\Sally\Desktop\majors.txt", true)) {
            //    foreach (string s in majorNames)
            //    {
            //        writer.WriteLine(s);
            //    }
            //}

            foreach (string major in dictionary.Keys)
            {
                HtmlDocument majorDoc = web.Load(url + major);
                var majorRoot = majorDoc.DocumentNode.SelectNodes("//a[@name]");
                if (majorRoot != null)
                {
                    foreach (var node in majorRoot)
                    {
                        var bNode = node.FirstChild.FirstChild;
                        var brNode = bNode.NextSibling.NextSibling;
                        string courseTitle = bNode.InnerText;
                        int toDelete = courseTitle.Length;
                        if (bNode.NextSibling.Name == "i")
                        {
                            toDelete += bNode.NextSibling.InnerText.Length;
                        }
                        string link = node.FirstChild.LastChild.Attributes["href"].Value;
                        //앙~ 기모띠<3

                        string courseDesc = node.InnerText.Substring(toDelete);
                        courseDesc = courseDesc.Substring(0, courseDesc.IndexOf("View course details"));

                        Match match = courseNameReg.Match(courseTitle);
                        if (match.Success)
                        {
                            courseAbbr = match.Value;
                            courseTitle = courseTitle.Substring(courseAbbr.Length + 1);
                            Match match1 = courseNumReg.Match(courseTitle);
                            if (match1.Success)
                            {
                                courseNum = match1.Value;
                                courseTitle = courseTitle.Substring(courseNum.Length + 1);
                                Match match2 = courseTitleReg.Match(courseTitle);
                                if (match2.Success)
                                {
                                    courseTitleOfficial = match2.Value;
                                    courseTitle = courseTitle.Substring(courseTitleOfficial.Length + 1);
                                    string credits = courseTitle.Split('(', ')')[1];
                                    string type = courseTitle.Substring(courseTitle.IndexOf(')') + 1).Trim();


                                    string prerequisite = "";

                                    if (courseDesc.Contains("Prerequisite:"))
                                    {
                                        prerequisite = courseDesc.Substring(courseDesc.IndexOf("Prerequisite:") + 14);
                                        prerequisite = prerequisite.Substring(0, prerequisite.Length - 1);
                                        courseDesc = courseDesc.Substring(0, courseDesc.IndexOf("Prerequisite:"));
                                    }
                                    Console.WriteLine(courseAbbr + "_" + courseNum);
                                    newLine = string.Format("{0},{1},{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"", dictionary[major], courseAbbr + "_" + courseNum, courseAbbr, courseNum, courseTitleOfficial, courseDesc, credits, link, type, prerequisite, courseAbbr.Replace(" ", ""));
                                    csv.AppendLine(newLine);
                                }
                            }
                        }

                        
                        
                    }
                }
            }
            File.WriteAllText(@"C:\Users\Sally\Desktop\courses.csv", csv.ToString());
        }
    }
}
