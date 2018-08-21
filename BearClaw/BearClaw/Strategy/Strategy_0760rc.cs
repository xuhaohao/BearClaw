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
    class Strategy_0760rc : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.0760rc.com";
        }

        public override string GetUri(string keyword)
        {
            var key = HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));
            //var key = System.Text.Encoding.UTF8.GetBytes(keyword);
            //% cd % e2 % c3 % b3
            var url = String.Format(@"http://www.0760rc.com/search/offer_search_result.aspx?keyword={0}&jcity1Hidden=101000&areatitle=&lat=0&lng=0&zoom=0&ma=0", key);
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
