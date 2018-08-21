using BearClaw.Models;
using System.Collections.Generic;

namespace BearClaw.Strategy
{
    public abstract class MyStrategy
    {

        public static readonly Dictionary<string, MyStrategy> Dictionary = new Dictionary<string, MyStrategy>();
        public static readonly List<MyStrategy> List = new List<MyStrategy>();

        public static readonly List<string> KewWordList = new List<string>();
        static MyStrategy()
        {
            //StrategyList.Add(new Strategy_job0663());
            List.Add(new Strategy_0760rc());
            List.Add(new Strategy_51job());
            List.Add(new Strategy_58());
            List.Add(new Strategy_gbeport());
            List.Add(new Strategy_job001());
            List.Add(new Strategy_job5156());
            List.Add(new Strategy_job_sky_zs());
            List.Add(new Strategy_zhaopin_sou());

            foreach (var item in List)
            {
                Dictionary.Add(item.GetDomain(), item);
            }

            KewWordList.AddRange(new string[] { "外贸", "出口", "国际", "海外" , "外销" , "亚马逊", "Amazon", "跨境" , "eBay" , "Aliexpress" , "Wish", "Lazada", "天猫", "B2B" });
        }

        public abstract string GetDomain();
        public abstract string GetUri(string keyword);
        public abstract List<Jobs> Strategy(string htmlText);

        protected string JoinUrl(string baseUrl, string other) {
            if (!other.StartsWith("/"))
            {
                return baseUrl + "/" + other;
            }
            else {
                return baseUrl + other;
            }
        }

    }

   
}
