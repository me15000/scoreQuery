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
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Collections.Specialized;

namespace scoreQuery.Spider
{
    //public static class GZip
    //{




    //    /// <summary>
    //    /// GZip解压函数
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    public static byte[] GZipDecompress(byte[] data)
    //    {


    //        int bufferSize = 256;


    //        using (MemoryStream stream = new MemoryStream())
    //        {
    //            using (GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
    //            {
    //                byte[] bytes = new byte[bufferSize];
    //                int n;
    //                while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
    //                {
    //                    stream.Write(bytes, 0, n);
    //                }
    //                gZipStream.Close();
    //                gZipStream.Dispose();
    //            }

    //            byte[] rdata = stream.ToArray();

    //            stream.Close();
    //            stream.Dispose();

    //            return rdata;
    //        }
    //    }
    //    /// <summary>
    //    /// GZip压缩函数
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    public static byte[] GZipCompress(byte[] data)
    //    {
    //        using (MemoryStream stream = new MemoryStream())
    //        {
    //            using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress))
    //            {
    //                gZipStream.Write(data, 0, data.Length);
    //                gZipStream.Close();
    //                gZipStream.Dispose();
    //            }
    //            return stream.ToArray();
    //        }
    //    }
    //}
    //public class HttpDataInfo
    //{
    //    public HttpDataInfo()
    //    {
    //        headers = new NameValueCollection();
    //    }

    //    public int StatusCode { get; set; }
    //    public string StatusDescription { get; set; }
    //    public string Scheme { get; set; }


    //    NameValueCollection headers;
    //    public NameValueCollection Headers
    //    {
    //        get { return headers; }
    //        set { headers = value; }
    //    }

    //    public string HeadersStrings { get; set; }

    //    public int ContentLength { get; set; }
    //}

    //public class HttpData : HttpDataInfo
    //{
    //    public byte[] BodyData { get; set; }
    //}

    //public class HttpClient
    //{

    //    const string Sign_Content_Length = "Content-Length";
    //    const string Sign_Header_End = "\r\n";

    //    const string Sign_Chunked_End = "\r\n";
    //    const string Sign_Chunked = "chunked";

    //    const int None_Data = -1;
    //    const int Buffer_Size = 256;

    //    byte[] ParseChunkedData(List<byte> data)
    //    {
    //        int endPosition = data.Count;

    //        List<byte> list = new List<byte>();

    //        StringBuilder sb = new StringBuilder();

    //        for (int i = 0; i < data.Count; i++)
    //        {
    //            byte b = data[i];

    //            char c = (char)b;

    //            sb.Append(c);

    //            string line = sb.ToString();

    //            int inx = line.IndexOf(Sign_Chunked_End);

    //            if (inx > 0)
    //            {
    //                string hex = line.Substring(0, inx);

    //                int count = Convert.ToInt32(hex, 16);

    //                if (count > 0)
    //                {
    //                    var range = data.GetRange(i + 1, count);

    //                    list.AddRange(range);

    //                    i = i + count + 2;
    //                }
    //                else if (count == 0)
    //                {
    //                    break;
    //                }

    //                sb.Clear();
    //            }
    //        }

    //        return list.ToArray();
    //    }

    //    bool IsSame(char[] chars, List<byte> bytes)
    //    {
    //        if (chars.Length != bytes.Count)
    //        {
    //            return false;
    //        }

    //        bool same = true;

    //        for (int i = 0; i < chars.Length; i++)
    //        {
    //            same = same && chars[i] == bytes[i];
    //        }

    //        return same;

    //    }

    //    char[] Sign_End = { '\r', '\n', '\r', '\n' };

    //    int receiveTimeout = 5000;

    //    /// <summary>
    //    /// 接收超时默认2秒
    //    /// </summary>
    //    public int ReceiveTimeout
    //    {
    //        get { return receiveTimeout; }
    //        set { receiveTimeout = value; }
    //    }

    //    int sendTimeout = 5000;

    //    /// <summary>
    //    /// 发送超时默认1秒
    //    /// </summary>
    //    public int SendTimeout
    //    {
    //        get { return sendTimeout; }
    //        set { sendTimeout = value; }
    //    }




    //    List<byte> ReceiveAllData(Socket socket, byte[] headersData)
    //    {
    //        int zero_times = 0;
    //        int zero_max = 3;

    //        List<byte> list = new List<byte>(1024 * 64);

