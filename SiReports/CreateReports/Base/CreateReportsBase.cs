using System;
using System.IO;
using System.Collections.Generic;
using GrapeCity.Documents.Excel;

using DBAccess.Service;
using SiReports.Model;
using SiReports.Tools;
using DBAccess.Context;

namespace SiReports.CreateReports.Base
{
    /// <summary>
    /// DioDocs ベースクラス
    /// </summary>
    public class CreateReportsBase
    {
        protected string _TemplatePath = CommonContext.TemplateFolder;
        protected string _OutputPath = CommonContext.DownLoadFolder;

        protected string _OutputFullPath;
        protected string _TemplateFullPath;

        /// <summary>レポートId</summary>
        protected string _ReportId { set; get; }

        /// <summary>テンプレート名</summary>
        protected string _TemplateFimeName { set; get; }

        /// <summary>ワークブック</summary>
        protected Workbook _Workbook;

        // -------- Header

        /// <summary>ヘッダーRow</summary>
        protected int _HeaderRow { set; get; }

        /// <summary>ヘッダーCol</summary>
        protected int _HeaderCol { set; get; }

        /// <summary>ヘッダーRowCnt</summary>
        protected int _HeaderRowCnt { set; get; }

        /// <summary>ヘッダーColCnt</summary>
        protected int _HeaderColCnt { set; get; }

        // -------- Header

        // -------- Detail

        /// <summary>明細 Row</summary>
        protected int _DetailRow { set; get; }

        /// <summary>明細 Col</summary>
        protected int _DetailCol { set; get; }

        /// <summary>明細 RowCnt</summary>
        protected int _DetailRowCnt { set; get; }

        /// <summary>明細 ColCnt</summary>
        protected int _DetailColCnt { set; get; }

        // -------- Detail

        /// <summary>行高さ</summary>
        protected double _RowHeight { set; get; }

        /// <summary>行コピーコピー元範囲</summary>
        protected string _CopyMotoRange { set; get; }

        /// <summary>行コピーコピー先範囲</summary>
        protected string _CopySakiRange { set; get; }

        /// <summary>ヘッダー領域</summary>
        public List<AreaModel> _AreaHead { set; get; }

        /// <summary>明細領域</summary>
        public List<AreaModel> _AreaDetail { set; get; }

        /// <summary>
        /// PDFファイル名
        /// </summary>
        protected string pdfFileName { set; get; }

        /// <summary>
        /// レポート初期設定
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="templateFilename">テンプレートファイル名</param>
        protected void SetReportInit(string reportId, string templateFilename)
        {
            // 帳票ID
            _ReportId = new(reportId);

            // テンプレート
            _TemplateFimeName = templateFilename;

            _TemplateFullPath = _TemplatePath + _TemplateFimeName + ".xlsx";
        }

        /// <summary>
        /// ヘッダー領域を設定
        /// </summary>
        public List<AreaModel> SetHeaderArea()
        {
            // ヘッダー領域を設定
            return DioDocsReports.SetHeaderCell(_HeaderRow, _HeaderCol, _HeaderRowCnt, _HeaderColCnt);
        }

        /// <summary>
        /// 明細領域を設定
        /// </summary>
        public List<AreaModel> SetDetailArea()
        {
            // ヘッダー領域を設定
            return DioDocsReports.SetDetailCell(_DetailRow, _DetailCol, _DetailRowCnt, _DetailColCnt);
        }

        /// <summary>
        /// Range行コピー、行挿入してコピー
        /// </summary>
        public void RangeRowCopy()
        {
            _Workbook.ActiveSheet.Range[_CopySakiRange].EntireRow.Insert();
            _Workbook.ActiveSheet.Range[_CopySakiRange].RowHeight = _RowHeight;
            _Workbook.ActiveSheet.Range[_CopyMotoRange].Copy(_Workbook.ActiveSheet.Range[_CopySakiRange]);
        }

        /// <summary>
        /// PDF帳票保存
        /// </summary>
        protected void PDFSave(string fileName)
        {
            _OutputFullPath = _OutputPath + fileName;

            // Pdf 作成
            DioDocsReports.OutputPdf(_OutputFullPath);
        }

        /// <summary>
        /// EXCEL帳票保存
        /// </summary>
        /// <returns></returns>

        protected void ExcelSave(string filePath)
        {
            // シート保護
            _Workbook.ActiveSheet.Protection = true;

            // Excel Save
            DioDocsReports.Save(filePath);
        }

        /// <summary>
        /// テンプレートオープン
        /// </summary>
        public void OpenTemplate()
        {

            if (File.Exists(_TemplateFullPath))
            {
                // テンプレートOPEN
                _Workbook = DioDocsReports.OpenTemplate(_TemplateFullPath);
            }
            else
            {
                _Workbook = null;
            }
        }
    }
}