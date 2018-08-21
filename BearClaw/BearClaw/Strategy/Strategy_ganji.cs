using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearClaw.Models;

namespace BearClaw.Strategy
{
    class Strategy_ganji : MyStrategy
    {
        public override string GetDomain()
        {
            throw new NotImplementedException();
        }

        public override string GetUri(string keyword)
        {
            //http://zhongshan.ganji.com/zhaopin/s/_外贸/?from=zhaopin_indexpage
            throw new NotImplementedException();
        }

        public override List<Jobs> Strategy(string htmlText)
        {
            throw new NotImplementedException();
        }
    }
}
