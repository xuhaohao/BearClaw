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
    class Strategy_Liepin : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.liepin.com";
        }

        public override string GetUri(string keyword)
        {
            //https://www.liepin.com/zhongshan/
            var key = HttpUtility.UrlEncode(keyword, Encoding.UTF8);
            //https://www.liepin.com/zhaopin/?sfrom=click-pc_homepage-centre_searchbox-search_new&dqs=050130&key=%E5%A4%96%E8%B4%B8
            var url = String.Format(@"https://www.liepin.com/zhaopin/?sfrom=click-pc_homepage-centre_searchbox-search_new&dqs=050130&key={0}", key);
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
