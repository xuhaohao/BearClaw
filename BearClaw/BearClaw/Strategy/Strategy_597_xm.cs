using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;

namespace BearClaw.Strategy
{
    class Strategy_597_xm : MyStrategy
    {
        
        public override string GetDomain()
        {
            return "xm.597.com";
        }

        public override string GetUri()
        {
            return @"http://xm.597.com/zhaopin/g3502c3/?q=%E5%A4%96%E8%B4%B8";
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//li[@class='firm-md2']");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    //var jobNode = htmlNode.ParentNode.ParentNode.ChildNodes[5].FirstChild;
                    //var href = jobNode.GetAttributeValue("href", "");
                    //var job = new Jobs() { Name = jobNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                    //jobs.Add(job);
                    foreach (var item in App.Area_Sub)
                    {
                        if (htmlNode.InnerText.Contains(item)) {
                            var companyNode = htmlNode.ParentNode.ChildNodes[3].FirstChild;
                            var href = companyNode.GetAttributeValue("href", "");
                            var job = new Jobs() { Name = companyNode.InnerText, Url = JoinUrl("http://xm.597.com", href), TimeTag = DateTime.Now.ToString() };
                            jobs.Add(job);
                        }
                    }


                }
            }
            return jobs;
        }
    }
}
