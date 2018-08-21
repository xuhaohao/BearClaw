using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;

namespace BearClaw.Strategy
{
    
    class Strategy_Boss : MyStrategy
    {
        public override string GetDomain()
        {
            throw new NotImplementedException();
        }

        public override string GetUri(string keyword)
        {
            //https://www.zhipin.com/job_detail/?query=&scity=101281700&industry=&position=
            throw new NotImplementedException();
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            throw new NotImplementedException();
        }
    }
}
