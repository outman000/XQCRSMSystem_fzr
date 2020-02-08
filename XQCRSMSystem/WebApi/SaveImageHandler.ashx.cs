using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using XQCRSMSystem.Model;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;


namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// SaveImageHandler 的摘要说明
    /// </summary>
    public class SaveImageHandler : IHttpHandler
    {
        /// <summary>  
        /// 获取配置文件Key对应Value值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        public string UploadFileNew(HttpContext context)
        {
            string logFile = System.Web.HttpContext.Current.Server.MapPath("") + "\\log";
            string returnStr = "";
            try
            {
                UploadFileModel model = new UploadFileModel();
                HttpPostedFile file = context.Request.Files["uploadfile_Img"];
                WriteMsg("FileName=" + file.FileName);
                if (file != null)
                {
                    //公司编号+上传日期文件主目录
                    model.Catalog = DateTime.Now.ToString("yyyyMMddHHmmss");
                    model.ImgIndex = 1;
                    //文件名
                    model.FileName = file.FileName;
                    model.Length = file.ContentLength.ToString();
                    //获取文件后缀
                    string extensionName = System.IO.Path.GetExtension(file.FileName);

                    //存储时生成新文件名
                    model.InternalName = System.Guid.NewGuid().ToString("N") + extensionName;
                   
                    //保存文件路径
                    string filePathName = System.IO.Path.Combine(GetConfigValue("ImageAbsoluteFolderTemp"), "");
                    WriteMsg("filePathName=" + filePathName);
                    if (!System.IO.Directory.Exists(filePathName))
                    {
                        System.IO.Directory.CreateDirectory(filePathName);
                    }
                    //相对路径
                    string relativeUrl = GetConfigValue("ImageRelativeFolderTemp");
                    WriteMsg("relativeUrl=" + relativeUrl);
                    file.SaveAs(System.IO.Path.Combine(filePathName, model.InternalName));

                    model.Path = filePathName;
                    //获取临时文件相对完整路径
                    model.Url = System.IO.Path.Combine(relativeUrl, model.InternalName).Replace("\\", "/");

                }
                returnStr = JsonConvert.SerializeObject(model);
                return returnStr;
            }
            catch (Exception ex)
            {

                returnStr = "ex.Message=" + ex.Message + "; ex.StackTrace=" + ex.StackTrace + ex.ToString();
                WriteMsg(returnStr);
                return returnStr;
            }

    

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



        public void ProcessRequest(HttpContext context)
        {
            string returnJosn = UploadFileNew(context);
            WriteMsg("returnJosn=" + returnJosn);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnJosn);
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