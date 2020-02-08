using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// CheckWYUserHandler 的摘要说明
    /// </summary>
    public class CheckWYUserHandler : IHttpHandler
    {
        //验证物业登录信息
        public void ProcessRequest(HttpContext context)
        {
            ReturnInfo res = new ReturnInfo();
            PropertyInfo Info = new PropertyInfo();
            var jsonString = String.Empty;
            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            if (jsonString != null && jsonString != "")
            {
                try
                {
                    jsonString = utf8_gb2312(jsonString);
                    WriteMsg("打印物业人员登录参数：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);
                    DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                    //string str = " select  *  from  XQCR_Property_Info  where  CommunityID='" + Info.CommunityID.Trim() + "'  and SubdistrictID='" + Info.SubdistrictID.Trim() + "'  and UserID='" + Info.UserID.Trim() + "' and Password='" + Info.Password.Trim() + "' ";
                    //DataSet ds = DbHelperSQL.Query(str);

                    // 判断参数 是否有 空值
                    if (!String.IsNullOrEmpty(Info.CommunityID.Trim()) && !String.IsNullOrEmpty(Info.SubdistrictID.Trim()) && !String.IsNullOrEmpty(Info.UserID.Trim()) && !String.IsNullOrEmpty(Info.Password.Trim()))
                    {
                        string sql = " select  *  from  XQCR_Property_Info  where  CommunityID=@CommunityID  and  SubdistrictID=@SubdistrictID " +
                            "  and UserID=@UserID  and Password=@Password ";

                        SqlParameter[] parameters = {
                            new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@UserID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@Password", SqlDbType.NVarChar,50)
                        };
                        parameters[0].Value = Info.CommunityID.Trim();
                        parameters[1].Value = Info.SubdistrictID.Trim();
                        parameters[2].Value = Info.UserID.Trim();
                        parameters[3].Value = Info.Password.Trim();

                        DataSet ds = DbHelperSQL.Query(sql, parameters);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            res.ret = 0;
                            res.msg = "登陆成功";
                        }
                        else
                        {
                            res.ret = 1;
                            res.msg = "用户名或密码错误";
                        }
                    }
                    else
                    {
                        res.ret = 2;
                        res.msg = "参数不全";
                    }
                   
                }
                catch (Exception ex)
                {
                    res.ret = 3;
                    res.msg = "fail:" + ex.Message;
                    WriteMsg("fail:" + ex.Message + ex.StackTrace);
                }
            }

            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
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


        // 参数类
        public class PropertyInfo
        {
            public string CommunityID;

            public string SubdistrictID;

            public string UserID;

            public string Password;
        }

        //返回值信息
        public class ReturnInfo
        {

            public int ret { get; set; }
            public string msg { get; set; }

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