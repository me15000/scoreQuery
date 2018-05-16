using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace scoreQuery.Spider
{
    public class SchoolsSpider
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
                nvc["schoolid"] = Convert.ToInt32(info["schoolid"]);
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
            int pageCount = 0;

            int threads = 5;

            var taskF = new TaskFactory();

            do
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < threads; i++)
                {

                    var task = taskF.StartNew((pageObj) =>
                    {

                        int nowpage = (int)pageObj;

                        if (pageCount > 0)
                        {
                            if (nowpage >= pageCount)
                            {
                                hasNext = false;
                            }
                        }


                        if (hasNext)
                        {
                            RunSchool(nowpage, pagesize, out pageCount);
                        }


                    }, page);

                    tasks.Add(task);

                    page++;

                }


                Task.WaitAll(tasks.ToArray());

                Console.WriteLine("complete");

            } while (hasNext);


        }

    }

    public class SchoolsScoreSpider
    {
        static Dictionary<string, string> ExamieeType = new Dictionary<string, string>()
        {
            {"文科","10034" },
            {"理科","10035" },
            {"综合","10090" },
            {"艺术类","10091" },
            {"体育类","10093" }
        };

        static Dictionary<string, string> BatchType = new Dictionary<string, string>()
        {
            {"一批","10036" },
            {"二批","10037" },
            {"三批","10038" },
            {"专科","10148" },
            {"提前","10149" }


        };

        static Dictionary<string, string> Provinces = new Dictionary<string, string>()
        {
            {"安徽","10008" },
            {"澳门","10145" },
            {"北京","10003" },
            {"重庆","10028" },
            {"福建","10024" },
            {"甘肃","10023" },
            {"贵州","10026" },
            {"广东","10011" },
            {"广西","10012" },
            {"河北","10016" },
            {"河南","10017" },
            {"黑龙江","10031" },
            {"湖北","10021" },
            {"湖南","10022" },
            {"海南","10019" },


            {"江苏","10014" },
            {"江西","10015" },
            {"吉林","10004" },
            {"辽宁","10027" },
            {"内蒙古","10002" },
            {"宁夏","10007" },
            {"青海","10030" },
            {"上海","10000" },
            {"山东","10009" },
            {"山西","10010" },
            {"陕西","10029" },
            {"四川","10005" },
            {"天津","10006" },
            {"新疆","10013" },
            {"西藏","10025" },

            {"香港","10020" },
            {"云南","10001" },
            {"台湾","10146" },
            {"浙江","10018" }
        };


        const string ScoreQueryUrl = "http://gkcx.eol.cn/schoolhtm/scores/provinceScores{0}_{1}_{2}_{3}.xml";//{学校id}_{省份id}_{文理科}_{批次}


        static Common.DB.IDBHelper db = Common.DB.Factory.CreateDBHelper();

        bool ExistsDB(ScoreInfo info)
        {
            bool exists = db.Exists("select top 1 1 from [school.score] where schoolid=@0 and provinceid=@1 and examieeid=@2 and batchid=@3 and [year]=@4", info.schoolid, info.provinceid, info.examieeid, info.batchid, info.year);

            return exists;
        }

        void SaveDB(ScoreInfo info)
        {


            if (!ExistsDB(info))
            {
                var nvc = new Common.DB.NVCollection();
                nvc["schoolid"] = info.schoolid;
                nvc["provinceid"] = info.provinceid;
                nvc["examieeid"] = info.examieeid;
                nvc["batchid"] = info.batchid;
                nvc["year"] = info.year;
                nvc["maxScore"] = info.maxScore;
                nvc["minScore"] = info.minScore;
                nvc["avgScore"] = info.avgScore;
                nvc["ps"] = info.ps;
                nvc["fc"] = info.fc;
                nvc["rb"] = info.rb;

                nvc["rs"] = info.rs;
                nvc["ph"] = info.ph;

                db.ExecuteNoneQuery("insert into [school.score]([schoolid],[provinceid],[examieeid],[batchid],[year],[maxScore],[minScore],[avgScore],[ps],[fc],[rb],[rs],[ph]) values(@schoolid,@provinceid,@examieeid,@batchid,@year,@maxScore,@minScore,@avgScore,@ps,@fc,@rb,@rs,@ph)", nvc);
            }
        }

        class ScoreInfo
        {
            public int schoolid { get; set; }
            public string provinceid { get; set; }
            public string examieeid { get; set; }
            public string batchid { get; set; }
            public int year { get; set; }

            public string maxScore { get; set; }
            public string minScore { get; set; }
            public string avgScore { get; set; }
            public string ps { get; set; }
            public string fc { get; set; }
            public string rb { get; set; }
            public string rs { get; set; }
            public string ph { get; set; }
        }

        void RunSchoolScore(int schoolid)
        {



            List<Task> tasks = new List<Task>();


            foreach (var pro in Provinces)
            {
                foreach (var bat in BatchType)
                {
                    foreach (var exa in ExamieeType)
                    {

                        object taskobj = new
                        {
                            schoolid = schoolid,
                            proid = pro.Value,
                            exaid = exa.Value,
                            batid = bat.Value
                        };

                        var task = new Task((obj) =>
                        {
                            dynamic dyobj = obj;

                            RunSchoolScore(dyobj.schoolid, dyobj.proid, dyobj.exaid, dyobj.batid);

                        }, taskobj);

                        tasks.Add(task);
                    }
                }
            }

            int threads = 1;

            var taskF = new TaskFactory();

            bool hasNext = true;
            int taskn = 0;
            int taskCount = tasks.Count;
            do
            {
                var taskList = new List<Task>(threads);
                for (int i = 0; i < threads; i++)
                {
                    if (taskn >= taskCount)
                    {
                        hasNext = false;
                        break;
                    }

                    var task = tasks[taskn];

                    taskList.Add(task);

                    task.Start();

                    taskn++;
                }


                Task.WaitAll(taskList.ToArray());

                Console.WriteLine("complete");

            } while (hasNext);

        }

        void RunSchoolScore(int schoolid, string provinceid, string examieeid, string batchid)
        {
            if (ExistsDB(new ScoreInfo { schoolid = schoolid, provinceid = provinceid, examieeid = examieeid, batchid = batchid }))
            {
                Console.WriteLine("exists ");
                return;
            }

            string url = string.Format(ScoreQueryUrl, schoolid, provinceid, examieeid, batchid);

            Console.WriteLine(url);

            using (var wc = new WebClient())
            {
                byte[] data = wc.DownloadData(url);

                if (data == null)
                {
                    return;
                }

                string xmlContent = Encoding.GetEncoding("utf-8").GetString(data);


                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlContent);
                var nodes = xmldoc.SelectNodes("//score");

                if (nodes != null)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        var node = nodes[i];

                        int year = int.Parse(node.SelectSingleNode("year").InnerText);
                        string maxScore = node.SelectSingleNode("maxScore").InnerText;
                        string avgScore = node.SelectSingleNode("avgScore").InnerText;
                        string minScore = node.SelectSingleNode("minScore").InnerText;
                        string ps = node.SelectSingleNode("ps").InnerText;
                        string fc = node.SelectSingleNode("fc").InnerText;
                        string rb = node.SelectSingleNode("rb").InnerText;
                        string rs = node.SelectSingleNode("rs").InnerText;
                        string ph = node.SelectSingleNode("ph").InnerText;

                        var ent = new ScoreInfo()
                        {
                            year = year,
                            maxScore = maxScore,
                            avgScore = avgScore,
                            minScore = minScore,
                            ps = ps,
                            fc = fc,
                            rb = rb,
                            rs = rs,
                            ph = ph,
                            batchid = batchid,
                            examieeid = examieeid,
                            provinceid = provinceid,
                            schoolid = schoolid
                        };

                        SaveDB(ent);
                    }
                }

            }

        }

        public void RunSchoolsScores()
        {
            bool hasNext = true;


            int previd = 0;


            do
            {
                var list = db.GetDataList("select top 5 schoolid from [school.data] where schoolid>@0 order by schoolid asc", previd);

                for (int i = 0; i < list.Count; i++)
                {
                    var ent = list[i];

                    previd = Convert.ToInt32(ent["schoolid"]);

                    RunSchoolScore(previd);

                }

                if (list.Count == 0)
                {
                    hasNext = false;
                }


            } while (hasNext);

        }
    }

    class Program
    {


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("schools:抓取所有学校；ss:抓取所有学校的往年分数");
                return;
            }

            switch (args[0])
            {
                case "schools":
                    new SchoolsSpider().RunSchools();
                    break;

                case "ss":
                    new SchoolsScoreSpider().RunSchoolsScores();
                    break;

                default:

                    break;
            }


        }
    }
}
