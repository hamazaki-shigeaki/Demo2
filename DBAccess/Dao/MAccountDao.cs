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
    /// MAccountのDaoクラスを定義します。
    /// </summary>
    public partial class MAccountDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MAccountDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MAccount)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MAccount entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_ACCOUNT (");
            sql.Append("        ID");
            sql.Append("        ,BANK_CD");
            sql.Append("        ,SITEN_CD");
            sql.Append("        ,KAMOKU");
            sql.Append("        ,KOUZA_NO");
            sql.Append("        ,ZANDAKA");
            sql.Append("        ,CIF_NO");
            sql.Append("        ,KANA");
            sql.Append("        ,COTION_FLG");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@BankCd");
            sql.Append("        ,@SitenCd");
            sql.Append("        ,@Kamoku");
            sql.Append("        ,@KouzaNo");
            sql.Append("        ,@Zandaka");
            sql.Append("        ,@CifNo");
            sql.Append("        ,@Kana");
            sql.Append("        ,@CotionFlg");
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
            param.Add("BankCd", entity.BankCd);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("Kamoku", entity.Kamoku);
            param.Add("KouzaNo", entity.KouzaNo);
            param.Add("Zandaka", entity.Zandaka);
            param.Add("CifNo", entity.CifNo);
            param.Add("Kana", entity.Kana);
            param.Add("CotionFlg", entity.CotionFlg);
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
        /// Update(MAccount)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MAccount entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_ACCOUNT");
            sql.Append($"   SET BANK_CD = @BankCd");
            sql.Append($",       SITEN_CD = @SitenCd");
            sql.Append($",       KAMOKU = @Kamoku");
            sql.Append($",       KOUZA_NO = @KouzaNo");
            sql.Append($",       ZANDAKA = @Zandaka");
            sql.Append($",       CIF_NO = @CifNo");
            sql.Append($",       KANA = @Kana");
            sql.Append($",       COTION_FLG = @CotionFlg");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("BankCd", entity.BankCd);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("Kamoku", entity.Kamoku);
            param.Add("KouzaNo", entity.KouzaNo);
            param.Add("Zandaka", entity.Zandaka);
            param.Add("CifNo", entity.CifNo);
            param.Add("Kana", entity.Kana);
            param.Add("CotionFlg", entity.CotionFlg);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MAccount)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MAccount entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_ACCOUNT");
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
        public MAccount Find(long? id)
        {
            var cb = new MAccountCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MAccount>(cb); 
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
