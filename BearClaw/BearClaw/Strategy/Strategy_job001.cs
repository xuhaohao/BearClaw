using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;
using BearClaw.Common;
using System.Diagnostics;

namespace BearClaw.Strategy
{
    class Strategy_job001 : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.job001.cn";
        }

        public override string GetUri()
        {
            return @"http://www.job001.cn/jobs/jobs-list.php?listType=&key=%CD%E2%C3%B3&keyType=0&trade=&jobcategory=&citycategory=308&wage=&education=&isEduAbove=1&experience=&isExpAbove=1&nature=&settr=&sort=district&com_nature=&com_scale=&lang=&experienceMin=&experienceMax=&educationMin=&educationMax=&wageMin=&wageMax=&isDistrict=0&sortField=";
        }


        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            //*[@id="infolists"]/div[3]/ul/li[2]/a
            //*[@id="infolists"]/div[3]/ul/li[3]
            var htmlNodes = doc.DocumentNode.SelectNodes("//*[@id=\"infolists\"]/div/ul/li[2]/a");

            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var jobName = htmlNode.InnerText;
                    if (jobName.Contains("外贸")) {
                        var jobNode = htmlNode.ParentNode.ParentNode.ChildNodes[5].FirstChild;
                        var href = jobNode.GetAttributeValue("href", "");
                        var job = new Jobs() { Name = jobNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                        job.Ext1 = GetDomain();
                        jobs.Add(job);
                    }

                }
            }
            return jobs;
        }
    }
}
