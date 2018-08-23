using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_ganji : MyStrategy
    {
        public override string GetDomain()
        {
            return "zhongshan.ganji.com";
        }

        public override string GetUri(string keyword)
        {
            //http://zhongshan.ganji.com/zhaopin/s/_外贸/?from=zhaopin_indexpage
            var url = String.Format(@"http://zhongshan.ganji.com/zhaopin/s/_{0}/?from=zhaopin_indexpage", keyword);
            return url;
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@id,'_EntUrl')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    if (!string.IsNullOrEmpty(htmlNode.InnerText) && htmlNode.InnerText.Contains(App.Area))
                    {
                        var href = htmlNode.GetAttributeValue("href", "");
                        var job = new Jobs() { Name = htmlNode.InnerText, Url = JoinUrl(@"http://www.0760rc.com/search", href), TimeTag = DateTime.Now.ToString() };
                        job.Ext1 = GetDomain();
                        jobs.Add(job);
                    }
                }
            }
            return jobs;
        }
    }
}
