using DBAccess.Service;
using System.Collections.Generic;
using DBAccess.Model;

namespace SiReports.Model
{
    /// <summary>
    /// 帳票出力パラメータクラス
    /// </summary>
    public class ReportParam
    {
        /// <summary>帳票ID</summary>
        public string ReportId { set; get; }

        /// <summary>テンプレートファイル名</summary>
        public string TemplateFileName { set; get; }
    }
}