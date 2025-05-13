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
    /// MBankのDaoクラスを定義します。
    /// </summary>
    public partial class MBankDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MBankDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MBank)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MBank entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_BANK (");
            sql.Append("        ID");
            sql.Append("        ,BANK_CD");
            sql.Append("        ,SITEN_CD");
            sql.Append("        ,BANK_NAME");
            sql.Append("        ,BANK_NAME_KANA");
            sql.Append("        ,BRANCH_NAME");
            sql.Append("        ,BRANCH_NAME_KANA");
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
            sql.Append("        ,@BankName");
            sql.Append("        ,@BankNameKana");
            sql.Append("        ,@BranchName");
            sql.Append("        ,@BranchNameKana");
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
            param.Add("BankName", entity.BankName);
            param.Add("BankNameKana", entity.BankNameKana);
            param.Add("BranchName", entity.BranchName);
            param.Add("BranchNameKana", entity.BranchNameKana);
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
        /// Update(MBank)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MBank entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_BANK");
            sql.Append($"   SET BANK_CD = @BankCd");
            sql.Append($",       SITEN_CD = @SitenCd");
            sql.Append($",       BANK_NAME = @BankName");
            sql.Append($",       BANK_NAME_KANA = @BankNameKana");
            sql.Append($",       BRANCH_NAME = @BranchName");
            sql.Append($",       BRANCH_NAME_KANA = @BranchNameKana");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("BankCd", entity.BankCd);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("BankName", entity.BankName);
            param.Add("BankNameKana", entity.BankNameKana);
            param.Add("BranchName", entity.BranchName);
            param.Add("BranchNameKana", entity.BranchNameKana);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MBank)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MBank entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_BANK");
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
        public MBank Find(long? id)
        {
            var cb = new MBankCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MBank>(cb); 
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
