using System;
using System.Reflection;
using log4net;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;

namespace LogWriters
{
    /// <summary>
    /// ログ作成クラスを定義します。
    /// </summary>
    public static class LogWriter
    {
        /// <summary>ログ出力 </summary>
        private static readonly ILog logger = LogManager.GetLogger(Assembly.GetAssembly(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType), "syslog");

        /// <summary>SQLログ出力 </summary>
        private static readonly ILog logsql = LogManager.GetLogger(Assembly.GetAssembly(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType), "sqllog");

        /// <summary>
        /// トレースログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">Exception</param>
        public static void Trace(string message, Exception ex)
        {
            logger.Trace(message, ex); 
        }

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            logger.Error(message, ex);
        }

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Error(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void Error(Exception ex)
        {
            logger.Error(ex);
        }

        /// <summary>
        /// インフォメーションログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// 警告ログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// 致命エラーログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Fatal(string message)
        {
            logger.Fatal(message);
        }

        /// <summary>
        /// SQLログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void SqlLog(string message)
        {
            logsql.Info(message);
        }
    }
}
