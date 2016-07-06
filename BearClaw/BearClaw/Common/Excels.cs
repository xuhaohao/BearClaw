using BearClaw.Models;
using Codaxy.Xlio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearClaw.Common
{
    class Excels
    {
        public static void Export(string outputFileName, ICollection<Jobs> jobCollections) {
            var workbook = new Workbook();
            var sheet = workbook.Sheets.AddSheet("data");

            Heads(sheet);
            Datas(sheet, jobCollections);

            workbook.Save(outputFileName);
        }

        public static void Heads(Sheet sheet) {
            sheet["A1"].Value = "序号";
            sheet["B1"].Value = "公司名称";
            sheet["C1"].Value = "公司信息链接";
            sheet["D1"].Value = "来源";
            sheet["E1"].Value = "收录时间";

            sheet.Columns[0].Width = 15;
            sheet.Columns[1].Width = 40;
            sheet.Columns[2].Width = 30;
            sheet.Columns[3].Width = 20;
            sheet.Columns[4].Width = 20;
        }

        public static void Datas(Sheet sheet, ICollection<Jobs> jobCollections) {
            for (int i = 0; i < jobCollections.Count; i++)
            {
                var rowNo = i + 2;
                var job = jobCollections.ElementAt(i);
                sheet["A" + rowNo].Value = i;
                sheet["B" + rowNo].Value = job.Name;
                sheet["C" + rowNo].Value = job.Url;
                sheet["D" + rowNo].Value = job.Ext1;
                sheet["E" + rowNo].Value = job.TimeTag;
            }
        }
    }
}
