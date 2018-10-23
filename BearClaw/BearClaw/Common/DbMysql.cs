using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using BearClaw.Models;
using MySql.Data.MySqlClient;
using SqliteORM;

namespace BearClaw.Common
{
    internal class DbMysql
    {
        public static string Conn =
            "Database='bearclaw';Data Source='100.0.12.238';User Id='root';Password='123456';charset='utf8';pooling=true";

        public static ObservableCollection<Params> ParamsCache;

        static DbMysql()
        {
            ParamsCache = new ObservableCollection<Params>();
            var dataSet = MySqlHelper.ExecuteDataset(Conn, "select * from params");
            var dt = dataSet.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var param = new Params
                {
                    Id = long.Parse(row["Id"].ToString()),
                    FieldGroup = GetString(row["FieldGroup"]),
                    Name = GetString(row["Name"]),
                    Value = GetString(row["Value"])
                };

                ParamsCache.Add(param);
            }

            //CreateUpdate(new List<Jobs>(new Jobs[1]{new Jobs()
            //{
            //    Address = "address",
            //    Ext1 = "e1",
            //    Ext2 = "ex2",
            //    Name = "name",
            //    Tel = "tel",
            //    TimeTag = "tt",
            //    Url = "url"
            //}}));
            //Valid(new List<Jobs>(new Jobs[2]
            //{
            //    new Jobs()
            //    {
            //        Address = "address",
            //        Ext1 = "e1",
            //        Ext2 = "ex2",
            //        Name = "name",
            //        Tel = "tel",
            //        TimeTag = "tt",
            //        Url = "url"
            //    },new Jobs()
            //    {
            //        Address = "address",
            //        Ext1 = "e1",
            //        Ext2 = "ex2",
            //        Name = "name",
            //        Tel = "tel",
            //        TimeTag = "tt",
            //        Url = "url"
            //    }
            //}));
        }

        public static void CreateUpdate(List<Jobs> jobs)
        {
            const string sql =
                "INSERT INTO jobs ( Name, Url, Tel, Sended, KeyWord ,Address, Ext1, Ext2, TimeTag) VALUES ( ?Name, ?Url, ?Tel, ?Sended, ?KeyWord , ?Address, ?Ext1, ?Ext2, ?TimeTag);";

            foreach (Jobs job in jobs)
            {
                var spArr = new[]
                {
                    new MySqlParameter("?Name", job.Name.Trim()),
                    new MySqlParameter("?Url", job.Url),
                    new MySqlParameter("?Tel", job.Tel),
                    new MySqlParameter("?Sended", job.Sended),
                    new MySqlParameter("?KeyWord", job.KeyWord),
                    new MySqlParameter("?Address", job.Address),
                    new MySqlParameter("?Ext1", job.Ext1),
                    new MySqlParameter("?Ext2", job.Ext2),
                    new MySqlParameter("?TimeTag", job.TimeTag)
                };
                int operate = MySqlHelper.ExecuteNonQuery(Conn, sql, spArr);
            }
        }


        public static string GetString(object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        public static IEnumerable<Jobs> Valid(List<Jobs> jobs)
        {
            //消除jobs中重名项
            var nameMap = new Dictionary<String, Jobs>();
            foreach (Jobs job in jobs)
            {
                string name = job.Name.Trim();
                if (!nameMap.ContainsKey(name))
                {
                    nameMap.Add(name, job);
                }
            }
            var sql = "select Name from jobs where Name in ('" + string.Join("','", nameMap.Keys) + "')";

            var dataSet = MySqlHelper.ExecuteDataset(Conn, sql);
            var dt = dataSet.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var name = row[0].ToString();
                if (nameMap.ContainsKey(name))
                {
                    nameMap.Remove(name);
                }
            }

            return nameMap.Values;
        }


        public static ObservableCollection<Jobs> GetJobs()
        {
            var jobs = new ObservableCollection<Jobs>();

            var dataSet = MySqlHelper.ExecuteDataset(Conn, "select * from jobs");
            var dt = dataSet.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var job = new Jobs
                {
                    Id = long.Parse(row["Id"].ToString()),
                    Name = GetString(row["Name"]),
                    Url = GetString(row["Url"]),
                    Tel = GetString(row["Name"]),
                    Sended = int.Parse(row["Sended"].ToString()),
                    KeyWord = GetString(row["KeyWord"]),
                    Address = GetString(row["Address"]),
                    Ext1 = GetString(row["Ext1"]),
                    Ext2 = GetString(row["Ext2"]),
                    TimeTag = GetString(row["TimeTag"])
                };

                jobs.Add(job);
            }
            return jobs;
        }

        public static List<Jobs> GetJobsList(Where where)
        {
            var jobs = new List<Jobs>();

            return jobs;
        }

        public static bool Exist(string name)
        {
            bool exist = false;
            //using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
            //{
            //    var rows = adapter.Select().Where(Where.Equal("Name",name)).Count();
            //    exist = rows > 0;
            //}
            return exist;
        }


        //public static void InitDb() {
        //    using (TableAdapter<Jobs> adapter = TableAdapter<Jobs>.Open())
        //    {
        //        var rows = adapter.Select();
        //    }

        //    using (TableAdapter<Params> adapter = TableAdapter<Params>.Open())
        //    {
        //        var rows = adapter.Select();
        //    }
        //}


        public static Params GetFirstParam(string fieldGroup, string name)
        {
            Params param = null;
            param = ParamsCache.First(a => { return a.FieldGroup.Equals(fieldGroup) && a.Name.Equals(name); });
            return param;
        }

        public static List<Params> GetParams(string fieldGroup, string name)
        {
            List<Params> result =
                ParamsCache.Where(a => { return a.FieldGroup.Equals(fieldGroup) && a.Name.Equals(name); }).ToList();
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