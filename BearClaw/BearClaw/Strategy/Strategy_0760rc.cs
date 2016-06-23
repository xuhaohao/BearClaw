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
    class Strategy_0760rc : MyStrategy
    {
        public override string GetDomain()
        {
            return "www.0760rc.com";
        }

        public override string GetUri()
        {
            return @"http://www.0760rc.com/search/offer_search_result.aspx?keyword=%cd%e2%c3%b3&jcity1Hidden=101000&areatitle=&lat=0&lng=0&zoom=0&ma=0";
        }



        public override Dictionary<string, Jobs> Strategy(string htmlText)
        {
            var jobMap = new Dictionary<string, Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);
            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@id,'_EntUrl')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var href = htmlNode.GetAttributeValue("href", "");
                    var companyName = htmlNode.InnerText;
                    if (!jobMap.ContainsKey(companyName)) {
                        var job = new Jobs() { Name = companyName, Url = JoinUrl(@"http://www.0760rc.com/search", href), TimeTag = DateTime.Now.ToString() };
                        jobMap.Add(companyName,job);
                    }
                }
            }
            return jobMap;
        }
    }
}
