using SqliteORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearClaw.Models
{
    [Table]
    public class Jobs
    {
        [PrimaryKey]
        public long Id { get; set; }
        [Field]
        public string Name { get; set; }
        [Field]
        public string Url { get; set; }
        [Field]
        public string Tel { get; set; }
        [Field]
        public long Sended { get; set; }
        [Field]
        public string Address { get; set; }
        [Field]
        public string Ext1 { get; set; }
        [Field]
        public string Ext2 { get; set; }
        [Field]
        public string TimeTag { get; set; }
    }
}
