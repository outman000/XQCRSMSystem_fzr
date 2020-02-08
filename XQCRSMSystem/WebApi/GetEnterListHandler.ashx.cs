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
    /// GetEnterListHandler 的摘要说明
    /// </summary>
    public class GetEnterListHandler : IHttpHandler
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
                    List<EnterInfo> list = new List<EnterInfo>();
                    jsonString = utf8_gb2312(jsonString);
                    WriteMsg("打印进入信息查询参数：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);

                    if (!String.IsNullOrEmpty(Info.CommunityID.Trim()) && !String.IsNullOrEmpty(Info.SubdistrictID.Trim()))
                    {
                        DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();

                        string sql = "select a.*,b.Mobile,c.name as Community,d.name as Subdistrict,b.Address,b.ResidentOrVisitor " +
                            " from XQCR_EnterList a left join XQCR_Personnel_Info b " +
                            " on a.UserID = b.ID left join XQCR_CommunityDistrict c " +
                            " on a.CommunityID = c.code  left join XQCR_CommunityDistrict d " +
                            " on a.SubdistrictID = d.code where 1=1  and a.CommunityID=@CommunityID and a.SubdistrictID=@SubdistrictID ";
                        List<SqlParameter> paras = new List<SqlParameter>();
                        SqlParameter para1 = new SqlParameter("@CommunityID", Info.CommunityID.Trim());
                        SqlParameter para2 = new SqlParameter("@SubdistrictID", Info.SubdistrictID.Trim());
                        paras.Add(para1);
                        paras.Add(para2);
                        //常驻 或 访客
                        if (!String.IsNullOrEmpty(Info.ResidentOrVisitor.Trim()))
                        {
                            sql += " and b.ResidentOrVisitor=@ResidentOrVisitor ";
                            SqlParameter paran = new SqlParameter("@ResidentOrVisitor", Info.ResidentOrVisitor.Trim());
                            paras.Add(paran);
                        }
                        //反馈结果
                        if (!String.IsNullOrEmpty(Info.Status.Trim()))
                        {
                            sql += " and a.Status=@Status ";
                            SqlParameter paran = new SqlParameter("@Status", Info.Status.Trim());
                            paras.Add(paran);
                        }
                        //进入时间
                        if (!String.IsNullOrEmpty(Info.Date.Trim()))
                        {
                            sql += " and convert(nvarchar (50),a.CreateDate,23)=@Date ";
                            SqlParameter paran = new SqlParameter("@Date", Info.Date.Trim());
                            paras.Add(paran);
                        }
                        //关键词  查的是 姓名、身份证、手机号码、详细住址
                        if (!String.IsNullOrEmpty(Info.keys.Trim()))
                        {
                            sql += "  and ( a.Name like @keys or a.CertificateID like @keys or b.Mobile like @keys or b.Address like @keys ) ";
                            SqlParameter paran = new SqlParameter("@keys", "%"+Info.keys.Trim()+"%");
                            paras.Add(paran);
                        }
                        sql += " order by a.CreateDate desc";

                        DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            EnterInfo info = new EnterInfo();
                            info.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            info.UserID = ds.Tables[0].Rows[i]["UserID"].ToString();
                            info.CommunityID = ds.Tables[0].Rows[i]["CommunityID"].ToString();
                            info.Community = ds.Tables[0].Rows[i]["Community"].ToString();
                            info.SubdistrictID = ds.Tables[0].Rows[i]["SubdistrictID"].ToString();
                            info.Subdistrict = ds.Tables[0].Rows[i]["Subdistrict"].ToString();
                            info.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                            info.Certificate = ds.Tables[0].Rows[i]["Certificate"].ToString();
                            info.CertificateID = ds.Tables[0].Rows[i]["CertificateID"].ToString();
                            info.Mobile = ds.Tables[0].Rows[i]["Mobile"].ToString();
                            info.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                            info.ResidentOrVisitor = ds.Tables[0].Rows[i]["ResidentOrVisitor"].ToString();
                            info.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                            info.Date = ds.Tables[0].Rows[i]["CreateDate"].ToString();

                            list.Add(info);
                        }
                        res.ret = 0;
                        res.list = list;
                        res.msg = "信息列表";

                    }
                    else
                    {
                        res.ret = 1;
                        res.list = null;
                        res.msg = "参数为空";
                    }

                }
                catch (Exception ex)
                {
                    res.ret = 2;
                    res.list = null;
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

            public string ResidentOrVisitor;//访客或常驻

            public string Date;//进入时间

            public string Status;//状态

            public string keys;//查的是 姓名、身份证、手机号码、详细住址
        }


        // 返回值子集类
        //人员基础信息类
        public class EnterInfo
        {
            public string ID;//ID

            public string UserID;//ID

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

            public string Status;//进入状态

            public string Date;//进入时间

        }


        //返回值信息
        public class ReturnInfo
        {

            public int ret { get; set; }

            public List<EnterInfo> list { get; set; }//进入信息列表 
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