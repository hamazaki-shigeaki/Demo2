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
    /// TOcrMotoのDaoクラスを定義します。
    /// </summary>
    public partial class TOcrMotoDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public TOcrMotoDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(TOcrMoto)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(TOcrMoto entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO T_OCR_MOTO (");
            sql.Append("        ID");
            sql.Append("        ,SUIT_NO");
            sql.Append("        ,IMPORT_DATE");
            sql.Append("        ,SEQNO");
            sql.Append("        ,YYYY");
            sql.Append("        ,MM");
            sql.Append("        ,DD");
            sql.Append("        ,APPLY");
            sql.Append("        ,APPLY2");
            sql.Append("        ,SIHARAI_KIN");
            sql.Append("        ,AZUKARI_KIN");
            sql.Append("        ,ZANDAKA");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@SuitNo");
            sql.Append("        ,@ImportDate");
            sql.Append("        ,@Seqno");
            sql.Append("        ,@Yyyy");
            sql.Append("        ,@Mm");
            sql.Append("        ,@Dd");
            sql.Append("        ,@Apply");
            sql.Append("        ,@Apply2");
            sql.Append("        ,@SiharaiKin");
            sql.Append("        ,@AzukariKin");
            sql.Append("        ,@Zandaka");
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
            param.Add("ImportDate", entity.ImportDate);
            param.Add("Seqno", entity.Seqno);
            param.Add("Yyyy", entity.Yyyy);
            param.Add("Mm", entity.Mm);
            param.Add("Dd", entity.Dd);
            param.Add("Apply", entity.Apply);
            param.Add("Apply2", entity.Apply2);
            param.Add("SiharaiKin", entity.SiharaiKin);
            param.Add("AzukariKin", entity.AzukariKin);
            param.Add("Zandaka", entity.Zandaka);
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
        /// Update(TOcrMoto)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(TOcrMoto entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE T_OCR_MOTO");
            sql.Append($"   SET SUIT_NO = @SuitNo");
            sql.Append($",       IMPORT_DATE = @ImportDate");
            sql.Append($",       SEQNO = @Seqno");
            sql.Append($",       YYYY = @Yyyy");
            sql.Append($",       MM = @Mm");
            sql.Append($",       DD = @Dd");
            sql.Append($",       APPLY = @Apply");
            sql.Append($",       APPLY2 = @Apply2");
            sql.Append($",       SIHARAI_KIN = @SiharaiKin");
            sql.Append($",       AZUKARI_KIN = @AzukariKin");
            sql.Append($",       ZANDAKA = @Zandaka");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("ImportDate", entity.ImportDate);
            param.Add("Seqno", entity.Seqno);
            param.Add("Yyyy", entity.Yyyy);
            param.Add("Mm", entity.Mm);
            param.Add("Dd", entity.Dd);
            param.Add("Apply", entity.Apply);
            param.Add("Apply2", entity.Apply2);
            param.Add("SiharaiKin", entity.SiharaiKin);
            param.Add("AzukariKin", entity.AzukariKin);
            param.Add("Zandaka", entity.Zandaka);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(TOcrMoto)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(TOcrMoto entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM T_OCR_MOTO");
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
        public TOcrMoto Find(long? id)
        {
            var cb = new TOcrMotoCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<TOcrMoto>(cb); 
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
