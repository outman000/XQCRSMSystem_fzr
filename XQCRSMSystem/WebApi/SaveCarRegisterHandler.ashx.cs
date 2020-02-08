using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using XQCRSMSystem.Model;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// SaveCarRegisterHandler 的摘要说明
    /// </summary>
    public class SaveCarRegisterHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnInfo res = new ReturnInfo();
            CarRegisterInfo myInfo = new CarRegisterInfo();

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
                    WriteMsg("出入小区车牌号登记基本信息参数：" + jsonString);
                    myInfo = JsonConvert.DeserializeAnonymousType(jsonString, myInfo);

                    if (!String.IsNullOrEmpty(myInfo.CommunityID.Trim()) && !String.IsNullOrEmpty(myInfo.SubdistrictID.Trim()) && !String.IsNullOrEmpty(myInfo.CarNumber.Trim()))
                    {
                        XQCR_CarRegister register = new XQCR_CarRegister();
                        XQCR_CarRegister car = new XQCR_CarRegister();
                        car.ID = Guid.NewGuid().ToString();
                        car.CommunityID = myInfo.CommunityID.Trim();
                        car.Community = myInfo.Community.Trim();
                        car.SubdistrictID = myInfo.SubdistrictID.Trim();
                        car.Subdistrict = myInfo.Subdistrict.Trim();
                        car.CarNumber = myInfo.CarNumber.Trim();
                        car.Name = myInfo.Name.Trim();
                        car.Mobile = myInfo.Mobile.Trim();
                        car.CreateUser = myInfo.PropertyUser.Trim();
                        car.CreateDate = DateTime.Now;
                        car.UpdateUser = myInfo.PropertyUser.Trim();
                        car.UpdateDate = DateTime.Now;


                        //插入 小区出入车牌号登记信息
                        int i = register.Add(car);
                        if (i > 0)
                        {
                            res.ret = 0;
                            res.msg = "success";
                            WriteMsg("填报信息成功，证车牌号：  " + myInfo.CarNumber);
                        }
                        else
                        {
                            res.ret = 1;
                            res.msg = "fail";
                            WriteMsg("填报信息失败！");
                        }
                    }
                    else
                    {
                        res.ret = 2;
                        res.msg = "参数不能为空";
                        WriteMsg("填报信息成功，证车牌号：  " + myInfo.CarNumber);
                    }
                    
                }
                catch (Exception ex)
                {
                    res.ret = 3;
                    res.msg = "Exception";
                    WriteMsg("填报信息失败：  " + ex);
                }
            }
            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
        }


        //参数类
        public class CarRegisterInfo
        {
            public string CommunityID;//社区ID

            public string Community;//社区

            public string SubdistrictID;//小区ID

            public string Subdistrict;//小区

            public string Name;//姓名

            public string Mobile;//联系电话

            public string CarNumber;//车牌号

            public string PropertyUser;//物业账号
            
        }



        //返回值信息
        public class ReturnInfo
        {

            public int ret { get; set; }
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