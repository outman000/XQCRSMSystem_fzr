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
    /// GetUserInfoListHandler 的摘要说明
    /// </summary>
    public class GetUserInfoListHandler : IHttpHandler
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
                    List<UserInfo> list = new List<UserInfo>();
                    jsonString = utf8_gb2312(jsonString);
                    WriteMsg("打印物业人员查询参数：" + jsonString);
                    Info = JsonConvert.DeserializeAnonymousType(jsonString, Info);
                    if (!String.IsNullOrEmpty(Info.CommunityID.Trim()) && !String.IsNullOrEmpty(Info.SubdistrictID.Trim()))
                    {
                        DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                        string str = " select  *  from  XQCR_Personnel_Info  where CommunityID=@CommunityID and SubdistrictID=@SubdistrictID ";
                        List<SqlParameter> paras = new List<SqlParameter>();
                        SqlParameter para1 = new SqlParameter("@CommunityID", Info.CommunityID.Trim());
                        SqlParameter para2 = new SqlParameter("@SubdistrictID", Info.SubdistrictID.Trim());
                        paras.Add(para1);
                        paras.Add(para2);
                        //常驻 或 访客
                        if (!String.IsNullOrEmpty(Info.ResidentOrVisitor.Trim()))
                        {
                            str += " and ResidentOrVisitor=@ResidentOrVisitor ";
                            SqlParameter paran = new SqlParameter("@ResidentOrVisitor", Info.ResidentOrVisitor.Trim());
                            paras.Add(paran);
                        }
                        //登记时间
                        if (!String.IsNullOrEmpty(Info.Date.Trim()))
                        {
                            str += " and convert(nvarchar (50),CreateDate,23)=@Date ";
                            SqlParameter paran = new SqlParameter("@Date", Info.Date.Trim());
                            paras.Add(paran);
                        }
                        //关键词  查的是 姓名、身份证、手机号码、详细住址
                        if (!String.IsNullOrEmpty(Info.keys.Trim()))
                        {
                            str += "  and ( Name like @keys or CertificateID like @keys or Mobile like @keys or Address like @keys ) ";
                            SqlParameter paran = new SqlParameter("@keys", "%" + Info.keys.Trim() + "%");
                            paras.Add(paran);
                        }
                        str += " order by CreateDate desc";
                        DataSet ds = DbHelperSQL.Query(str, paras.ToArray());

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UserInfo info = new UserInfo();
                            info.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            info.PersonID = ds.Tables[0].Rows[i]["PersonID"].ToString();
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
                            info.Memo = ds.Tables[0].Rows[i]["Memo"].ToString();
                            info.CreateDate = ds.Tables[0].Rows[i]["CreateDate"].ToString();
                            
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

            public string Date;//登记时间

            public string keys;//查的是 姓名、身份证、手机号码、详细住址
        }


        // 返回值子集类
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


        //返回值信息
        public class ReturnInfo
        {

            public int ret { get; set; }

            public List<UserInfo> list { get; set; }//人员信息列表 
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