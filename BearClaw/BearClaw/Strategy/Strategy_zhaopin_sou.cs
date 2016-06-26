using System;
using System.Collections.Generic;
using BearClaw.Models;
using HtmlAgilityPack;
using System.Net;
using System.Text;

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
            return string.Format("http://sou.zhaopin.com/jobs/searchresult.ashx?jl=厦门&kw=外贸&sm=0&sf=0&st=99999&isadv=1&sg=77c94680577d4655a7ca2d62a24e7835&p=1", App.Area); ;
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
                    var webClient = new WebClient();
                    webClient.Encoding = Encoding.UTF8;
                    var content = webClient.DownloadString(href);
                    var companyDoc = new HtmlDocument();
                    companyDoc.LoadHtml(content);
                    var addressText = "";
                    var addressNode = companyDoc.DocumentNode.SelectSingleNode("//span[@class=\"comAddress\"]");
                    if (addressNode != null)
                    {
                        addressText = addressNode.InnerText;
                    }
                    foreach (var item in App.Area_Sub)
                    {
                        if (addressText.Contains(item))
                        {
                            var job = new Jobs() { Name = companyName, Url = href, TimeTag = DateTime.Now.ToString() };
                            job.Ext1 = GetDomain();
                            if (!jobMap.ContainsKey(companyName))
                            {
                                jobMap.Add(companyName, job);
                            }
                        }
                    }
                }
            }
            return jobMap;
        }
    }
}
