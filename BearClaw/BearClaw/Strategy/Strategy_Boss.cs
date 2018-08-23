using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    
    class Strategy_Boss : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.zhipin.com";
        }

        public override string GetUri(string keyword)
        {
            //https://www.zhipin.com/job_detail/?query=&scity=101281700&industry=&position=
            //var key = HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));
            //var key = System.Text.Encoding.UTF8.GetBytes(keyword);
            //% cd % e2 % c3 % b3
            var url = String.Format(@"https://www.zhipin.com/job_detail/?query={0}&scity=101281700&industry=&position=", keyword);
            return url;
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            //*[@id="main"]/div/div[2]/ul/li[1]/div/div[2]/div/h3/a
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
