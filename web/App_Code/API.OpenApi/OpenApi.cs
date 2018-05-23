
using System;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

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

                case "/special/type/list.json":
                    special_type_list_json();
                    break;

                case "/special/list.json":
                    special_list_json();
                    break;

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

            }
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

            if (page > pc)
            {
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

            EchoSuccJson(datalist);


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

            var entity = db.GetData("select id,name,code,zycengci,zytype,bnum,znum,zyid,ranking,rankingType,des from [special.data] where " + sqlwhere, nvc);

            if (entity == null)
            {
                EchoFailJson("error data is null");
                return;
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

            if (page > pc)
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
            var data = db.GetData("select schoolid,year,type,title,[key],data from [school.article] where [key]=@ ", key);


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