using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;
using HtmlAgilityPack;
using System.Net;

namespace BearClaw.Strategy
{
    class Strategy_xmrc : MyStrategy
    {

        private const string addressMark = "联系地址：";

        public override string GetDomain()
        {
            return "www.xmrc.com.cn";
        }

        public override string GetUri()
        {
            return "http://www.xmrc.com.cn/net/info/resultg.aspx?a=a&g=g&jobtype=&releaseTime=365&searchtype=3&keyword=%E5%A4%96%E8%B4%B8&sortby=updatetime&ascdesc=Desc&PageSize=100&PageIndex=1";
        }

        public override Dictionary<string, Jobs> Strategy(string htmlText)
        {
            var jobMap = new Dictionary<string, Jobs>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlText);

            var htmlNodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'showco.aspx?CompanyID=')]");
            if (htmlNodes != null)
            {
                foreach (var htmlNode in htmlNodes)
                {
                    var companyName = htmlNode.InnerText;
                    if (!jobMap.ContainsKey(companyName))
                    {
                        var href = JoinUrl("http://www.xmrc.com.cn/net/info", htmlNode.GetAttributeValue("href", ""));
                        var addressText = htmlNode.ParentNode.ParentNode.ChildNodes[7].FirstChild.InnerText.Trim();
                        if (addressText.Equals("厦门市"))
                        {

                            var webClient = new WebClient();
                            webClient.Encoding = Encoding.UTF8;
                            var content = webClient.DownloadString(href);
                            var companyDoc = new HtmlDocument();
                            companyDoc.LoadHtml(content);
                            var addressNode = companyDoc.DocumentNode.SelectSingleNode(string.Format("//td[contains(text(),'{0}')]", addressMark));
                            if (addressNode != null)
                            {
                                addressText = addressNode.InnerText.Replace(addressMark, string.Empty);
                            }
                        }
                        foreach (var item in App.Area_Sub)
                        {
                            if (addressText.Contains(item))
                            {
                                var job = new Jobs() { Name = htmlNode.InnerText, Url = href, TimeTag = DateTime.Now.ToString() };
                                job.Ext1 = GetDomain();
                                jobMap.Add(companyName, job);
                            }
                        }
                    }
                }
            }
            return jobMap;
        }
    }
}
