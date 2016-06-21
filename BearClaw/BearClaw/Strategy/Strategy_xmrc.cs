using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_xmrc : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.xmrc.com.cn";
        }

        public override string GetUri()
        {
            return "http://www.xmrc.com.cn/net/info/resultg.aspx?a=a&g=g&jobtype=&releaseTime=365&searchtype=3&keyword=%E5%A4%96%E8%B4%B8&sortby=updatetime&ascdesc=Desc&PageSize=100&PageIndex=1";
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            var htmlNodes = doc.DocumentNode.SelectNodes("//li/div[2]/a");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var href = htmlNode.GetAttributeValue("href", "");
                    var job = new Jobs() { Name = htmlNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                    jobs.Add(job);
                }
            }
            return jobs;
        }
    }
}
