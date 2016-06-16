using System;
using System.Collections.Generic;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_zhaopin_sou : MyStrategy
    {
        public override string GetDomain()
        {
            return "sou.zhaopin.com";
        }

        public override string GetUri()
        {
            return string.Format("http://sou.zhaopin.com/jobs/searchresult.ashx?jl={0}&kw=外贸&sm=0&p=1&sf=0&st=99999&isadv=1", App.Area); ;
        }
        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            ////*[@id="newlist_list_content_table"]/table[2]/tbody/tr[1]/td[3]/a
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'company.zhaopin.com/')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var href = htmlNode.GetAttributeValue("href", "");
                    var job = new Jobs() { Name = htmlNode.InnerText, Url =  href, TimeTag = DateTime.Now.ToString() };
                    jobs.Add(job);
                }
            }
            return jobs;
        }
    }
}
