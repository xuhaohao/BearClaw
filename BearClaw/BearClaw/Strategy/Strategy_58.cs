using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;
using BearClaw.Common;
using System.Diagnostics;
using System.Web;

namespace BearClaw.Strategy
{
    class Strategy_58 : MyStrategy
    {
        public override string GetDomain()
        {
            return "zs.58.com";
        }

        public override string GetUri(string keyword)
        {
            var key = HttpUtility.UrlEncode(keyword, Encoding.UTF8);
            key = HttpUtility.UrlEncode(key, Encoding.UTF8);
            var url = String.Format(@"http://zs.58.com/job/?PGTID=0d100000-0030-3961-47ed-05af6f7f3a78&ClickID=6&key={0}", key);
            return url;
            //return @"http://zs.58.com/job/?PGTID=0d100000-0030-3961-47ed-05af6f7f3a78&ClickID=6&key=%252525E5%252525A4%25252596%252525E8%252525B4%252525B8";
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"jingzhun\"]/dd[2]/a");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    if (!string.IsNullOrEmpty(htmlNode.InnerText) && htmlNode.InnerText.Contains(App.Area))
                    {
                        var href = htmlNode.GetAttributeValue("href", "");
                        var job = new Jobs() { Name = htmlNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                        job.Ext1 = GetDomain();
                        jobs.Add(job);
                    }
                }
            }
            return jobs;
        }
    }
}
