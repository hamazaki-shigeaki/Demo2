using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Utilities
{
    /// <summary>
    /// 共通関数クラスを記述します
    /// </summary>
    public static class GenericUtil
    {
        /// <summary>
        /// 年度算出
        /// </summary>
        /// <param name="pymd">変換元年月日</param>
        /// <returns>年度</returns>
        public static DateTime GetNendoYmd(DateTime pymd)
        {
            int WRK_NEN1 = pymd.Year;
            int WRK_TUKI1 = pymd.Month - 3;
            if (WRK_TUKI1 <= 0)
            {
                WRK_NEN1 = WRK_NEN1 - 1;
            }
            else
            {
                WRK_NEN1 = pymd.Year;
            }
            return new DateTime(WRK_NEN1, pymd.Month, 1);
        }

        /// <summary>
        /// 指定した日付から指定した日数の営業日（土日除く）を算出
        /// </summary>
        /// <param name="syoriDt">指定日付</param>
        /// <param name="days">日数</param>
        /// <returns>日付リスト</returns>
        public static IList<DateTime> GetWeekDayOfExceptWeekEndDays(DateTime syoriDt, long days)
        {
            IList<DateTime> list = new List<DateTime>();
            DateTime dt = syoriDt.Date;
            for (long i = 1; i <= days; )
            {
                if (dt.DayOfWeek != DayOfWeek.Sunday && dt.DayOfWeek != DayOfWeek.Saturday)
                {
                    list.Add(dt);
                    i++;                // 返した日付数の加算 
                }
                dt = dt.AddDays(1);     // 日付を加算 
            }
            return list;
        }

        /// <summary>
        /// 時間テキストをDBカラム用に適正化する
        /// </summary>
        /// <param name="values">変換元時間</param>
        /// <returns>変換結果</returns>
        public static String optimizeTextTmForDBcolumn(String values)
        {
            if (String.IsNullOrEmpty(values) || "_:_".Equals(values))
            {
                return null;
            }
            return values.Replace(":", "").Trim();
        }

        /// <summary>
        /// 日付テキストをDBカラム用に適正化する
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <returns>変換結果</returns>
        public static String optimizeTextDtForDBcolumn(String values)
        {
            if (String.IsNullOrEmpty(values) || "____/__/__".Equals(values))
            {
                return null;
            }
            return values.Replace("/", "").Trim();
        }

        /// <summary>
        /// トリム
        /// </summary>
        /// <param name="value">変換元文字列</param>
        /// <param name="trimChars">トリム文字</param>
        /// <returns>変換結果</returns>
        public static String Trim(String value, char[] trimChars = null)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            if (trimChars == null)
            {
                return value.Trim();
            }
            return value.Trim(trimChars);
        }

        /// <summary>
        /// ゼロ埋め
        /// </summary>
        /// <param name="value">変換元文字列</param>
        /// <param name="ketasuu">桁数</param>
        /// <returns>変換結果</returns>
        public static String ZeroPadding(String value, int ketasuu)
        {
            return String.Format("{0:D" + ketasuu + "}", GenericUtil.ConvertStringToLong(value));
        }

        /// <summary>
        /// 左空白埋め ketasuu: プラスなら右寄せ、マイナスなら左寄せ
        /// </summary>
        /// <param name="value">変換元文字列</param>
        /// <param name="ketasuu">桁数</param>
        /// <returns>変換結果</returns>
        public static String SpacePadding(String value, int ketasuu)
        {
            return String.Format("{0," + ketasuu + "}", value);
        }

        /// <summary>
        /// 期間(日数)算出(片端)
        /// </summary>
        /// <param name="FromDt">日付From</param>
        /// <param name="ToDt">日付To</param>
        /// <returns>日数</returns>
        public static int GetDateSpan(String FromDt, String ToDt)
        {

            DateTime cnvFromDt;
            DateTime cnvToDt;

            // DateTimeに変換できるかチェック
            if (!DateTime.TryParse(FromDt, out cnvFromDt))
            {
                OriginalException.TryParseException("GetDateSpan:FromDt=", FromDt);
            }
            if (!DateTime.TryParse(ToDt, out cnvToDt))
            {
                OriginalException.TryParseException("GetDateSpan:ToDt=", ToDt);
            }
            TimeSpan span = cnvToDt - cnvFromDt;
            return span.Days;
        }

        /// <summary>
        /// 期間(日数)算出(片端)
        /// </summary>
        /// <param name="FromDt">日付From</param>
        /// <param name="ToDt">日付To</param>
        /// <returns>日数</returns>
        public static int GetHiSuu(DateTime? FromDt, DateTime? ToDt)
        {
            if (FromDt.HasValue == false)
            {
                return 0;
            }
            if (ToDt.HasValue == false)
            {
                return 0;
            }
            TimeSpan span = (DateTime)ToDt - (DateTime)FromDt;
            return span.Days;
        }


        /// <summary>
        /// 日付妥当性チェック
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>判定結果</returns>
        public static Boolean IsDate(String date)
        {
            Boolean flg = false;
            string strDate = date;   // チェックする日付を設定
            DateTime dt;

            // DateTimeに変換できるかチェック
            if (DateTime.TryParse(strDate, out dt))
            {
                flg = true;
            }
            return flg;
        }

        /// <summary>
        /// 時間妥当性チェック
        /// </summary>
        /// <param name="date">時刻</param>
        /// <returns>判定結果</returns>
        public static Boolean IsTime(String time)
        {
            Boolean flg = false;
            string strTime = "2015/01/01 " + time;   // チェックする日付を設定
            DateTime dt;

            // DateTimeに変換できるかチェック
            if (DateTime.TryParse(strTime, out dt))
            {
                flg = true;
            }
            return flg;
        }

        /// <summary>
        /// 文字（Byte）の長さ
        /// </summary>
        /// <param name="str">チェック対象文字列</param>
        /// <param name="Encoding">エンコーディング文字列</param>
        /// <returns>文字列長(バイト数)</returns>
        public static int StrByteLength(String str, String Encode = "Shift_JIS")
        {
            Encoding sjisEnc = Encoding.GetEncoding(Encode);
            return sjisEnc.GetByteCount(str);
        }

        /// <summary>
        /// 数値チェック
        /// </summary>
        /// <param name="str">数値文字列</param>
        /// <returns>判定結果</returns>
        public static Boolean IsNumber(String str)
        {
            double d;
            if (double.TryParse(str, out d))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 数値チェック 小数点を含むか
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>判定結果</returns>
        public static Boolean IsDecimal(double doub)
        {
            if (doub - System.Math.Truncate(doub) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 指定したyyy mm dd から日付型を返却する
        /// </summary>
        /// <param name="yyyy">年</param>
        /// <param name="mm">月</param>
        /// <param name="dd">日</param>
        /// <returns>変換結果</returns>
        public static DateTime ConvertDateTimeFromIntQYYMMDD(int? yyyy, int? mm, int? dd)
        {
            return new DateTime(ConvertIntQ_Toint(yyyy), ConvertIntQ_Toint(mm), ConvertIntQ_Toint(dd));
        }


        /// <summary>
        /// 現在日付
        /// </summary>
        /// <returns>現在日付</returns>
        public static DateTime GetNowDate()
        {
            return DateTime.Now.Date;
        }

        /// <summary>
        /// 現在日付と時間
        /// </summary>
        /// <returns>現在時刻</returns>
        public static DateTime GetNowDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 現在時間と時間（４ケタ）を形式指定して取得
        /// </summary>
        /// <param name="fmt">書式</param>
        /// <returns>現在時刻</returns>
        public static String GetNowDateTimeTypeString(String fmt = "yyyy/MM/dd H:mm:ss:fff")
        {
            return DateTime.Now.ToString(fmt);
        }

        /// <summary>
        /// 現在時間か指定した時間（４ケタ）を形式指定して取得
        /// デフォルト：H:mm
        /// </summary>
        /// <param name="fmt">書式</param>
        /// <param name="time">時間</param>
        /// <returns>変換結果</returns>
        public static String GetNowTimeTypeString(String fmt = "Hmmss", String time = null)
        {
            String ret = "";
            DateTime dt;
            if (String.IsNullOrEmpty(time))
            {
                dt = DateTime.Now;
                ret = dt.ToString(fmt);
            }
            else
            {
                //４ケタのみ
                if (time.Length == 4)
                {
                    ret = time.Substring(0, 2) + ":" + time.Substring(2, 2);
                    string strTime = "2015/01/01 " + ret;   // チェックする日付を設定

                    // DateTimeに変換できるかチェック
                    if (DateTime.TryParse(strTime, out dt))
                    {
                        ret = dt.ToString(fmt);
                    }
                    else
                    {
                        OriginalException.TryParseException("GetNowTimeTypeString:time=", time);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 日付(yyyyMMdd or yyyy/MM/dd)　文字型→日付け型に変換 変換不可時はデフォ：NULL or 本日日付
        /// </summary>
        /// <param name="strDate">日付文字列</param>
        /// <param name="retNow">返却フラグ：trueの場合、変換不可の場合、現在時刻を返却</param>
        /// <returns>変換結果</returns>
        public static DateTime? ConvertDateStringToDateTimeQ(String strDate = null, Boolean retNow = false)
        {
            DateTime dt;

            //８ケタ　ハイフンなしとみなす
            if (strDate.Length == 8)
            {
                strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2);
            }

            // DateTimeに変換できるかチェック
            if (DateTime.TryParse(strDate, out dt))
            {

                return dt;

            }
            else
            {
                if (retNow)
                {
                    return DateTime.Now;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 本日日付か指定した日付を形式指定して取得
        /// デフォルト：YYYYMMDD
        /// </summary>
        /// <param name="fmt">書式</param>
        /// <param name="strDate">日付文字列</param>
        /// <param name="retEmptyJudge">空返却フラグ：TRUEの場合、文字列が空の場合はNULLで返す</param>
        /// <returns>変換結果</returns>
        public static String GetNowDateTypeString(String fmt = "yyyyMMdd", String strDate = null, Boolean retEmptyJudge = false)
        {
            String ret = "";
            DateTime dt;
            if (String.IsNullOrEmpty(strDate))
            {
                //空返却がTRUEの場合に空の場合はNULLで返す
                if (retEmptyJudge)
                {
                    return null;
                }

                dt = DateTime.Now;
                ret = dt.ToString(fmt);
            }
            else
            {
                //８ケタ　ハイフンなしとみなす
                if (strDate.Length == 8)
                {
                    strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2);
                }

                // DateTimeに変換できるかチェック
                if (DateTime.TryParse(strDate, out dt))
                {
                    ret = dt.ToString(fmt);
                }
            }

            return ret;
        }

        /// <summary>
        /// ILIST → DataTable変換
        /// </summary>
        /// <typeparam name="T">リストクラス</typeparam>
        /// <param name="list">変換元リスト</param>
        /// <returns>変換結果</returns>
        public static DataTable ConvertToDataTable<T>(T list) where T : IList
        {
            var table = new DataTable(typeof(T).GetGenericArguments()[0].Name);
            typeof(T).GetGenericArguments()[0].GetProperties().
                ToList().ForEach(p => table.Columns.Add(p.Name, p.PropertyType));
            foreach (var item in list)
            {
                var row = table.NewRow();
                item.GetType().GetProperties().
                    ToList().ForEach(p => row[p.Name] = p.GetValue(item, null));
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// DataTable → ILIST変換
        /// </summary>
        /// <typeparam name="T">変換用クラス</typeparam>
        /// <param name="table">変換元データテーブル</param>
        /// <returns>変換結果</returns>
        public static T ConvertToList<T>(DataTable table) where T : IList, new()
        {
            var list = new T();
            foreach (DataRow row in table.Rows)
            {
                var item = Activator.CreateInstance(typeof(T).GetGenericArguments()[0]);
                list.GetType().GetGenericArguments()[0].GetProperties().ToList().
                    ForEach(p => p.SetValue(item, row[p.Name], null));
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// 数字のチェック
        /// </summary>
        /// <param name="values">数値文字列</param>
        /// <returns>判定結果</returns>
        public static Boolean IsNumeric(String values)
        {
            if (string.IsNullOrEmpty(values))
            {
                return true;
            }
            Boolean flag = true;
            foreach (char c in values)
            {
                //数字以外の文字が含まれているか調べる
                if (c < '0' || '9' < c)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// intの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static int IntTryparse(Object value, int ret = 0)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!int.TryParse(str, System.Globalization.NumberStyles.AllowThousands, null, out ret))
            {
                OriginalException.TryParseException("IntTryparse:value=", value);
            }
            return ret;
        }

        /// <summary>
        /// int?の変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static int? IntQTryparse(Object value, int? ret = null)
        {
            if (value == null || String.IsNullOrWhiteSpace(StringTryparse(value)))
            {
                return null;
            }
            var str = value.ToString();
            if (!int.TryParse(str, System.Globalization.NumberStyles.AllowThousands, null, out int retValue))
            {
                OriginalException.TryParseException("IntQTryparse:value=", value);
            }
            return retValue;
        }

        /// <summary>
        /// longの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static long LongTryparse(Object value, long ret = 0)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!long.TryParse(str, System.Globalization.NumberStyles.AllowThousands, null, out ret))
            {
                OriginalException.TryParseException("LongTryparse:value=", value);
            }
            return ret;
        }

        /// <summary>
        /// longQの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static long? LongQTryparse(Object value, long? ret = null)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!long.TryParse(str, System.Globalization.NumberStyles.AllowThousands, null, out long retValue))
            {
                OriginalException.TryParseException("LongQTryparse:value=", value);
            }
            return retValue;
        }

        /// <summary>
        /// floatの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static float FloatTryparse(Object value, float ret = 0)
        {
            if (value == null)
            {
                return ret;
            }

            var str = value.ToString();
            if (!float.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint , null, out ret))
            {
                OriginalException.TryParseException("FloatTryparse:value=", value);
            }
            return ret;
        }

        /// <summary>
        /// floatQの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static float? FloatQTryparse(Object value, float? ret = null)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!float.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, null, out float retValue))
            {
                OriginalException.TryParseException("FloatQTryparse:value=", value);
            }
            return retValue;
        }

        /// <summary>
        /// Decimalの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static Decimal DecimalTryparse(Object value, Decimal ret = 0)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!Decimal.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, null, out ret))
            {
                OriginalException.TryParseException("DecimalTryparse:value=", value);
            }
            return ret;
        }

        /// <summary>
        /// Decimalの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static Decimal? DecimalQTryparse(Object value, Decimal? ret = null)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!Decimal.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, null, out Decimal outValue))
            {
                OriginalException.TryParseException("DecimalQTryparse:value=", value);
            }
            return outValue;
        }

        /// <summary>
        /// Doubleの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static Double DoubleTryparse(Object value, Double ret = 0)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!Double.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, null, out Double outValue))
            {
                OriginalException.TryParseException("DoubleTryparse:value=", value);
            }
            return outValue;
        }

        /// <summary>
        /// Doubleの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static Double? DoubleQTryparse(Object value, Double? ret = null)
        {
            if (value == null)
            {
                return ret;
            }
            var str = value.ToString();
            if (!Double.TryParse(str, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, null, out Double outValue))
            {
                OriginalException.TryParseException("DoubleQTryparse:value=", value);
            }
            return outValue;
        }

        /// <summary>
        /// Stringの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static String StringTryparse(Object value, String ret = null)
        {
            if (value != null)
            {
                ret = value.ToString();
            }
            return ret;
        }

        /// <summary>
        /// 大文字変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <returns>変換結果</returns>
        public static String ToUpper(Object value)
        {
            String val = StringTryparse(value);
            if (val != null)
            {
                return val.ToUpper();
            }
            return String.Empty;
        }

        /// <summary>
        /// Stringの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static String IntQStringTryparse(Object value, String ret = null)
        {
            int? val = IntQTryparse(value, null);
            if (val != null)
            {
                ret = val.ToString();
            }
            return ret;
        }

        /// <summary>
        /// Booleanの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static Boolean BooleanTryparse(Object value, Boolean ret = false)
        {
            Boolean outret = ret;
            if (value == null)
            {
                return ret;
            }
            if(!Boolean.TryParse(value.ToString(), out outret))
            {
                OriginalException.TryParseException("BooleanTryparse:value=", value);
            }
            return outret;
        }

        /// <summary>
        /// DateTimeの変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <returns>変換結果</returns>
        public static DateTime DateTimeTryparse(Object value)
        {
            DateTime ret = DateTime.Now;
            if (value != null)
            {
                if (!DateTime.TryParse(value.ToString(), out ret))
                {
                    OriginalException.TryParseException("DateTimeTryparse:value=", value);
                }
            }
            return ret;
        }

        /// <summary>
        /// DateTime?の変換
        /// </summary>
        /// <param name="value">変換元オブジェクト</param>
        /// <param name="ret">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static DateTime? DateTimeQTryparse(Object value, Boolean IsDate = false)
        {
            if (value == null)
            {
                return null;
            }
            DateTime ret;
            Boolean bret = DateTime.TryParse(value.ToString(), out ret);
            //変換できなかった場合はnullを返却
            if (!bret)
            {
                return null;
            }
            if (IsDate)
            {
                return ret.Date;
            }
            return ret;
        }

        /// <summary>
        /// 全角チェック
        /// </summary>
        /// <param name="str">チェック対象文字列</param>
        /// <param name="Encode">エンコード</param>
        /// <returns>チェック結果</returns>
        public static Boolean IsZenkaku(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int num = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(str);
            return num == str.Length * 2;
        }

        /// <summary>
        /// 半角チェック
        /// </summary>
        /// <param name="str">チェック対象文字列</param>
        /// <param name="Encode">エンコード</param>
        /// <returns>チェック結果</returns>
        public static bool IsHankaku(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int num = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(str);
            return num == str.Length;
        }

        /// <summary>
        /// 指定位置から指定されたバイト数分の文字列を取得
        /// </summary>
        /// <param name="sData">文字列</param>
        /// <param name="nStart">取得を開始する文字数(VBと同じ仕様)</param>
        /// <param name="nLen">バイト数</param>
        /// <returns>取得した文字列</returns>
        /// <remarks></remarks>
        public static string MidB(string sData, int nStart, int nLen, String Encode = "Shift_JIS")
        {
            Encoding oSJisEncoding = Encoding.GetEncoding(Encode);
            byte[] nByteAry = oSJisEncoding.GetBytes(sData);

            // 開始が最大文字数より後ろだった場合、空文字を戻す
            if (nByteAry.Length < nStart)
            {
                return "";
            }

            // nLenが最大文字数を超えないように調整
            if (nByteAry.Length < (nStart - 1) + nLen)
            {
                nLen = nByteAry.Length - (nStart - 1);
            }

            // 指定バイト数取りだし
            string sMidStr = oSJisEncoding.GetString(nByteAry, nStart - 1, nLen);

            // 最初の文字が全角の途中で切れていた場合はカット
            string sLeft = oSJisEncoding.GetString(nByteAry, 0, nStart);
            char sFirstMoji = sData[sLeft.Length - 1];
            if (sMidStr != "" && sFirstMoji != sMidStr[0])
            {
                sMidStr = sMidStr.Substring(1);
            }

            // 最後の文字が全角の途中で切れていた場合はカット
            sLeft = oSJisEncoding.GetString(nByteAry, 0, (nStart - 1) + nLen);
            char sLastMoji = sData[sLeft.Length - 1];
            if (sMidStr != "" && sLastMoji != sMidStr[sMidStr.Length - 1])
            {
                sMidStr = sMidStr.Substring(0, sMidStr.Length - 1);
            }

            return sMidStr;
        }

        /// <summary>
        /// 文字列がふくまれているかどうかをチェックする。
        /// </summary>
        /// <param name="target">チェック対象文字列</param>
        /// <param name="word">検索文字列</param>
        /// <returns>チェック結果</returns>
        public static Boolean HasString(string target, string word)
        {
            if (word == "")
                return false;
            if (target.IndexOf(word) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 時間切り捨て日付
        /// </summary>
        /// <param name="value">変換元日付</param>
        /// <returns>変換結果</returns>
        public static DateTime DateTimeTruncateTime(DateTime? value)
        {
            DateTime WkDate = GenericUtil.ConvertDateTimeQtoDateTime(value);
            return new DateTime(WkDate.Year, WkDate.Month, WkDate.Day);
        }

        /// <summary>
        /// 年月(文字列)変換
        /// </summary>
        /// <param name="values">変換元年月日</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertNengetu(DateTime? values, String fmt = "yyyy/MM")
        {
            if (values == null)
            {
                return String.Empty;
            }

            // ★★★ 文字列に変換後、DateTimeに変換し、DateTimeを返す
            //DateTime ret = ConvertDateTimeQtoDateTime(values);
            var strDate = ConvertDateTimeQtoString(values);
            var ret = (DateTime)ConvertDateStringToDateTimeQ(strDate);

            return ret.ToString(fmt);
        }

        /// <summary>
        /// 年月度（西暦）
        /// </summary>
        /// <param name="values">変換年月日</param>
        /// <param name="fmt">書式</param>
        /// <returns></returns>
        public static String ConvertSeirekiNengetu(DateTime values, String fmt = "yyyy年M月")
        {
            return values.ToString(fmt);
        }

        /// <summary>
        /// 年月度（和暦）
        /// </summary>
        /// <param name="values">変換元年月日</param>
        /// <param name="fmt">書式</param>
        /// <returns></returns>
        public static String ConvertWarekiNengetu(DateTime values, String fmt = "gy年M月")
        {
            return ConvertWareki(values, fmt);
        }

        /// <summary>
        /// 指定日付の月末取得(nullならばシステム日付)
        /// </summary>
        /// <param name="pmYmd">指定日付</param>
        /// <returns>月末日付</returns>
        public static DateTime GetEndOfTheMonth(DateTime? pmYmd)
        {
            var ymd = DateTimeTryparse(pmYmd);
            return new DateTime(ymd.Year, ymd.Month, DateTime.DaysInMonth(ymd.Year, ymd.Month));
        }

        /// <summary>
        /// 指定日付の月末取得
        /// </summary>
        /// <param name="ymd">指定日</param>
        /// <returns>月末日付</returns>
        public static DateTime GetEndOfTheMonth(DateTime ymd)
        {
            return new DateTime(ymd.Year, ymd.Month, DateTime.DaysInMonth(ymd.Year, ymd.Month));
        }

        /// <summary>
        /// 指定日付の月初取得(nullならばシステム日付)
        /// </summary>
        /// <param name="ymd">指定日</param>
        /// <returns>月初日付</returns>
        public static DateTime GetStartOfTheMonth(DateTime? pmYmd)
        {
            var ymd = DateTimeTryparse(pmYmd);
            return new DateTime(ymd.Year, ymd.Month, 1);
        }

        /// <summary>
        /// 指定日付の月初取得
        /// </summary>
        /// <param name="ymd">指定日</param>
        /// <returns>月初日付</returns>
        public static DateTime GetStartOfTheMonth(DateTime ymd)
        {
            return new DateTime(ymd.Year, ymd.Month, 1);
        }

        /// <summary>
        /// DateTimeQ型データを日付のみに変換（時間はゼロ）
        /// ※MutiRowの日付型セルの変換時などで使う
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <returns>変換結果</returns>
        public static DateTime? ConvertDateTimeQtoDateTimeQNoTime(DateTime? values)
        {
            if (values.HasValue)
            {
                return ((DateTime)values).Date;
            }
            return null;
        }

        /// <summary>
        /// DateTimeQ型データをDateTime型に変換
        /// 
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <returns>変換結果</returns>
        public static DateTime ConvertDateTimeQtoDateTime(DateTime? values)
        {
            if (values != null)
            {
                return (DateTime)values;
            }
            return new DateTime(1900, 01, 01);
        }

        /// <summary>
        /// 有効な方の日付を返す
        /// </summary>
        /// <param name="dateA">日付A</param>
        /// <param name="dateB">日付B</param>
        /// <returns>変換結果</returns>
        public static DateTime ConvertDateTimeAorB(DateTime? dateA, DateTime? dateB)
        {
            if (dateA.HasValue)
            {
                return (DateTime)dateA;
            }
            return (DateTime)dateB;
        }

        /// <summary>
        /// DateTimeQ型データをString型に変換
        /// 
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertDateTimeQtoString(DateTime? values, String fmt = "yyyy/MM/dd")
        {
            if (values != null)
            {
                return ConvertShortDateString(values, fmt);
            }

            return String.Empty;
        }

        /// <summary>
        /// 日付型データを文字列の和暦日付に変換する
        /// 
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String _ConvertWareki(DateTime? values, String fmt = "gy年M月d日")
        {
            //和暦でDataTimeを文字列に変換する
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);

            String ret = "";
            if (values != null)
            {
                ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                DateTime dt = (DateTime)values;
                ret = dt.ToString(fmt, ci);

            }
            return ret;
        }

        /// <summary>
        /// 日付型データを文字列の和暦日付に変換する
        /// 
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertWareki(DateTime? values, String fmt = "yy年MM月dd日")
        {
            //和暦でDataTimeを文字列に変換する
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);

            String ret = "";
            if (values != null)
            {
                ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                DateTime dt = (DateTime)values;

                int year = GenericUtil.IntTryparse(dt.ToString("yy", ci));
                int month = dt.Month;
                int day = dt.Day;

                String StrYear = String.Format("{0,2}", year);
                String StrMonth = String.Format("{0,2}", month);
                String StrDay = String.Format("{0,2}", day);

                //y、M、dでなくyy、MM、ddかもしれんので揃える。gは一旦消す
                String wkfmt = fmt.Replace("g", "").Replace("yy", "y").Replace("MM", "M").Replace("dd", "d");

                String wkdate = wkfmt.Replace("y", StrYear).Replace("M", StrMonth).Replace("d", StrDay);
                String gn = dt.ToString("gg", ci);

                ret = gn + wkdate;
            }
            return ret;
        }

        /// <summary>
        /// 日付型データを文字列の和暦日付に変換する
        /// 
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertWarekiYYMDD(DateTime? values, String fmt = "yy年MM月dd日")
        {
            //和暦でDataTimeを文字列に変換する
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);

            String ret = "";
            if (values != null)
            {
                ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                DateTime dt = (DateTime)values;

                int year = GenericUtil.IntTryparse(dt.ToString("yy", ci));
                int month = dt.Month;
                int day = dt.Day;

                String StrYear = year.ToString("00");
                String StrMonth = month.ToString("00");
                String StrDay = day.ToString("00");

                ret = StrYear + StrMonth + StrDay;
            }
            return ret;
        }

        /// <summary>
        /// 日付型データを文字列の元号記号年月日に変換
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertWarekiWithGengoKigou(DateTime? values, String fmt = "gy.M.d")
        {

            //和暦でDataTimeを文字列に変換する
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);

            String ret = "";
            if (values != null)
            {
                //ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                DateTime dt = (DateTime)values;
                //ret = dt.ToString(fmt, ci);

                ret = ConvertWareki(values, fmt);

                var formatInfo = default(System.Globalization.DateTimeFormatInfo);
                var culture = new System.Globalization.CultureInfo("ja-JP");
                var calendar = new System.Globalization.JapaneseCalendar();
                formatInfo = culture.DateTimeFormat;
                formatInfo.Calendar = calendar;
                String eraKigoName = "";
                int era = formatInfo.Calendar.GetEra(dt);

                string eraString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                for (int index = 0; index < eraString.Length; index++)
                {
                    if (formatInfo.GetEra(eraString[index].ToString()) == era)
                    {
                        eraKigoName = eraString[index].ToString();
                    }
                }

                //元号部分を元号記号に置換えして返却
                return ret.Replace(formatInfo.GetEraName(era), eraKigoName);
            }
            return "";
        }

        /// <summary>
        /// 日付型データを文字列の元号記号年月日に変換
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String _ConvertWarekiWithGengoKigou(DateTime? values, String fmt = "gy.M.d")
        {

            //和暦でDataTimeを文字列に変換する
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);

            String ret = "";
            if (values != null)
            {
                ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                DateTime dt = (DateTime)values;
                ret = dt.ToString(fmt, ci);

                var formatInfo = default(System.Globalization.DateTimeFormatInfo);
                var culture = new System.Globalization.CultureInfo("ja-JP");
                var calendar = new System.Globalization.JapaneseCalendar();
                formatInfo = culture.DateTimeFormat;
                formatInfo.Calendar = calendar;
                String eraKigoName = "";
                int era = formatInfo.Calendar.GetEra(dt);

                string eraString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                for (int index = 0; index < eraString.Length; index++)
                {
                    if (formatInfo.GetEra(eraString[index].ToString()) == era)
                    {
                        eraKigoName = eraString[index].ToString();
                    }
                }

                //元号部分を元号記号に置換えして返却
                return ret.Replace(formatInfo.GetEraName(era), eraKigoName);
            }
            return "";
        }

        /// <summary>
        /// 日付型？をフォーマット指定した日付の文字列に変換
        /// デフォルト：YYYY/MM/DD
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <returns>変換結果</returns>
        public static String ConvertShortDateStringWithZero(DateTime? values, String fmt = "yyyy/M/d")
        {
            String ret = "";
            if (values != null)
            {
                DateTime dt = (DateTime)values;
                ret = dt.ToString(fmt);
            }
            return ret;
        }

        /// <summary>
        /// 日付型？をフォーマット指定した日付の文字列に変換
        /// デフォルト：YYYY/MM/DD
        /// </summary>
        /// <param name="values">変換元日付</param>
        /// <param name="fmt">書式</param>
        /// <param name="Zerosupless">ゼロサプレスフラグ</param>
        /// <returns>変換結果</returns>
        public static String ConvertShortDateString(DateTime? values, String fmt = "yyyy/MM/dd", Boolean Zerosupless = false)
        {
            String ret = "";
            if (values != null)
            {
                DateTime dt = (DateTime)values;

                int year = dt.Year;
                int month = dt.Month;
                int day = dt.Day;

                int hh = dt.Hour;
                int mm = dt.Minute;
                int ss = dt.Second;

                String StrYear = String.Format("{0,4}", year);
                String StrMonth = String.Format("{0,2}", month);
                String StrDay = String.Format("{0,2}", day);

                String StrHH = String.Format("{0,2}", hh);
                String StrMM = String.Format("{0,2}", mm);
                String StrSS = String.Format("{0,2}", ss);

                //String wkdate = null;
                String wkfmt = fmt;
                if (String.IsNullOrWhiteSpace(wkfmt) && Zerosupless)
                {
                    //M、dでなくMM、ddかもしれんので揃える。
                    wkfmt = fmt.Replace("MM", "M").Replace("dd", "d").Replace("HH", "H").Replace("mm", "m").Replace("ss", "s");
                }

                ret = dt.ToString(wkfmt);
                //wkdate = wkfmt.Replace("yyyy", StrYear).Replace("M", StrMonth).Replace("d", StrDay).Replace("H", StrHH).Replace("m", StrMM).Replace("s", StrSS);

                //ret = wkdate;

            }
            return ret;
        }

        /// <summary>
        /// 文字列型日付を日付型Qに変換
        /// 入力形式デフォルト：yyyyMMdd
        /// 参考："yyyy/MM/dd HH:mm:ss"　"yyyyMMddHHmmss"
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <param name="inputfmt">入力書式</param>
        /// <returns>変換結果</returns>
        public static DateTime? ConvertStringToDateTimeQ(String values, String inputfmt = "yyyyMMdd")
        {
            if (String.IsNullOrEmpty(values))
            {
                return null;
            }

            DateTime dt;

            //日付形式に変換
            if (!DateTime.TryParseExact(values, inputfmt, null, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out dt))
            {
                return null;
            }
            else
            {
                return dt;
            }
        }


        /// <summary>
        /// 表示用金額の書式（３ケタ区切り）に変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <param name="def">デフォルト値</param>
        /// <param name="NotZero">ゼロ許可</param>
        /// <returns>変換結果</returns>
        public static String StringMoneyDispFormat(object values, String def = "0", Boolean NotZero = false)
        {
            String ret = def;
            if (values != null)
            {
                ret = String.Format("{0:#,0}", DecimalTryparse(values));
            }

            if (NotZero)
            {
                if (DecimalTryparse(values) == 0)
                {
                    ret = "";
                }
            }

            return ret;
        }

        /// <summary>
        /// string型→Decimal型へ変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <returns>変換結果</returns>
        public static Decimal ConvertStringToDecimal(string values)
        {
            Decimal ret = 0;
            if (String.IsNullOrEmpty(values))
            {
                return ret;
            }
            if (!Decimal.TryParse(values, out ret))
            {
                OriginalException.TryParseException("ConvertStringToDecimalQ:value=", values);
            };
            return ret;
        }

        /// <summary>
        /// string型→DecimalQ型へ変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <returns>変換結果</returns>
        public static Decimal? ConvertStringToDecimalQ(string values)
        {
            Decimal ret = 0;
            if (String.IsNullOrEmpty(values))
            {
                return 0;
            }
            if (!Decimal.TryParse(values, out ret))
            {
                OriginalException.TryParseException("ConvertStringToDecimalQ:value=", values);
            }
            return ret;
        }

        /// <summary>
        /// string変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <param name="opv">nullの場合の返却値</param>
        /// <returns>変換結果</returns>
        public static String ConvertString(String values, String opv = null)
        {
            if (String.IsNullOrEmpty(values))
            {
                values = opv;
            }
            return values;
        }

        /// <summary>
        /// string型→intQ型へ変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <param name="opv">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static int? ConvertStringToIntQ(string values, int? opv = null)
        {
            if (String.IsNullOrEmpty(values) || String.IsNullOrEmpty(values.Trim()))
            {
                return opv;
            }

            return int.Parse(values);
        }

        /// <summary>
        /// string型→int型へ変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <param name="opv">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static int ConvertStringToInt(string values, string opv = "0")
        {
            if (String.IsNullOrEmpty(values) || String.IsNullOrEmpty(values.Trim()))
            {
                values = opv;
            }

            return int.Parse(values);

        }

        /// <summary>
        /// string型→long型へ変換
        /// </summary>
        /// <param name="values">変換元文字列</param>
        /// <returns>変換結果</returns>
        public static long ConvertStringToLong(string values)
        {

            if (String.IsNullOrEmpty(values) || String.IsNullOrEmpty(values.Trim()))
            {
                values = "0";
            }

            return long.Parse(values);

        }

        /// <summary>
        /// int?型→intへ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static int ConvertIntQ_Toint(int? values)
        {
            int vl = 0;
            if (values != null)
            {
                vl = (int)values;
            }

            return vl;

        }

        /// <summary>
        /// Long?型→Long型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static long ConvertLongQ_ToLong(long? values)
        {
            long vl = 0;
            if (values != null)
            {
                vl = (long)values;
            }

            return vl;
        }

        /// <summary>
        /// Decimal?型→Int型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static int ConvertDecimalQToInt(Decimal? values)
        {
            int vl = 0;
            if (values != null)
            {
                vl = (int)values;
            }

            return vl;
        }

        /// <summary>
        /// Decimal?型→Long型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static long ConvertDecimalQToLong(Decimal? values)
        {
            long vl = 0;
            if (values != null)
            {
                vl = (long)values;
            }

            return vl;
        }

        /// <summary>
        /// Long型→Decimal型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static Decimal ConvertLongQToDecimal(long? values)
        {
            Decimal vl = 0;
            if (values != null)
            {
                vl = (Decimal)values;
            }

            return vl;
        }

        /// <summary>
        /// Long型→Double型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static double ConvertLongToDouble(long values)
        {
            double vl = 0;

            vl = (double)values;

            return vl;
        }

        /// <summary>
        /// LongQ型→String型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <param name="val">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static String ConvertLongQToString(long? values, String val = "0")
        {
            String vl = val;
            if (values != null)
            {
                vl = values.ToString();
            }

            return vl;
        }

        /// <summary>
        /// IntQ型→String型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <param name="val">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static String ConvertIntQToString(int? values, String val = "0")
        {
            String vl = val;
            if (values != null)
            {
                vl = values.ToString();
            }

            return vl;
        }
        /// <summary>
        /// Double型→Decimal型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static Decimal ConvertDoubleToDecimal(Double values)
        {
            Decimal vl = 0;

            vl = (Decimal)values;

            return vl;
        }

        /// <summary>
        /// Double型→long型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static long ConvertDoubleToLong(Double values)
        {
            long vl = 0;

            vl = (long)values;

            return vl;
        }

        /// <summary>
        /// Double型→int型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static int ConvertDoubleToInt(Double values)
        {
            int vl = 0;

            vl = (int)values;

            return vl;

        }

        /// <summary>
        /// プリミティブdecimal?型→String型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <param name="val">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static String ConvertDecimalQToString(decimal? values, String val = "0")
        {

            String vl = val;
            if (values != null)
            {
                vl = values.ToString();
            }

            return vl;
        }

        /// <summary>
        /// プリミティブdecimal?型→Decimal型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static Decimal ConvertDecimalQToDecimal(decimal? values)
        {

            Decimal vl = 0;
            if (values != null)
            {
                vl = (Decimal)values;
            }

            return vl;
        }

        /// <summary>
        /// プリミティブDouble?型→Double型へ変換
        /// </summary>
        /// <param name="values">変換元数値</param>
        /// <returns>変換結果</returns>
        public static Double ConvertDoubleQToDouble(double? values)
        {

            Double vl = 0;
            if (values != null)
            {
                vl = (Double)values;
            }

            return vl;
        }

        /// ------------------------------------------------------------------------
        /// <summary>
        ///     指定した精度の数値に切り上げします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り上げられた数値。</returns>
        /// ------------------------------------------------------------------------
        public static double ToRoundUp(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Ceiling(dValue * dCoef) / dCoef :
                                System.Math.Floor(dValue * dCoef) / dCoef;
        }

        /// ------------------------------------------------------------------------
        /// <summary>
        ///     指定した精度の数値に切り捨てします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        /// ------------------------------------------------------------------------
        public static double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Floor(dValue * dCoef) / dCoef :
                                System.Math.Ceiling(dValue * dCoef) / dCoef;

        }

        /// ------------------------------------------------------------------------
        /// <summary>
        ///     指定した精度の数値に四捨五入します。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に四捨五入された数値。</returns>
        /// ------------------------------------------------------------------------
        public static double ToHalfAdjust(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Floor((dValue * dCoef) + 0.5) / dCoef :
                                System.Math.Ceiling((dValue * dCoef) - 0.5) / dCoef;
        }

        /// <summary>
        /// 端数処理考慮　消費税計算
        /// </summary>
        /// <param name="Kingaku"></param>
        /// <param name="TaxRate"></param>
        /// <param name="SyoriCode"></param>
        /// <param name="iDigits"></param>
        /// <returns></returns>
        public static Decimal CalcTax(Decimal? Kingaku, Decimal? TaxRate, int? SyoriCode, int iDigits = 0)
        {

            Decimal WkKingaku = GenericUtil.DecimalTryparse(Kingaku);
            Decimal WkTaxRate = GenericUtil.DecimalTryparse(TaxRate);

            //CDef.EN_HSU_KBN.切り上げ
            if (3 == SyoriCode)
            {
                return GenericUtil.DecimalTryparse(GenericUtil.ToRoundUp(GenericUtil.DoubleTryparse(WkKingaku * WkTaxRate), iDigits));
            }
            //CDef.EN_HSU_KBN.切り捨て
            if (2 == SyoriCode)
            {
                return GenericUtil.DecimalTryparse(GenericUtil.ToRoundDown(GenericUtil.DoubleTryparse(WkKingaku * WkTaxRate), iDigits));
            }
            //CDef.EN_HSU_KBN.四捨五入
            if (1 == SyoriCode)
            {
                return GenericUtil.DecimalTryparse(GenericUtil.ToHalfAdjust(GenericUtil.DoubleTryparse(WkKingaku * WkTaxRate), iDigits));
            }

            return 0;
        }

        /// <summary>
        /// 指定したファイルを上書き移動
        /// </summary>
        /// <param name="stFilePath"></param>
        public static void MoveFile(string stFilePath, String MovePath)
        {

            if (stFilePath == null)
            {
                return;
            }

            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // ファイルをコピーする
                cFileInfo.MoveTo(MovePath, true);
            }
        }

        /// <summary>
        /// 指定したファイルを上書きコピー
        /// </summary>
        /// <param name="stFilePath"></param>
        public static void CopyFile(string stFilePath, String CopyPath)
        {

            if (stFilePath == null)
            {
                return;
            }

            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // ファイルをコピーする
                cFileInfo.CopyTo(CopyPath, true);
            }
        }

        /// <summary>
        /// 指定したファイルの存在チェック
        /// </summary>
        /// <param name="stFilePath"></param>
        public static Boolean IsExistsFile(string stFilePath)
        {
            if (stFilePath == null)
            {
                return false;
            }

            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            return cFileInfo.Exists;
        }

        /// <summary>
        /// 指定したファイルを削除
        /// </summary>
        /// <param name="stFilePath"></param>
        public static void DeleteFile(string stFilePath)
        {

            if (stFilePath == null)
            {
                return;
            }

            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // 読み取り専用属性がある場合は、読み取り専用属性を解除する
                if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                }

                // ファイルを削除する
                cFileInfo.Delete();
            }
        }

        /// <summary>
        /// フォルダ内のファイルを全て削除
        /// </summary>
        /// <param name="orgFolder"></param>
        /// <param name="RenameFolder"></param>
        public static void DeleteFileInFolder(String TagetFolder, string searchPattern = "*.*")
        {
            if (!System.IO.Directory.Exists(TagetFolder))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(TagetFolder);
            FileInfo[] files = di.GetFiles(searchPattern);
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }


        /// <summary>
        /// フォルダ作成
        /// </summary>
        /// <param name="Location"></param>
        public static void CreateForder(String Location)
        {
            if (!System.IO.Directory.Exists(Location))
            {
                System.IO.Directory.CreateDirectory(Location);
            }
        }

        /// <summary>
        /// テキストファイル読み込み
        /// </summary>
        /// <param name="fileNm"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String TextFileReader(String fileNm, string encoding = "Shift_JIS")
        {
            StreamReader sr = null;
            //存在しない場合
            if (!System.IO.File.Exists(fileNm))
            {
                return null;
            }

            sr = new StreamReader(
                    fileNm, Encoding.GetEncoding(encoding));

            string text = sr.ReadToEnd();

            return text;
        }

        /// <summary>
        /// Internet Explorerの新規Windowで開く
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static String IEWindowsOpenScript(String Url)
        {

            return "window.open('" + Url + "','_blank','scrollbars=no, status=no, toolbar=no, location=no');\n";
        }

        /// <summary>
        /// Internet Explorerの新規Windowで開く
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static String IEModelessWindowsOpenScript(String Url, String clientid = null)
        {
            String script = null;
            if (!String.IsNullOrEmpty(clientid))
            {

                script = "var txt = document.getElementById('<%=this." + clientid + ".ClientID%>'); txt=";
            }

            return script = script + "showModelessDialog('" + Url + "',window,'scroll:no;resizable:yes;status:false;dialogWidth:1010px;dialogHeight:670px;edge:sunken;');\n";

            //dialogWidth:pixels	-	表示するウィンドウの横幅
            //dialogHeight:pixels	-	表示するウィンドウの縦幅
            //dialogLeft:pixels	-	表示位置（X座標）
            //dialogTop:pixels	-	表示位置（Y座標）
            //center:{ yes | no | 1 | 0 | on | off }	yes	画面中央に表示／表示しない
            //dialogHide:{ yes | no | 1 | 0 | on | off }	no	印刷／印刷プレビュー時に表示／表示しない
            //edge:{ sunken | raised }	raised	ウィンドウ枠形状
            //help:{ yes | no | 1 | 0 | on | off }	yes	Helpアイコン表示
            //resizable:{ yes | no | 1 | 0 | on | off }	no	リサイズ可能／不可能
            //scroll:{ yes | no | 1 | 0 | on | off }	yes	スクロールする／しない
            //status:{ yes | no | 1 | 0 | on | off }	no	ステータスバーを表示／表示しない
            //unadorned:{ yes | no | 1 | 0 | on | off }	no	外観指定
        }


        /// <summary>
        /// システム日付から指定日数を加算した日付文字列を返却
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static String NowDateFromCalcDay(int calcDay, String retFormat = "yyyy/MM/dd")
        {
            // 日付と時刻を格納するための変数を宣言する
            DateTime dtBirth = DateTime.Now;

            // 加算する
            dtBirth = dtBirth.AddDays(calcDay);

            return dtBirth.ToString(retFormat);
        }

        /// <summary>
        /// システム日付から指定日数を加算した日付を返却
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static DateTime NowDateFromCalcDay(int calcDay)
        {
            // 日付と時刻を格納するための変数を宣言する
            DateTime dtBirth = DateTime.Now;

            // 加算する
            dtBirth = dtBirth.AddDays(calcDay);

            return dtBirth;
        }

        /// <summary>
        /// システム日付から指定月数を加算した日付を返却
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static String NowDateFromCalcMonth(int calcMonth, String retFormat = "yyyy/MM/dd")
        {
            // 日付と時刻を格納するための変数を宣言する
            DateTime dtBirth = DateTime.Now;

            // 加算する
            dtBirth = dtBirth.AddMonths(calcMonth);

            return dtBirth.ToString(retFormat);
        }

        /// <summary>
        /// システム日付（もしくは指定日）から指定月数(calcMonth)を加算した日付を返却 
        /// ※optDay=>日の指定 0:そのまま 1:月初日 99:月末日
        /// </summary>
        /// <param name="calcMonth"></param>
        /// <param name="optDay"></param>
        /// <param name="nowDate"></param>
        /// <returns></returns>
        public static DateTime NowDateFromCalcMonth(int calcMonth = 0, int optDay = 0, DateTime? nowDate = null)
        {
            // 日付と時刻を格納するための変数を宣言する
            DateTime dtBirth = DateTime.Now;
            if (nowDate != null)
            {
                dtBirth = ConvertDateTimeQtoDateTime(nowDate);
            }

            // 加算する
            dtBirth = dtBirth.AddMonths(calcMonth);

            //そのまま
            if (optDay == 0)
            {
                //なにもしない
            }
            else
                //月初
                if (optDay == 1)
            {
                dtBirth = GetStartOfTheMonth(dtBirth);
            }
            else
                    //月末
                    if (optDay == 99)
            {
                dtBirth = GetEndOfTheMonth(dtBirth);
            }
            else
            {
                //指定日
                dtBirth = new DateTime(dtBirth.Year, dtBirth.Month, optDay);
            }

            return dtBirth;
        }

        /// <summary>
        /// 文字列切出し
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String CuttingChar(string text, int size, string encoding = "Shift_JIS")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            Encoding e = System.Text.Encoding.GetEncoding(encoding);
            return new String(text.TakeWhile((c, i) => e.GetByteCount(text.Substring(0, i + 1)) <= size).ToArray());
        }

        /// <summary>
        /// 対象文字列取得
        /// </summary>
        /// <param name="RecordString"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String GetRecordSubString(String RecordString, int startIndex, int length)
        {

            //取得可能な長さを算出（データが次行折り返しの場合の対処）
            int zanLen = RecordString.Length - startIndex;
            if (zanLen > length)
            {
                zanLen = length;
            }

            return RecordString.Substring(startIndex, zanLen);
        }

        /// <summary>
        /// 西暦の1900年代分かれ目を判定
        /// </summary>
        /// <param name="nen"></param>
        /// <returns></returns>
        public static int Seireki1900Hantei(int nen)
        {
            if (50 <= nen)
            {
                nen = 1900 + nen;
            }
            else
            {
                nen = 2000 + nen;
            }
            return nen;
        }

        /// <summary>
        /// 2つの日付の月差を求める（絶対値）
        /// </summary>
        ///対象日付1
        ///対象日付2
        /// <returns></returns>
        public static int MonthDiff(DateTime dTime1, DateTime dTime2)
        {
            int iRet = 0;
            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MaxValue;

            if (dTime1 < dTime2)
            {
                dtFrom = dTime1;
                dtTo = dTime2;
            }
            else
            {
                dtFrom = dTime2;
                dtTo = dTime1;
            }

            // 月差計算（年差考慮(差分1年 → 12(ヶ月)加算)）
            iRet = (dtTo.Month + (dtTo.Year - dtFrom.Year) * 12) - dtFrom.Month;

            if (iRet < 0)
            {
                iRet = iRet * -1;
            }

            return iRet;
        }

        /// <summary>
        /// ファイルパターンに合致するファイル一覧を取得（ファイル名降順）
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public static FileInfo[] GetFileListOrderByDesc(String dirPath, String SearchPattern)
        {
            FileInfo[] files = null;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            files = di.GetFiles(SearchPattern);
            Array.Sort(files,
                delegate (FileInfo f1, FileInfo f2)
                {
                    // ファイル名で昇順
                    //return f1.Name.CompareTo(f2.Name);

                    // ファイル名で降順
                    //return -f1.Name.CompareTo(f2.Name);

                    // 書き込み時刻で昇順
                    // return f1.LastWriteTime.CompareTo(f2.LastWriteTime);

                    // 書き込み時刻で降順
                    return -f1.LastWriteTime.CompareTo(f2.LastWriteTime);

                });

            return files;
        }

        /// <summary>
        /// 最新ファイル情報取得
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public static FileInfo GetLatestFileInfo(String dirPath, String SearchPattern)
        {

            DirectoryInfo d = new DirectoryInfo(dirPath);

            FileInfo LatestFileInfo = null;

            String fileNm = Path.GetFileName(SearchPattern);

            //ディレクトリが存在するかをチェック
            if (Directory.Exists(dirPath))
            {

                FileInfo[] fileList = GetFileListOrderByDesc(dirPath, fileNm);
                if (fileList == null || fileList.Length == 0)
                {
                    //なにもしない
                }
                else
                {
                    return fileList[0];
                }

                foreach (DirectoryInfo sub_d in d.GetDirectories())
                {
                    //サブディレクトリも見るための再帰呼び出し
                    return GetLatestFileInfo(sub_d.FullName, fileNm);
                }

            }

            return LatestFileInfo;
        }

        /// ---------------------------------------------------------------------------------------
        /// <summary>
        ///     指定した検索パターンに一致するファイルを最下層まで検索しすべて返します。</summary>
        /// <param name="stRootPath">
        ///     検索を開始する最上層のディレクトリへのパス。</param>
        /// <param name="stPattern">
        ///     パス内のファイル名と対応させる検索文字列。</param>
        /// <returns>
        ///     検索パターンに一致したすべてのファイルパス。</returns>
        /// ---------------------------------------------------------------------------------------
        public static string[] GetFilesMostDeep(string stRootPath, string stPattern)
        {
            System.Collections.Specialized.StringCollection hStringCollection = (
                new System.Collections.Specialized.StringCollection()
            );

            // このディレクトリ内のすべてのファイルを検索する
            foreach (string stFilePath in System.IO.Directory.GetFiles(stRootPath, stPattern))
            {
                hStringCollection.Add(stFilePath);
            }

            // このディレクトリ内のすべてのサブディレクトリを検索する (再帰)
            foreach (string stDirPath in System.IO.Directory.GetDirectories(stRootPath))
            {
                string[] stFilePathes = GetFilesMostDeep(stDirPath, stPattern);

                // 条件に合致したファイルがあった場合は、ArrayList に加える
                if (stFilePathes != null)
                {
                    hStringCollection.AddRange(stFilePathes);
                }
            }

            // StringCollection を 1 次元の String 配列にして返す
            string[] stReturns = new string[hStringCollection.Count];
            hStringCollection.CopyTo(stReturns, 0);

            return stReturns;
        }

        /// <summary>
        /// 空白文字除去
        /// </summary>
        /// <param name="RecordString"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String RemoveStringSpace(String val)
        {
            return val.Trim().Replace(" ", "").Replace("　", "");
        }

        /// <summary>
        /// 半角カナの小文字（ｧ|ｨ|ｩ|ｪ|ｫ|ｯ|ｬ|ｭ|ｮ）を半角カナの大文字（ｱ|ｲ|ｳ|ｴ|ｵ|ﾂ|ﾔ|ﾕ|ﾖ）に変換します。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceHankakuKanaKomojiToOmoji(string input)
        {
            if (input == null)
            {
                return input;
            }
            return Regex.Replace(input, @"ｧ|ｨ|ｩ|ｪ|ｫ|ｯ|ｬ|ｭ|ｮ", new MatchEvaluator(CapText));
        }

        /// <summary>
        /// ReplaceHankakuKanaKomojiToOmojiで入力値が半角カナの小文字と一致したときに発生するイベントです。
        /// 半角カナの小文字を半角カナの大文字に変換します。
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string CapText(Match match)
        {
            switch (match.Value)
            {
                case "ｧ":
                    return "ｱ";
                case "ｨ":
                    return "ｲ";
                case "ｩ":
                    return "ｳ";
                case "ｪ":
                    return "ｴ";
                case "ｫ":
                    return "ｵ";
                case "ｯ":
                    return "ﾂ";
                case "ｬ":
                    return "ﾔ";
                case "ｭ":
                    return "ﾕ";
                case "ｮ":
                    return "ﾖ";
            }
            return match.Value;
        }

        /// <summary>
        /// ひらがなをカタカナに変換
        /// </summary>
        public static string Convertkatakana(string s)
        {
            StringBuilder sb = new StringBuilder();
            char[] target = s.ToCharArray();
            char c;
            for (int i = 0; i < target.Length; i++)
            {
                c = target[i];
                if (c >= 'ぁ' && c <= 'ん')
                { //-> カタカナの範囲
                    c = (char)(c - 'ぁ' + 'ァ');  //-> 変換
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 全角カタカナを半角カタカナに変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ZenkakuKatakanaToHankakuKatakana(String s)
        {
            StringBuilder sb = new StringBuilder();
            char[] target = s.ToCharArray();
            char c;
            for (int i = 0; i < target.Length; i++)
            {
                c = target[i];
                String convertedChar = zenkakuKatakanaToHankakuKatakana(c);
                sb.Append(convertedChar);
            }
            return sb.ToString();

        }
        public static String zenkakuKatakanaToHankakuKatakana(char c)
        {
            if (c >= ZENKAKU_KATAKANA_FIRST_CHAR && c <= ZENKAKU_KATAKANA_LAST_CHAR)
            {
                return HANKAKU_KATAKANA[c - ZENKAKU_KATAKANA_FIRST_CHAR];
            }
            else
            {
                return c.ToString();
            }
        }
        private static char[] ZENKAKU_KATAKANA = {
                'ァ', 'ア', 'ィ', 'イ', 'ゥ','ウ', 'ェ', 'エ', 'ォ', 'オ',
                'カ', 'ガ', 'キ', 'ギ', 'ク', 'グ', 'ケ', 'ゲ', 'コ', 'ゴ',
                'サ', 'ザ', 'シ', 'ジ', 'ス', 'ズ', 'セ', 'ゼ', 'ソ', 'ゾ',
                'タ', 'ダ', 'チ', 'ヂ', 'ッ', 'ツ', 'ヅ', 'テ', 'デ', 'ト', 'ド',
                'ナ', 'ニ', 'ヌ','ネ', 'ノ',
                'ハ', 'バ', 'パ', 'ヒ', 'ビ', 'ピ', 'フ', 'ブ', 'プ', 'ヘ', 'ベ','ペ', 'ホ', 'ボ', 'ポ',
                'マ', 'ミ', 'ム', 'メ', 'モ',
                'ャ', 'ヤ', 'ュ', 'ユ','ョ', 'ヨ',
                'ラ', 'リ', 'ル', 'レ', 'ロ',
                'ヮ', 'ワ', 'ヰ', 'ヱ', 'ヲ', 'ン', 'ヴ', 'ヵ', 'ヶ' };
        private static String[] HANKAKU_KATAKANA = {
                "ｧ", "ｱ", "ｨ", "ｲ", "ｩ","ｳ", "ｪ", "ｴ", "ｫ", "ｵ",
                "ｶ", "ｶﾞ", "ｷ", "ｷﾞ", "ｸ", "ｸﾞ", "ｹ","ｹﾞ", "ｺ", "ｺﾞ",
                "ｻ", "ｻﾞ", "ｼ", "ｼﾞ", "ｽ", "ｽﾞ", "ｾ", "ｾﾞ", "ｿ","ｿﾞ",
                "ﾀ", "ﾀﾞ", "ﾁ", "ﾁﾞ", "ｯ", "ﾂ", "ﾂﾞ", "ﾃ", "ﾃﾞ", "ﾄ", "ﾄﾞ",
                "ﾅ", "ﾆ", "ﾇ", "ﾈ", "ﾉ",
                "ﾊ", "ﾊﾞ", "ﾊﾟ", "ﾋ", "ﾋﾞ", "ﾋﾟ", "ﾌ","ﾌﾞ", "ﾌﾟ", "ﾍ", "ﾍﾞ", "ﾍﾟ", "ﾎ", "ﾎﾞ", "ﾎﾟ",
                "ﾏ", "ﾐ", "ﾑ", "ﾒ", "ﾓ",
                "ｬ", "ﾔ", "ｭ", "ﾕ", "ｮ", "ﾖ",
                "ﾗ", "ﾘ", "ﾙ", "ﾚ", "ﾛ",
                "ﾜ","ﾜ","ｲ", "ｴ", "ｦ", "ﾝ", "ｳﾞ", "ｶ", "ｹ" };
        private static char ZENKAKU_KATAKANA_FIRST_CHAR = ZENKAKU_KATAKANA[0];
        private static char ZENKAKU_KATAKANA_LAST_CHAR = ZENKAKU_KATAKANA[ZENKAKU_KATAKANA.Length - 1];

        /// <summary>
        /// 半角カタカナを全角カタカナに変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HankakuKatakanaToZenkakuKatakana(string s)
        {
            Regex re = new Regex(@"[ｦ-ﾝ]ﾞ|[ｦ-ﾝ]ﾟ|[ｦ-ﾝ]");
            string output = re.Replace(s, match =>
            {//Matchした場合の処理
                if (dictKatakana.ContainsKey(match.Value))
                {//辞書にあれば辞書にある値を返す
                    return dictKatakana[match.Value];
                }
                else
                {//辞書になければ変換せずに返す
                    return match.Value;
                }
            });
            return output;
        }
        private static Dictionary<string, string> dictKatakana = new Dictionary<string, string>()
        {
            {"ｱ","ア"},{"ｨ","ィ"},{"ｲ","イ"},{"ｩ","ゥ"},{"ｳ","ウ"},{"ｪ","ェ"},{"ｴ","エ"},{"ｫ","ォ"},{"ｵ","オ"},
            {"ｶ","カ"},{"ｶﾞ","ガ"},{"ｷ","キ"},{"ｷﾞ","ギ"},{"ｸ","ク"},{"ｸﾞ","グ"},{"ｹ","ケ"},{"ｹﾞ","ゲ"},{"ｺ","コ"},{"ｺﾞ","ゴ"},
            {"ｻ","サ"},{"ｻﾞ","ザ"},{"ｼ","シ"},{"ｼﾞ","ジ"},{"ｽ","ス"},{"ｽﾞ","ズ"},{"ｾ","セ"},{"ｾﾞ","ゼ"},{"ｿ","ソ"},{"ｿﾞ","ゾ"},
            {"ﾀ","タ"},{"ﾀﾞ","ダ"},{"ﾁ","チ"},{"ﾁﾞ","ヂ"},{"ｯ","ッ"},{"ﾂ","ツ"},{"ﾂﾞ","ヅ"},{"ﾃ","テ"},{"ﾃﾞ","デ"},{"ﾄ","ト"},{"ﾄﾞ","ド"},
            {"ﾅ","ナ"},{"ﾆ","ニ"},{"ﾇ","ヌ"},{"ﾈ","ネ"},{"ﾉ","ノ"},{"ﾊ","ハ"},{"ﾊﾞ","バ"},{"ﾊﾟ","パ"},{"ﾋ","ヒ"},{"ﾋﾞ","ビ"},{"ﾋﾟ","ピ"},{"ﾌ","フ"},{"ﾌﾞ","ブ"},{"ﾌﾟ","プ"},{"ﾍ","ヘ"},{"ﾍﾞ","ベ"},{"ﾍﾟ","ペ"},{"ﾎ","ホ"},{"ﾎﾞ","ボ"},{"ﾎﾟ","ポ"},
            {"ﾏ","マ"},{"ﾐ","ミ"},{"ﾑ","ム"},{"ﾒ","メ"},{"ﾓ","モ"},
            {"ｬ","ャ"},{"ﾔ","ヤ"},{"ｭ","ュ"},{"ﾕ","ユ"},{"ｮ","ョ"},{"ﾖ","ヨ"},
            {"ﾗ","ラ"},{"ﾘ","リ"},{"ﾙ","ル"},{"ﾚ","レ"},{"ﾛ","ロ"},
            {"ヮ","ヮ"},{"ﾜ","ワ"},{"ヰ","ヰ"},{"ヱ","ヱ"},{"ｦ","ヲ"},{"ﾝ","ン"},{"ｳﾞ","ヴ"},{"ヵ","ヵ"},{"ヶ","ヶ"},
            {"ｰ","ー"}
        };

        /// <summary>
        /// 半角記号を全角記号に変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HankakuSymbolToZenkakuSymbol(string s)
        {
            Regex re = new Regex(@"[!-/:-@[-`{-~]|\s|[ｰ]|[･]");
            string output = re.Replace(s, match =>
            {//Matchした場合の処理
                if (dictSymbolHan.ContainsKey(match.Value))
                {//辞書にあれば辞書にある値を返す
                    return dictSymbolHan[match.Value];
                }
                else
                {//辞書になければ変換せずに返す
                    return match.Value;
                }
            });
            return output;
        }
        private static Dictionary<string, string> dictSymbolHan = new Dictionary<string, string>()
        {
            {"!","！"},{"\"","”"},{"#","＃"},{"$","＄"},{"%","％"},{"&","＆"},{"'","’"},{"(","（"},{")","）"},{"=","＝"},
            {"~","～"},{"|","｜"},{"-","－"},{"^","＾"},{"\\","￥"},{"`","‘"},{"{","｛"},{"@","＠"},{"[","［"},{"+","＋"},
            {"*","＊"},{"}","｝"},{";","；"},{":","："},{"]","］"},{"<","＜"},{">","＞"},{"?","？"},{",","，"},{".","．"},
            {"/","／"},{" ","　"},{"ｰ","ー"},{"･","・"}
        };

        /// <summary>
        /// 全角記号を半角記号に変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ZenkakuSymbolToHankakuSymbol(string s)
        {
            Regex re = new Regex(@"[！-／：-＠［-｀｛-～]|[\s　]|[ー]|[￥]|[”]|[’]|[‘]|[・]");
            string output = re.Replace(s, match =>
            {//Matchした場合の処理
                if (dictSymbolZen.ContainsKey(match.Value))
                {//辞書にあれば辞書にある値を返す
                    return dictSymbolZen[match.Value];
                }
                else
                {//辞書になければ変換せずに返す
                    return match.Value;
                }
            });
            return output;
        }
        private static Dictionary<string, string> dictSymbolZen = new Dictionary<string, string>()
        {
            {"！","!"},{"”","\""},{"＃","#"},{"＄","$"},{"％","%"},{"＆","&"},{"’","'"},{"（","("},{"）",")"},{"＝","="},
            {"～","~"},{"｜","|"},{"－","-"},{"＾","^"},{"￥","\\"},{"‘","`"},{"｛","{"},{"＠","@"},{"［","["},{"＋","+"},
            {"＊","*"},{"｝","}"},{"；",";"},{"：",":"},{"］","]"},{"＜","<"},{"＞",">"},{"？","?"},{"，",","},{"．","."},
            {"／","/"},{"　"," "},{"ー","ｰ"},{"・","･"}
        };

        /// <summary>
        /// 日付け（元号yy年mm月dd日）を漢数字に変換
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceKansuziDate(DateTime? input)
        {
            if (input == null)
            {
                return String.Empty;
            }

            DateTime dt = GenericUtil.ConvertDateTimeQtoDateTime(input);

            //和暦
            System.Globalization.CultureInfo ci =
                new System.Globalization.CultureInfo("ja-JP", false);
            ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            //元号
            string gengou = dt.ToString("gg", ci);
            //年
            int year = GenericUtil.IntTryparse(dt.ToString("yy",ci));
            //月
            int month = dt.Month;
            //日
            int day = dt.Day;

            String output = gengou + Capkanzi(year) + "年" + Capkanzi(month) + "月" + Capkanzi(day) + "日";

            return output;
        }

        /// <summary>
        /// 漢数字取得
        /// </summary>
        /// <param name="dt_int"></param>
        /// <returns></returns>
        private static string Capkanzi(int dt_int)
        {
            String[] kansu = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            String[] kansu2 = { "", "十", "二十", "三十" };

            string dt10 = String.Empty;
            string dt1 = String.Empty;

            //1の位を漢数字
            dt1 = kansu[dt_int % 10];
            if (dt_int >= 10)
            {
                //10の位を漢数字
                dt10 = kansu2[((dt_int - (dt_int % 10)) / 10) % 10];
            }

            string kanzi = dt10 + dt1;

            return kanzi;
        }

        /// <summary>
        /// 数字を漢数字に変換する
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String ConvertKansuzi(long? val)
        {
            if (val == null || val == 0)
            {
                return String.Empty;
            }

            String[] kansu = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            String[] kansu2 = { "", "十", "百", "千" };
            String[] kansu3 = { "", "万", "億", "兆" };

            String Kansuzi = String.Empty;
            //桁数　
            int Keta = 0;

            while (val > 0)
            {
                //桁数は４桁区切りで　String[] kansu3 = { "", "万", "億", "兆" };）
                int k = Keta % 4;
                //１桁目を取得
                int n = GenericUtil.IntTryparse(val % 10);

                //桁数が４桁目で、値が10000以上の場合　{ "", "万", "億", "兆" }
                if (k == 0 && val % 10000 > 0)
                {
                    Kansuzi = kansu3[Keta / 4] + Kansuzi;
                }

                //桁数が４桁目ではなく、値が１の場合
                if (k != 0 && n==1)
                {
                    Kansuzi = kansu2[k] + Kansuzi;
                }
                //桁数が４桁目ではなく、値が１以外の場合
                else if(n!=0)
                {
                    Kansuzi = kansu[n] + kansu2[k] + Kansuzi;
                }

                Keta++;
                val = val / 10;
            }

            return Kansuzi;

        }

        /// <summary>
        /// 全角数字を半角数字に変換する。
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        static public string ZenToHanNum(this string s)
        {
            return Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
        }

        /// <summary>
        /// 半角数字を全角数字に変換する。
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        static public string HanToZenNum(this string s)
        {
            return Regex.Replace(s, "[0-9]", p => ((char)(p.Value[0] - '0' + '０')).ToString());
        }

        /// <summary>
        /// 半角英字を全角英字に変換する。
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        static public string HanToZenAlpha(this string s)
        {
            var str = Regex.Replace(s, "[a-z]", p => ((char)(p.Value[0] - 'a' + 'ａ')).ToString());

            return Regex.Replace(str, "[A-Z]", p => ((char)(p.Value[0] - 'A' + 'Ａ')).ToString());
        }

        /// <summary>
        /// 全角英字を半角英字に変換する。
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        static public string ZenToHanAlpha(this string s)
        {
            var str = Regex.Replace(s, "[ａ-ｚ]", p => ((char)(p.Value[0] - 'ａ' + 'a')).ToString());

            return Regex.Replace(str, "[Ａ-Ｚ]", p => ((char)(p.Value[0] - 'Ａ' + 'A')).ToString());
        }

        /// <summary>
        /// 任意文字列に指定した間隔で指定した文字を挿入
        /// </summary>
        /// <param name="str"></param>
        /// <param name="SpInterval"></param>
        /// <returns></returns>
        public static String StringAtEqualntervalsInsert(String str, int SpInterval, String spchar = " ")
        {


            for (int i = SpInterval; i < str.Length; i += (SpInterval + 1)) str = StringInsert(str, spchar, i);

            return str;
        }

        /// <summary>
        /// 指定位置へ任意文字を追加
        /// </summary>
        /// <param name="str"></param>
        /// <param name="spchar"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string StringInsert(string str, string spchar, int i)
        {
            return str.Insert(i, spchar);
        }

        /// <summary>
        /// コード表示用変換
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetDispCode42(string Code)
        {
            if (String.IsNullOrEmpty(Code))
            {
                return String.Empty;
            }
            if (Code.Length < 6)
            {
                return String.Empty;
            }
            return Code.Substring(0, 4) + "-" + Code.Substring(4, 2);
        }

        /// <summary>
        /// コード表示用変換
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetDispCode422(string Code)
        {
            if (String.IsNullOrEmpty(Code))
            {
                return String.Empty;
            }
            if (Code.Length < 8)
            {
                return String.Empty;
            }
            return Code.Substring(0, 4) + "-" + Code.Substring(4, 2) + "-" + Code.Substring(6, 2);
        }

        /// <summary>
        /// コード表示用変換
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetDispCode46(string Code)
        {
            if (String.IsNullOrEmpty(Code))
            {
                return String.Empty;
            }

            if (Code.Length < 10)
            {
                return String.Empty;
            }
            return Code.Substring(0, 4) + "-" + Code.Substring(4, 6);
        }

        /// <summary>
        /// コード表示用変換
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetDispCode462(string Code)
        {
            if (String.IsNullOrEmpty(Code))
            {
                return String.Empty;
            }

            if (Code.Length < 12)
            {
                return String.Empty;
            }
            return Code.Substring(0, 4) + "-" + Code.Substring(4, 6) + "-" + Code.Substring(10, 2);
        }

        /// <summary>
        /// コード表示用変換
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetDispCode832(string Code)
        {
            if (String.IsNullOrEmpty(Code))
            {
                return String.Empty;
            }

            if (Code.Length < 13)
            {
                return String.Empty;
            }
            return Code.Substring(0, 8) + "-" + Code.Substring(8, 3) + "-" + Code.Substring(11, 2);
        }

        /// <summary>
        /// 加入年齢算出
        /// </summary>
        /// <param name="pymd"></param>
        /// <param name="pSeiymd"></param>
        /// <returns></returns>
        public static int CalcKanyuAge(DateTime? pymd, DateTime? pSeiymd)
        {
            //年月日か生年月日がNULLの場合は何もしない
            if (pymd == null || pSeiymd == null)
            {
                return 0;
            }

            DateTime ymd = GenericUtil.ConvertDateTimeQtoDateTime(pymd);
            DateTime seiymd = GenericUtil.ConvertDateTimeQtoDateTime(pSeiymd);

            int age = ymd.Year - seiymd.Year;

            String MD = GenericUtil.ZeroPadding(GenericUtil.StringTryparse(ymd.Month), 2) +
                           GenericUtil.ZeroPadding(GenericUtil.StringTryparse(ymd.Day), 2);

            String SeiMD = GenericUtil.ZeroPadding(GenericUtil.StringTryparse(seiymd.Month), 2) +
                           GenericUtil.ZeroPadding(GenericUtil.StringTryparse(seiymd.Day), 2);

            if (SeiMD.CompareTo(MD) > 0)
            {
                age = age - 1;
            }

            return age;
        }

        /// <summary>
        /// 文字列置き換え
        /// </summary>
        /// <param name="Val"></param>
        /// <param name="OldVal"></param>
        /// <param name="NewVal"></param>
        /// <returns></returns>
        public static String Replace(String Val, String OldVal, String NewVal)
        {
            if (String.IsNullOrEmpty(Val))
            {
                return Val;
            }

            return Val.Replace(OldVal, NewVal);
        }

        // <summary>
        /// Linax用のパスに変換「\\」→「/」
        /// </summary>
        /// <param name="Path">Windows用パス「\\」</param>
        /// <returns>Linax用パス「/」</returns>
        public static String PathReplaceWtoL(String Path)
        {
            if (String.IsNullOrEmpty(Path))
            {
                return Path;
            }

            return Path.Replace("\\", "/");
        }

        /// <summary>
        /// クラス内の文字型プロパティから指定する文字列を除去する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pmb"></param>
        /// <param name="RemovalStringList"></param>
        public static void PropertiesRemovalString<T>(T pmb, List<String> RemovalStringList = null) where T : class
        {
            //除去文字列を省力している場合はリターンをセット
            if (RemovalStringList == null)
            {
                RemovalStringList = new List<String>();
                RemovalStringList.Add(Environment.NewLine);
            }

            // プロパティ情報出力をループで回して検索条件の文字列からリターン情報を除去する
            PropertyInfo[] infoArray = pmb.GetType().GetProperties();
            foreach (PropertyInfo info in infoArray)
            {
                if (info.PropertyType.Name.Equals("String"))
                {
                    var ageProperty = typeof(T).GetProperty(info.Name);
                    String txt = GenericUtil.StringTryparse(ageProperty.GetValue(pmb));

                    if (!String.IsNullOrEmpty(txt))
                    {
                        foreach (String RemoveString in RemovalStringList)
                        {
                            ageProperty.SetValue(pmb, txt.Replace(RemoveString, ""));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 電話番号に「‐」を挿入
        /// </summary>
        public static string FormatPhoneNumber(string input, bool strict = false)
        {
            //NULL値はそのまま返却
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }


            //市外局番5桁、局番1桁の番号
            var numList5_1 = new List<string>()
        {
            "01564", "01558", "01586", "01587", "01634", "01632",
            "01547", "05769", "04992", "04994", "01456", "01457",
            "01466", "01635", "09496", "08477", "08512", "08396",
            "08388", "08387", "08514", "07468", "01655", "01648",
            "01656", "01658", "05979", "04996", "01654", "01372",
            "01374", "09969", "09802", "09912", "09913", "01398",
            "01377", "01267", "04998", "01397", "01392",
        };
            //市外局番4桁、局番2桁の番号
            var numList4_2 = new List<string>()
        {
            "0768", "0770", "0772", "0774", "0773", "0767", "0771", "0765",
            "0748", "0747", "0746", "0826", "0749", "0776", "0763", "0761",
            "0766", "0778", "0824", "0797", "0796", "0555", "0823", "0798",
            "0554", "0820", "0795", "0556", "0791", "0790", "0779", "0558",
            "0745", "0794", "0557", "0799", "0738", "0567", "0568", "0585",
            "0586", "0566", "0564", "0565", "0587", "0584", "0581", "0572",
            "0574", "0573", "0575", "0576", "0578", "0577", "0569", "0594",
            "0827", "0736", "0735", "0725", "0737", "0739", "0743", "0742",
            "0740", "0721", "0599", "0561", "0562", "0563", "0595", "0596",
            "0598", "0597", "0744", "0852", "0956", "0955", "0954", "0952",
            "0957", "0959", "0966", "0965", "0964", "0950", "0949", "0942",
            "0940", "0930", "0943", "0944", "0948", "0947", "0946", "0967",
            "0968", "0987", "0986", "0985", "0984", "0993", "0994", "0997",
            "0996", "0995", "0983", "0982", "0973", "0972", "0969", "0974",
            "0977", "0980", "0979", "0978", "0920", "0898", "0855", "0854",
            "0853", "0553", "0856", "0857", "0863", "0859", "0858", "0848",
            "0847", "0835", "0834", "0833", "0836", "0837", "0846", "0845",
            "0838", "0865", "0866", "0892", "0889", "0887", "0893", "0894",
            "0897", "0896", "0895", "0885", "0884", "0869", "0868", "0867",
            "0875", "0877", "0883", "0880", "0879", "0829", "0550", "0228",
            "0226", "0225", "0224", "0229", "0233", "0237", "0235", "0234",
            "0223", "0220", "0192", "0191", "0187", "0193", "0194", "0198",
            "0197", "0195", "0238", "0240", "0260", "0259", "0258", "0257",
            "0261", "0263", "0266", "0265", "0264", "0256", "0255", "0243",
            "0242", "0241", "0244", "0246", "0254", "0248", "0247", "0186",
            "0185", "0144", "0143", "0142", "0139", "0145", "0146", "0154",
            "0153", "0152", "0138", "0137", "0125", "0124", "0123", "0126",
            "0133", "0136", "0135", "0134", "0155", "0156", "0176", "0175",
            "0174", "0178", "0179", "0184", "0183", "0182", "0173", "0172",
            "0162", "0158", "0157", "0163", "0164", "0167", "0166", "0165",
            "0267", "0250", "0533", "0422", "0532", "0531", "0436", "0428",
            "0536", "0299", "0294", "0293", "0475", "0295", "0297", "0296",
            "0495", "0438", "0466", "0465", "0467", "0478", "0476", "0470",
            "0463", "0479", "0493", "0494", "0439", "0268", "0480", "0460",
            "0538", "0537", "0539", "0279", "0548", "0280", "0282", "0278",
            "0277", "0269", "0270", "0274", "0276", "0283", "0551", "0289",
            "0287", "0547", "0288", "0544", "0545", "0284", "0291", "0285",
        };
            //市外局番4桁、局番3桁の番号
            var numList4_3 = new List<string>()
        {
            "0120", "0570", "0800", "0990"
        };
            //市外局番3桁、局番3桁の番号
            var numList3_3 = new List<string>()
        {
            "058", "052", "011", "096", "049", "015", "048", "072",
            "084", "028", "024", "076", "023", "047", "029", "075",
            "025", "055", "026", "079", "082", "027", "078", "077",
            "083", "022", "086", "089", "045", "044", "092", "046",
            "017", "093", "059", "073", "019", "087", "042", "018",
            "043", "088",
        };
            //市外局番3桁、局番4桁の番号
            var numList3_4 = new List<string>()
        {
            "050",
        };
            //市外局番2桁、局番4桁の番号
            var numList2_4 = new List<string>()
        {
            "03", "04", "06",
        };
            //市外局番3桁、一般ルールなら局番4桁、総務省ルールなら局番3桁
            var numList3_3or4 = new List<string>()
        {
            "020", "070", "080", "090",
        };
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            //一般ルールか総務省ルールかで登録するグループを変える
            if (strict)
            { numList3_3.AddRange(numList3_3or4); }
            else
            { numList3_4.AddRange(numList3_3or4); }

            //番号変換クラスに変換ルールを登録
            var replacer = new Replacer();
            replacer.AddRule(numList5_1, 1);
            replacer.AddRule(numList4_2, 2);
            replacer.AddRule(numList4_3.Concat(numList3_3), 3);
            replacer.AddRule(numList3_4.Concat(numList2_4), 4);

            //入力文字列から数値以外の文字を除去
            var number = Regex.Replace(input, @"\D", "");

            //変換を実行
            var formatted = replacer.TryReplace(number);
            if (formatted != "")
            { return formatted; }

            //局番なしの場合の変換
            var matches = Regex.Match(number, @"\A(00(?:[013-8]|2\d|91[02-9])\d)(\d+)\z");
            return matches.Success ? matches.Groups[1] + "-" + matches.Groups[2] : input;
        }

        /// <summary>
        /// 郵便番号に「‐」を挿入
        /// </summary>
        public static string FormatPostNumber(string input)
        {
            if (!string.IsNullOrWhiteSpace(input) && input.Length == 7)
            {
                return input.Insert(3, "-");
            }
            return input;
        }

        /// <summary>
        /// 日付文字変換SQL作成用
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>変換語文字列</returns>
        public static string DateToString(DateTime? date)
        {
            string ret = "";
            if (date.HasValue)
            {
                //ret = "to_date('" + ((DateTime)date).ToString("yyyyMMdd") + "','yyyyMMdd')";
                ret = "'" + ((DateTime)date).ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                ret = "null";
            }
            return ret;
        }

        /// <summary>
        /// 日付時刻文字変換SQL作成用
        /// </summary>
        /// <param name="date">日付時刻</param>
        /// <returns>変換語文字列</returns>
        public static string DateTimeToString(DateTime? date)
        {
            string ret = "";
            if (date.HasValue)
            {
                //ret = "to_timestamp('" + ((DateTime)date).ToString("yyyyMMddHHmmss") + "','yyyyMMddHH24MISS')";
                ret = "'" + ((DateTime)date).ToString("yyyy-MM-dd HH:mm:ss.mmm") + "'";
            }
            else
            {
                ret = "null";
            }
            return ret;
        }

        /// <summary>
        /// 数値文字変換SQL作成用
        /// </summary>
        /// <param name="value">数値</param>
        /// <returns>変換語文字列</returns>
        public static string LongToString(long? value)
        {
            string ret = "";
            if (value.HasValue)
            {
                ret = value.ToString();
            }
            else
            {
                ret = "null";
            }
            return ret;
        }

        /// <summary>
        /// 数値文字変換SQL作成用
        /// </summary>
        /// <param name="value">数値</param>
        /// <returns>変換語文字列</returns>
        public static string DecimalToString(Decimal? value)
        {
            string ret = "";
            if (value.HasValue)
            {
                ret = value.ToString();
            }
            else
            {
                ret = "null";
            }
            return ret;
        }

        /// <summary>
        /// 文字列SQL用変換作成用
        /// </summary>
        /// <param name="value">文字列</param>
        /// <returns>変換語文字列</returns>
        public static string StringToString(string str)
        {
            string ret = "";
            if (str == null)
            {
                ret = "null";
            }
            else
            {
                ret = "'" + str.Replace("'", "''") + "'";
            }
            return ret;
        }

        /// <summary>
        /// bool SQL用変換作成用
        /// </summary>
        /// <param name="value">文字列</param>
        /// <returns>変換語文字列</returns>
        public static string BoolToString(bool? bl)
        {
            if (!bl.HasValue)
            {
                return ("null");
                //return ("false");
            }
            else if ((bool)bl)
            {
                return ("1");
                //return ("true");
            }
            else
            {
                return ("0");
                //return ("false");
            }
        }

        /// <summary>
        /// 消費税計算
        /// </summary>
        /// <param name="kingaku"></param>
        /// <param name="TaxRate"></param>
        /// <returns></returns>
        public static long CalcTax(long? kingaku, decimal TaxRate)
        {
            //小数点以下切り捨て
            return LongTryparse(CalcTax(DecimalTryparse(kingaku), TaxRate, 2, 0));
        }

        /// <summary>
        /// 口座番号の前４桁を隠す
        /// </summary>
        /// <param name="kouzano"></param>
        /// <returns></returns>
        public static string HideKouzaNo(string kouzano)
        {
            if (String.IsNullOrWhiteSpace(kouzano))
            {
                return kouzano;
            }

            if (kouzano.Length <= 4)
            {
                return kouzano;
            }

            //前４桁をアスタリスクで隠ぺい
            kouzano = "****" + kouzano.Substring(4, kouzano.Length - 4);

            return  kouzano;
        }

        /// <summary>
        /// 月数を返す(1月未満は0)
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public static int GetTukiCnt(DateTime startDay, DateTime endDay)
        {
            int tukiCnt = 0;

            if (startDay.Year == endDay.Year && startDay.Month == endDay.Month)
            {
                if (startDay.Day == 1 && endDay.Day == DateTime.DaysInMonth(endDay.Year, endDay.Month))
                {
                    tukiCnt = 1;
                }
            }
            else
            {
                if(startDay.Year == endDay.Year)
                {
                    if (startDay.Day - endDay.Day == 1)
                    {
                        var nextMonth = startDay.AddMonths(1);
                        tukiCnt = endDay.Month - startDay.Month;
                    }
                    else if (startDay.Day > endDay.Day)
                    {
                        tukiCnt = endDay.Month - startDay.Month - 1;
                    }
                    else
                    {
                        tukiCnt = endDay.Month - startDay.Month;
                    }
                }
                else
                {
                    if (startDay.Month == endDay.Month && startDay.Day == (endDay.Day + 1))
                    {
                        tukiCnt = (endDay.Year - startDay.Year) * 12;
                    }
                    else if (startDay.Month == endDay.Month && startDay.Day <= endDay.Day)
                    {
                        tukiCnt = (endDay.Year - startDay.Year) * 12;
                    }
                    else if (endDay.Month == startDay.Month)
                    {
                        tukiCnt = 11;
                        if ((endDay.Year - startDay.Year) > 1)
                        {
                            tukiCnt += (endDay.Year - startDay.Year) * 12;
                        }
                    }
                    else if (endDay.Month < startDay.Month)
                    {
                        tukiCnt = endDay.Month + 12 - startDay.Month;
                        if ((endDay.Year - startDay.Year) > 1)
                        {
                            tukiCnt += (endDay.Year - startDay.Year) * 12;
                        }
                    }
                    else
                    {
                        tukiCnt = endDay.Month - startDay.Month;
                        if ((endDay.Year - startDay.Year) > 1)
                        {
                            tukiCnt += (endDay.Year - startDay.Year) * 12;
                        }
                    }
                }
            }
            if(tukiCnt < 0)
            {
                tukiCnt = 0;
            }
            return tukiCnt;
        }

        /// <summary>
        /// 日数を返す(月数除く)
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public static int GetHiCnt(DateTime startDay, DateTime endDay)
        {
            int hiCnt = 0;

            var tukiCnt = GetTukiCnt(startDay, endDay);

            if(tukiCnt > 0)
            {
                var startTuki = startDay.AddMonths(tukiCnt);
                var sa = endDay - startTuki;
                hiCnt = sa.Days + 1;
            }
            else
            {
                var sa = endDay - startDay;
                hiCnt = sa.Days + 1;

            }
            return hiCnt;
        }

        /// <summary>
        ///  令和年取得
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        public static int GetReiwa(DateTime ymd)
        {
            return ymd.Year - 2018;
        }
    }

    public class Replacer
    {
        private List<Regex> regexList = new List<Regex>();
        //headNumbersから始まる番号用の置換ルールを登録
        public void AddRule(IEnumerable<string> headNumbers, int middleDigits)
        {
            regexList.Add(new Regex(
                "^(" + string.Join("|", headNumbers) + ")(.{" + middleDigits + "})(.+)"));
        }

        //該当する番号なら置換した文字列を返し、該当しない番号なら空文字を返す
        public string TryReplace(string input)
        {
            foreach (var regex in regexList)
            {
                string result = regex.Replace(input, "$1-$2-$3");
                //置換されたか判定。文字列比較でもいいんだけどこっちの方が高速
                if (object.ReferenceEquals(result, input) == false)
                { return result; }
            }
            return "";
        }
    }
}
