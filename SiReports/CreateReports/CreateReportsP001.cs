using System;
using SiReports.CreateReports.Base;
using SiReports.Model;
using SiReports.Tools;
using DBAccess.Model;
using System.Collections.Generic;
using GrapeCity.Documents.Pdf.Spec;
using static Model.AllCommon.CDef;

namespace SiReports.CreateReports
{
    /// <summary>
    /// 入出金表
    /// </summary>
    public class CreateReportsP001 : CreateReportsBase
    {

        /// <summary>シート名</summary>
        private const string _BookName = "入出金表";
        private const string _SheetName = "入出金表";

        PrintStatus _RetuenStatus;

        ///-----------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///-----------------------------------------------------
        public CreateReportsP001()
        {
            SetReportInit("P001", _BookName);
 
            // リターンステータス
            _RetuenStatus = new PrintStatus("P001")
            {
                Status = PrintStatus.PrintOK,
            };

        }

        ///-----------------------------------------------------
        /// <summaryz
        /// 印刷処理
        /// </summary>
        /// <returns>帳票出力結果ステータス</returns>
        ///-----------------------------------------------------
        public PrintStatus CreateReports(SessionModel sessionInfo, IList<MDepositTable> mDepositTableList)
        {
            // テンプレートOPEN
            OpenTemplate();

            if(_Workbook == null)
            {
                _RetuenStatus.Status = PrintStatus.TemplateNothing;
                return _RetuenStatus;
            }

            // テンプレートのシートをアクティブ
            _Workbook.Worksheets[_SheetName].Activate();

            // ヘッダー領域を設定
            var headArea = DioDocsReports.SetHeaderCell(0, 0, 1, 7);

            // ヘッダー領域にデータを設定
            DioDocsReports.SetHedar<MDepositTable>(headArea, mDepositTableList);

            // 明細域を設定
            var detailArea = DioDocsReports.SetDetailCell(2, 0, 1, 7);

            DioDocsReports.SetDetailCopy(2, mDepositTableList.Count);

            // 明細データ設定
            DioDocsReports.SetDetail<MDepositTable>(detailArea, mDepositTableList);

            DateTime today = DateTime.Now;
            string Year = today.Year.ToString("0000");
            string Month = today.Month.ToString("00");
            string Day = today.Day.ToString("00");
            string Hour = today.Hour.ToString("00");
            string Minute = today.Minute.ToString("00");
            string Second = today.Second.ToString("00");

            _RetuenStatus.ExcelFilePath = _OutputPath + _BookName + "_" + Year + Month + Day + "_" + Hour + Minute + Second + ".xlsx";

            //_RetuenStatus.PdfFilePath = _OutputPath + _BookName + "_" + Year + Month + Day + "_" + Hour + Minute + Second + ".pdf";

            // Excel出力
            ExcelSave(_RetuenStatus.ExcelFilePath);

            // Pdf出力
            //PDFSave(_RetuenStatus.PdfFilePath);

            // ステータス返却
            return _RetuenStatus;
        }
    }
}
