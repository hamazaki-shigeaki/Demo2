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
    /// MSuitのDaoクラスを定義します。
    /// </summary>
    public partial class MSuitDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MSuitDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MSuit)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MSuit entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_SUIT (");
            sql.Append("        ID");
            sql.Append("        ,SUIT_NO");
            sql.Append("        ,SITUATION");
            sql.Append("        ,CUSTOMER_CD");
            sql.Append("        ,APPLY");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@SuitNo");
            sql.Append("        ,@Situation");
            sql.Append("        ,@CustomerCd");
            sql.Append("        ,@Apply");
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
            param.Add("SuitNo", entity.SuitNo);
            param.Add("Situation", entity.Situation);
            param.Add("CustomerCd", entity.CustomerCd);
            param.Add("Apply", entity.Apply);
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
        /// Update(MSuit)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MSuit entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_SUIT");
            sql.Append($"   SET SUIT_NO = @SuitNo");
            sql.Append($",       SITUATION = @Situation");
            sql.Append($",       CUSTOMER_CD = @CustomerCd");
            sql.Append($",       APPLY = @Apply");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("Situation", entity.Situation);
            param.Add("CustomerCd", entity.CustomerCd);
            param.Add("Apply", entity.Apply);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MSuit)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MSuit entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_SUIT");
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
        public MSuit Find(long? id)
        {
            var cb = new MSuitCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MSuit>(cb); 
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
