using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace XQCRSMSystem.Model
{
    public class XQCR_EnterList
    {
        /// <summary>
        /// ID
        /// </summary>		
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// UserID
        /// </summary>		
        private string _userid;
        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// CommunityID
        /// </summary>		
        private string _communityid;
        public string CommunityID
        {
            get { return _communityid; }
            set { _communityid = value; }
        }
        /// <summary>
        /// SubdistrictID
        /// </summary>		
        private string _subdistrictid;
        public string SubdistrictID
        {
            get { return _subdistrictid; }
            set { _subdistrictid = value; }
        }
        /// <summary>
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Certificate
        /// </summary>		
        private string _certificate;
        public string Certificate
        {
            get { return _certificate; }
            set { _certificate = value; }
        }
        /// <summary>
        /// CertificateID
        /// </summary>		
        private string _certificateid;
        public string CertificateID
        {
            get { return _certificateid; }
            set { _certificateid = value; }
        }
        /// <summary>
        /// Status
        /// </summary>		
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// CreateUser
        /// </summary>		
        private string _createuser;
        public string CreateUser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        /// <summary>
        /// CreateDate
        /// </summary>		
        private DateTime _createdate;
        public DateTime CreateDate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        /// <summary>
        /// UpdateUser
        /// </summary>		
        private string _updateuser;
        public string UpdateUser
        {
            get { return _updateuser; }
            set { _updateuser = value; }
        }
        /// <summary>
        /// UpdateDate
        /// </summary>		
        private DateTime _updatedate;
        public DateTime UpdateDate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        /// <summary>
        /// bak1
        /// </summary>		
        private string _bak1;
        public string bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }
        /// <summary>
        /// bak2
        /// </summary>		
        private string _bak2;
        public string bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }
        /// <summary>
        /// bak3
        /// </summary>		
        private string _bak3;
        public string bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }
        /// <summary>
        /// bak4
        /// </summary>		
        private byte[] _bak4;
        public byte[] bak4
        {
            get { return _bak4; }
            set { _bak4 = value; }
        }
        /// <summary>
        /// bak5
        /// </summary>		
        private string _bak5;
        public string bak5
        {
            get { return _bak5; }
            set { _bak5 = value; }
        }







        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(XQCRSMSystem.Model.XQCR_EnterList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XQCR_EnterList(");
            strSql.Append("ID,UserID,CommunityID,SubdistrictID,Name,Certificate,CertificateID,Status,CreateUser,CreateDate,UpdateUser,UpdateDate,bak1,bak2,bak3,bak4,bak5");
            strSql.Append(") values (");
            strSql.Append("@ID,@UserID,@CommunityID,@SubdistrictID,@Name,@Certificate,@CertificateID,@Status,@CreateUser,@CreateDate,@UpdateUser,@UpdateDate,@bak1,@bak2,@bak3,@bak4,@bak5");
            strSql.Append(") ");

            SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UserID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Certificate", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CertificateID", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@Status", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateUser", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@UpdateUser", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UpdateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@bak1", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak2", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak3", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak4", SqlDbType.VarBinary,50) ,
                        new SqlParameter("@bak5", SqlDbType.NVarChar,200)

            };

            parameters[0].Value = model.ID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.CommunityID;
            parameters[3].Value = model.SubdistrictID;
            parameters[4].Value = model.Name;
            parameters[5].Value = model.Certificate;
            parameters[6].Value = model.CertificateID;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateUser;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.UpdateUser;
            parameters[11].Value = model.UpdateDate;
            parameters[12].Value = model.bak1;
            parameters[13].Value = model.bak2;
            parameters[14].Value = model.bak3;
            parameters[15].Value = model.bak4;
            parameters[16].Value = model.bak5;
            int i = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return i;

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XQCRSMSystem.Model.XQCR_EnterList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update XQCR_EnterList set ");

            strSql.Append(" ID = @ID , ");
            strSql.Append(" UserID = @UserID , ");
            strSql.Append(" CommunityID = @CommunityID , ");
            strSql.Append(" SubdistrictID = @SubdistrictID , ");
            strSql.Append(" Name = @Name , ");
            strSql.Append(" Certificate = @Certificate , ");
            strSql.Append(" CertificateID = @CertificateID , ");
            strSql.Append(" Status = @Status , ");
            strSql.Append(" CreateUser = @CreateUser , ");
            strSql.Append(" CreateDate = @CreateDate , ");
            strSql.Append(" UpdateUser = @UpdateUser , ");
            strSql.Append(" UpdateDate = @UpdateDate , ");
            strSql.Append(" bak1 = @bak1 , ");
            strSql.Append(" bak2 = @bak2 , ");
            strSql.Append(" bak3 = @bak3 , ");
            strSql.Append(" bak4 = @bak4 , ");
            strSql.Append(" bak5 = @bak5  ");
            strSql.Append(" where ID=@ID  ");

            SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UserID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CommunityID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@SubdistrictID", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Name", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Certificate", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CertificateID", SqlDbType.NVarChar,100) ,
                        new SqlParameter("@Status", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateUser", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@CreateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@UpdateUser", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@UpdateDate", SqlDbType.DateTime) ,
                        new SqlParameter("@bak1", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak2", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak3", SqlDbType.NVarChar,200) ,
                        new SqlParameter("@bak4", SqlDbType.VarBinary,50) ,
                        new SqlParameter("@bak5", SqlDbType.NVarChar,200)

            };

            parameters[0].Value = model.ID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.CommunityID;
            parameters[3].Value = model.SubdistrictID;
            parameters[4].Value = model.Name;
            parameters[5].Value = model.Certificate;
            parameters[6].Value = model.CertificateID;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateUser;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.UpdateUser;
            parameters[11].Value = model.UpdateDate;
            parameters[12].Value = model.bak1;
            parameters[13].Value = model.bak2;
            parameters[14].Value = model.bak3;
            parameters[15].Value = model.bak4;
            parameters[16].Value = model.bak5;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from XQCR_EnterList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.NVarChar,50)          };
            parameters[0].Value = ID;


            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XQCRSMSystem.Model.XQCR_EnterList GetModel(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, UserID, CommunityID, SubdistrictID, Name, Certificate, CertificateID, Status, CreateUser, CreateDate, UpdateUser, UpdateDate, bak1, bak2, bak3, bak4, bak5  ");
            strSql.Append("  from XQCR_EnterList ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.NVarChar,50)          };
            parameters[0].Value = ID;


            XQCRSMSystem.Model.XQCR_EnterList model = new XQCRSMSystem.Model.XQCR_EnterList();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                model.ID = ds.Tables[0].Rows[0]["ID"].ToString();
                model.UserID = ds.Tables[0].Rows[0]["UserID"].ToString();
                model.CommunityID = ds.Tables[0].Rows[0]["CommunityID"].ToString();
                model.SubdistrictID = ds.Tables[0].Rows[0]["SubdistrictID"].ToString();
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Certificate = ds.Tables[0].Rows[0]["Certificate"].ToString();
                model.CertificateID = ds.Tables[0].Rows[0]["CertificateID"].ToString();
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                model.CreateUser = ds.Tables[0].Rows[0]["CreateUser"].ToString();
                if (ds.Tables[0].Rows[0]["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(ds.Tables[0].Rows[0]["CreateDate"].ToString());
                }
                model.UpdateUser = ds.Tables[0].Rows[0]["UpdateUser"].ToString();
                if (ds.Tables[0].Rows[0]["UpdateDate"].ToString() != "")
                {
                    model.UpdateDate = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateDate"].ToString());
                }
                model.bak1 = ds.Tables[0].Rows[0]["bak1"].ToString();
                model.bak2 = ds.Tables[0].Rows[0]["bak2"].ToString();
                model.bak3 = ds.Tables[0].Rows[0]["bak3"].ToString();
                if (ds.Tables[0].Rows[0]["bak4"].ToString() != "")
                {
                    model.bak4 = (byte[])ds.Tables[0].Rows[0]["bak4"];
                }
                model.bak5 = ds.Tables[0].Rows[0]["bak5"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM XQCR_EnterList ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM XQCR_EnterList ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

    }
}