    //        int nowContentLength = 0;

    //        byte[] buffer = new byte[Buffer_Size];

    //        try
    //        {
    //            while (true)
    //            {
    //                int receiveCount = socket.Receive(buffer, Buffer_Size, SocketFlags.None);

    //                if (receiveCount > 0)
    //                {
    //                    zero_times = 0;


    //                    for (int i = 0; i < receiveCount; i++)
    //                    {
    //                        list.Add(buffer[i]);
    //                    }
    //                    nowContentLength += receiveCount;
    //                }

    //                if (receiveCount == 0 || receiveCount < Buffer_Size)
    //                {
    //                    zero_times++;
    //                    if (zero_times >= zero_max)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //        }

    //        return list;
    //    }

    //    public HttpData GetData(Socket socket, byte[] headersData)
    //    {
    //        List<byte> list_data = ReceiveAllData(socket, headersData);

    //        byte[] header_data = null;
    //        List<byte> body_data_list = null;

    //        int sign_len = Sign_End.Length;

    //        //拆分出 HTTP Header Body
    //        if (list_data.Count > sign_len)
    //        {

    //            for (int i = 0; i <= list_data.Count - sign_len; i++)
    //            {
    //                var range = list_data.GetRange(i, sign_len);

    //                if (IsSame(Sign_End, range))
    //                {
    //                    header_data = list_data.GetRange(0, i + sign_len).ToArray();

    //                    if (header_data.Length < list_data.Count)
    //                    {
    //                        body_data_list = list_data.GetRange(i + sign_len, list_data.Count - header_data.Length);
    //                    }

    //                    break;
    //                }
    //            }
    //        }


    //        if (header_data == null)
    //        {
    //            return null;
    //        }

    //        string headerString = Encoding.ASCII.GetString(header_data);

    //        HttpDataInfo info = ParseHeaderString(headerString);

    //        HttpData data = null;

    //        if (info != null)
    //        {
    //            data = new HttpData
    //            {

    //                HeadersStrings = info.HeadersStrings
    //                ,
    //                Scheme = info.Scheme
    //                ,
    //                StatusCode = info.StatusCode
    //                ,
    //                StatusDescription = info.StatusDescription
    //                ,
    //                Headers = info.Headers
    //            };

    //            data.BodyData = new byte[0];

    //            if (body_data_list != null)
    //            {
    //                if (info.ContentLength > 0)
    //                {
    //                    if (body_data_list == null)
    //                    {
    //                        throw new Exception("ContentLength>0 and ReceiveBodyData is null");
    //                    }
    //                    else
    //                    {
    //                        data.BodyData = body_data_list.ToArray();
    //                    }
    //                }
    //                else if (info.ContentLength == None_Data)
    //                {
    //                    string tran = info.Headers["Transfer-Encoding"] ?? string.Empty;

    //                    if (tran.Equals(Sign_Chunked, StringComparison.OrdinalIgnoreCase))
    //                    {

    //                        if (body_data_list == null)
    //                        {

    //                            throw new Exception("Transfer-Encoding is " + Sign_Chunked + " and ReceiveBodyData is null");
    //                        }
    //                        else
    //                        {
    //                            data.BodyData = ParseChunkedData(body_data_list);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        data.BodyData = body_data_list.ToArray();
    //                    }
    //                }
    //            }

    //            if (data.BodyData != null)
    //            {
    //                data.ContentLength = data.BodyData.Length;
    //            }
    //            else
    //            {
    //                data.ContentLength = 0;
    //            }
    //        }

    //        return data;
    //    }


    //    public HttpData GetData(string host, byte[] headersData)
    //    {
    //        return GetData(host, 80, headersData);
    //    }

    //    public HttpData GetData(string host, string headersString)
    //    {
    //        return GetData(host, Encoding.ASCII.GetBytes(headersString));
    //    }

    //    public HttpData GetData(string host, int port, string headersString)
    //    {
    //        return GetData(host, port, Encoding.ASCII.GetBytes(headersString));
    //    }

    //    public HttpData GetData(string host, int port, byte[] headersData)
    //    {
    //        HttpData data = null;

    //        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    //        {
    //            socket.ReceiveTimeout = receiveTimeout;
    //            socket.SendTimeout = sendTimeout;


    //            int loopTimes = 0;
    //            int loopTimesMax = 2;

    //            loop:

