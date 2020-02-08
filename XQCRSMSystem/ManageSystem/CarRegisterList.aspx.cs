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
    public partial class CarRegisterList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
                //加载 社区、小区信息
                LoadDDL();
                //加载列表信息
                GridLoad();
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


        protected void GridLoad()
        {
            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();

            string sql = "select * from XQCR_CarRegister where 1=1 ";
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

            //登记时间 stime
            if (!String.IsNullOrEmpty(tbStime.Text.Trim()))
            {
                sql += " and convert(nvarchar (50),CreateDate,23)>= @sDate ";
                SqlParameter paran = new SqlParameter("@sDate", tbStime.Text.Trim());
                paras.Add(paran);
            }
            //登记时间 etime
            if (!String.IsNullOrEmpty(tbEtime.Text.Trim()))
            {
                sql += " and convert(nvarchar (50),CreateDate,23)<= @eDate ";
                SqlParameter paran = new SqlParameter("@eDate", tbEtime.Text.Trim());
                paras.Add(paran);
            }
            //关键词  查的是 姓名、身份证、手机号码、详细住址
            if (!String.IsNullOrEmpty(tbKeys.Text.Trim()))
            {
                sql += "  and ( Name like @keys or CarNumber like @keys or Mobile like @keys ) ";
                SqlParameter paran = new SqlParameter("@keys", "%" + tbKeys.Text.Trim() + "%");
                paras.Add(paran);
            }
            sql += " order by CreateDate desc";

            DataSet dt = DbHelperSQL.Query(sql, paras.ToArray());
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
            }
        }

        #endregion



        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridLoad();
        }

        //导出
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView1.AllowPaging = false;
            GridView1.AllowSorting = false;


            GridLoad();
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyyMMddhhmmss");
            str = str + ".xls";
            GridViewToExcel(GridView1, "application/ms-excel", str);

            GridView1.AllowPaging = true;
            GridView1.AllowSorting = true;


            GridLoad();
        }
        //跳转


        /// 导出方法
        /// <summary>
        /// 将网格数据导出到Excel
        /// </summary>
        /// <param name="ctrl">网格名称(如GridView1)</param>
        /// <param name="FileType">要导出的文件类型(Excel:application/ms-excel)</param>
        /// <param name="FileName">要保存的文件名</param>
        public static void GridViewToExcel(Control ctrl, string FileType, string FileName)
        {
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//注意编码
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentType = FileType;//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword
            ctrl.Page.EnableViewState = false;
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            ctrl.RenderControl(hw);



            //导出文本格式
            HttpContext.Current.Response.Write("<style type=\"text/css\">td{vnd.ms-excel.numberformat:@;}</style>");
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" + tw.ToString());
            HttpContext.Current.Response.End();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
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
    }
}