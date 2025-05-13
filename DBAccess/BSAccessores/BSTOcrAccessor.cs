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
    /// TOcrのDBAccessorクラスを定義します。
    /// </summary>
    public partial class BSTOcrAccessor : CommonLogic
    {
        /// <summary>TOcrのDao</summary>
        protected TOcrDao Dao = null;
 
        /// <summary>DbContext</summary>
        protected ApplicationDbContext Context;
 
        /// <summary>DbContext</summary>
        public bool WhereFlg { get; set; } = false;

        /// <summary>DbContext</summary>
        public string TableDbName = "T_OCR"; 

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        
        public BSTOcrAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new TOcrDao(context);
            Context = context;
        }
        #endregion

        /// <summary>
        /// データ検索
        /// </summary>
        /// <returns>取得結果</returns>
        public IList<TOcr> Get()
        {
            return Query<TOcr>("SELECT * FROM T_OCR ");
        }
 
        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response TOcrCUD(IList<TOcr> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            foreach (var data in datalist)
            {
                var res = TOcrCUD(data);
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
        public virtual Response TOcrCUD(TOcr data)
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
        public Response TOcrBulkCUD(IList<TOcr> datalist)
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
            IList<TOcr> inserList = new List<TOcr>();
            // アップデート用データ
            IList<TOcr> updateList = new List<TOcr>();
            // デリート用データ
            IList<TOcr> deleteList = new List<TOcr>();

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
                var res = TOcrCUD(data);
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
        public TOcr Find(TOcr data)
        {
            var rec = Dao.Find(data.Id);
            if (rec == null)
            {
                 OriginalException.ExclusiveException("TOcr Key=" + ((long)data.Id).ToString());
            }
            if (CompareFatetime(rec.UpdateDatetime, data.UpdateDatetime))
            {
                 OriginalException.ExclusiveException("TOcr Key=" + ((long)data.Id).ToString());
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
        public int InsetdataList(IList<TOcr> inserList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<TOcr> dataList = new List<TOcr>();
            for (int idx = 0; idx < inserList.Count; idx++)
            {
                dataList.Add(inserList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetInsertSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<TOcr>();
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
        public int UpdateList(IList<TOcr> UpdateList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<TOcr> dataList = new List<TOcr>();
            for (int idx = 0; idx < UpdateList.Count; idx++)
            {
                dataList.Add(UpdateList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetUpdateSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<TOcr>();
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
        public string GetInsertSql(IList<TOcr> inserList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

            var sql = new StringBuilder();
            sql.Append("INSERT INTO T_OCR (");
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
        public string EditInsertSql(TOcr entity)
        {
            var sql = new StringBuilder();
            sql.Append(" " + GenericUtil.LongToString(entity.Id) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SuitNo) + ", ");
            sql.Append(" " + GenericUtil.DateToString(entity.ImportDate) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.Seqno) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Yyyy) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Mm) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Dd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Apply) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Apply2) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.SiharaiKin) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.AzukariKin) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.Zandaka) + ", ");
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
        public string GetUpdateSql(IList<TOcr> updateList)
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
        public string EditUpdateSql(TOcr entity)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE T_OCR");
            sql.Append(" SET");
            sql.Append("   SUIT_NO  = " + GenericUtil.StringToString(entity.SuitNo));
            sql.Append(",   IMPORT_DATE  = " + GenericUtil.DateToString(entity.ImportDate));
            sql.Append(",   SEQNO  = " + GenericUtil.LongToString(entity.Seqno));
            sql.Append(",   YYYY  = " + GenericUtil.StringToString(entity.Yyyy));
            sql.Append(",   MM  = " + GenericUtil.StringToString(entity.Mm));
            sql.Append(",   DD  = " + GenericUtil.StringToString(entity.Dd));
            sql.Append(",   APPLY  = " + GenericUtil.StringToString(entity.Apply));
            sql.Append(",   APPLY2  = " + GenericUtil.StringToString(entity.Apply2));
            sql.Append(",   SIHARAI_KIN  = " + GenericUtil.LongToString(entity.SiharaiKin));
            sql.Append(",   AZUKARI_KIN  = " + GenericUtil.LongToString(entity.AzukariKin));
            sql.Append(",   ZANDAKA  = " + GenericUtil.LongToString(entity.Zandaka));
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
        public string CreateCSV(IList<TOcr> dataList, bool header = true)
        {
            var csv = new StringBuilder();
            if (header)
            {
                csv.Append("ID" + "\t");
                csv.Append("SUIT_NO" + "\t");
                csv.Append("IMPORT_DATE" + "\t");
                csv.Append("SEQNO" + "\t");
                csv.Append("YYYY" + "\t");
                csv.Append("MM" + "\t");
                csv.Append("DD" + "\t");
                csv.Append("APPLY" + "\t");
                csv.Append("APPLY2" + "\t");
                csv.Append("SIHARAI_KIN" + "\t");
                csv.Append("AZUKARI_KIN" + "\t");
                csv.Append("ZANDAKA" + "\t");
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
                csv.Append(data.SuitNo + "\t");
                csv.Append(((DateTime)data.ImportDate).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.Seqno.ToString() + "\t");
                csv.Append(data.Yyyy + "\t");
                csv.Append(data.Mm + "\t");
                csv.Append(data.Dd + "\t");
                csv.Append(data.Apply + "\t");
                csv.Append(data.Apply2 + "\t");
                csv.Append(data.SiharaiKin.ToString() + "\t");
                csv.Append(data.AzukariKin.ToString() + "\t");
                csv.Append(data.Zandaka.ToString() + "\t");
                csv.Append(data.RegisterUser + "\t");
                csv.Append(((DateTime)data.RegisterDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.RegisterProcess + "\t");
                csv.Append(data.UpdateUser + "\t");
                csv.Append(((DateTime)data.UpdateDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.UpdateProcess + "\r\n");
            }
            return csv.ToString();
        }

        public virtual void DefaultSet(TOcr data)
        {
        }

    }
}
