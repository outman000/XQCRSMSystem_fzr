using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// GetXQHandler 的摘要说明
    /// </summary>
    public class GetXQHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //返回值
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
                    WriteMsg("打印参数社区Code：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);

                    //判断 参数 是否为空
                    if (!String.IsNullOrEmpty(Info.Code.Trim()))
                    {
                        try
                        {
                            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                            string str = "   select * from XQCR_CommunityDistrict where ParentId=@Code  ";

                            SqlParameter[] parameters = {
                                new SqlParameter("@Code", SqlDbType.NVarChar,50) 
                            };
                            parameters[0].Value = Info.Code.Trim();

                            DataSet ds = DbHelperSQL.Query(str, parameters);

                            List<XQInfo> list = new List<XQInfo>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                XQInfo info = new XQInfo();
                                info.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                                info.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                                info.ParentID = ds.Tables[0].Rows[i]["ParentID"].ToString();
                                info.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                                info.remark = ds.Tables[0].Rows[i]["remark"].ToString();
                                info.sort = ds.Tables[0].Rows[i]["sort"].ToString();
                                list.Add(info);
                            }

                            res.ret = 0;
                            res.Datas = list;
                            res.msg = "查询成功";

                        }
                        catch (Exception ex)
                        {
                            res.ret = 1;
                            res.Datas = null;
                            res.msg = "查询失败";
                            WriteMsg("出错误：" + ex.Message + ex.StackTrace);
                        }
                    }
                    else
                    {
                        res.ret = 2;
                        res.Datas = null;
                        res.msg = "参数为空";

                    }
                }
                catch (Exception ex)
                {
                    res.ret = 3;
                    res.Datas = null;
                    res.msg = "出现异常";
                }
               
            }

            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
        }

        // 参数类
        public class ParaInfo
        {
            public string Code;//社区ID

        }

        //子集信息
        public class XQInfo
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ParentID { get; set; }
            public string Code { get; set; }
            public string remark { get; set; }
            public string sort { get; set; }

        }

        //返回值信息
        public class ReturnInfo
        {
            public int ret { get; set; }
            public List<XQInfo> Datas { get; set; } //社区数据
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