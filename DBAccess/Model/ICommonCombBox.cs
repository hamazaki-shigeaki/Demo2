using Model.AllCommon;
using System;
using System.Collections.Generic;

namespace DBAccess.Model
{
    /// <summary>
    /// コンボボックス用汎用モデルクラスを定義します。
    /// </summary>
    public class ICommonCombBox : IDisposable
    {
        /// <summary>値</summary>
        public long? Value { get; set; }

        /// <summary>表示文字列</summary>
        public string Text { get; set; }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
