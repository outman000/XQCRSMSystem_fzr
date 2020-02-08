using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;


namespace XQCRSMSystem.ManageSystem
{
    public partial class UserBlackListAdd : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
            }
        }

 

        //提交保存
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //判断该 社区、小区是否已有账号
            string sql = " select  *  from  XQCR_BlackList where 1=1 ";
            List<SqlParameter> paras = new List<SqlParameter>();
            //姓名
            sql += " and  Name=@Name ";
            SqlParameter paran1 = new SqlParameter("@Name", tbName.Text.Trim());
            paras.Add(paran1);
            //身份证
            sql += " and CertificateID=@CertificateID ";
            SqlParameter paran2 = new SqlParameter("@CertificateID", tbIDCard.Text.Trim());
            paras.Add(paran2);
            
            DataSet dt = DbHelperSQL.Query(sql, paras.ToArray());
            if (dt.Tables[0].Rows.Count > 0)
            {
                Response.Write("<script language='javascript'>alert('黑名单已有该居民，请勿添加！');</script>");
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into XQCR_BlackList(");
                strSql.Append("ID, Name,Certificate,CertificateID,Mobile,Memo ");
                strSql.Append(") values (");
                strSql.Append("@ID,@Name,@Certificate,@CertificateID,@Mobile,@Memo");
                strSql.Append(") ");

                SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Certificate", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@CertificateID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Mobile", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@Memo", SqlDbType.NVarChar,50)  

            };

                parameters[0].Value = Guid.NewGuid().ToString();
                parameters[1].Value = tbName.Text.Trim();
                parameters[2].Value = "身份证";
                parameters[3].Value = tbIDCard.Text.Trim();
                parameters[4].Value = tbMobile.Text.Trim();
                parameters[5].Value = tbMemo.Text.Trim();
 
        
                int i = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (i > 0)
                {
                    Response.Write("<script language='javascript'>alert('保存成功！');</script>");
                    Response.Redirect("UserBlackList.aspx");
                }
            }


        }
        //返回
        protected void btnRturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserBlackList.aspx");
        }
    }
}