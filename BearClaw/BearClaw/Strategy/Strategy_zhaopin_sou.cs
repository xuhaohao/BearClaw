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
        public override Dictionary<string, Jobs> Strategy(string htmlText)
        {
            var jobMap = new Dictionary<string, Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'company.zhaopin.com/')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var href = htmlNode.GetAttributeValue("href", "");
                    var companyName = htmlNode.InnerText;
                    if (!jobMap.ContainsKey(companyName))
                    {
                        var job = new Jobs() { Name = htmlNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                        job.Ext1 = GetDomain();
                        jobMap.Add(companyName, job);
                    }
                }
            }
            return jobMap;
        }
    }
}
