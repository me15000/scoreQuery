using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace scoreQuery.Spider
{


    class Program
    {
        const string QuerySchoolsUrl = "https://data-gkcx.eol.cn/soudaxue/queryschool.html?messtype=json&schooltype=&page={0}&size=10";

        class SchoolListInfo
        {

            public Dictionary<string, string> totalRecord { get; set; }

            public List<Dictionary<string, object>> school { get; set; }
        }

        static Common.DB.IDBHelper db = Common.DB.Factory.CreateDBHelper();
        void SaveDB(Dictionary<string, object> info)
        {
            bool exists = db.Exists("select top 1 1 from [school.data] where schoolid=@0", info["schoolid"]);

            if (!exists)
            {
                Console.WriteLine(info["schoolid"]);

                var nvc = new Common.DB.NVCollection();
                nvc["schoolid"] = info["schoolid"];
                nvc["schoolname"] = info["schoolname"] is JArray ? string.Empty : info["schoolname"];
                nvc["province"] = info["province"] is JArray ? string.Empty : info["province"];
                nvc["schooltype"] = info["schooltype"] is JArray ? string.Empty : info["schooltype"];
                nvc["schoolproperty"] = info["schoolproperty"] is JArray ? string.Empty : info["schoolproperty"];
                nvc["edudirectly"] = info["edudirectly"] is JArray ? string.Empty : info["edudirectly"];
                nvc["f985"] = info["f985"] is JArray ? string.Empty : info["f985"];
                nvc["f211"] = info["f211"] is JArray ? string.Empty : info["f211"];
                nvc["level"] = info["level"] is JArray ? string.Empty : info["level"];
                nvc["autonomyrs"] = info["autonomyrs"] is JArray ? string.Empty : info["autonomyrs"];
                nvc["library"] = info["library"] is JArray ? string.Empty : info["library"];
                nvc["membership"] = info["membership"] is JArray ? string.Empty : info["membership"];
                nvc["schoolnature"] = info["schoolnature"] is JArray ? string.Empty : info["schoolnature"];
                nvc["shoufei"] = info["shoufei"] is JArray ? string.Empty : info["shoufei"];
                nvc["jianjie"] = info["jianjie"] is JArray ? string.Empty : info["jianjie"];

                nvc["schoolcode"] = info["schoolcode"] is JArray ? string.Empty : info["schoolcode"];
                nvc["ranking"] = info["ranking"] is JArray ? string.Empty : info["ranking"];
                nvc["rankingCollegetype"] = info["rankingCollegetype"] is JArray ? string.Empty : info["rankingCollegetype"];
                nvc["guanwang"] = info["guanwang"] is JArray ? string.Empty : info["guanwang"];
                nvc["oldname"] = info["oldname"] is JArray ? string.Empty : info["oldname"];
                nvc["master"] = info["master"] is JArray ? string.Empty : info["master"];

                nvc["num"] = info["num"] is JArray ? string.Empty : info["num"];
                nvc["firstrate"] = info["firstrate"] is JArray ? string.Empty : info["firstrate"];

                string sql = "insert into [school.data]([schoolid],[schoolname],[province],[schooltype],[schoolproperty],[edudirectly],[f985],[f211],[level],[autonomyrs],[library],[membership],[schoolnature],[shoufei],[jianjie],[schoolcode],[ranking],[rankingCollegetype],[guanwang],[oldname],[master],[num],[firstrate]) values(@schoolid,@schoolname,@province,@schooltype,@schoolproperty,@edudirectly,@f985,@f211,@level,@autonomyrs,@library,@membership,@schoolnature,@shoufei,@jianjie,@schoolcode,@ranking,@rankingCollegetype,@guanwang,@oldname,@master,@num,@firstrate)";

                db.ExecuteNoneQuery(sql, nvc);
            }
        }

        void RunSchool(int page, int size, out int pageCount)
        {


            pageCount = 0;

            string url = string.Format(QuerySchoolsUrl, page);

            Console.WriteLine(url);

            using (var wc = new WebClient())
            {
                byte[] data = wc.DownloadData(url);

                if (data == null)
                {
                    return;
                }
                string json = Encoding.GetEncoding("UTF-8").GetString(data);

                if (!string.IsNullOrEmpty(json))
                {
                    var info = JsonConvert.DeserializeObject<SchoolListInfo>(json);
                    if (info != null)
                    {
                        int records = int.Parse(info.totalRecord["num"]);

                        pageCount = Convert.ToInt32(Math.Ceiling((decimal)records / (decimal)size));
                        if (info.school != null)
                        {
                            for (int i = 0; i < info.school.Count; i++)
                            {
                                var sch = info.school[i];

                                SaveDB(sch);
                            }
                        }
                    }
                }
            }
        }

        public void RunSchools()
        {
            bool hasNext = true;
            int page = 1;
            int pagesize = 10;

            int threads = 5;

            var taskF = new TaskFactory();

            do
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < threads; i++)
                {

                    var task = taskF.StartNew((pageObj) =>
                    {
                        int pageCount = 0;

                        int nowpage = (int)pageObj;

                        if (nowpage < pageCount)
                        {
                            hasNext = false;
                        }
                        else
                        {
                            RunSchool(nowpage, pagesize, out pageCount);
                        }

                    }, page);

                    tasks.Add(task);

                    page++;

                }


                Task.WaitAll(tasks.ToArray());



            } while (hasNext);


        }


        static void Main(string[] args)
        {
            new Program().RunSchools();
        }
    }
}
