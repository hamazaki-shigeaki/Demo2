using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiReports.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintStatus
    {
        public const int PrintOK = 1;

        public const int DataNoting = 0;

        public const int TemplateNothing = -1;

        /// <summary>
        /// 帳票ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// Excelファイルフルパス
        /// </summary>
        public string ExcelFilePath { get; set; }

        /// <summary>
        /// PDFファイルフルパス
        /// </summary>
        public string PdfFilePath { get; set; }

        /// <summary>
        /// プリントステータス
        /// </summary>
        public int Status { get; set; } = PrintOK;

        public PrintStatus(string id)
        {
            ReportId = id;
        }
    }
}
