using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;

namespace BearClaw.Strategy
{
    class Strategy_job1001 : MyStrategy
    {
        public override string GetDomain()
        {
            throw new NotImplementedException();
        }

        public override string GetUri(string keyword)
        {
            //http://www.job1001.com/SearchResult.php
            throw new NotImplementedException();
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            throw new NotImplementedException();
        }
    }
}
