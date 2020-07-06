using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelCake.Intrusive;

namespace CrawlerSamples.AutoRunner.choutiXinre
{
   
        [ExportEntity(EnumColor.LightGray, "用户信息")]
        [ImportEntity(titleRowIndex: 0, headRowIndex: 1, dataRowIndex: 2)]
        public class ChoutiXinRe : ExcelBase
        {
            [Export(name: "编号", index: 1, prefix: "ID:")]
            [Import(name: "编号", prefix: "ID:")]
            public int ID { set; get; }

            [Export("名", 2)]
            [Import("名")]
            public string Name { set; get; }
            [Export("顶", 3)]
            [Import("顶")]
            public int count { set; get; }
            public static void IntrusiveExport(List<ChoutiXinRe> list)
            {
                string[] sex = new string[] { "男", "女" };
                Random random = new Random();
                //for (var i = 0; i < 100; i++)
                //{
                //    list.Add(new CarFamilyDatas()
                //    { 
                //    });
                //}
                var temp = list.ExportToExcelBytes(); //导出为byte[]

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var exportTitle = "导出文件";
                var filePath = Path.Combine(path, exportTitle + DateTime.Now.Ticks + ".xlsx");
                FileInfo file = new FileInfo(filePath);
                File.WriteAllBytes(file.FullName, temp);
                Console.WriteLine("IntrusiveExport导出完成!");
            }
        }
   
}
