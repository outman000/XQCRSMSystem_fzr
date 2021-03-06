﻿using System;
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
    public partial class UserBlackList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();
        
                //加载列表信息
                GridLoad();
            }
        }


        protected void GridLoad()
        {
            DbHelperSQL.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["XQCRDB"].ToString();

            string sql = " select  *  from  XQCR_BlackList where 1=1 ";
            List<SqlParameter> paras = new List<SqlParameter>();

            //关键词  查的是 姓名 
            if (!String.IsNullOrEmpty(tbName.Text.Trim()))
            {
                sql += "  and ( Name like @Name  ) ";
                SqlParameter paran = new SqlParameter("@Name", "%" + tbName.Text.Trim() + "%");
                paras.Add(paran);
            }
            //关键词  查的是  身份证
            if (!String.IsNullOrEmpty(tbIDCard.Text.Trim()))
            {
                sql += "  and ( CertificateID like @IDCard  ) ";
                SqlParameter paran = new SqlParameter("@IDCard", "%" + tbIDCard.Text.Trim() + "%");
                paras.Add(paran);
            }
            sql += " order by  name  asc";

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


        //取消该黑名单
        protected void lCancel_Click(object sender, EventArgs e)
        {
            int rowIndex = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            //该行对应的ID
            string id = GridView1.DataKeys[rowIndex].Values["ID"].ToString();
            string update = " delete XQCR_BlackList  where id='" + id + "'";
            int i = DbHelperSQL.ExecuteSql(update);
            if (i > 0)
            {
                GridLoad();
                Response.Write("<script language='javascript'>alert('取消成功！');</script>");
            }
        }

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
            GridView1.Columns[4].Visible = false;

            GridLoad();
            DateTime dt = DateTime.Now;
            string str = dt.ToString("yyyyMMddhhmmss");
            str = str + ".xls";
            GridViewToExcel(GridView1, "application/ms-excel", str);

            GridView1.AllowPaging = true;
            GridView1.AllowSorting = true;
            GridView1.Columns[4].Visible = true;
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

 
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserBlackListAdd.aspx");
        }
    }
}