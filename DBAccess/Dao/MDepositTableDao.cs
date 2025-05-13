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
    /// MDepositTableのDaoクラスを定義します。
    /// </summary>
    public partial class MDepositTableDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MDepositTableDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MDepositTable)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MDepositTable entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_DEPOSIT_TABLE (");
            sql.Append("        ID");
            sql.Append("        ,DEPOSIT_ID");
            sql.Append("        ,SUIT_NO");
            sql.Append("        ,BANK_CD");
            sql.Append("        ,SITEN_CD");
            sql.Append("        ,ACCESS_DATE");
            sql.Append("        ,PAYMENT_DESTINATION");
            sql.Append("        ,ACCOUNTS_RECEIVABLE");
            sql.Append("        ,SOURCE_PAYMENT");
            sql.Append("        ,ACCOUNTS_PAYABLE");
            sql.Append("        ,ZANDAKA");
            sql.Append("        ,APPLY");
            sql.Append("        ,MEMO");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@DepositId");
            sql.Append("        ,@SuitNo");
            sql.Append("        ,@BankCd");
            sql.Append("        ,@SitenCd");
            sql.Append("        ,@AccessDate");
            sql.Append("        ,@PaymentDestination");
            sql.Append("        ,@AccountsReceivable");
            sql.Append("        ,@SourcePayment");
            sql.Append("        ,@AccountsPayable");
            sql.Append("        ,@Zandaka");
            sql.Append("        ,@Apply");
            sql.Append("        ,@Memo");
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
            param.Add("DepositId", entity.DepositId);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("BankCd", entity.BankCd);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("AccessDate", entity.AccessDate);
            param.Add("PaymentDestination", entity.PaymentDestination);
            param.Add("AccountsReceivable", entity.AccountsReceivable);
            param.Add("SourcePayment", entity.SourcePayment);
            param.Add("AccountsPayable", entity.AccountsPayable);
            param.Add("Zandaka", entity.Zandaka);
            param.Add("Apply", entity.Apply);
            param.Add("Memo", entity.Memo);
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
        /// Update(MDepositTable)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MDepositTable entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_DEPOSIT_TABLE");
            sql.Append($"   SET DEPOSIT_ID = @DepositId");
            sql.Append($",       SUIT_NO = @SuitNo");
            sql.Append($",       BANK_CD = @BankCd");
            sql.Append($",       SITEN_CD = @SitenCd");
            sql.Append($",       ACCESS_DATE = @AccessDate");
            sql.Append($",       PAYMENT_DESTINATION = @PaymentDestination");
            sql.Append($",       ACCOUNTS_RECEIVABLE = @AccountsReceivable");
            sql.Append($",       SOURCE_PAYMENT = @SourcePayment");
            sql.Append($",       ACCOUNTS_PAYABLE = @AccountsPayable");
            sql.Append($",       ZANDAKA = @Zandaka");
            sql.Append($",       APPLY = @Apply");
            sql.Append($",       MEMO = @Memo");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("DepositId", entity.DepositId);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("BankCd", entity.BankCd);
            param.Add("SitenCd", entity.SitenCd);
            param.Add("AccessDate", entity.AccessDate);
            param.Add("PaymentDestination", entity.PaymentDestination);
            param.Add("AccountsReceivable", entity.AccountsReceivable);
            param.Add("SourcePayment", entity.SourcePayment);
            param.Add("AccountsPayable", entity.AccountsPayable);
            param.Add("Zandaka", entity.Zandaka);
            param.Add("Apply", entity.Apply);
            param.Add("Memo", entity.Memo);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MDepositTable)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MDepositTable entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_DEPOSIT_TABLE");
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
        public MDepositTable Find(long? id)
        {
            var cb = new MDepositTableCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MDepositTable>(cb); 
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
