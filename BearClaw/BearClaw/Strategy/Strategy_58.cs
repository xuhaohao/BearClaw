using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;
using BearClaw.Common;
using System.Diagnostics;
using System.Net;

namespace BearClaw.Strategy
{
    class Strategy_58 : MyStrategy
    {
        private const string addressMark = "联系地址：";
        public override string GetDomain()
        {
            return "xm.58.com";
        }

        public override string GetUri()
        {
            return @"http://xm.58.com/job/?PGTID=0d100000-0030-3961-47ed-05af6f7f3a78&ClickID=6&key=%252525E5%252525A4%25252596%252525E8%252525B4%252525B8";
        }

        public override Dictionary<string, Jobs> Strategy(string htmlText)
        {
            var jobMap = new Dictionary<string, Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[@class=\"fl\"]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var companyName = htmlNode.InnerText;
                    if (!jobMap.ContainsKey(companyName))
                    {
                        var href = htmlNode.GetAttributeValue("href", "");

                        var webClient = new WebClient();
                        webClient.Encoding = Encoding.UTF8;
                        var content = webClient.DownloadString(href);
                        var companyDoc = new HtmlDocument();
                        companyDoc.LoadHtml(content);
                        var addressText = "";
                        var addressNode = companyDoc.DocumentNode.SelectSingleNode("//td[@class=\"adress\"]");
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
                                if (!jobMap.ContainsKey(companyName)) {
                                    jobMap.Add(companyName, job);
                                }
                            }
                        }

                    } 
                }
            }
            return jobMap;
        }
    }
}
