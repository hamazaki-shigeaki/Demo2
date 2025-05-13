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
    /// MAccountのDBAccessorクラスを定義します。
    /// </summary>
    public partial class BSMAccountAccessor : CommonLogic
    {
        /// <summary>MAccountのDao</summary>
        protected MAccountDao Dao = null;
 
        /// <summary>DbContext</summary>
        protected ApplicationDbContext Context;
 
        /// <summary>DbContext</summary>
        public bool WhereFlg { get; set; } = false;

        /// <summary>DbContext</summary>
        public string TableDbName = "M_ACCOUNT"; 

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        
        public BSMAccountAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new MAccountDao(context);
            Context = context;
        }
        #endregion

        /// <summary>
        /// データ検索
        /// </summary>
        /// <returns>取得結果</returns>
        public IList<MAccount> Get()
        {
            return Query<MAccount>("SELECT * FROM M_ACCOUNT ");
        }
 
        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response MAccountCUD(IList<MAccount> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            foreach (var data in datalist)
            {
                var res = MAccountCUD(data);
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
        public virtual Response MAccountCUD(MAccount data)
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
        public Response MAccountBulkCUD(IList<MAccount> datalist)
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
            IList<MAccount> inserList = new List<MAccount>();
            // アップデート用データ
            IList<MAccount> updateList = new List<MAccount>();
            // デリート用データ
            IList<MAccount> deleteList = new List<MAccount>();

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
                var res = MAccountCUD(data);
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
        public MAccount Find(MAccount data)
        {
            var rec = Dao.Find(data.Id);
            if (rec == null)
            {
                 OriginalException.ExclusiveException("MAccount Key=" + ((long)data.Id).ToString());
            }
            if (CompareFatetime(rec.UpdateDatetime, data.UpdateDatetime))
            {
                 OriginalException.ExclusiveException("MAccount Key=" + ((long)data.Id).ToString());
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
        public int InsetdataList(IList<MAccount> inserList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MAccount> dataList = new List<MAccount>();
            for (int idx = 0; idx < inserList.Count; idx++)
            {
                dataList.Add(inserList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetInsertSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MAccount>();
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
        public int UpdateList(IList<MAccount> UpdateList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MAccount> dataList = new List<MAccount>();
            for (int idx = 0; idx < UpdateList.Count; idx++)
            {
                dataList.Add(UpdateList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetUpdateSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MAccount>();
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
        public string GetInsertSql(IList<MAccount> inserList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

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
        public string EditInsertSql(MAccount entity)
        {
            var sql = new StringBuilder();
            sql.Append(" " + GenericUtil.LongToString(entity.Id) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.BankCd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SitenCd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Kamoku) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.KouzaNo) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.Zandaka) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CifNo) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Kana) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.CotionFlg) + ", ");
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
        public string GetUpdateSql(IList<MAccount> updateList)
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
        public string EditUpdateSql(MAccount entity)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE M_ACCOUNT");
            sql.Append(" SET");
            sql.Append("   BANK_CD  = " + GenericUtil.StringToString(entity.BankCd));
            sql.Append(",   SITEN_CD  = " + GenericUtil.StringToString(entity.SitenCd));
            sql.Append(",   KAMOKU  = " + GenericUtil.StringToString(entity.Kamoku));
            sql.Append(",   KOUZA_NO  = " + GenericUtil.StringToString(entity.KouzaNo));
            sql.Append(",   ZANDAKA  = " + GenericUtil.LongToString(entity.Zandaka));
            sql.Append(",   CIF_NO  = " + GenericUtil.StringToString(entity.CifNo));
            sql.Append(",   KANA  = " + GenericUtil.StringToString(entity.Kana));
            sql.Append(",   COTION_FLG  = " + GenericUtil.LongToString(entity.CotionFlg));
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
        public string CreateCSV(IList<MAccount> dataList, bool header = true)
        {
            var csv = new StringBuilder();
            if (header)
            {
                csv.Append("ID" + "\t");
                csv.Append("BANK_CD" + "\t");
                csv.Append("SITEN_CD" + "\t");
                csv.Append("KAMOKU" + "\t");
                csv.Append("KOUZA_NO" + "\t");
                csv.Append("ZANDAKA" + "\t");
                csv.Append("CIF_NO" + "\t");
                csv.Append("KANA" + "\t");
                csv.Append("COTION_FLG" + "\t");
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
                csv.Append(data.BankCd + "\t");
                csv.Append(data.SitenCd + "\t");
                csv.Append(data.Kamoku + "\t");
                csv.Append(data.KouzaNo + "\t");
                csv.Append(data.Zandaka.ToString() + "\t");
                csv.Append(data.CifNo + "\t");
                csv.Append(data.Kana + "\t");
                csv.Append(data.CotionFlg.ToString() + "\t");
                csv.Append(data.RegisterUser + "\t");
                csv.Append(((DateTime)data.RegisterDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.RegisterProcess + "\t");
                csv.Append(data.UpdateUser + "\t");
                csv.Append(((DateTime)data.UpdateDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.UpdateProcess + "\r\n");
            }
            return csv.ToString();
        }

        public virtual void DefaultSet(MAccount data)
        {
        }

    }
}
