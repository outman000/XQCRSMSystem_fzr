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
    public partial class UserEnterInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string id = Request.QueryString["ID"];
                hidID.Value = id;
                DataLosd();
                GridLoad();
            }
        }

        public void DataLosd()
        {
            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
            string str = "   select * from XQCR_Personnel_Info where id=@ID  ";
            SqlParameter[] parameters = {
                new SqlParameter("@ID", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = hidID.Value.Trim();
            DataSet ds = DbHelperSQL.Query(str, parameters);
            if (ds.Tables[0].Rows.Count>0)
            {
                lCommunity.Text = ds.Tables[0].Rows[0]["Community"].ToString();
                lSubdistrict.Text = ds.Tables[0].Rows[0]["Subdistrict"].ToString();
                lName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                lIDcard.Text = ds.Tables[0].Rows[0]["CertificateID"].ToString();
                lMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                lAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                lType.Text = ds.Tables[0].Rows[0]["ResidentOrVisitor"].ToString();
                lCreateDate.Text = ds.Tables[0].Rows[0]["CreateDate"].ToString();

                switch (lType.Text)
                {
                    case "0": lType.Text = "常住"; break; //
                    case "1": lType.Text = "访客"; break; //
                    default: lType.Text = "未登记"; break;
                }
            }
        }

        protected void GridLoad()
        {
            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();

            string sql = "select a.*,b.Mobile,c.name as Community,d.name as Subdistrict,b.Address,b.ResidentOrVisitor " +
                    " from XQCR_EnterList a left join XQCR_Personnel_Info b " +
                    " on a.UserID = b.ID left join XQCR_CommunityDistrict c " +
                    " on a.CommunityID = c.code  left join XQCR_CommunityDistrict d " +
                    " on a.SubdistrictID = d.code where UserID=@UserID  order by a.CreateDate desc";
            SqlParameter[] parameters = {
                new SqlParameter("@UserID", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = hidID.Value.Trim();
            DataSet dt = DbHelperSQL.Query(sql, parameters);
 
            this.GridView1.DataSource = dt.Tables[0];
            this.GridView1.DataKeyNames = new string[] { "ID" };
            this.GridView1.DataBind();
            this.ddlCurrentPage.Items.Clear();
            for (int i = 1; i <= this.GridView1.PageCount; i++)
            {
                this.ddlCurrentPage.Items.Add(i.ToString());
            }
            if (GridView1.Rows.Count > 0)
            {
                this.ddlCurrentPage.SelectedIndex = this.GridView1.PageIndex;
            }
        }

        #region Gridview基础函数
        //分页 事件
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = this.ddlCurrentPage.SelectedIndex;
            GridLoad();
        }
        protected void lnkbtnFrist_Click(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = 0;
            GridLoad();
        }
        protected void lnkbtnPre_Click(object sender, EventArgs e)
        {
            if (this.GridView1.PageIndex > 0)
            {
                this.GridView1.PageIndex = this.GridView1.PageIndex - 1;
                GridLoad();
            }
        }
        protected void lnkbtnNext_Click(object sender, EventArgs e)
        {
            if (this.GridView1.PageIndex < this.GridView1.PageCount)
            {
                this.GridView1.PageIndex = this.GridView1.PageIndex + 1;
                GridLoad();
            }
        }
        protected void lnkbtnLast_Click(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = this.GridView1.PageCount;
            GridLoad();
        }
        public void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //显示
                Label ltype = (Label)e.Row.FindControl("ltype");
                Label lstatus = (Label)e.Row.FindControl("lstatus");

                switch (ltype.Text)
                {
                    case "0": ltype.Text = "常住"; break; //
                    case "1": ltype.Text = "访客"; break; //
                    default: ltype.Text = "未登记"; break;
                }
                switch (lstatus.Text)
                {
                    case "0": lstatus.Text = "绿灯"; break; //
                    case "9": lstatus.Text = "红灯"; break; //
                    case "8": lstatus.Text = "黄灯"; break; //
                    default: lstatus.Text = "黄灯"; break;
                }
            }
        }
        #endregion Gridview基础函数

        
        protected void btnRturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserInfoList.aspx");
        }
    }
}