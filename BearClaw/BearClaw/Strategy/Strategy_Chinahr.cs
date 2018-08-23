using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using BearClaw.Models;
using System.Web;

namespace BearClaw.Strategy
{
    class Strategy_Chinahr : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.chinahr.com";
        }

        public override string GetUri(string keyword)
        {
            //http://www.chinahr.com/sou/?city=25%2C308&keyword=外贸
            var url = String.Format(@"http://www.chinahr.com/sou/?city=25%2C308&keyword={0}", keyword);
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
