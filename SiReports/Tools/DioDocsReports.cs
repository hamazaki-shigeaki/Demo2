using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using GrapeCity.Documents.Excel;

using SiReports.Model;
using DBAccess.Context;
using System.Globalization;

namespace SiReports.Tools
{
    /// <summary>
    /// DioDocs帳票作成クラス
    /// </summary>
    public static class DioDocsReports
    {
        /// <summary>
        /// ライセンス
        /// </summary>
        public const string License = null;

         /// <summary>ワークブック</summary>
        private static Workbook _Workbook;

        /// <summary>ヘッダー定義記号（左）長さ</summary>
        private static int _HederKigoLeftLen = 4;

        /// <summary>明細定義記号（左）長さ</summary>
        private static int _DetailKigoLeftLen = 3;

        /// <summary>
        /// テンプレートOPEN
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ワークブック</returns>
        public static Workbook OpenTemplate(string filePath)
        {
            try
            {
                _Workbook = new Workbook();
                //_Workbook = new Workbook(License);

                _Workbook.Culture = CultureInfo.GetCultureInfo("ja-JP");

                _Workbook.Open(filePath, OpenFileFormat.Xlsx);
            }
            catch(Exception ex)
            {
                var msg = ex;
            }

            return _Workbook;
        }

        /// <summary>
        /// Excel Save
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public static void Save(string filePath)
        {
            _Workbook.Save(filePath);
        }

        /// <summary>
        /// シート削除
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public static void DeleteSheet(string sheetName)
        {
            _Workbook.Worksheets[sheetName].Delete();  
        }

        /// <summary>
        /// PDF出力(シート)
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public static void OutputPdfSheet(string filePath)
        {
            _Workbook.ActiveSheet.Save(filePath, SaveFileFormat.Pdf);
        }

        /// <summary>
        /// PDF出力(ブック)
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ステータス</returns>
        public static void OutputPdf(string filePath)
        {
            try
            {
                _Workbook.Save(filePath);
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
            }
        }

        /// <summary>
        /// マージン設定
        /// </summary>
        /// <param name="top">上</param>
        /// <param name="bottom">下</param>
        /// <param name="left">左</param>
        /// <param name="right">右</param>
        public static void SetMargin(double top, double bottom, double left, double right)
        {
            var sheet = _Workbook.ActiveSheet;
            sheet.PageSetup.TopMargin = top;
            sheet.PageSetup.BottomMargin = bottom;
            sheet.PageSetup.LeftMargin = left;
            sheet.PageSetup.RightMargin = right;
        }

        /// <summary>
        /// ヘッダーエリア設定
        /// </summary>
        /// <param name="row1">行1</param>
        /// <param name="col1">列1</param>
        /// <param name="rowcnt">行数</param>
        /// <param name="colcnt">列数</param>
        public static List<AreaModel> SetHeaderCell(int row1, int col1, int rowcnt, int colcnt)
        {
            List<AreaModel> areaList = new ();
            var range = _Workbook.ActiveSheet.Range[row1, col1, rowcnt, colcnt];

            for (int r = 0; r < range.RowCount; r++)
            {
                for (int c = 0; c < range.ColumnCount; c++)
                {
                    var tmp = range[0 + r, c].Value?.ToString();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        if ((tmp.Length > 5) && (tmp.Substring(0, _HederKigoLeftLen) == "{***") && tmp.Substring(tmp.Length - 1, 1) == "}")
                        {
                            var addData = new AreaModel()
                            {
                                AreaName = tmp[_HederKigoLeftLen..].Replace("}", ""),
                                Row = row1 + r,
                                Col = col1 + c
                            };
                            areaList.Add(addData);
                            range[0 + r, c].Value = "";
                        }
                    }
                }
            }
            return areaList;
        }

