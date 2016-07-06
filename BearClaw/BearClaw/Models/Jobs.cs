
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearClaw.Models
{

    public class Jobs
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public string Tel { get; set; }

        public long Sended { get; set; }

        public string Address { get; set; }

        public string Ext1 { get; set; }

        public string Ext2 { get; set; }

        public string TimeTag { get; set; }
    }
}
