﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using System.Text.RegularExpressions;
using BearClaw.Common;
using System.Diagnostics;
using System.Web;

namespace BearClaw.Strategy
{
    class Strategy_job0663 : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.job001.cn";
        }

        public override string GetUri(string keyword)
        {
            var key = HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));
            var url = String.Format(@"http://www.job001.cn/jobs/jobs-list.php?listType=&key={0}&keyType=0&trade=&jobcategory=&citycategory=308&wage=&education=&isEduAbove=1&experience=&isExpAbove=1&nature=&settr=&sort=district&com_nature=&com_scale=&lang=&experienceMin=&experienceMax=&educationMin=&educationMax=&wageMin=&wageMax=&isDistrict=0&sortField=", key);
            return url;
            //return @"http://www.job001.cn/jobs/jobs-list.php?listType=&key=%CD%E2%C3%B3&keyType=0&trade=&jobcategory=&citycategory=308&wage=&education=&isEduAbove=1&experience=&isExpAbove=1&nature=&settr=&sort=district&com_nature=&com_scale=&lang=&experienceMin=&experienceMax=&educationMin=&educationMax=&wageMin=&wageMax=&isDistrict=0&sortField=";
        }
        public override List<Jobs> Strategy(string htmlText)
        {
            List<Jobs> jobs = new List<Jobs>();
            Regex reg = new Regex(@"(?<=<H2>)(.*?)(?=</H2>)", RegexOptions.IgnoreCase);//[^(<td>))] 
            MatchCollection mc = reg.Matches(htmlText);

            var newDic = new Dictionary<string, JobEntity>();
            foreach (var item in mc)
            {
                var strValue = item.ToString();
                if (!string.IsNullOrEmpty(strValue) && strValue.Contains(App.Area))
                {
                    var job = new Jobs() { Name = strValue, TimeTag = DateTime.Now.ToString() };
                    job.Ext1 = GetDomain();
                    jobs.Add(job);
                }
            }
            return jobs;
        }
    }
}