        /// <summary>
        /// 明細エリア設定
        /// </summary>
        /// <param name="row1">行1</param>
        /// <param name="col1">列1</param>
        /// <param name="rowcnt">行数</param>
        /// <param name="colcnt">列数</param>
        public static List<AreaModel> SetDetailCell(int row1, int col1, int rowcnt, int colcnt)
        {
            List<AreaModel> areaList = new ();
            var range = _Workbook.ActiveSheet.Range[row1, col1, rowcnt, colcnt];
            for (int r = 0; r < range.RowCount; r++)
            {
                for (int c = 0; c < range.ColumnCount; c++)
                {
                    var tmp = range[r, c].Value?.ToString();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        if ((tmp.Length > (_DetailKigoLeftLen + 1)) && (tmp.Substring(0, _DetailKigoLeftLen) == "{**") && (tmp.Substring(tmp.Length - 1, 1) == "}"))
                        {
                            if (tmp.Substring((_DetailKigoLeftLen + 1), 1) != "*")
                            {
                                var addData = new AreaModel()
                                {
                                    AreaName = tmp[_DetailKigoLeftLen..].Replace("}", ""),
                                    Row = row1 + r,
                                    Col = col1 + c
                                };
                                areaList.Add(addData);
                                range[0 + r, c].Value = "";
                            }
                        }
                    }
                }
            }
            return areaList;
        }

        /// <summary>
        /// ヘッダーデータ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetHedar<T>(List<T> list, List<AreaModel> areaList)
        {
            foreach (var a in list)
            {
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                        }
                    }
                }
                break;
            }
        }

        /// <summary>
        /// ヘッダーデータ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetHedar<T>(IList<AreaModel> areaList, IList<T> list)
        {
            foreach (var a in list)
            {
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                        }
                    }
                }
                break;
            }
        }

        /// <summary>
        /// ヘッダーデータ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetHedarMultiPage<T>(List<T> list, List<AreaModel> areaList,int pageRow)
        {
            foreach (var a in list)
            {
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                            dat.Row += pageRow;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// エリアデータ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="data">データ</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetData<T>(T data, List<AreaModel> areaList)
        {
            foreach (var property in data.GetType().GetProperties())
            {
                foreach (var dat in areaList)
                {
                    if (dat.AreaName == property.Name)
                    {
                        _Workbook.ActiveSheet.Cells[dat.Row, dat.Col].Value = data.GetType().GetProperty(property.Name).GetValue(data);
                    }
                }
            }
        }

        /// <summary>
        /// 明細データ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetDetail<T>(List<AreaModel> areaList, IEnumerable<T> list)
        {
            foreach (var a in list)
            {
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row++, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 明細データ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        public static void SetDetailMultiPage<T>(IEnumerable<T> list, List<AreaModel> areaList, int pageRow, int perPage)
        {
            var cnt = 0;
            foreach (var a in list)
            {
                cnt++;
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row++, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                        }
                    }
                }
                if(((cnt % perPage) == 0))
                {
                    foreach (var dat in areaList)
                    {
                        dat.Row += (pageRow - perPage);
                    }
                }
            }
        }

        /// <summary>
        /// 明細データ設定
        /// </summary>
        /// <typeparam name="T">テーブル</typeparam>
        /// <param name="list">データリスト</param>
        /// <param name="areaList">エリアリスト</param>
        /// <param name="gyoCnt">行数</param>
        public static void SetDetailMulti<T>(IEnumerable<T> list, List<AreaModel> areaList, int gyoCnt)
        {
            foreach (var a in list)
            {
                foreach (var property in a.GetType().GetProperties())
                {
                    foreach (var dat in areaList)
                    {
                        if (dat.AreaName == property.Name)
                        {
                            _Workbook.ActiveSheet.Cells[dat.Row, dat.Col].Value = a.GetType().GetProperty(property.Name).GetValue(a);
                            dat.Row += gyoCnt;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cellデータ設定
        /// </summary>
        /// <param name="col">行</param>
        /// <param name="row">列</param>
        /// <param name="obj">データ</param>
        public static void SetCell(int row, int col, object obj)
        {
            _Workbook.ActiveSheet.Cells[row, col].Value = obj;
        }

        /// <summary>
        /// ケイ線/色 設定
        /// </summary>
        /// <param name="row1">行１</param>
        /// <param name="col1">列１</param>
        /// <param name="row2">行２</param>
        /// <param name="col2">列２</param>
        /// <param name="style">罫線スタイル</param>
        /// <param name="color">色</param>
        public static void SetLine(int row1, int col1, int row2, int col2, BorderLineStyle style, Color color)
        {
            IRange rangeB2 = _Workbook.ActiveSheet.Range[row1, col1, row2, col2];
            rangeB2.Borders.LineStyle = style;
            rangeB2.Borders.Color = color;
        }

        /// <summary>
        /// コピーテンプレート(シート)
        /// </summary>
        public static void CopyTemplete()
        {
            // テンプレートのシートをアクティブ
            _Workbook.Worksheets["temp"].Activate();

            // テンプレートのシートをコピー
            var copy_worksheet = _Workbook.ActiveSheet.Copy();

            // コピーしたシートに名前を設定
            copy_worksheet.Name = _Workbook.Worksheets.Count.ToString();

            // コピーしたシートをアクティブ
            copy_worksheet.Activate();
        }

        /// <summary>
        /// コピーテンプレート(シート)
        /// </summary>
        public static void CopyTempleteByName(string sheetName)
        {
            // テンプレートのシートをアクティブ
            _Workbook.Worksheets[sheetName].Activate();

            // テンプレートのシートをコピー
            var copy_worksheet = _Workbook.ActiveSheet.Copy();

            // コピーしたシートに名前を設定
            copy_worksheet.Name = _Workbook.Worksheets.Count.ToString();

            // コピーしたシートをアクティブ
            copy_worksheet.Activate();
        }

        /// <summary>
        /// コピーテンプレート(シート)
        /// </summary>
        public static void CopyTemplete(string name)
        {
            // テンプレートのシートをアクティブ
            _Workbook.Worksheets[name].Activate();

            // テンプレートのシートをコピー
            var copy_worksheet = _Workbook.ActiveSheet.Copy();

            // コピーしたシートに名前を設定
            copy_worksheet.Name = _Workbook.Worksheets.Count.ToString();

            // コピーしたシートをアクティブ
            copy_worksheet.Activate();
        }

        /// <summary>
        /// ページコピー
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <param name="pageCnt">ページ数</param>
        public static void PageCopy(int row, int col, int pageCnt)
        {
            for (int c = 1; c < pageCnt; c++)
            {
                _Workbook.ActiveSheet.Range[0, 0, row - 1, col - 1].Copy(_Workbook.ActiveSheet.Range[c*row, 0, row - 1, col - 1]);
                _Workbook.ActiveSheet.HPageBreaks.Add(_Workbook.ActiveSheet.Range[c * row, 0]);
            }
        }

        /// <summary>
        /// 明細行コピー
        /// </summary>
        /// <param name="lineNo">コピー行</param>
        /// <param name="gyoCnt">行数</param>
        public static void SetDetailCopy(int lineNo, int gyoCnt)
        {
            for (int c = 1; c < gyoCnt; c++)
            {
                _Workbook.ActiveSheet.Range[lineNo + 1, 0, 1, 999].Insert(InsertShiftDirection.Down);
                _Workbook.ActiveSheet.Range[lineNo, 0, 1, 999].Copy(_Workbook.ActiveSheet.Range[lineNo + 1, 0, 1, 999]);
                //行の高さはコピーされないため、設定
                _Workbook.ActiveSheet.Range[lineNo + c, 0, 1, 999].RowHeight = _Workbook.ActiveSheet.Range[lineNo, 0, 1, 999].RowHeight;
            }
        }

        /// <summary>
        /// SUM設定
        /// </summary>
        /// <param name="row">SUM設定・行</param>
        /// <param name="col">SUM設定・列</param>
        /// <param name="row1">開始・行</param>
        /// <param name="col1">開始・列</param>
        /// <param name="row2">終了・行</param>
        /// <param name="col2">終了・列</param>
        public static void SetFormulaSum(int row, int col, int row1, int col1 , int row2, int col2)
        {
            var formula = "=SUM(" + ToExcelStr(row1, col1) + ":" + ToExcelStr(row2, col2);

            // 数式の実行を遅延
            DeferUpdate( true );

            // 数式の設定
            _Workbook.ActiveSheet.Range[row, col].Formula = formula;

            // 数式の実行を再開
            DeferUpdate(false);
        }

        /// <summary>
        /// 数式設定
        /// </summary>
        /// <param name="row">SUM設定・行</param>
        /// <param name="col">SUM設定・列</param>
        /// <param name="formula">式</param>
        public static void SetFormula(int row, int col, string formula)
        {
            // 数式の設定
            _Workbook.ActiveSheet.Range[row, col].Formula = formula;
        }

        /// <summary>
        /// 数式の実行制御
        /// </summary>
        /// <param name="flg">true:抑制、false:解除</param>
        public static void DeferUpdate(bool flg)
        {
            _Workbook.DeferUpdateDirtyState = flg;
        }

        /// <summary>
        /// エクセルA1参照形式文字列作成
        /// </summary>
        /// <param name="row">行(0始まり)</param>
        /// <param name="col">カラム(0始まり)</param>
        /// <returns>A1列文字列</returns>
        public static string ToExcelStr(int row, int col)
        {
            string ret = ToAlphabet(col + 1) + (row + 1).ToString();
            return ret;
        }

        /// <summary>
        /// 数値をExcelのカラム名的なアルファベット文字列へ変換します。
        /// </summary>
        /// <param name="self">カラム値(1始まり)</param>
        /// <returns>
        /// Excelのカラム名的なアルファベット文字列。
        /// (変換できない場合は、空文字を返します。)
        /// </returns>
        public static string ToAlphabet(this int col)
        {
            if (col <= 0) return "";
            int n = col % 26;
            n = (n == 0) ? 26 : n;
            string s = ((char)(n + 64)).ToString();
            if (col == n) return s;
            return ((col - n) / 26).ToAlphabet() + s;
        }

        /// <summary>
        /// Excelのカラム名的なアルファベットを数値へ変換します。
        /// </summary>
        /// <param name="alphabet">列文字列</param>
        /// <returns>
        /// 数値(列番号1始まり)
        /// (変換できない場合は、0を返します。)
        /// </returns>
        public static int ToInt(this string alphabet)
        {
            int result = 0;
            if (string.IsNullOrEmpty(alphabet)) return result;

            char[] chars = alphabet.ToCharArray();
            int len = alphabet.Length - 1;
            foreach (var c in chars)
            {
                int asc = (int)c - 64;
                if (asc < 1 || asc > 26) return 0;
                result += asc * (int)Math.Pow((double)26, (double)len--);
            }
            return result;
        }

        /// <summary>
        /// Range行コピー、行挿入してコピー
        /// </summary>
        public static void RangeRowCopy(int row, int rowcnt, double heigh)
        {
            var str = string.Format(@"R{0}C1:R{0}C100", row.ToString());
            var row1 = row + 1;
            var row2 = row + rowcnt;
            var str2 = string.Format(@"R{0}C1:R{0}C100", row1.ToString());
            var str3 = string.Format(@"{0}:{1}", row.ToString(), row2.ToString());
            for (var c = 0; c < rowcnt; c++)
            {
                _Workbook.ActiveSheet.Range[str2].Insert(InsertShiftDirection.Down);
                _Workbook.ActiveSheet.Range[str].Copy(_Workbook.ActiveSheet.Range[str2]);
            }
            _Workbook.ActiveSheet.Range[str3].RowHeight = heigh;
        }
    }
}