using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using System.Text.RegularExpressions;
using BearClaw.Common;
using System.Diagnostics;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_job0663 : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.job0663.com";
        }

        public override string GetUri()
        {
            return @"http://www.job0663.com/search/offer_search_result.aspx?keyword=外贸&amp;jcity1Hidden=102000&amp;jcity1pipei=on";
            //return @"http://www.job0663.com/search/offer_search_result.aspx?keyword=外贸&amp;jtype1Hidden=&amp;jcity1Hidden=102000&amp;page=1";
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
                    if (htmlNode.InnerText != null )
                    {
                        var address = htmlNode.ParentNode.ParentNode.ChildNodes[7];

                        if (address != null && address.InnerText != null && address.InnerText.Contains(App.Area))
                        {
                            var href = htmlNode.GetAttributeValue("href", "");
                            var job = new Jobs() { Name = htmlNode.InnerText, Url = JoinUrl(@"http://www.job0663.com/search", href), TimeTag = DateTime.Now.ToString() };
                            jobs.Add(job);
                        }
                    }
                }
            }
            return jobs;
        }
    }
}