    //            if (!socket.Connected)
    //            {
    //                socket.Connect(host, port);
    //            }

    //            if (!socket.Connected)
    //            {
    //                loopTimes++;

    //                if (loopTimes < loopTimesMax)
    //                {
    //                    goto loop;
    //                }
    //            }

    //            if (socket.Connected)
    //            {
    //                socket.Send(headersData);
    //                data = GetData(socket, headersData);
    //                socket.Shutdown(SocketShutdown.Both);
    //            }
    //        }

    //        return data;
    //    }

    //    public HttpData GetData(IPEndPoint address, string headersString)
    //    {
    //        return GetData(address, Encoding.ASCII.GetBytes(headersString));
    //    }

    //    public HttpData GetData(IPEndPoint address, byte[] headersData)
    //    {
    //        HttpData data = null;

    //        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    //        {
    //            socket.ReceiveTimeout = receiveTimeout;
    //            socket.SendTimeout = sendTimeout;
    //            int loopTimes = 0;
    //            int loopTimesMax = 2;

    //            loop:

    //            if (!socket.Connected)
    //            {
    //                socket.Connect(address);
    //            }

    //            if (!socket.Connected)
    //            {
    //                loopTimes++;

    //                if (loopTimes < loopTimesMax)
    //                {
    //                    goto loop;
    //                }
    //            }

    //            if (socket.Connected)
    //            {
    //                socket.Send(headersData);
    //                data = GetData(socket, headersData);
    //                socket.Shutdown(SocketShutdown.Both);
    //            }
    //        }

    //        return data;
    //    }

    //    public static HttpDataInfo ParseHeaderString(string headersStringText)
    //    {

    //        string[] headersStrings = headersStringText.Split(new string[] { Sign_Header_End }, StringSplitOptions.None);

    //        if (headersStrings.Length > 0)
    //        {
    //            HttpDataInfo info = new HttpDataInfo();
    //            info.ContentLength = None_Data;

    //            info.HeadersStrings = headersStringText;

    //            string firstLine = headersStrings[0].Trim();

    //            if (!string.IsNullOrEmpty(firstLine))
    //            {
    //                int inx_space1 = firstLine.IndexOf(' ');

    //                int inx_space2 = firstLine.IndexOf(' ', inx_space1 + 1);

    //                if (inx_space2 > inx_space1 && inx_space1 > 0)
    //                {
    //                    string codeString = firstLine.Substring(inx_space1 + 1, inx_space2 - inx_space1);

    //                    int code = 0;

    //                    if (int.TryParse(codeString, out code))
    //                    {
    //                        info.StatusCode = code;
    //                    }

    //                    info.Scheme = firstLine.Substring(0, inx_space1);

    //                    info.StatusDescription = firstLine.Substring(inx_space2 + 1);
    //                }
    //            }

    //            string headerSplitSign = ": ";

    //            if (headersStrings.Length > 1)
    //            {
    //                for (int i = 1; i < headersStrings.Length; i++)
    //                {
    //                    string header = headersStrings[i].Trim();

    //                    int signIndex = header.IndexOf(headerSplitSign);

    //                    int valueIndex = signIndex + headerSplitSign.Length;

    //                    if (signIndex > 0 && valueIndex < header.Length)
    //                    {
    //                        string key = header.Substring(0, signIndex);

    //                        string value = header.Substring(valueIndex);

    //                        if (key.Equals(Sign_Content_Length, StringComparison.OrdinalIgnoreCase))
    //                        {
    //                            int contentLength = 0;
    //                            if (int.TryParse(value, out contentLength))
    //                            {
    //                                info.ContentLength = contentLength;
    //                            }
    //                            else
    //                            {
    //                                info.ContentLength = None_Data;
    //                            }
    //                        }

    //                        info.Headers[key] = value;
    //                    }
    //                }
    //            }

    //            return info;
    //        }

    //        return null;
    //    }

    //}

