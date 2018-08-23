using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_Baidu : MyStrategy
    {
        public override string GetDomain()
        {
            return "zhaopin.baidu.com";
        }

        public override string GetUri(string keyword)
        {
            //https://zhaopin.baidu.com/quanzhi?query=&city=中山
            var url = String.Format(@"https://zhaopin.baidu.com/quanzhi?query={0}&city=中山", keyword);
            return url;
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            //*[@id="qz-list-box"]/div[1]/div[2]/div[1]/span
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
