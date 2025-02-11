using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp_SC
{
    internal class Program
    {
        static string[] GetFileNamesInDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("指定的目录不存在");
                return null;
            }

            return Directory.GetFiles(directoryPath);
        }
        static void Main(string[] args)
        {
            List<string> CsList = new List<string>();
            Console.WriteLine("输入目录：");
            string path = Console.ReadLine();
            string[] files = GetFileNamesInDirectory(path);
            Console.WriteLine("文件总数：" + files.Length);
            Console.WriteLine("任意键开始分析统计...");
            Console.ReadLine();
            foreach (string file in files)
            {
                Console.WriteLine(file);
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject(line);
                    if (jsonObject != null && jsonObject["管制扇区1"].Value == jsonObject["管制扇区2"].Value && jsonObject["相似程度"].Value == "HIGHLYSC")
                    {
                        string datetimeString = jsonObject["时间"].Value;
                        DateTime dateTime = Convert.ToDateTime(datetimeString);
                        string date = dateTime.ToString("d");
                        string content = date + ',' + jsonObject["航班号1"].Value + ',' + jsonObject["航班号2"].Value + ',' + jsonObject["管制扇区1"].Value;
                        if (!CsList.Contains(content))
                        {
                            CsList.Add(content);
                        }
                    }
                }
                File.WriteAllLines(path+ "相似呼号列表.csv", CsList.ToArray());

            }
            Console.WriteLine(CsList.Count.ToString()+" 条记录，文件保存为："+ path + "相似呼号列表.csv");
            Console.ReadKey();
        }
    }
}
