using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;
using System.Security;
using System.Security.Cryptography;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// TestHandler 的摘要说明
    /// </summary>
    public class TestHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //返回值
            ReturnInfo res = new ReturnInfo();
            //uid
            string uid = "";
            if (context.Request.Form["uid"] != null && context.Request.Form["uid"] != "")
            {
                uid = context.Request.Form["uid"].ToString();
            }
            WriteMsg("打印参数用户uid：" + uid);

            if (uid.Trim() != "")
            {
                //wx0e1049c16a871a61
                string paasid = ConfigurationManager.AppSettings["paasid"].ToString();
                //16TQdO2xkRq5a0csdjAVmkVxV2A2Iuig
                string Token = ConfigurationManager.AppSettings["Token"].ToString();

                //由调用者/被调用者/网关生成的非重复的随机字符串（十分钟内不能重复）
                string nonce = GetRandomCode(20);
                //时间戳
                string timestamp = GetTimeStamp();
                //拼接字符串  未加密前
                string combinstr = timestamp + Token + nonce + timestamp;
                WriteMsg("未加密字符串拼接：" + combinstr);
                string signature = sha256(combinstr);
                WriteMsg("已加密字符串：" + signature);



                UserInfo userInfo = new UserInfo();
                //调用腾讯接口 获取用户 姓名、身份证等信息，参数是 uid
                //http://smartgate.bhteda.com/ebus/tifapi/user/getuserinfo
                string url = ConfigurationManager.AppSettings["WGUrl"].ToString();

                string postData = "\"uid\":\""+ uid + "\"";
                string resStr = GetPage(url, postData, paasid, signature, timestamp, nonce);
                resStr = utf8_gb2312(resStr);
                WriteMsg("打印接口返回值：" + resStr);
                //userInfo = JsonConvert.DeserializeAnonymousType(resStr, userInfo);

                res.ret = 0;
                res.info = userInfo;
                res.msg = resStr;

            }
            else
            {
                res.ret = 1;
                res.info = null;
                res.msg = "参数信息为空";

            }

            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
        }



      
 


        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        private string GetRandomCode(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// sha256 加密
        /// </summary>
        /// <returns></returns>
        public string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// 后台再请求别的接口的方法，体现了添加request header
        /// </summary>
        /// <returns></returns>
        public string GetPage(string posturl, string postData,string paasid,string signature,string timestamp,string nonce)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.Headers.Add("x-tif-paasid", paasid);
                request.Headers.Add("x-tif-signature", signature);
                request.Headers.Add("x-tif-timestamp", timestamp);
                request.Headers.Add("x-tif-nonce", nonce);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                }
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                WriteMsg("接口调用出现异常：" + ex.Message + ex.StackTrace);
                return string.Empty;
            }
        }

        //用户信息基础信息类
        public class UserInfo
        {
            public string appid;//当前账号的appid

            public string openid;//当前账号的openid

            public string unionid;//当前账号的unionid

            public string cid;//当前账号绑定的身份证号

            public string name;//当前账号绑定的身份证姓名

            public string phone;//当前账号绑定的手机号	

            public string cid_start_date;//当前账号绑定的身份证起始时间

            public string cid_expire_date;//当前账号绑定的身份证失效时间

            public string face_cid;//最近刷脸身份证号

            public string face_name;//最近刷脸身份证姓名

            public string face_time;//最近刷脸时间

            public string real_cid;//最近支付实名身份证号

            public string real_name;//最近支付实名身份证姓名

            public string real_time;//最近支付实名时间

        }
 

        //返回值信息
        public class ReturnInfo
        {
            public int ret { get; set; }
            public UserInfo info { get; set; }
            public string msg { get; set; }

        }


        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }


        //生成错误日志文件
        public static void WriteMsg(string msg)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/") + "log\\";
                if (!Directory.Exists(path))//判断是否有该文件 
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件 
                if (!File.Exists(logFileName))//判断日志文件是否为当天
                {
                    FileStream fs;
                    fs = File.Create(logFileName);//创建文件
                    fs.Close();
                }
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流

                writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\r\n" + msg);
                writer.WriteLine("--------------------------------分割线--------------------------------");
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {

            }

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