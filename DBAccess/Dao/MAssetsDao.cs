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
    /// MAssetsのDaoクラスを定義します。
    /// </summary>
    public partial class MAssetsDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MAssetsDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MAssets)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MAssets entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("INSERT INTO M_ASSETS (");
            sql.Append("        ID");
            sql.Append("        ,ASSETS_ID");
            sql.Append("        ,SUIT_NO");
            sql.Append("        ,ASSET_TYPE");
            sql.Append("        ,KINGAKU");
            sql.Append("        ,DUE_DATE");
            sql.Append("        ,PAYMENT_METHOD");
            sql.Append("        ,TRANSACTION_DETAILS");
            sql.Append("        ,PAYER");
            sql.Append("        ,PAYMENT_DESTINATION");
            sql.Append("        ,APPLY");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@AssetsId");
            sql.Append("        ,@SuitNo");
            sql.Append("        ,@AssetType");
            sql.Append("        ,@Kingaku");
            sql.Append("        ,@DueDate");
            sql.Append("        ,@PaymentMethod");
            sql.Append("        ,@TransactionDetails");
            sql.Append("        ,@Payer");
            sql.Append("        ,@PaymentDestination");
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
            param.Add("AssetsId", entity.AssetsId);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("AssetType", entity.AssetType);
            param.Add("Kingaku", entity.Kingaku);
            param.Add("DueDate", entity.DueDate);
            param.Add("PaymentMethod", entity.PaymentMethod);
            param.Add("TransactionDetails", entity.TransactionDetails);
            param.Add("Payer", entity.Payer);
            param.Add("PaymentDestination", entity.PaymentDestination);
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
        /// Update(MAssets)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MAssets entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_ASSETS");
            sql.Append($"   SET ASSETS_ID = @AssetsId");
            sql.Append($",       SUIT_NO = @SuitNo");
            sql.Append($",       ASSET_TYPE = @AssetType");
            sql.Append($",       KINGAKU = @Kingaku");
            sql.Append($",       DUE_DATE = @DueDate");
            sql.Append($",       PAYMENT_METHOD = @PaymentMethod");
            sql.Append($",       TRANSACTION_DETAILS = @TransactionDetails");
            sql.Append($",       PAYER = @Payer");
            sql.Append($",       PAYMENT_DESTINATION = @PaymentDestination");
            sql.Append($",       APPLY = @Apply");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("AssetsId", entity.AssetsId);
            param.Add("SuitNo", entity.SuitNo);
            param.Add("AssetType", entity.AssetType);
            param.Add("Kingaku", entity.Kingaku);
            param.Add("DueDate", entity.DueDate);
            param.Add("PaymentMethod", entity.PaymentMethod);
            param.Add("TransactionDetails", entity.TransactionDetails);
            param.Add("Payer", entity.Payer);
            param.Add("PaymentDestination", entity.PaymentDestination);
            param.Add("Apply", entity.Apply);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MAssets)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MAssets entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_ASSETS");
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
        public MAssets Find(long? id)
        {
            var cb = new MAssetsCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MAssets>(cb); 
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
