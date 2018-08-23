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
    class Strategy_zsjjob : MyStrategy
    {
        public override string GetDomain()
        {
            return "zs.zsjjob.com";
        }

        public override string GetUri(string keyword)
        {
            //http://zs.zsjjob.com
            var key = HttpUtility.UrlEncode(keyword, Encoding.UTF8);
            var url = String.Format(@"http://zs.zsjjob.com/joblist.aspx?keyword={0}", key);
            return url;
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            ////*[@id="JobApplyList_Table"]/div[2]/div[2]/div[1]/a
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@class,'search_job_com_name')]");
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
