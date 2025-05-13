using System;
using System.Text;
using Dapper;
using DBAccess.Model;
using DBAccess.Context;
using DBAccess.Logics;
using Model.CBean;

namespace DBAccess.Dao
{
    /// <summary>
    /// MStaffのDaoクラスを定義します。
    /// </summary>
    public partial class MStaffDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MStaffDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MStaff)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MStaff entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_STAFF (");
            sql.Append("        ID");
            sql.Append("        ,STAFF_ID");
            sql.Append("        ,FULL_NAME");
            sql.Append("        ,KANA");
            sql.Append("        ,SEX");
            sql.Append("        ,DATE_OF_HIRE");
            sql.Append("        ,SITEN_CD");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@StaffId");
            sql.Append("        ,@FullName");
            sql.Append("        ,@Kana");
            sql.Append("        ,@Sex");
            sql.Append("        ,@DateOfHire");
            sql.Append("        ,@SitenCd");
            sql.Append("        ,@RegisterUser");
            sql.Append("        ,@RegisterDatetime");
            sql.Append("        ,@RegisterProcess");
            sql.Append("        ,@UpdateUser");
            sql.Append("        ,@UpdateDatetime");
            sql.Append("        ,@UpdateProcess");
            sql.Append(");");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("StaffId", entity.StaffId);
            param.Add("FullName", entity.FullName);
            param.Add("Kana", entity.Kana);
            param.Add("Sex", entity.Sex);
            param.Add("DateOfHire", entity.DateOfHire);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("RegisterUser", con.UserId.ToString());
            param.Add("RegisterDatetime", now);
            param.Add("RegisterProcess", con.Process);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", now);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Update"
        /// <summary>
        /// Update(MStaff)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MStaff entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_STAFF");
            sql.Append($"   SET STAFF_ID = @StaffId");
            sql.Append($",       FULL_NAME = @FullName");
            sql.Append($",       KANA = @Kana");
            sql.Append($",       SEX = @Sex");
            sql.Append($",       DATE_OF_HIRE = @DateOfHire");
            sql.Append($",       SITEN_CD = @SitenCd");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("StaffId", entity.StaffId);
            param.Add("FullName", entity.FullName);
            param.Add("Kana", entity.Kana);
            param.Add("Sex", entity.Sex);
            param.Add("DateOfHire", entity.DateOfHire);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MStaff)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MStaff entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_STAFF");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("UpdateDatetime", entity.UpdateDatetime);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Find"
        /// <summary>
        /// Find
        /// </summary>
        /// <param name="id">ID</param>
        public MStaff Find(long? id)
        {
            var cb = new MStaffCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MStaff>(cb); 
        }
        #endregion

        #region "Exist"
        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>取得結果(存在:=true,存在しない:=false)</returns>
        public bool Exist(long? id)
        {
            if(Find(id) == null) 
            {
               return false; 
            }
            return true;
        }
        #endregion
    }
}
