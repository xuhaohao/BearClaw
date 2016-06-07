using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;
using BearClaw.Common;
using System.Diagnostics;

namespace BearClaw.Strategy
{
    class Strategy_51job : MyStrategy
    {
        public override string GetDomain()
        {
            return "search.51job.com";
        }

        public override string GetUri()
        {
            return @"http://search.51job.com/jobsearch/search_result.php?fromJs=1&jobarea=030700%2C00&funtype=0000&industrytype=00&keyword=%E5%A4%96%E8%B4%B8&keywordtype=2&lang=c&stype=2&postchannel=0000&fromType=1&confirmdate=9";
        }
        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'jobs.51job.com/all/')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                        var address = htmlNode.ParentNode.ParentNode.ChildNodes[4].FirstChild;

                        if (address != null && address.InnerText != null && address.InnerText.Contains("中山"))
                        {
                            var href = htmlNode.GetAttributeValue("href", "");
                            var job = new Jobs() { Name = htmlNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                            jobs.Add(job);
                        }
                }
            }
            return jobs;
        }
    }
}
