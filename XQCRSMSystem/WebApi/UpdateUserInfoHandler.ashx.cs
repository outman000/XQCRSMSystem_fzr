using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// UpdateUserInfoHandler 的摘要说明
    /// </summary>
    public class UpdateUserInfoHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnInfo res = new ReturnInfo();
            ParaInfo Info = new ParaInfo();
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
                    WriteMsg("打印出入人员更新参数：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);
                    DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                    string str = " update  XQCR_Personnel_Info set ResidentOrVisitor='" + Info.ResidentOrVisitor.Trim() + "',Status='1',UpdateDate='" + 
                        DateTime.Now + "',UpdateUser='" + Info.PropertyID.Trim() + "'  where  ID='" + Info.ID.Trim() + "'";
                    int i = DbHelperSQL.ExecuteSql(str);
                    if (i > 0)
                    {
                        res.ret = 0;
                        res.msg = "更新成功";
                    }
                    else
                    {
                        res.ret = 1;
                        res.msg = "更新失败";
                    }
                }
                catch (Exception ex)
                {
                    res.ret = 2;
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
        public class ParaInfo
        {
            public string ID;//出入人员ID

            public string PropertyID;//物业人员账号

            public string ResidentOrVisitor; //访客或常驻
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