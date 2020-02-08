using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using XQCRSMSystem.Model;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// WYScanHandler 的摘要说明
    /// </summary>
    public class WYScanHandler : IHttpHandler
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
                    WriteMsg("打印扫码参数：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);

                    //数据 推送IOC 城市大脑  预留功能 

                    //进行数据比对 返回核对结果
                    DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                    //查询 本人在该小区登记信息
                    string sql = " select  *  from  XQCR_Personnel_Info  where  ResidentOrVisitor='0' and  CommunityID=@CommunityID   " +
                           "  and  SubdistrictID=@SubdistrictID  and Name=@Name  and CertificateID=@CertificateID ";

                    SqlParameter[] parameters = {
                            new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@CertificateID", SqlDbType.NVarChar,50)
                        };
                    parameters[0].Value = Info.CommunityID.Trim();
                    parameters[1].Value = Info.SubdistrictID.Trim();
                    parameters[2].Value = Info.Name.Trim();
                    parameters[3].Value = Info.CertificateID.Trim();

                    DataSet dstr = DbHelperSQL.Query(sql, parameters);

                    if (dstr.Tables[0].Rows.Count > 0)
                    {
                        //定义 进入信息 model
                        XQCR_EnterList enterList = new XQCR_EnterList();
                        XQCR_EnterList enter = new XQCR_EnterList();

                        enter.ID = Guid.NewGuid().ToString();
                        enter.UserID = dstr.Tables[0].Rows[0]["ID"].ToString();
                        enter.CommunityID = Info.CommunityID.Trim();
                        enter.SubdistrictID = Info.SubdistrictID.Trim();
                        enter.Name = Info.Name.Trim();
                        enter.Certificate = dstr.Tables[0].Rows[0]["Certificate"].ToString();
                        enter.CertificateID = Info.CertificateID.Trim();
                        enter.CreateUser = Info.CertificateID.Trim();
                        enter.CreateDate = DateTime.Now;
                        enter.UpdateUser = Info.CertificateID.Trim();
                        enter.UpdateDate = DateTime.Now;


                        //查询是否在 黑名单
                        string check = " select  *  from  XQCR_BlackList  where Name=@Name and CertificateID=@CertificateID ";
                        SqlParameter[] paracheck = {
                                             new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                                             new SqlParameter("@CertificateID", SqlDbType.NVarChar,50)
                                        };
                        paracheck[0].Value = Info.Name.Trim();
                        paracheck[1].Value = Info.CertificateID.Trim();
                        DataSet ds = DbHelperSQL.Query(check, paracheck);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // 保存进入信息 model
                            enter.Status = "9";
                            int n = enterList.Add(enter);
                            if (n > 0)
                            {
                                res.ret = 9;
                                res.msg = "属于黑名单人员，不得进入";
                            }
                            else
                            {
                                res.ret = 3;
                                res.msg = "插入进入信息表出现异常";
                            }
                        }
                        else
                        {
                            // 保存进入信息 model
                            enter.Status = "0";
                            int n = enterList.Add(enter);
                            if (n > 0)
                            {
                                res.ret = 0;
                                res.msg = "绿灯通过";
                            }
                            else
                            {
                                res.ret = 3;
                                res.msg = "插入进入信息表出现异常";
                            }
                        }
                    }
                    else
                    {
                        //定义 进入信息 model
                        XQCR_EnterList enterList = new XQCR_EnterList();
                        XQCR_EnterList enter = new XQCR_EnterList();

                        enter.ID = Guid.NewGuid().ToString();
                        enter.UserID = "";
                        enter.CommunityID = Info.CommunityID.Trim();
                        enter.SubdistrictID = Info.SubdistrictID.Trim();
                        enter.Name = Info.Name.Trim();
                        enter.Certificate = "身份证";
                        enter.CertificateID = Info.CertificateID.Trim();
                        enter.CreateUser = Info.CertificateID.Trim();
                        enter.CreateDate = DateTime.Now;
                        enter.UpdateUser = Info.CertificateID.Trim();
                        enter.UpdateDate = DateTime.Now;
                        enter.Status = "8";
                        int n = enterList.Add(enter);

                        if (n > 0)
                        {
                            res.ret = 8;
                            res.msg = "未登记信息，请先登记再进入";
                        }
                        else
                        {
                            res.ret = 3;
                            res.msg = "插入进入信息表出现异常";
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.ret = 2;
                    res.msg = "出现异常";
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

        //返回值子集类


        // 参数类
        public class ParaInfo
        {
            public string CommunityID;//社区ID

            public string SubdistrictID;//小区

            public string Name;//姓名

            public string CertificateID;//证件编号
        }

 

        //返回值信息
        public class ReturnInfo
        {
            public int ret { get; set; }
            public string imgUrl { get; set; }
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