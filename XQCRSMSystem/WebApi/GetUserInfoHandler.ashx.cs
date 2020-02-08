using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// GetUserInfoHandler 的摘要说明
    /// </summary>
    public class GetUserInfoHandler : IHttpHandler
    {
        // 获取出入人员 基本信息 
        public void ProcessRequest(HttpContext context)
        {
            //返回值
            ReturnInfo res = new ReturnInfo();
            ParaInfo ParaInfo = new ParaInfo();
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
                    WriteMsg("打印参数人员信息ID：" + jsonString);
                    ParaInfo = JsonConvert.DeserializeAnonymousType(jsonString, ParaInfo);

                    if (!String.IsNullOrEmpty(ParaInfo.ID.Trim()))
                    {
                        DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                        string str = " select * from XQCR_Personnel_Info  where   ID=@ID ";
                        SqlParameter[] parameters = {
                                new SqlParameter("@ID", SqlDbType.NVarChar,50)
                            };
                        parameters[0].Value = ParaInfo.ID.Trim();
                        DataSet ds = DbHelperSQL.Query(str, parameters);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            UserInfo Info = new UserInfo();
                            Info.ID = ds.Tables[0].Rows[0]["ID"].ToString();
                            Info.PersonID = ds.Tables[0].Rows[0]["PersonID"].ToString();
                            Info.CommunityID = ds.Tables[0].Rows[0]["CommunityID"].ToString();
                            Info.Community = ds.Tables[0].Rows[0]["Community"].ToString();
                            Info.SubdistrictID = ds.Tables[0].Rows[0]["SubdistrictID"].ToString();
                            Info.Subdistrict = ds.Tables[0].Rows[0]["Subdistrict"].ToString();
                            Info.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                            Info.Certificate = ds.Tables[0].Rows[0]["Certificate"].ToString();
                            Info.CertificateID = ds.Tables[0].Rows[0]["CertificateID"].ToString();
                            Info.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                            Info.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                            Info.ResidentOrVisitor = ds.Tables[0].Rows[0]["ResidentOrVisitor"].ToString();
                            Info.Memo = ds.Tables[0].Rows[0]["Memo"].ToString();
                            Info.CreateDate = ds.Tables[0].Rows[0]["CreateDate"].ToString();

                            res.ret = 0;
                            res.info = Info;
                            res.msg = "查看填报信息";

                        }
                        else
                        {
                            res.ret = 0;
                            res.info = null;
                            res.msg = "该居民未填报";
                        }
                    }
                    else
                    {
                        res.ret = 1;
                        res.info = null;
                        res.msg = "参数信息为空";

                    }

                }
                catch (Exception ex)
                {
                    res.ret = 2;
                    res.info = null;
                    res.msg = "出现异常";
                }

            }

 

       
            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
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
            public string ID;//ID

        }

        //人员基础信息类
        public class UserInfo
        {
            public string ID;//ID

            public string PersonID;//ID

            public string CommunityID;//社区ID

            public string Community;//社区

            public string SubdistrictID;//小区ID

            public string Subdistrict;//小区

            public string Name;//姓名

            public string Certificate;//证件类型

            public string CertificateID;//证件编号

            public string Mobile;//联系电话

            public string Address;//详细地址信息

            public string ResidentOrVisitor;//是否常驻或访客

            public string Memo;//备注

            public string CreateDate;//创建时间
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


        //返回值信息
        public class ReturnInfo
        {
            public int ret { get; set; }
            public UserInfo info { get; set; }
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