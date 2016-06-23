using BearClaw.Models;
using SqliteORM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearClaw.Common
{
    class Db
    {
        private readonly static string DbPath = string.Format(@"Data Source ={0}db\bear.db", AppDomain.CurrentDomain.BaseDirectory);

        public static ObservableCollection<Params> ParamsCache;

        static Db() {
            DbConnection.Initialise(DbPath);
            ParamsCache = new ObservableCollection<Params>();
            using (TableAdapter<Params> adapter = TableAdapter<Params>.Open())
            {
                var list = adapter.Select();
                foreach (Params item in list)
                {
                    ParamsCache.Add(item);
                }
            }
        }

        public static void CreateUpdate(List<Jobs> jobs) {
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                foreach (var job in jobs)
                {
                    job.Name = job.Name.Trim();
                    adapter.CreateUpdate(job);
                }
            }
        }

        public static List<Jobs> Valid(Dictionary<string,Jobs> jobMap)
        {
            //消除数据库中的重名项
            List<Jobs> result = new List<Jobs>();
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                foreach (var job in jobMap.Values)
                {
                    var name = job.Name.Trim();
                    var rows = adapter.Select().Where(Where.Equal("Name", name)).Count();
                    if (rows == 0)
                    {
                        job.Name = name;
                        result.Add(job);
                    }
                }
            }
            return result;
        }


        public static ObservableCollection<Jobs> GetJobs()
        {
            ObservableCollection<Jobs> jobs = new ObservableCollection<Jobs>();
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                var rows = adapter.Select().Reverse();

                foreach (Jobs item in rows)
                {
                    jobs.Add(item);
                }
            }
            return jobs;
        }

        public static List<Jobs> GetJobsList(Where where)
        {
            List<Jobs> jobs = new List<Jobs>();
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                jobs = adapter.Select().Where(where).Reverse().ToList();
            }
            return jobs;
        }

        public static bool Exist(string name) {
            var exist = false;
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                var rows = adapter.Select().Where(Where.Equal("Name",name)).Count();
                exist = rows > 0;
            }
            return exist;
        }


        public static void InitDb() {
            using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            {
                var rows = adapter.Select();
            }

            using (TableAdapter<Params> adapter = TableAdapter<Params>.Open())
            {
                var rows = adapter.Select();
            }
        }


        public static Params GetFirstParam(string fieldGroup, string name) {
            Params param = null;
            param = ParamsCache.First(a =>
            {
                return a.FieldGroup.Equals(fieldGroup) && a.Name.Equals(name);
            });
            return param;
        }

        public static List<Params> GetParams(string fieldGroup, string name)
        {
            List<Params> result = ParamsCache.Where(a =>
            {
                return a.FieldGroup.Equals(fieldGroup) && a.Name.Equals(name);
            }).ToList();
            return result;
        }

        public static void UpdateParamValue(Params param)
        {
            using (TableAdapter<Params> adapter = TableAdapter<Params>.Open())
            {
                adapter.CreateUpdate(param);
            }
        }






    }
}
