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
    /// MKubunのDaoクラスを定義します。
    /// </summary>
    public partial class MKubunDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MKubunDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MKubun)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MKubun entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_KUBUN (");
            sql.Append("        ID");
            sql.Append("        ,kubun_id");
            sql.Append("        ,code");
            sql.Append("        ,name");
            sql.Append("        ,display_oder");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@KubunId");
            sql.Append("        ,@Code");
            sql.Append("        ,@Name");
            sql.Append("        ,@DisplayOder");
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
            param.Add("KubunId", entity.KubunId);
            param.Add("Code", entity.Code);
            param.Add("Name", entity.Name);
            param.Add("DisplayOder", entity.DisplayOder);
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
        /// Update(MKubun)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MKubun entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_KUBUN");
            sql.Append($"   SET kubun_id = @KubunId");
            sql.Append($",       code = @Code");
            sql.Append($",       name = @Name");
            sql.Append($",       display_oder = @DisplayOder");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("KubunId", entity.KubunId);
            param.Add("Code", entity.Code);
            param.Add("Name", entity.Name);
            param.Add("DisplayOder", entity.DisplayOder);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MKubun)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MKubun entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_KUBUN");
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
        public MKubun Find(long? id)
        {
            var cb = new MKubunCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MKubun>(cb); 
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
