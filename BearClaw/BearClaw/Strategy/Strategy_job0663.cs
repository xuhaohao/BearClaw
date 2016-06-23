using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using System.Text.RegularExpressions;
using BearClaw.Common;
using System.Diagnostics;

namespace BearClaw.Strategy
{
    class Strategy_job0663 : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.job001.cn";
        }

        public override string GetUri()
        {
            return @"http://www.job001.cn/jobs/jobs-list.php?listType=&key=%CD%E2%C3%B3&keyType=0&trade=&jobcategory=&citycategory=308&wage=&education=&isEduAbove=1&experience=&isExpAbove=1&nature=&settr=&sort=district&com_nature=&com_scale=&lang=&experienceMin=&experienceMax=&educationMin=&educationMax=&wageMin=&wageMax=&isDistrict=0&sortField=";
        }
        public override Dictionary<string, Jobs> Strategy(string htmlText)
        {
            var jobMap = new Dictionary<string, Jobs>();
            Regex reg = new Regex(@"(?<=<H2>)(.*?)(?=</H2>)", RegexOptions.IgnoreCase);//[^(<td>))] 
            MatchCollection mc = reg.Matches(htmlText);

            var newDic = new Dictionary<string, JobEntity>();
            foreach (var item in mc)
            {
                var companyName = item.ToString();
                if (!jobMap.ContainsKey(companyName))
                {
                    var job = new Jobs() { Name = companyName, TimeTag = DateTime.Now.ToString() };
                    jobMap.Add(companyName, job);
                }
            }
            return jobMap;
        }
    }
}
