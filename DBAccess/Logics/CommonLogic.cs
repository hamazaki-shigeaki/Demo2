#define PostgreSQL
//#define SqlServer

using Dapper;
using DBAccess.Context;
using LogWriters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Utilities;

namespace DBAccess.Logics
{
    /// <summary>
    /// データベース共通ロジッククラスを定義します    /// </summary>
    public class CommonLogic : IDisposable
    {
        #region "変数"
        /// <summary>DBコネクション</summary>
        //[ThreadStatic]
        //static DbConnection Con;
        DbConnection Con { set; get; }


        DbConnection _Connection
        {
            get{
                
                if (Con == null)
                {
                    Con = _Context.Database.GetDbConnection();
                }

                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                return Con;
            }
        }


        /// <summary>コンテキスト</summary>
        static ApplicationDbContext _Context = null;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public CommonLogic(ApplicationDbContext context) 
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _Context = context;
 
            Con = context.Database.GetDbConnection();

        }
        #endregion

        #region "メソッド"
        /// <summary>
        /// 検索処理を行います。
        /// </summary>
        /// <typeparam name="T">取得モデル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual IList<T> Query<T>(string sql, DynamicParameters parameters = null)
        {
            try
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                SqlLogWiter(sql, parameters);
                var eList = _Connection.Query<T>(sql, parameters);
                var list = eList;
                return (List<T>)list;
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
                LogWriter.Error(sql);
                if (parameters != null)
                {
                    LogWriter.Error(ParametersToString(parameters));
                }
                
                throw ex;
            }
        }
         
        /// <summary>
        /// 検索処理を行います。
        /// </summary>
        /// <typeparam name="T">取得モデル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual object QuerySingleOrDefault<T>(string sql, DynamicParameters parameters = null)
        {
            try
            {
                SqlLogWiter(sql, parameters);
                var obj = _Connection.QuerySingleOrDefault<T>(sql, parameters);
                return obj;
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
                LogWriter.Error(sql);
                if (parameters != null)
                {
                    LogWriter.Error(ParametersToString(parameters));
                }

                throw ex;
            }
        }

        /// <summary>
        /// 更新処理を行います。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>処理件数</returns>
        public virtual int Execute(string sql, DynamicParameters parameters = null)
        {
            try
            {
                SqlLogWiter(sql, parameters);
                return _Connection.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
                LogWriter.Error(sql);
                if (parameters != null)
                {
                    LogWriter.Error(ParametersToString(parameters));
                }

                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// SQLログ出力
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        private void SqlLogWiter(string sql, DynamicParameters parameters = null)
        {
            LogWriter.SqlLog(sql);
            if (parameters == null) return;
            LogWriter.SqlLog(ParametersToString(parameters));
        }

        /// <summary>
        /// SQLパラメータのログ出力
        /// </summary>
        /// <param name="parameters">パラメータ</param>
        private string ParametersToString(DynamicParameters parameters)
        {
            var result = new StringBuilder();

            if (parameters != null)
            {
                var firstParam = true;
                var parametersLookup = (SqlMapper.IParameterLookup)parameters;
                foreach (var paramName in parameters.ParameterNames)
                {
                    if (!firstParam)
                    {
                        result.Append(", ");
                    }
                    firstParam = false;

                    result.Append('@');
                    result.Append(paramName);
                    result.Append(" = ");
                    try
                    {
                        var value = parametersLookup[paramName];// parameters.Get<dynamic>(paramName);
                        result.Append((value != null) ? value.ToString() : "{null}");
                    }
                    catch
                    {
                        result.Append("unknown");
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// シーケンスの取得
        /// </summary>
        /// <param name="seqName">シーケンス物理名</param>
        /// <returns>取得結果</returns>
        public long GetSeq(string seqName)
        {
            var sql = string.Empty;
            sql = MakeSeqSql(seqName);
            var ret = Query<SeqModel>(sql);
            return ret[0].SeqNo;
        }

        /// <summary>
        /// シーケンス取得SQL文作成
        /// </summary>
        /// <param name="seqName">シーケンス物理名</param>
        /// <returns>SQL</returns>
        public string MakeSeqSql(string seqName)
        {
            return @"SELECT NEXT VALUE FOR {0} as seq_no".Replace("{0}", seqName);
        }

        /// <summary>
        /// 時刻比較(秒まで)
        /// </summary>
        /// <param name="aTime">比較元</param>
        /// <param name="bTime">比較先</param>
        /// <returns>一致:False 不一致:True</returns>
        public bool CompareFatetime(DateTime? aTime , DateTime? bTime )
        {
            DateTime time01 = (DateTime)aTime;
            DateTime time02 = (DateTime)bTime;

            if(time01.Year == time02.Year &&
               time01.Month == time01.Month &&
               time01.Day == time01.Day &&
               time01.Hour == time01.Hour &&
               time01.Minute == time01.Minute &&
               time01.Second == time01.Second &&
               time01.Millisecond == time01.Millisecond
               )
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// データリスト取得
        /// </summary>
        /// <typeparam name="T">取得結果テーブル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual IList<T> ExecuteList<T>(dynamic cb)
        {
            try
            {
                return Query<T>(cb.ToDisplaySql(), cb.GetDapperParam());
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// データリスト取得
        /// </summary>
        /// <typeparam name="T">取得結果テーブル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual IList<T> ExecuteList<T>(string sql, DynamicParameters parameters)
        {
            return Query<T>(sql, parameters);
        }

        /// <summary>
        /// データ1件取得
        /// </summary>
        /// <typeparam name="T">取得結果テーブル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual T ExecuteEntity<T>(string sql, DynamicParameters parameters)
        {
            return (T)QuerySingleOrDefault<T>(sql, parameters);

        }

        /// <summary>
        /// データ1件取得
        /// </summary>
        /// <typeparam name="T">取得結果テーブル</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>取得結果</returns>
        public virtual T ExecuteEntity<T>(dynamic cb)
        {
            return (T)QuerySingleOrDefault<T>(cb.ToDisplaySql(), cb.GetDapperParam());
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// SeqNo取得用モデルクラス
    /// </summary>
    public class SeqModel
    {
        public long SeqNo { get; set; }
    }
}
