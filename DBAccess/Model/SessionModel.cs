using Model.ExEntity;
using System.Collections.Generic;

namespace DBAccess.Model
{
    public class SessionModel
    {
        /// <summary>
        /// 呼び出し元ページ
        /// </summary>
        public string CallPage { get; set; }

        /// <summary>
        /// 親ページ
        /// </summary>
        public List<string> PrePage { get; set; } = new List<string>();

        /// <summary>
        /// カレントページ
        /// </summary>
        public string CurrentPage { get; set; }

        /// <summary>
        /// IndexNo
        /// </summary>
        public long IndexNo { get; set; }

        /// <summary>
        /// MSuit
        /// </summary>
        public MSuit _MSuit { get; set; }

    }
}
