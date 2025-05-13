namespace Utilities
{
    /// <summary>
    /// オリジナルException作成
    /// </summary>
    public static class OriginalException
    {
        /// <summary>
        /// 排他エラーのException発行
        /// </summary>
        public static void ExclusiveException(string msg)
        {
            throw new System.ArgumentException("Exclusive error", msg);
        }

        /// <summary>
        /// 排他エラーのException発行
        /// </summary>
        public static void TryParseException(string funcName, object obj)
        {
            if(obj == null)
            {
                throw new System.ArgumentException("TryParse error", funcName + ":null");
            }
            else
            {
                throw new System.ArgumentException("TryParse error", funcName + ":" + obj.ToString());
            }
        }

        /// <summary>
        /// 検索時、複数件取得エラー
        /// </summary>
        /// <param name="sql"></param>
        public static void EntityDuplicatedException(string sql)
        {
            throw new System.ArgumentException("Duplicate error", "SQL:" + sql);
        }
    }
}