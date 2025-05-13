using System;
using System.Text;
using System.Collections.Generic;
 
using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Common;
using DBAccess.Context;
using DBAccess.Logics;
using DBAccess.Dao;
using Utilities;

namespace DBAccess.BSAccessores
{
    /// <summary>
    /// MDepositTableのDBAccessorクラスを定義します。
    /// </summary>
    public partial class BSMDepositTableAccessor : CommonLogic
    {
        /// <summary>MDepositTableのDao</summary>
        protected MDepositTableDao Dao = null;
 
        /// <summary>DbContext</summary>
        protected ApplicationDbContext Context;
 
        /// <summary>DbContext</summary>
        public bool WhereFlg { get; set; } = false;

        /// <summary>DbContext</summary>
        public string TableDbName = "M_DEPOSIT_TABLE"; 

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        
        public BSMDepositTableAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new MDepositTableDao(context);
            Context = context;
        }
        #endregion

        /// <summary>
        /// データ検索
        /// </summary>
        /// <returns>取得結果</returns>
        public IList<MDepositTable> Get()
        {
            return Query<MDepositTable>("SELECT * FROM M_DEPOSIT_TABLE ");
        }
 
        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response MDepositTableCUD(IList<MDepositTable> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            foreach (var data in datalist)
            {
                var res = MDepositTableCUD(data);
                if(!res.Status)
                {
                     return _Response;
                };
                _Response.Cnt += res.Cnt;
            }
            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="data">更新用データ</param>
        /// <returns>件数</returns>
        public virtual Response MDepositTableCUD(MDepositTable data)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            if (string.IsNullOrEmpty(data.ChangeSw))
            {
               _Response.Message = MessageClass.EM0997;
               return _Response;
            }
            if (data.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
            {
                data.Id = GetSeq(TableDbName + "_id_seq");
                DefaultSet(data);
                _Response.Cnt += Dao.Insert(data);
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
            {
                DefaultSet(data);
                _Response.Cnt += Dao.Update(data);
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.DELETE.Code)
            {
                _Response.Cnt += Dao.Delete(data);
            }
            if(_Response.Cnt != 1)
            {
                 _Response.Message = MessageClass.EM0999;
                 return _Response;
            }
            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response MDepositTableBulkCUD(IList<MDepositTable> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            // 更新時刻
            DateTime now = DateTime.Now;
            // インサート用データ
            IList<MDepositTable> inserList = new List<MDepositTable>();
            // アップデート用データ
            IList<MDepositTable> updateList = new List<MDepositTable>();
            // デリート用データ
            IList<MDepositTable> deleteList = new List<MDepositTable>();

            // データ振分
            foreach (var data in datalist)
            {
                if (data.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
                {
                    data.Id = GetSeq(TableDbName + "_id_seq");
                    data.RegisterUser = Context.UserId.ToString();
                    data.RegisterDatetime = now;
                    data.RegisterProcess = Context.Process;
                    data.UpdateUser = Context.UserId.ToString();
                    data.UpdateDatetime = now;
                    data.UpdateProcess = Context.Process;
                    inserList.Add(data);
                }
                else if (data.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
                {
                    data.UpdateUser = Context.UserId.ToString();
                    data.UpdateProcess = Context.Process;
                    updateList.Add(data);
                }
                else if (data.ChangeSw == CDef.CHANGE_SW.DELETE.Code)
                {
                    deleteList.Add(data);
                }
            }
            // 追加処理
            var insCnt = InsetdataList(inserList);
            _Response.Cnt += insCnt;

            // 更新処理
            var updCnt = UpdateList(updateList);
            _Response.Cnt += updCnt;

            // 削除処理
            foreach (var data in deleteList)
            {
                var res = MDepositTableCUD(data);
                if(!res.Status)
                {
                     return _Response;
                };
                _Response.Cnt += res.Cnt;
            }

            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// 更新対象のデータを読み込み
        /// </summary>
        /// <param name="data">更新データ</param>
        public MDepositTable Find(MDepositTable data)
        {
            var rec = Dao.Find(data.Id);
            if (rec == null)
            {
                 OriginalException.ExclusiveException("MDepositTable Key=" + ((long)data.Id).ToString());
            }
            if (CompareFatetime(rec.UpdateDatetime, data.UpdateDatetime))
            {
                 OriginalException.ExclusiveException("MDepositTable Key=" + ((long)data.Id).ToString());
            }
            return rec;
        }

        /// <summary>
        /// 存在チェックを行う
        /// </summary>
        /// <param name="data">更新データ</param>
        public bool Exist(long? id)
        {
            return Dao.Exist(id);
        }

        /// <summary>
        /// １０００件単位でインサート
        /// </summary>
        /// <param name="inserList">インサート用データリスト</param>
        /// <returns>更新件数</returns>
        public int InsetdataList(IList<MDepositTable> inserList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MDepositTable> dataList = new List<MDepositTable>();
            for (int idx = 0; idx < inserList.Count; idx++)
            {
                dataList.Add(inserList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetInsertSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MDepositTable>();
                }
            }
            if (dataList.Count != CommonContext.KousinCnt && dataList.Count > 0)
            {
                sql = GetInsertSql(dataList);
                cnt += Execute(sql);
            }
            if (inserList.Count != cnt)
            {
                OriginalException.ExclusiveException(sql);
            }
            return cnt;
        }

        /// <summary>
        /// １０００件単位でUPDATE
        /// </summary>
        /// <param name="inserList">UPDATE用データリスト</param>
        /// <returns>更新件数</returns>
        public int UpdateList(IList<MDepositTable> UpdateList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MDepositTable> dataList = new List<MDepositTable>();
            for (int idx = 0; idx < UpdateList.Count; idx++)
            {
                dataList.Add(UpdateList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetUpdateSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MDepositTable>();
                }
            }
            if (dataList.Count != CommonContext.KousinCnt && dataList.Count > 0)
            {
                sql = GetUpdateSql(dataList);
                cnt += Execute(sql);
            }
            if (UpdateList.Count != cnt)
            {
                OriginalException.ExclusiveException(sql);
            }
            return cnt;
        }
        /// <summary>
        /// </summary>
        /// <param name="inserList">新規追加用データリスト</param>
        /// <returns>SQL文</returns>
        public string GetInsertSql(IList<MDepositTable> inserList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

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
            sql.Append(")");
            strSql = sql.ToString();

            var strValueSql = string.Empty;
            foreach (var entity in inserList)
            {
                sql = new StringBuilder();
                if (strValueSql != string.Empty)
                {
                    sql.Append(", (");
                }
                else
                {
                    sql.Append(" VALUES(");
                }
                sql.Append(EditInsertSql(entity));
                sql.Append(")");
                strValueSql += sql.ToString();
            }
            return strSql + strValueSql;
        }

        /// <summary>
        /// Value句の作成
        /// </summary>
        /// <param name="entity">エンティティ</param>
        /// <returns>SQL</returns>
        public string EditInsertSql(MDepositTable entity)
        {
            var sql = new StringBuilder();
            sql.Append(" " + GenericUtil.LongToString(entity.Id) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.DepositId) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SuitNo) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.BankCd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SitenCd) + ", ");
            sql.Append(" " + GenericUtil.DateToString(entity.AccessDate) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PaymentDestination) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.AccountsReceivable) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SourcePayment) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.AccountsPayable) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.Zandaka) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Apply) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Memo) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.RegisterUser) + ", ");
            sql.Append(" " + GenericUtil.DateTimeToString(entity.RegisterDatetime) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.RegisterProcess) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.UpdateUser) + ", ");
            sql.Append(" " + GenericUtil.DateTimeToString(entity.UpdateDatetime) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.UpdateProcess) + " ");
            return sql.ToString();
        }

        /// <summary>
        /// 更新用SQL作成
        /// </summary>
        /// <param name="updateList">更新用データリスト</param>
        /// <returns>SQL文</returns>
        public string GetUpdateSql(IList<MDepositTable> updateList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

            var sql = new StringBuilder();
            var strValueSql = string.Empty;
            foreach (var entity in updateList)
            {
                sql = new StringBuilder();
                if (strValueSql != string.Empty)
                {
                    sql.Append(";");
                }
                sql.Append(EditUpdateSql(entity));
                strValueSql += sql.ToString();
            }
            return strSql + strValueSql;
        }

        /// <summary>
        /// Update句の作成
        /// </summary>
        /// <param name="entity">エンティティ</param>
        /// <returns>SQL</returns>
        public string EditUpdateSql(MDepositTable entity)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE M_DEPOSIT_TABLE");
            sql.Append(" SET");
            sql.Append("   DEPOSIT_ID  = " + GenericUtil.LongToString(entity.DepositId));
            sql.Append(",   SUIT_NO  = " + GenericUtil.StringToString(entity.SuitNo));
            sql.Append(",   BANK_CD  = " + GenericUtil.StringToString(entity.BankCd));
            sql.Append(",   SITEN_CD  = " + GenericUtil.StringToString(entity.SitenCd));
            sql.Append(",   ACCESS_DATE  = " + GenericUtil.DateToString(entity.AccessDate));
            sql.Append(",   PAYMENT_DESTINATION  = " + GenericUtil.StringToString(entity.PaymentDestination));
            sql.Append(",   ACCOUNTS_RECEIVABLE  = " + GenericUtil.LongToString(entity.AccountsReceivable));
            sql.Append(",   SOURCE_PAYMENT  = " + GenericUtil.StringToString(entity.SourcePayment));
            sql.Append(",   ACCOUNTS_PAYABLE  = " + GenericUtil.LongToString(entity.AccountsPayable));
            sql.Append(",   ZANDAKA  = " + GenericUtil.LongToString(entity.Zandaka));
            sql.Append(",   APPLY  = " + GenericUtil.StringToString(entity.Apply));
            sql.Append(",   MEMO  = " + GenericUtil.StringToString(entity.Memo));
            sql.Append(",   REGISTER_USER  = " + GenericUtil.StringToString(entity.RegisterUser));
            sql.Append(",   REGISTER_DATETIME  = " + GenericUtil.DateToString(entity.RegisterDatetime));
            sql.Append(",   REGISTER_PROCESS  = " + GenericUtil.StringToString(entity.RegisterProcess));
            sql.Append(",   UPDATE_USER  = " + GenericUtil.StringToString(entity.UpdateUser));
            sql.Append(",   UPDATE_DATETIME  = " + GenericUtil.DateToString(entity.UpdateDatetime));
            sql.Append(",   UPDATE_PROCESS  = " + GenericUtil.StringToString(entity.UpdateProcess));
            sql.Append(" WHERE id = " + GenericUtil.LongToString(entity.Id));
            if (WhereFlg)
            {
                  sql.Append(" AND   str(update_datetime, 'yyyyMMddHH24MISS') = '" + ((DateTime)entity.UpdateDatetime).ToString("yyyyMMddHHmmss") + "'");
            }
            return sql.ToString();
        }

        /// <summary>
        /// CSV作成
        /// </summary>
        /// <param name="dataList">データリスト</param>
        /// <param name="header">ヘッダー有無</param>
        /// <returns>SQL</returns>
        public string CreateCSV(IList<MDepositTable> dataList, bool header = true)
        {
            var csv = new StringBuilder();
            if (header)
            {
                csv.Append("ID" + "\t");
                csv.Append("DEPOSIT_ID" + "\t");
                csv.Append("SUIT_NO" + "\t");
                csv.Append("BANK_CD" + "\t");
                csv.Append("SITEN_CD" + "\t");
                csv.Append("ACCESS_DATE" + "\t");
                csv.Append("PAYMENT_DESTINATION" + "\t");
                csv.Append("ACCOUNTS_RECEIVABLE" + "\t");
                csv.Append("SOURCE_PAYMENT" + "\t");
                csv.Append("ACCOUNTS_PAYABLE" + "\t");
                csv.Append("ZANDAKA" + "\t");
                csv.Append("APPLY" + "\t");
                csv.Append("MEMO" + "\t");
                csv.Append("REGISTER_USER" + "\t");
                csv.Append("REGISTER_DATETIME" + "\t");
                csv.Append("REGISTER_PROCESS" + "\t");
                csv.Append("UPDATE_USER" + "\t");
                csv.Append("UPDATE_DATETIME" + "\t");
                csv.Append("UPDATE_PROCESS" + "\r\n");
            }
            foreach (var data in dataList)
            {
                csv.Append(data.Id.ToString() + "\t");
                csv.Append(data.DepositId.ToString() + "\t");
                csv.Append(data.SuitNo + "\t");
                csv.Append(data.BankCd + "\t");
                csv.Append(data.SitenCd + "\t");
                csv.Append(((DateTime)data.AccessDate).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.PaymentDestination + "\t");
                csv.Append(data.AccountsReceivable.ToString() + "\t");
                csv.Append(data.SourcePayment + "\t");
                csv.Append(data.AccountsPayable.ToString() + "\t");
                csv.Append(data.Zandaka.ToString() + "\t");
                csv.Append(data.Apply + "\t");
                csv.Append(data.Memo + "\t");
                csv.Append(data.RegisterUser + "\t");
                csv.Append(((DateTime)data.RegisterDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.RegisterProcess + "\t");
                csv.Append(data.UpdateUser + "\t");
                csv.Append(((DateTime)data.UpdateDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.UpdateProcess + "\r\n");
            }
            return csv.ToString();
        }

        public virtual void DefaultSet(MDepositTable data)
        {
        }

    }
}