    public class HttpUtil
    {
        /*
        public static string HttpClientGet(string Url, string encoding = "utf-8")
        {
            Uri uri = new Uri(Url);

            var rsb = new StringBuilder();

            rsb.AppendFormat("GET {0} HTTP/1.1\r\n", uri.PathAndQuery);
            rsb.AppendFormat("Host: {0}\r\n", uri.Host);
            rsb.AppendFormat("Connection: close\r\n");
            rsb.Append("\r\n");



            HttpClient client = new HttpClient();
            client.ReceiveTimeout = 3000;
            client.SendTimeout = 3000;

            int loopTimes = 0;
            loop:
            loopTimes++;
            var data = client.GetData(uri.Host, Encoding.ASCII.GetBytes(rsb.ToString()));

            if (data == null)
            {
                if (loopTimes < 3)
                {

                    Console.WriteLine("httpget data is null loop:" + loopTimes);
                    goto loop;
                }

                return null;
            }

            if (data.StatusCode != 200)
            {
                Console.WriteLine(data.StatusCode);
                return null;
            }
            byte[] bodydata = data.BodyData;

            if (bodydata != null)
            {
                return Encoding.GetEncoding(encoding).GetString(bodydata);
            }
            else
            {
                return null;
            }
        }
        */



        public static string HttpGet(string Url, string encoding = "utf-8")
        {
            int loopTimes = 0;
            loop:
            loopTimes++;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=" + encoding;
                request.Timeout = 3000;
                request.AllowAutoRedirect = false;
                request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine(response.StatusCode);
                        return null;
                    }


