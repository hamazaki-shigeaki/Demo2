using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Context
{
    /// <summary>
    /// DBContext のクラス定義です。
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region "変数"
        /// <summary>接続情報</summary>
        private DbConnection database = null;

        /// <summary>ユーザーID</summary>
        public string UserId;

        /// <summary>プロセス名称</summary>
        public string Process = string.Empty;
        #endregion

        /// <summary>
        /// コンテキスト
        /// </summary>
        /// <param name="options">接続オプション</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            database = this.Database.GetDbConnection();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            database.Dispose();
        }
    }
}