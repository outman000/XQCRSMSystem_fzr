using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// GetSQHandler 的摘要说明
    /// </summary>
    public class GetSQHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            ReturnInfo res = new ReturnInfo();
            try
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                string str = "   select * from XQCR_CommunityDistrict where ParentId='31983E14-4768-4250-8D6A-8BDF3A04FE29' ";
                DataSet ds = DbHelperSQL.Query(str);
                List<SQInfo> list = new List<SQInfo>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SQInfo info = new SQInfo();
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
            //string returnStr ="test";
            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
        }


        //子集信息
        public class SQInfo
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
            public List<SQInfo> Datas { get; set; } //社区数据
            public string msg { get; set; }

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