﻿using System;
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
    class Strategy_job_sky_zs : MyStrategy
    {
        public override string GetDomain()
        {
            return "zs.job-sky.com";
        }

        public override string GetUri()
        {
            return @"http://zs.job-sky.com/qiuzhi/search.aspx";
        }
        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'ViewCompanyDetails.aspx?aid=') and contains(text(),'外贸')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var trNode = htmlNode.ParentNode.ParentNode.ParentNode;
                    if (trNode.HasChildNodes) {
                        var jobNode = trNode.ChildNodes[7].FirstChild.FirstChild;
                        var href = jobNode.GetAttributeValue("href", "");
                        if (jobNode.InnerText != null && jobNode.InnerText.Trim().Length > 0)
                        {
                            var job = new Jobs() { Name = jobNode.InnerText, Url = JoinUrl(@"http://zs.job-sky.com/qiuzhi", href), TimeTag = DateTime.Now.ToString() };
                            job.Ext1 = GetDomain();
                            jobs.Add(job);
                        }
                    }

                }
            }
            return jobs;
        }
    }
}
