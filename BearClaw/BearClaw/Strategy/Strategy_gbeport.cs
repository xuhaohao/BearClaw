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
    class Strategy_gbeport : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.gbeport.gov.cn";
        }

        public override string GetUri()
        {
            return @"http://www.gbeport.gov.cn/CardInfoList.aspx";
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'card.net/Z')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var href = htmlNode.GetAttributeValue("href", "");
                    var name = htmlNode.InnerText;
                    if (name != null && name.Contains(App.Area)) {
                        var job = new Jobs() { Name = htmlNode.InnerText, Url = JoinUrl(@"http://www.gbeport.gov.cn", href), TimeTag = DateTime.Now.ToString() };
                        job.Ext1 = GetDomain();
                        jobs.Add(job);
                    }
                }
            }
            return jobs;
        }
    }
}
