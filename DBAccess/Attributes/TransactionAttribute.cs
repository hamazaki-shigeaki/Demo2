using System.Transactions;
using MethodBoundaryAspect.Fody.Attributes;
using LogWriters;

namespace DBAccess.Attributes
{
    /// <summary>
    /// Transaction のアトリビュートを定義します。
    /// </summary>
    public sealed class TransactionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 開始処理
        /// </summary>
        /// <param name="args"></param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            //LogWriter.Info("Stsrt");
            args.MethodExecutionTag = new TransactionScope();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="args"></param>
        public override void OnExit(MethodExecutionArgs args)
        {
            var transactionScope = (TransactionScope)args.MethodExecutionTag;

            transactionScope.Complete();
            //LogWriter.Info("Complete");
            transactionScope.Dispose();
        }

        /// <summary>
        /// Exception処理
        /// </summary>
        /// <param name="args"></param>
        public override void OnException(MethodExecutionArgs args)
        {
            var transactionScope = (TransactionScope)args.MethodExecutionTag;
            transactionScope.Dispose();
            LogWriter.Error(args.Exception);
            throw args.Exception;
        }
    }
}

