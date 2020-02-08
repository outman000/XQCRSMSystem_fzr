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
    public partial class PropertyInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                //加载 社区、小区信息
                LoadDDL();
            }
        }

        public void LoadDDL()
        {
            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
            string str1 = " select * from XQCR_CommunityDistrict where ParentId='31983E14-4768-4250-8D6A-8BDF3A04FE29' ";
            DataSet ds = DbHelperSQL.Query(str1);

            ddlCommunity.DataSource = ds.Tables[0].DefaultView;
            ddlCommunity.DataValueField = "Code";
            ddlCommunity.DataTextField = "Name";
            ddlCommunity.DataBind();
            this.ddlCommunity.Items.Insert(0, new ListItem("-请选择-", "99"));
            this.ddlSubdistrict.Items.Insert(0, new ListItem("-请选择-", "99"));

        }

        protected void ddlCommunity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCommunity.SelectedValue != "99")
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                ddlSubdistrict.Items.Clear();
                string str = "   select * from XQCR_CommunityDistrict where ParentId=@Code  ";

                SqlParameter[] parameters = {
                     new SqlParameter("@Code", SqlDbType.NVarChar,50)
                };
                parameters[0].Value = ddlCommunity.SelectedValue;

                DataSet ds = DbHelperSQL.Query(str, parameters);

                ddlSubdistrict.DataSource = ds.Tables[0].DefaultView;
                ddlSubdistrict.DataValueField = "Code";
                ddlSubdistrict.DataTextField = "Name";
                ddlSubdistrict.DataBind();
                this.ddlSubdistrict.Items.Insert(0, new ListItem("-请选择-", "99"));
            }

        }

        //提交保存
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //判断该 社区、小区是否已有账号
            string sql = " select  *  from  XQCR_Property_Info where 1=1 ";
            List<SqlParameter> paras = new List<SqlParameter>();
            //社区
            if (ddlCommunity.SelectedValue != "99")
            {
                sql += " and  CommunityID=@CommunityID ";
                SqlParameter paran = new SqlParameter("@CommunityID", ddlCommunity.SelectedValue.Trim());
                paras.Add(paran);
            }
            //小区
            if (ddlSubdistrict.SelectedValue != "99")
            {
                sql += " and SubdistrictID=@SubdistrictID ";
                SqlParameter paran = new SqlParameter("@SubdistrictID", ddlSubdistrict.SelectedValue.Trim());
                paras.Add(paran);
            }
            DataSet dt = DbHelperSQL.Query(sql, paras.ToArray());
            if (dt.Tables[0].Rows.Count > 0)
            {
                Response.Write("<script language='javascript'>alert('该小区已有账号，请勿添加！');</script>");
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into XQCR_Property_Info(");
                strSql.Append("ID,CommunityID,Community,SubdistrictID,Subdistrict,UserID,UserName,Password,Mobile,bak1,bak2,bak3,bak4,bak5");
                strSql.Append(") values (");
                strSql.Append("@ID,@CommunityID,@Community,@SubdistrictID,@Subdistrict,@UserID,@UserName,@Password,@Mobile,@bak1,@bak2,@bak3,@bak4,@bak5");
                strSql.Append(") ");

                SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Community", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Subdistrict", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@UserID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UserName", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Password", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Mobile", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@bak1", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@bak2", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@bak3", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@bak4", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@bak5", SqlDbType.NVarChar,50)

            };

                parameters[0].Value = Guid.NewGuid().ToString();
                parameters[1].Value = ddlCommunity.SelectedItem.Value;
                parameters[2].Value = ddlCommunity.SelectedItem.Text;
                parameters[3].Value = ddlSubdistrict.SelectedItem.Value;
                parameters[4].Value = ddlSubdistrict.SelectedItem.Text;
                parameters[5].Value = tbUserID.Text.Trim();
                parameters[6].Value = tbName.Text.Trim();
                parameters[7].Value = tbpwd.Text.Trim();
                parameters[8].Value = tbMobile.Text.Trim();
                parameters[9].Value = "";
                parameters[10].Value = "";
                parameters[11].Value = "";
                parameters[12].Value = "";
                parameters[13].Value = "";
                int i = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (i > 0)
                {
                    Response.Write("<script language='javascript'>alert('保存成功！');</script>");
                    Response.Redirect("PropertyList.aspx");
                }
            }

          
        }
        //返回
        protected void btnRturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PropertyList.aspx");
        }
    }
}