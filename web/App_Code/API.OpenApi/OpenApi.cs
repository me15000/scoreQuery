
using System;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace API
{
    /// <summary>
    /// 主入口
    /// </summary>
    public partial class OpenApi : IHttpHandler
    {
        HttpRequest Request;
        HttpResponse Response;



        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;


            switch (Request.PathInfo)
            {
                //[学校专业类别]	
                case "/special/type/list.json":
                    special_type_list_json();
                    break;

                //[学校专业列表]	
                case "/special/list.json":
                    special_list_json();
                    break;

                //[专业详细介绍查询]	
                case "/special/info.json":
                    special_info_json();
                    break;

                case "/school/info.json":
                    school_info_json();
                    break;

                case "/school/special/list.json":
                    school_special_list_json();
                    break;


                case "/school/article/list.json":
                    school_article_list_json();
                    break;

                case "/school/article/details.json":
                    school_article_details_json();
                    break;

                case "/school/list.json":
                    school_list_json();
                    break;

                case "/school/score/list.json":
                    school_score_list_json();
                    break;

                case "/school/special/score/list.json":
                    school_special_score_list_json();
                    break;


                case "/user/id.json":
                    user_id_json();
                    break;


                case "/school/score/query.json":
                    school_score_query_json();
                    break;



            }
        }


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


        List<Common.DB.NVCollection> GetCacheProvinceScores()
        {
            return Common.Helpers.CacheHelper.GetCacheObject<List<Common.DB.NVCollection>>("cache_province_scores", 3600 * 24, () =>
            {

                string sql = @"select ttb.*,(select score from [province.score] where provinceid=ttb.provinceid and [year]=ttb.[year] and batch=ttb.batch and [type]=ttb.[type] ) as score from(select MAX(tb.[year]) as [year],tb.provinceid,tb.batch,tb.[type] from (select provinceid,[year],[batch],[type] from [province.score]) as tb group by tb.provinceid,tb.batch,tb.[type]) as ttb";

                var db = Common.DB.Factory.CreateDBHelper();

                return db.GetDataList(sql);
            });


        }

        void school_score_query_json()
        {
            string province = Request.QueryString["province"] ?? string.Empty;//省份
            string kelei = Request.QueryString["kelei"] ?? string.Empty;//科类|文科理科综合
            string province_PM = Request.QueryString["province_PM"] ?? string.Empty;//排名
            string grade = Request.QueryString["grade"] ?? string.Empty;//分数


            if (!Provinces.ContainsKey(province))
            {
                EchoFailJson("not exists province");
                return;
            }

            if (!ExamieeType.ContainsKey(kelei))
            {
                EchoFailJson("not exists kelei");
                return;
            }

            int score = int.Parse(grade);
            int pm = int.Parse(province_PM);




            var provinceScores = GetCacheProvinceScores();

            var list = provinceScores.FindAll((obj) =>
            {
                int nowscore = Convert.ToInt32(obj["score"]);
                return obj["provinceid"].ToString().Equals(Provinces[province])
                && obj["type"].ToString().Equals(kelei)
                && nowscore < score
                && (DateTime.Now.Year - 1) == Convert.ToInt32(obj["year"]);
            });



            if (list == null)
            {
                EchoFailJson("nodata");
                return;
            }

            if (list.Count == 0)
            {
                EchoFailJson("nodata");
                return;
            }


            string batchName = null;
            string batchId = null;
            var ent = list.OrderByDescending(obj => Convert.ToInt32(obj["score"])).First();
            if (ent != null)
            {
                string batch = ent["batch"].ToString();


                if (batch.IndexOf("本科") >= 0)
                {
                    foreach (var item in BatchType)
                    {
                        if (batch.IndexOf(item.Key) >= 0)
                        {
                            batchName = batch;
                            batchId = item.Value;
                        }
                    }
                }


                if (batch.IndexOf("专科") >= 0)
                {
                    batchName = "专科";
                    batchId = BatchType["专科"];
                }
            }


            //冲刺
            string sqlwhere_cc = "and l.n_avgScore>=" + (score + 10) + " and l.n_avgScore<=" + (score + 20);

            //适中
            string sqlwhere_sz = "and l.n_avgScore>=" + (score - 10) + " and l.n_avgScore<=" + (score + 10);

            //保底
            string sqlwhere_bd = "and l.n_avgScore>=" + (score - 50) + " and l.n_avgScore<=" + (score - 10);

            var db = Common.DB.Factory.CreateDBHelper();

            string sqlwherebat = null;

            if (string.IsNullOrEmpty(batchId))
            {
                sqlwherebat = " and l.batchid in('" + BatchType["一批"] + "','" + BatchType["二批"] + "','" + BatchType["提前"] + "')";

            }
            else
            {
                sqlwherebat = " and l.batchid ='" + batchId + "'";

            }

            string sql = "select d.schoolid,d.schoolname,d.province,l.provinceid,l.batchid,l.[year],l.n_avgScore as score "
                + " from [school.data] as d left join [school.score.last] as l "
                + " on (l.schoolid=d.schoolid)"
                + " where l.provinceid='" + Provinces[province] + "' and l.examieeid='" + ExamieeType[kelei] + "'"
                + sqlwherebat;

            var data_cc = db.GetDataList(sql + sqlwhere_cc + " order by l.n_avgScore desc");
            var data_sz = db.GetDataList(sql + sqlwhere_sz + " order by l.n_avgScore desc");
            var data_bd = db.GetDataList(sql + sqlwhere_bd + " order by l.n_avgScore desc");


            Action<List<Common.DB.NVCollection>> addField = (datalist) =>
            {

                for (int i = 0; i < datalist.Count; i++)
                {
                    var item = datalist[i];

                    item["percent"] = ((30m - ((decimal)(Convert.ToInt32(item["score"])) - (decimal)score)) / 30m).ToString("p2");
                }
            };

            addField(data_cc);
            addField(data_sz);
            addField(data_bd);

            EchoSuccJson(new
            {
                batch = new
                {
                    batchName = batchName,
                    batchId = batchId
                },
                list_bd = new
                {
                    count = data_bd.Count,
                    data = data_bd
                },
                list_sz = new
                {
                    count = data_sz.Count,
                    data = data_sz
                },
                list_cc = new
                {
                    count = data_cc.Count,
                    data = data_cc
                }
            });

        }


        void user_id_json()
        {
            string dk = Request.QueryString["dk"] ?? string.Empty;
            var db = Common.DB.Factory.CreateDBHelper();

            int id = 0;

            object objid = db.ExecuteScalar<object>("select id from user.data where dk=@0", dk);
            if (objid == null || objid == DBNull.Value)
            {
                object exo = db.ExecuteScalar<object>("insert into[user.data](dk,date) values(@0,@1);select @@IDENTITY;", dk, DateTime.Now);

                if (exo != null && exo != DBNull.Value)
                {
                    id = Convert.ToInt32(exo);
                }
                else
                {
                    id = 0;
                }
            }
            else
            {
                id = Convert.ToInt32(objid);
            }

            EchoSuccJson(new
            {
                id = id
            });



        }


        void special_type_list_json()
        {
            var list = Common.Helpers.CacheHelper.GetCacheObject<List<Common.DB.NVCollection>>("cache_special_type_list", 3600 * 24, () =>
            {

                var db = Common.DB.Factory.CreateDBHelper();

                string sql = "select distinct zytype as type from [special.data]";

                return db.GetDataList(sql);
            });


            EchoSuccJson(list);

        }

        void special_list_json()
        {
            string name = Request.QueryString["name"];


            int page = int.Parse(Request.QueryString["page"] ?? "1");
            int pagesize = int.Parse(Request.QueryString["pagesize"] ?? "10");

            string type = Request.QueryString["type"] ?? string.Empty;
            string level = Request.QueryString["level"] ?? string.Empty;


            var query = new Common.DB.NVCollection();
            string sqlwhere = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                sqlwhere += " and name like '%" + GetSQLSafeStr(name) + "%'";
            }

            if (!string.IsNullOrEmpty(type))
            {
                sqlwhere += " and zytype=@type";
                query["type"] = type;
            }


            if (!string.IsNullOrEmpty(level))
            {
                sqlwhere += " and [zycengci]=@level";
                query["level"] = level;
            }



            var db = Common.DB.Factory.CreateDBHelper();

            var psql = Common.DB.Factory.CreatePagerQuery();

            psql.AbsolutePage = page;
            psql.PageSize = pagesize;
            psql.Table = "[special.data]";
            psql.Fields = "id,name,code,zycengci,[zytype],[bnum],znum,zyid,ranking,rankingType";

            psql.Sort = "id asc";
            psql.Where = sqlwhere;


            string csql = psql.GetCountQueryString();

            int rc = db.ExecuteScalar<int>(csql, query);

            int pc = Convert.ToInt32(Math.Ceiling((decimal)rc / (decimal)pagesize));

            if (page > pc && pc > 0)
            {
                EchoFailJson("page>pagecount");
                return;
            }

            string qsql = psql.GetQueryString();

            var datalist = db.GetDataList(qsql, query);


            var data = new
            {

                code = 0,
                status = "succ",
                count = rc,
                page = page,
                pagesize = pagesize,
                pagecount = pc,
                data = datalist
            };



            string json = JsonConvert.SerializeObject(data);

            Response.Write(json);

            //if (datalist == null)
            //{
            //    EchoFailJson("error data is null");
            //    return;
            //}

            //EchoSuccJson(datalist);


        }

        void special_info_json()
        {
            int id = int.Parse(Request.QueryString["id"] ?? "0");
            string name = Request.QueryString["name"] ?? string.Empty;

            string sqlwhere = null;
            var nvc = new Common.DB.NVCollection();

            if (!string.IsNullOrEmpty(name))
            {
                sqlwhere = "name=@name";
                nvc["name"] = name;
            }


            if (id > 0)
            {
                sqlwhere = "id=@id";
                nvc["id"] = id;
            }


            if (string.IsNullOrEmpty(sqlwhere))
            {
                EchoFailJson("error data is null");
                return;
            }

            var db = Common.DB.Factory.CreateDBHelper();

            var entity = db.GetData("select id,name,code,zycengci,zytype,bnum,znum,zyid,ranking,rankingType,des,data from [special.data] where " + sqlwhere, nvc);

            if (entity == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            string jsonData = entity["data"] as string ?? string.Empty;

            if (!string.IsNullOrEmpty(jsonData))
            {
                entity["data"] = JsonConvert.DeserializeObject<dynamic>(jsonData);
            }

            EchoSuccJson(entity);
        }

        string GetSQLSafeStr(string str)
        {
            return str.Replace("'", "''");
        }

        void school_special_score_list_json()
        {
            int schoolid = int.Parse(Request.QueryString["schoolid"] ?? "0");
            string provinceid = Request.QueryString["provinceid"];
            string examieeid = Request.QueryString["examieeid"];
            string specialname = Request.QueryString["specialname"];

            int year = int.Parse(Request.QueryString["year"] ?? "0");

            var db = Common.DB.Factory.CreateDBHelper();

            var query = new Common.DB.NVCollection();
            string sqlwhere = " schoolid=@schoolid";
            query["schoolid"] = schoolid;


            if (!string.IsNullOrEmpty(provinceid))
            {
                sqlwhere += " and provinceid=@provinceid";
                query["provinceid"] = provinceid;
            }

            if (!string.IsNullOrEmpty(examieeid))
            {
                sqlwhere += " and examieeid=@examieeid";
                query["examieeid"] = examieeid;
            }

            if (!string.IsNullOrEmpty(specialname))
            {
                sqlwhere += " and specialname like '%" + GetSQLSafeStr(specialname) + "%'";
            }

            if (year > 0)
            {
                sqlwhere += " and [year]=@year";
                query["year"] = year;
            }

            string sql = "select [schoolid],[provinceid],[examieeid],[specialname],[year],[maxfs],[varfs],[minfs],[pc],[stype] from [school.special] where " + sqlwhere + " order by year desc";



            var datalist = db.GetDataList(sql, query);

            if (datalist == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(datalist);


        }

        void school_score_list_json()
        {
            int schoolid = int.Parse(Request.QueryString["schoolid"] ?? "0");
            string provinceid = Request.QueryString["provinceid"];
            string examieeid = Request.QueryString["examieeid"];
            string batchid = Request.QueryString["batchid"];

            int year = int.Parse(Request.QueryString["year"] ?? "0");

            var db = Common.DB.Factory.CreateDBHelper();

            var query = new Common.DB.NVCollection();
            string sqlwhere = " schoolid=@schoolid";
            query["schoolid"] = schoolid;

            if (!string.IsNullOrEmpty(provinceid))
            {
                sqlwhere += " and provinceid=@provinceid";
                query["provinceid"] = provinceid;
            }

            if (!string.IsNullOrEmpty(examieeid))
            {
                sqlwhere += " and examieeid=@examieeid";
                query["examieeid"] = examieeid;
            }

            if (!string.IsNullOrEmpty(batchid))
            {
                sqlwhere += " and batchid=@batchid";
                query["batchid"] = batchid;
            }

            if (year > 0)
            {
                sqlwhere += " and [year]=@year";
                query["year"] = year;
            }

            string sql = "select [schoolid],[provinceid],[examieeid],[batchid],[year],[maxScore],[minScore],[avgScore],[ps],[fc],[rb],[rs],[ph] from [school.score] where " + sqlwhere + " order by year desc";

            var datalist = db.GetDataList(sql, query);

            if (datalist == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(datalist);
        }

        void school_list_json()
        {

            var db = Common.DB.Factory.CreateDBHelper();

            string schoolname = Request.QueryString["schoolname"] ?? string.Empty;
            string province = Request.QueryString["province"] ?? string.Empty;
            string schooltype = Request.QueryString["schooltype"] ?? string.Empty;
            string f985 = Request.QueryString["f985"] ?? string.Empty;
            string f211 = Request.QueryString["f211"] ?? string.Empty;
            string level = Request.QueryString["level"] ?? string.Empty;
            string schoolnature = Request.QueryString["schoolnature"] ?? string.Empty;
            string specialname = Request.QueryString["specialname"] ?? string.Empty;
            string schoolproperty = Request.QueryString["schoolproperty"] ?? string.Empty;

            string zhongdian = Request.QueryString["zd"] ?? string.Empty;
            string zhuoyue = Request.QueryString["zy"] ?? string.Empty;

            int rankingbegin = int.Parse(Request.QueryString["rankingbegin"] ?? "-1");
            int rankingend = int.Parse(Request.QueryString["rankingend"] ?? "-1");


            int page = int.Parse(Request.QueryString["page"] ?? "1");
            int pagesize = int.Parse(Request.QueryString["pagesize"] ?? "10");



            var query = new Common.DB.NVCollection();
            string sqlwhere = string.Empty;
            if (!string.IsNullOrEmpty(schoolname))
            {
                sqlwhere += " and schoolname like '%" + GetSQLSafeStr(schoolname) + "%'";
            }

            if (!string.IsNullOrEmpty(province))
            {
                if (province == "北上广")
                {
                    sqlwhere += " and (province='北京' or province='上海' or province='广东')";
                }
                else
                {
                    sqlwhere += " and province=@province";
                    query["province"] = province;
                }


            }

            if (!string.IsNullOrEmpty(schooltype))
            {
                sqlwhere += " and schooltype=@schooltype";
                query["schooltype"] = schooltype;
            }


            if (!string.IsNullOrEmpty(f985))
            {
                sqlwhere += " and f985=@f985";
                query["f985"] = f985;
            }
            if (!string.IsNullOrEmpty(f211))
            {
                sqlwhere += " and f211=@f211";
                query["f211"] = f211;
            }

            if (zhongdian == "1")
            {
                sqlwhere += " and (f211=1 or f985=1)";
            }

            if (zhuoyue == "1")
            {
                sqlwhere += " and (f211=1 and f985=1)";
            }

            if (!string.IsNullOrEmpty(level))
            {
                sqlwhere += " and level=@level";
                query["level"] = level;
            }
            if (!string.IsNullOrEmpty(schoolnature))
            {
                sqlwhere += " and schoolnature=@schoolnature";
                query["schoolnature"] = schoolnature;
            }


            if (!string.IsNullOrEmpty(schoolproperty))
            {
                sqlwhere += " and schoolproperty=@schoolproperty";
                query["schoolproperty"] = schoolproperty;
            }


            if (!string.IsNullOrEmpty(specialname))
            {
                sqlwhere += " and schoolid in(select schoolid from [school.special.data] where specialid in(select id from [special.data] where name like '%" + GetSQLSafeStr(specialname) + "%'))";
            }


            if (rankingbegin < 0)
            {
                rankingbegin = 0;
            }

            if (rankingend <= 0)
            {
                rankingend = int.MaxValue;
            }

            if (rankingbegin > 0 && rankingend > 0)
            {
                sqlwhere += " and ranking>=@rankingbegin and ranking<=@rankingend";
                query["rankingbegin"] = rankingbegin;
                query["rankingend"] = rankingend;
            }



            var psql = Common.DB.Factory.CreatePagerQuery();

            psql.AbsolutePage = page;
            psql.PageSize = pagesize;
            psql.Table = "[school.data]";
            psql.Fields = "[schoolid],[schoolname],[province],[schooltype],[schoolproperty],[edudirectly],[f985],[f211],[level],[autonomyrs],[library],[membership],[schoolnature],[shoufei],[jianjie],[schoolcode],[ranking],[rankingCollegetype],[guanwang],[oldname],[master],[num],[firstrate]";

            psql.Sort = "ranking asc";
            psql.Where = sqlwhere;


            string csql = psql.GetCountQueryString();

            int rc = db.ExecuteScalar<int>(csql, query);

            int pc = Convert.ToInt32(Math.Ceiling((decimal)rc / (decimal)pagesize));

            if (page > pc && pc > 0)
            {
                //psql.AbsolutePage = page = 1;

                EchoFailJson("page>pagecount");
                return;
            }

            string qsql = psql.GetQueryString();

            var datalist = db.GetDataList(qsql, query);

            if (datalist == null)
            {
                EchoFailJson("error data is null");
                return;
            }


            var data = new
            {

                code = 0,
                status = "succ",
                count = rc,
                page = page,
                pagesize = pagesize,
                pagecount = pc,
                data = datalist
            };



            string json = JsonConvert.SerializeObject(data);

            Response.Write(json);



        }

        void school_article_details_json()
        {
            string key = Request.QueryString["key"] ?? string.Empty;
            var db = Common.DB.Factory.CreateDBHelper();
            var data = db.GetData("select schoolid,year,type,title,[key],data from [school.article] where [key]=@0 ", key);


            if (data == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(data);
        }

        void school_article_list_json()
        {
            var db = Common.DB.Factory.CreateDBHelper();

            int schoolid = int.Parse(Request.QueryString["schoolid"] ?? "0");
            string type = Request.QueryString["type"] ?? string.Empty;

            var data = db.GetDataList("select schoolid,year,type,title,[key] from [school.article] where schoolid=@0 and type=@1", schoolid, type);


            if (data == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(data);

        }


        void school_special_list_json()
        {
            var db = Common.DB.Factory.CreateDBHelper();

            int schoolid = int.Parse(Request.QueryString["schoolid"] ?? "0");

            var data = db.GetDataList(@"SELECT TOP 1000  id as specialid,name  FROM [special.data] where id in(select specialid from [school.special.data] where schoolid=@0) order by id asc ", schoolid);

            if (data == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(data);
        }

        void school_info_json()
        {
            var db = Common.DB.Factory.CreateDBHelper();

            int schoolid = int.Parse(Request.QueryString["schoolid"] ?? "0");

            var entity = db.GetData(@"SELECT TOP 1000 [schoolid],[schoolname],[province],[schooltype],[schoolproperty],[edudirectly],[f985],[f211],[level],[autonomyrs],[library],[membership],[schoolnature],[shoufei],[jianjie],[schoolcode],[ranking],[rankingCollegetype],[guanwang],[oldname],[master],[num],[firstrate],[des] FROM [school.data] where schoolid=@0 ", schoolid);

            if (entity == null)
            {
                EchoFailJson("error data is null");
                return;
            }

            EchoSuccJson(entity);

        }


        void EchoSuccJson(object data)
        {
            var rsp = new Common.DB.NVCollection();

            rsp["code"] = 0;
            rsp["status"] = "succ";
            if (data != null)
            {
                rsp["data"] = data;
            }

            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(rsp));
        }


        void EchoFailJson(string msg = null)
        {
            var rsp = new Common.DB.NVCollection();

            rsp["code"] = -1;
            rsp["status"] = "fail";
            if (!string.IsNullOrEmpty(msg))
            {
                rsp["msg"] = msg;
            }

            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(rsp));
        }



        string GetClientIP()
        {
            var Request = HttpContext.Current.Request;

            string ip = null;

            ip = Request.Headers["Cdn-Src-Ip"] ?? string.Empty;
            if (string.IsNullOrEmpty(ip))
            {
                if (Request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (ip == null)
                    {
                        ip = Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    ip = Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(ip, "unknown", true) == 0)
                {
                    return Request.UserHostAddress;
                }
            }

            return ip;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }


}