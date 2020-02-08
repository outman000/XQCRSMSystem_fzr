using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using XQCRSMSystem.Model;

namespace XQCRSMSystem.WebApi
{
    /// <summary>
    /// SavePersonHandler 的摘要说明
    /// </summary>
    public class SavePersonHandler : IHttpHandler
    {
        //保存 出入小区人员基本信息
        public void ProcessRequest(HttpContext context)
        {

            ReturnInfo res = new ReturnInfo();
            UserInfo myInfo = new UserInfo();

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
                    WriteMsg("出入小区出入人员基本信息参数：" + jsonString);
                    myInfo = JsonConvert.DeserializeAnonymousType(jsonString, myInfo);

                    string ID = Guid.NewGuid().ToString();
                    DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();

                    //判断 必填参数 是否为空
                    if (!String.IsNullOrEmpty(myInfo.CommunityID.Trim()) && !String.IsNullOrEmpty(myInfo.SubdistrictID.Trim()) && 
                        !String.IsNullOrEmpty(myInfo.Name.Trim()) && !String.IsNullOrEmpty(myInfo.CertificateID.Trim()) && 
                        !String.IsNullOrEmpty(myInfo.Mobile.Trim()) && !String.IsNullOrEmpty(myInfo.Address.Trim()) && 
                        !String.IsNullOrEmpty(myInfo.ResidentOrVisitor.Trim()))
                    {

                        //本小区的常驻居民 一人只添加一条常驻信息，已添加常驻信息后，不可再录入
                        string sqlparas = " select  *  from   XQCR_Personnel_Info  where Name=@Name and CertificateID=@CertificateID " +
                            " and CommunityID=@CommunityID and SubdistrictID=@SubdistrictID  order by CreateDate desc ";
                        SqlParameter[] parameters = {
                             new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                             new SqlParameter("@CertificateID", SqlDbType.NVarChar,50),
                             new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                             new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50)
                        };
                        parameters[0].Value = myInfo.Name.Trim();
                        parameters[1].Value = myInfo.CertificateID.Trim();
                        parameters[2].Value = myInfo.CommunityID.Trim();
                        parameters[3].Value = myInfo.SubdistrictID.Trim();

                        DataSet dsql = DbHelperSQL.Query(sqlparas, parameters);
                        //查询是否录入信息
                        if (dsql.Tables[0].Rows.Count > 0)
                        {
                            string ResidentOrVisitor = dsql.Tables[0].Rows[0]["ResidentOrVisitor"].ToString();
                            //最新一次登记为 常驻居民，则不可再登记
                            if (ResidentOrVisitor == "0")
                            {
                                //二维码内容
                                string data = "{\"Name\": \"" + myInfo.Name.Trim() + "\",\"CertificateID\": \"" + myInfo.CertificateID.Trim() + "\"}";
                                //创建二维码返回文件路径名称
                                string fileName = Common.QRCoderHelper.CreateQRCodeToFile(data);
                                res.ret = 4;
                                res.msg = fileName;
                            }
                            //最新一次登记为 访客居民，可再登记
                            else if (ResidentOrVisitor == "1")
                            {
                                XQCR_Personnel_Info personnel_Info = new XQCR_Personnel_Info();
                                XQCR_Personnel_Info personnel = new XQCR_Personnel_Info();
                                personnel.ID = ID;
                                personnel.PersonID = myInfo.PersonID.Trim();
                                personnel.CommunityID = myInfo.CommunityID.Trim();
                                personnel.Community = myInfo.Community.Trim();
                                personnel.SubdistrictID = myInfo.SubdistrictID.Trim();
                                personnel.Subdistrict = myInfo.Subdistrict.Trim();
                                personnel.Name = myInfo.Name.Trim();
                                personnel.Certificate = myInfo.Certificate.Trim();
                                personnel.CertificateID = myInfo.CertificateID.Trim();
                                personnel.Mobile = myInfo.Mobile.Trim();
                                personnel.Address = myInfo.Address.Trim();
                                personnel.ResidentOrVisitor = myInfo.ResidentOrVisitor.Trim();
                                personnel.CreateUser = myInfo.CertificateID.Trim();
                                personnel.CreateDate = DateTime.Now;
                                personnel.UpdateUser = myInfo.CertificateID.Trim();
                                personnel.UpdateDate = DateTime.Now;
                                int m = personnel_Info.Add(personnel);

                                if (m > 0)
                                {
                                    //ResidentOrVisitor;//是否常驻或访客 必填：0，常驻；1，访客
                                    //常驻居民生成二维码；(二维码信息：姓名、身份证号)
                                    if (myInfo.ResidentOrVisitor.Trim() == "0")
                                    {
                                        //二维码内容
                                        string data = "{\"Name\": \"" + myInfo.Name.Trim() + "\",\"CertificateID\": \"" + myInfo.CertificateID.Trim() + "\"}";
                                        //创建二维码返回文件路径名称
                                        string fileName = Common.QRCoderHelper.CreateQRCodeToFile(data);
                                        res.ret = 4;
                                        res.msg = fileName;
                                    }
                                    //访客进行 黑名单验证  并记录进入时间
                                    if (myInfo.ResidentOrVisitor.Trim() == "1")
                                    {

                                        //定义 进入信息 model
                                        XQCR_EnterList enterList = new XQCR_EnterList();
                                        XQCR_EnterList enter = new XQCR_EnterList();

                                        enter.ID = Guid.NewGuid().ToString();
                                        enter.UserID = ID;
                                        enter.CommunityID = myInfo.CommunityID.Trim();
                                        enter.SubdistrictID = myInfo.SubdistrictID.Trim();
                                        enter.Name = myInfo.Name.Trim();
                                        enter.Certificate = myInfo.Certificate.Trim();
                                        enter.CertificateID = myInfo.CertificateID.Trim();
                                        enter.CreateUser = myInfo.CertificateID.Trim();
                                        enter.CreateDate = DateTime.Now;
                                        enter.UpdateUser = myInfo.CertificateID.Trim();
                                        enter.UpdateDate = DateTime.Now;

                                        string check = " select  *  from  XQCR_BlackList  where Name=@Name and CertificateID=@CertificateID ";
                                        SqlParameter[] paracheck = {
                                             new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                                             new SqlParameter("@CertificateID", SqlDbType.NVarChar,50)
                                        };
                                        paracheck[0].Value = myInfo.Name.Trim();
                                        paracheck[1].Value = myInfo.CertificateID.Trim();
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
                                    WriteMsg("填报信息成功，证件号：  " + myInfo.CertificateID);
                                }
                                else
                                {
                                    res.ret = 1;
                                    res.msg = "fail";
                                    WriteMsg("填报信息失败！");
                                }
                            }

                        }
                        //未进行信息录入
                        else
                        {
                            XQCR_Personnel_Info personnel_Info = new XQCR_Personnel_Info();
                            XQCR_Personnel_Info personnel = new XQCR_Personnel_Info();
                            personnel.ID = ID;
                            personnel.PersonID = myInfo.PersonID.Trim();
                            personnel.CommunityID = myInfo.CommunityID.Trim();
                            personnel.Community = myInfo.Community.Trim();
                            personnel.SubdistrictID = myInfo.SubdistrictID.Trim();
                            personnel.Subdistrict = myInfo.Subdistrict.Trim();
                            personnel.Name = myInfo.Name.Trim();
                            personnel.Certificate = myInfo.Certificate.Trim();
                            personnel.CertificateID = myInfo.CertificateID.Trim();
                            personnel.Mobile = myInfo.Mobile.Trim();
                            personnel.Address = myInfo.Address.Trim();
                            personnel.ResidentOrVisitor = myInfo.ResidentOrVisitor.Trim();
                            personnel.CreateUser = myInfo.CertificateID.Trim();
                            personnel.CreateDate = DateTime.Now;
                            personnel.UpdateUser = myInfo.CertificateID.Trim();
                            personnel.UpdateDate = DateTime.Now;
                            int n = personnel_Info.Add(personnel);

                            if (n > 0)
                            {
                                //ResidentOrVisitor;//是否常驻或访客 必填：0，常驻；1，访客
                                //常驻居民生成二维码；(二维码信息：姓名、身份证号)
                                if (myInfo.ResidentOrVisitor.Trim() == "0")
                                {
                                    //二维码内容
                                    string data = "{\"Name\": \"" + myInfo.Name.Trim() + "\",\"CertificateID\": \"" + myInfo.CertificateID.Trim() + "\"}";
                                    //创建二维码返回文件路径名称
                                    string fileName = Common.QRCoderHelper.CreateQRCodeToFile(data);
                                    res.ret = 4;
                                    res.msg = fileName;
                                }
                                //访客进行 黑名单验证  并记录进入时间
                                if (myInfo.ResidentOrVisitor.Trim() == "1")
                                {
                                    //定义 进入信息 model
                                    XQCR_EnterList enterList = new XQCR_EnterList();
                                    XQCR_EnterList enter = new XQCR_EnterList();

                                    enter.ID = Guid.NewGuid().ToString();
                                    enter.UserID = ID;
                                    enter.CommunityID = myInfo.CommunityID.Trim();
                                    enter.SubdistrictID = myInfo.SubdistrictID.Trim();
                                    enter.Name = myInfo.Name.Trim();
                                    enter.Certificate = myInfo.Certificate.Trim();
                                    enter.CertificateID = myInfo.CertificateID.Trim();
                                    enter.CreateUser = myInfo.CertificateID.Trim();
                                    enter.CreateDate = DateTime.Now;
                                    enter.UpdateUser = myInfo.CertificateID.Trim();
                                    enter.UpdateDate = DateTime.Now;


                                    string check = " select  *  from  XQCR_BlackList  where Name=@Name and CertificateID=@CertificateID ";
                                    SqlParameter[] paracheck = {
                                         new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                                         new SqlParameter("@CertificateID", SqlDbType.NVarChar,50)
                                    };
                                    paracheck[0].Value = myInfo.Name.Trim();
                                    paracheck[1].Value = myInfo.CertificateID.Trim();
                                    DataSet ds = DbHelperSQL.Query(check, paracheck);

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        // 保存进入信息 model
                                        enter.Status = "9";
                                        int m = enterList.Add(enter);
                                        if (m > 0)
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
                                        int m = enterList.Add(enter);
                                        if (m > 0)
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
                                WriteMsg("填报信息成功，证件号：  " + myInfo.CertificateID);
                            }
                            else
                            {
                                res.ret = 1;
                                res.msg = "fail";
                                WriteMsg("填报信息失败！");
                            }
                        }

                        ///保存 图片信息  

                        UploadFileModel files = myInfo.listImg;

                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into XQCR_UploadImg(");
                        strSql.Append("id, formid,Catalog,ImgIndex,FileName,InternalName,Path,Url,Length ,creatorID,createDate");
                        strSql.Append(") values (");
                        strSql.Append("@id, @formid,@Catalog,@ImgIndex,@FileName,@InternalName,@Path,@Url,@Length ,@creatorID,@createDate");
                        strSql.Append(") ");

                        SqlParameter[] paraSql = {
                            new SqlParameter("@id", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@formid", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@Catalog", SqlDbType.NVarChar,100) ,
                            new SqlParameter("@ImgIndex", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@FileName", SqlDbType.NVarChar,100) ,
                            new SqlParameter("@InternalName", SqlDbType.NVarChar,50),
                            new SqlParameter("@Path", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@Url", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@Length", SqlDbType.NVarChar,100) ,
                            new SqlParameter("@creatorID", SqlDbType.NVarChar,50) ,
                            new SqlParameter("@createDate", SqlDbType.DateTime)

                        };

                        paraSql[0].Value = Guid.NewGuid().ToString();
                        paraSql[1].Value = ID;
                        paraSql[2].Value = files.Catalog;
                        paraSql[3].Value = files.ImgIndex;
                        paraSql[4].Value = files.FileName;
                        paraSql[5].Value = files.InternalName;
                        paraSql[6].Value = files.Path;
                        paraSql[7].Value = files.Url;
                        paraSql[8].Value = files.Length;
                        paraSql[9].Value = myInfo.CertificateID;
                        paraSql[10].Value = DateTime.Now;

                        int j = DbHelperSQL.ExecuteSql(strSql.ToString(), paraSql);

                        WriteMsg("Url=" + files.Url);


                    }
                    else
                    {
                        res.ret = 4;
                        res.msg = "必填参数不能为空。";
                    }


                }
                catch (Exception ex)
                {
                    res.ret = 2;
                    res.msg = "出现异常";
                    WriteMsg("填报信息失败：  " + ex);
                }
            }
            string returnStr = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnStr);
        }




        //参数类
        public class UserInfo
        {
            public string PersonID;//腾讯唯一标识

            public string CommunityID;//社区ID

            public string Community;//社区

            public string SubdistrictID;//小区ID

            public string Subdistrict;//小区

            public string Name;//姓名

            public string Certificate;//证件类型

            public string CertificateID;//证件编号

            public string Mobile;//联系电话

            public string Address;//详细地址信息

            public string ResidentOrVisitor;//是否常驻或访客 必填：0，常驻；1，访客

            public UploadFileModel listImg;//头像信息

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