using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_0663job : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.0663job.com";
        }

        public override string GetUri()
        {
            return @"http://www.0663job.com/search/searjobok.aspx?keyword=外贸&lx=0&jtype1Hidden=35000&jcity1Hidden=102000&Industry=0&money=0&sex=0&datescale=30&expr=0&sort_cols1=refresh&sort_order1=desc&sort_cols2=--&sort_order2=asc&Medals=";
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'showent_')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    if (htmlNode.InnerText != null)
                    {
                        var href = htmlNode.GetAttributeValue("href", "");
                        if (href != null && href.Contains("../")) {
                            href = href.Replace("../", "");
                            var job = new Jobs() { Name = htmlNode.InnerText, Url = JoinUrl(@"http://www.0663job.com", href), TimeTag = DateTime.Now.ToString() };
                            jobs.Add(job);
                        }
                        
                    }
                }
            }
            return jobs;
        }
    }
}