                    string content = null;

                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream, Encoding.GetEncoding(encoding)))
                        {
                            content = reader.ReadToEnd();
                            reader.Close();
                        }

                        responseStream.Close();
                    }

                    if (string.IsNullOrEmpty(content))
                    {
                        return null;
                    }

                    byte[] bytes = new byte[] { 0xc2, 160 };
                    string oldValue = Encoding.GetEncoding(encoding).GetString(bytes);

                    return content.Replace(oldValue, "&nbsp;");
                }

            }
            catch (Exception ex)
            {
                if (loopTimes < 5)
                {
                    goto loop;
                }

                Console.WriteLine("error:" + Url);

            }

            return null;
        }

    }



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

        const string ScoreQueryUrl = "https://gkcx.eol.cn/schoolhtm/scores/provinceScores{0}_{1}_{2}_{3}.xml";//{学校id}_{省份id}_{文理科}_{批次}
        const string ScoreQueryUrlSpecial = "https://gkcx.eol.cn/commonXML/schoolSpecialPoint/schoolSpecialPoint{0}_{1}_{2}.xml";//{学校id}_{省份id}_{文理科}

        class SpecialScoreInfo
        {
            public int schoolid { get; set; }
            public string provinceid { get; set; }
            public string examieeid { get; set; }
            public string specialname { get; set; }
            public int year { get; set; }

            public string maxfs { get; set; }
            public string varfs { get; set; }
            public string minfs { get; set; }
            public string pc { get; set; }
            public string stype { get; set; }
        }


        static Common.DB.IDBHelper db = Common.DB.Factory.CreateDBHelper();

        bool ExistsDBSpecial(SpecialScoreInfo info)
        {
            bool exists = db.Exists("select top 1 1 from [school.special] where schoolid=@0 and provinceid=@1 and examieeid=@2 and specialname=@3 and [year]=@4", info.schoolid, info.provinceid, info.examieeid, info.specialname, info.year);

            return exists;
        }

        void SaveDBSpecial(SpecialScoreInfo info)
        {
            if (ExistsDBSpecial(info))
            {
                return;
            }

            var nvc = new Common.DB.NVCollection();
            nvc["schoolid"] = info.schoolid;
            nvc["provinceid"] = info.provinceid;
            nvc["examieeid"] = info.examieeid;
            nvc["specialname"] = info.specialname;
            nvc["year"] = info.year;
            nvc["maxfs"] = info.maxfs;
            nvc["varfs"] = info.varfs;
            nvc["minfs"] = info.minfs;
            nvc["pc"] = info.pc;
            nvc["stype"] = info.stype;

            db.ExecuteNoneQuery("insert into [school.special]([schoolid],[provinceid],[examieeid],[specialname],[year],[maxfs],[varfs],[minfs],[pc],[stype]) values(@schoolid,@provinceid,@examieeid,@specialname,@year,@maxfs,@varfs,@minfs,@pc,@stype)", nvc);
        }


        bool ExistsDB(ScoreInfo info)
        {
            bool exists = db.Exists("select top 1 1 from [school.score] where schoolid=@0 and provinceid=@1 and examieeid=@2 and batchid=@3 and [year]=@4", info.schoolid, info.provinceid, info.examieeid, info.batchid, info.year);

            return exists;
        }

        void SaveDB(ScoreInfo info)
        {


            if (ExistsDB(info))
            {
                return;
            }
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

        void RunSchoolScore(int schoolid, string provinceid)
        {


            foreach (var exa in ExamieeType)
            {
                Task.Delay(2000).Wait();

                foreach (var bat in BatchType)
                {
                    bool succ = false;
                    {

                        dynamic taskobj = new
                        {
                            schoolid = schoolid,
                            proid = provinceid,
                            exaid = exa.Value,
                            batid = bat.Value
                        };

                        succ = RunSchoolScore(taskobj.schoolid, taskobj.proid, taskobj.exaid, taskobj.batid);
                    }


                    if (succ)
                    {

                        foreach (var pro in Provinces)
                        {
                            if (pro.Value.Equals(provinceid))
                            {
                                continue;
                            }

                            dynamic taskobj = new
                            {
                                schoolid = schoolid,
                                proid = pro.Value,
                                exaid = exa.Value,
                                batid = bat.Value
                            };

                            RunSchoolScore(taskobj.schoolid, taskobj.proid, taskobj.exaid, taskobj.batid);
                        }
                    }
                  
                    
                }
            }


            /*
            int threads = 3;

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
            */
        }

        void RunSchoolSpecialScore(int schoolid, string provinceid = null)
        {


            foreach (var exa in ExamieeType)
            {
                Task.Delay(100).Wait();

                bool succ = false;

                {
                    dynamic taskobj = new
                    {
                        schoolid = schoolid,
                        proid = provinceid,
                        exaid = exa.Value
                    };



                    succ = RunSchoolSpecialScore(taskobj.schoolid, taskobj.proid, taskobj.exaid);
                }


                if (succ)
                {
                    foreach (var pro in Provinces)
                    {
                        if (pro.Value.Equals(provinceid))
                        {
                            continue;
                        }

                        dynamic taskobj = new
                        {
                            schoolid = schoolid,
                            proid = pro.Value,
                            exaid = exa.Value
                        };

                        RunSchoolSpecialScore(taskobj.schoolid, taskobj.proid, taskobj.exaid);
                    }
                }


            }





        }


        static System.Text.RegularExpressions.Regex Reg_Space = new System.Text.RegularExpressions.Regex(@"\s+");

        bool RunSchoolSpecialScore(int schoolid, string provinceid, string examieeid)
        {


            string url = string.Format(ScoreQueryUrlSpecial, schoolid, provinceid, examieeid);


            Console.WriteLine(url);


            int loopTimes = 0;
            loop:
            loopTimes++;



            string xmlContent = HttpUtil.HttpGet(url);
            if (string.IsNullOrEmpty(xmlContent))
            {
                return false;
            }

            var xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(xmlContent);

            }
            catch (Exception ex)
            {


                if (loopTimes < 3)
                {

                    Console.WriteLine("xml load error goto loop " + loopTimes);
                    goto loop;

                }
                else
                {
                    return false;
                }
            }

            if (xmldoc == null)
            {
                return false;
            }

            var nodes = xmldoc.SelectNodes("//areapiont");


            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    int year = int.Parse(node.SelectSingleNode("year").InnerText);
                    string specialname = node.SelectSingleNode("specialname").InnerText.Trim();
                    string maxfs = node.SelectSingleNode("maxfs").InnerText;
                    string varfs = node.SelectSingleNode("varfs").InnerText;
                    string minfs = node.SelectSingleNode("minfs").InnerText;
                    string pc = node.SelectSingleNode("pc").InnerText;
                    string stype = node.SelectSingleNode("stype").InnerText;

                    specialname = Reg_Space.Replace(specialname, string.Empty);

                    var ent = new SpecialScoreInfo()
                    {
                        year = year,

                        maxfs = maxfs,
                        minfs = minfs,
                        pc = pc,
                        specialname = specialname,
                        stype = stype,
                        varfs = varfs,

                        examieeid = examieeid,
                        provinceid = provinceid,
                        schoolid = schoolid
                    };

                    SaveDBSpecial(ent);
                }

            }
            return true;
        }

        bool RunSchoolScore(int schoolid, string provinceid, string examieeid, string batchid)
        {
            if (ExistsDB(new ScoreInfo { schoolid = schoolid, provinceid = provinceid, examieeid = examieeid, batchid = batchid }))
            {
                Console.WriteLine("exists ");
                return false;
            }

            string url = string.Format(ScoreQueryUrl, schoolid, provinceid, examieeid, batchid);

            Console.WriteLine(url);


            int loopTimes = 0;
            loop:
            loopTimes++;



            string xmlContent = HttpUtil.HttpGet(url);
            if (string.IsNullOrEmpty(xmlContent))
            {
                return false;
            }

            var xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(xmlContent);

            }
            catch (Exception ex)
            {


                if (loopTimes < 3)
                {

                    Console.WriteLine("xml load error goto loop " + loopTimes);
                    goto loop;

                }
                else
                {
                    return false;
                }
            }

            if (xmldoc == null)
            {
                return false;
            }

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

            return true;

        }

        public void RunSchoolsScores()
        {


            int previd = 0;
            bool hasNext = true;
            do
            {
                var list = db.GetDataList("select top 5 schoolid,province from [school.data] where schoolid>@0 order by schoolid asc", previd);

                for (int i = 0; i < list.Count; i++)
                {
                    var ent = list[i];

                    previd = Convert.ToInt32(ent["schoolid"]);
                    string province = ent["province"] as string ?? string.Empty;
                    if (string.IsNullOrEmpty(province))
                    {
                        continue;
                    }

                    if (!Provinces.ContainsKey(province))
                    {
                        continue;
                    }

                    string provinceid = Provinces[province];


                    RunSchoolScore(previd, provinceid);

                }

                if (list.Count == 0)
                {

                    Console.WriteLine("complete");

                    hasNext = false;
                }

            } while (hasNext);

            /*
            var timer = new System.Timers.Timer();
            timer.Interval = 60 * 10;
            timer.Enabled = true;
            timer.AutoReset = false;
            bool isruning = false;

            timer.Elapsed += new System.Timers.ElapsedEventHandler((obj, e) =>
            {
                Console.WriteLine("timer.Elapsed");

                if (isruning)
                {
                    return;
                }

                isruning = true;

                var list = db.GetDataList("select top 5 schoolid from [school.data] where schoolid>@0 order by schoolid asc", previd);

                for (int i = 0; i < list.Count; i++)
                {
                    var ent = list[i];

                    previd = Convert.ToInt32(ent["schoolid"]);

                    RunSchoolScore(previd);

                }

                if (list.Count == 0)
                {
                    timer.Stop();

                    Console.WriteLine("complete");
                }

                isruning = false;
            });



            timer.Start();

            Console.Read();
            */
        }

        public void RunSchoolsScoresSpecial()
        {
            /*province
山西*/

            int previd = 0;
            bool hasNext = true;
            do
            {
                var list = db.GetDataList("select top 5 schoolid,province from [school.data] where schoolid>@0 order by schoolid asc", previd);

                for (int i = 0; i < list.Count; i++)
                {
                    var ent = list[i];

                    previd = Convert.ToInt32(ent["schoolid"]);

                    string province = ent["province"] as string ?? string.Empty;
                    if (string.IsNullOrEmpty(province))
                    {
                        continue;
                    }

                    if (!Provinces.ContainsKey(province))
                    {
                        continue;
                    }

                    string provinceid = Provinces[province];

                    RunSchoolSpecialScore(previd, provinceid);

                }

                if (list.Count == 0)
                {

                    Console.WriteLine("complete");

                    hasNext = false;
                }

            } while (hasNext);

            /*
            var timer = new System.Timers.Timer();
            timer.Interval = 60 * 10;
            timer.Enabled = true;
            timer.AutoReset = false;
            bool isruning = false;

            timer.Elapsed += new System.Timers.ElapsedEventHandler((obj, e) =>
            {
                Console.WriteLine("timer.Elapsed");

                if (isruning)
                {
                    return;
                }

                isruning = true;

                var list = db.GetDataList("select top 5 schoolid from [school.data] where schoolid>@0 order by schoolid asc", previd);

                for (int i = 0; i < list.Count; i++)
                {
                    var ent = list[i];

                    previd = Convert.ToInt32(ent["schoolid"]);

                    RunSchoolScore(previd);

                }

                if (list.Count == 0)
                {
                    timer.Stop();

                    Console.WriteLine("complete");
                }

                isruning = false;
            });



            timer.Start();

            Console.Read();
            */
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

                case "sss":
                    new SchoolsScoreSpider().RunSchoolsScoresSpecial();
                    break;

                default:

                    break;
            }


        }
    }
}
