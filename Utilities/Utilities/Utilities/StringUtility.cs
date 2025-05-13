using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// 文字列ユーティリティ
    /// </summary>
    public class StringUtility
    {
        #region 文字エンコーディング

        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        private Encoding _myEncoding = Encoding.GetEncoding("Shift_JIS");

        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        public Encoding MyEncoding
        {
            get
            {
                return this._myEncoding;
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StringUtility()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="encoding">文字エンコーディング</param>
        public StringUtility(Encoding encoding)
        {
            this._myEncoding = encoding;
        }

        #endregion

        #region 文字列のバイト数を取得

        /// <summary>
        /// 文字列のバイト数を取得します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列のバイト数</returns>
        public int GetByteCount(string target)
        {
            return MyEncoding.GetByteCount(target);
        }

        #endregion

        #region 文字列からバイト単位で部分文字列を取得

        /// <summary>
        /// 文字列からバイト単位で部分文字列を取得します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="startIndexByte">開始位置インデックスバイト</param>
        /// <param name="lengthByte">部分文字列のバイト数</param>
        /// <returns>文字列からバイト単位で取得した部分文字列</returns>
        public string SubstringByte(string target, int startIndexByte, int lengthByte)
        {
            if (startIndexByte < 0)
            {
                throw new ArgumentOutOfRangeException("開始位置インデックスバイトが０未満です。");
            }

            if (lengthByte < 0)
            {
                throw new ArgumentOutOfRangeException("部分文字列のバイト数が０未満です。");
            }

            // 対象の文字列をバイト配列にする
            byte[] targetBytes = MyEncoding.GetBytes(target);

            // 開始位置インデックスバイト＋部分文字列のバイト数が文字列のバイト数を超える場合
            if (targetBytes.Length < startIndexByte + lengthByte)
            {
                throw new ArgumentOutOfRangeException("開始位置インデックスバイトまたは部分文字列のバイト数の指定に誤りがあります。");
            }

            // 対象の文字列からバイト単位で部分文字列を取得
            string partialString = MyEncoding.GetString(targetBytes, startIndexByte, lengthByte);

            // 先頭に全角文字途中の要素が混入している場合は除去
            partialString = TrimHeadHalfwayDoubleByteCharacter(partialString, target, targetBytes, startIndexByte);

            // 末尾に全角文字途中の要素が混入している場合は除去
            partialString = TrimEndHalfwayDoubleByteCharacter(partialString, target, targetBytes, startIndexByte, lengthByte);

            return partialString;
        }

        /// <summary>
        /// 先頭に全角文字途中の要素が混入している場合は除去します
        /// </summary>
        /// <param name="partialString">部分文字列</param>
        /// <param name="target">対象の文字列</param>
        /// <param name="targetBytes">対象の文字列のバイト配列</param>
        /// <param name="startIndexByte">開始位置インデックスバイト</param>
        /// <returns>先頭の全角文字途中の要素を除去した部分文字列</returns>
        private string TrimHeadHalfwayDoubleByteCharacter(string partialString, string target, byte[] targetBytes, int startIndexByte)
        {
            // 部分文字列が空の場合
            if (partialString == string.Empty)
            {
                return partialString;
            }

            // 開始位置の要素を含む部分文字列を取得
            string leftString = MyEncoding.GetString(targetBytes, 0, startIndexByte + 1);

            // 部分文字列の先頭要素が一致する場合
            if (target[leftString.Length - 1] == partialString[0])
            {
                return partialString;
            }

            // 先頭の全角文字途中の要素を除去
            return partialString.Substring(1);
        }

        /// <summary>
        /// 末尾に全角文字途中の要素が混入している場合は除去します
        /// </summary>
        /// <param name="partialString">部分文字列</param>
        /// <param name="target">対象の文字列</param>
        /// <param name="targetBytes">対象の文字列のバイト配列</param>
        /// <param name="startIndexByte">開始位置インデックスバイト</param>
        /// <param name="lengthByte">部分文字列のバイト数</param>
        /// <returns>末尾の全角文字途中の要素を除去した部分文字列</returns>
        private string TrimEndHalfwayDoubleByteCharacter(string partialString, string target, byte[] targetBytes, int startIndexByte, int lengthByte)
        {
            // 部分文字列が空の場合
            if (partialString == string.Empty)
            {
                return partialString;
            }

            // 最終要素を含む部分文字列を取得
            string leftString = MyEncoding.GetString(targetBytes, 0, startIndexByte + lengthByte);

            // 部分文字列の最終要素が一致する場合
            if (target[leftString.Length - 1] == partialString[partialString.Length - 1])
            {
                return partialString;
            }

            // 末尾の全角文字途中の要素を除去
            return partialString.Substring(0, partialString.Length - 1);
        }

        #endregion

        #region 文字列を指定されたバイト長に切り詰め

        /// <summary>
        /// 文字列を指定されたバイト長に左側から切り詰めます
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <returns>指定されたバイト長に切り詰めた文字列</returns>
        /// <remarks>
        /// <para>文字列が指定されたバイト長に満たない場合は、成型せずに返します</para>
        /// <para>切り詰めた文字列の末尾に全角文字途中の要素が混入している場合は除去します</para>
        /// </remarks>
        public string TruncateByteLeft(string target, int lengthByte)
        {
            // 対象の文字列のバイト長が指定されたバイト長以下の場合
            if (GetByteCount(target) <= lengthByte)
            {
                return target;
            }

            return SubstringByte(target, 0, lengthByte);
        }

        /// <summary>
        /// 文字列を指定されたバイト長に右側から切り詰めます
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <returns>指定されたバイト長に切り詰めた文字列</returns>
        /// <remarks>
        /// <para>文字列が指定されたバイト長に満たない場合は、成型せずに返します</para>
        /// <para>切り詰めた文字列の先頭に全角文字途中の要素が混入している場合は除去します</para>
        /// </remarks>
        public string TruncateByteRight(string target, int lengthByte)
        {
            // 対象の文字列のバイト長を取得
            int targetByteCount = GetByteCount(target);

            // 対象の文字列のバイト長が指定されたバイト長以下の場合
            if (targetByteCount <= lengthByte)
            {
                return target;
            }

            return SubstringByte(target, targetByteCount - lengthByte, lengthByte);
        }

        #endregion

        #region 文字列を指定されたバイト長に左詰めで成型

        /// <summary>
        /// 文字列を指定されたバイト長に左詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <param name="padCharacter">パッドキャラクター</param>
        /// <param name="isTruncateLeft">文字列を左側から切り詰める場合はtrue、それ以外はfalse</param>
        /// <returns>指定されたバイト長に左詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の先頭・末尾に全角文字途中の要素が混入している場合は除去し、パッドキャラクターを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthLeft(string target, int lengthByte, char padCharacter, bool isTruncateLeft)
        {
            // 部分文字列を取得
            string partialString = isTruncateLeft ? TruncateByteLeft(target, lengthByte) : TruncateByteRight(target, lengthByte);

            // パッドするバイト長を取得
            int padLengthByte = lengthByte - GetByteCount(partialString);

            // パッドする必要がない場合
            if (padLengthByte == 0)
            {
                return partialString;
            }

            // パッドする長さを取得
            int padLength = padLengthByte / GetByteCount(padCharacter.ToString());

            return partialString.PadRight(partialString.Length + padLength, padCharacter);
        }

        /// <summary>
        /// 文字列を指定されたバイト長に左詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <param name="padCharacter">パッドキャラクター</param>
        /// <returns>指定されたバイト長に左詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の末尾に全角文字途中の要素が混入している場合は除去し、パッドキャラクターを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthLeft(string target, int lengthByte, char padCharacter)
        {
            return FormFixedByteLengthLeft(target, lengthByte, padCharacter, true);
        }

        /// <summary>
        /// 文字列を指定されたバイト長に左詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <returns>指定されたバイト長に左詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の末尾に全角文字途中の要素が混入している場合は除去し、半角スペースを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthLeft(string target, int lengthByte)
        {
            return FormFixedByteLengthLeft(target, lengthByte, ' ');
        }

        #endregion

        #region 文字列を指定されたバイト長に右詰めで成型

        /// <summary>
        /// 文字列を指定されたバイト長に右詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <param name="padCharacter">パッドキャラクタ</param>
        /// <param name="isTruncateLeft">文字列を左側から切り詰める場合はtrue、それ以外はfalse</param>
        /// <returns>指定されたバイト長に右詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の先頭・末尾に全角文字途中の要素が混入している場合は除去し、パッドキャラクターを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthRight(string target, int lengthByte, char padCharacter, bool isTruncateLeft)
        {
            // 部分文字列を取得
            string partialString = isTruncateLeft ? TruncateByteLeft(target, lengthByte) : TruncateByteRight(target, lengthByte);

            // パッドするバイト長を取得
            int padLengthByte = lengthByte - GetByteCount(partialString);

            // パッドする必要がない場合
            if (padLengthByte == 0)
            {
                return partialString;
            }

            // パッドする長さを取得
            int padLength = padLengthByte / GetByteCount(padCharacter.ToString());

            return partialString.PadLeft(partialString.Length + padLength, padCharacter);
        }

        /// <summary>
        /// 文字列を指定されたバイト長に右詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <param name="padCharacter">パッドキャラクター</param>
        /// <returns>指定されたバイト長に右詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の末尾に全角文字途中の要素が混入している場合は除去し、パッドキャラクターを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthRight(string target, int lengthByte, char padCharacter)
        {
            return FormFixedByteLengthRight(target, lengthByte, padCharacter, true);
        }

        /// <summary>
        /// 文字列を指定されたバイト長に右詰めで成型します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="lengthByte">バイト長</param>
        /// <returns>指定されたバイト長に右詰めで成型された文字列</returns>
        /// <remarks>成型された文字列の末尾に全角文字途中の要素が混入している場合は除去し、半角スペースを追加します</remarks>
        /// <remarks>Unicodeでバイト長に奇数が指定された場合などは、バイト長を超えない最大の長さで成型します</remarks>
        public string FormFixedByteLengthRight(string target, int lengthByte)
        {
            return FormFixedByteLengthRight(target, lengthByte, ' ');
        }

        #endregion

        #region 文字列が整数かどうかを判定

        /// <summary>
        /// 文字列が符号なしの整数かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が符号なしの整数の場合はtrue、それ以外はfalse</returns>
        public static bool IsUnsignedIntegers(string target)
        {
            return new Regex("^[0-9]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が符号ありの整数かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が符号ありの整数の場合はtrue、それ以外はfalse</returns>
        public static bool IsIntegers(string target)
        {
            return new Regex("^[-+]?[0-9]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が小数かどうかを判定

        /// <summary>
        /// 文字列が符号なしの小数かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が符号なしの小数の場合はtrue、それ以外はfalse</returns>
        public static bool IsUnsignedDecimal(string target)
        {
            return new Regex("^[0-9]*\\.?[0-9]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が符号ありの小数かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が符号ありの小数の場合はtrue、それ以外はfalse</returns>
        public static bool IsDecimal(string target)
        {
            return new Regex("^[-+]?[0-9]*\\.?[0-9]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が英字かどうかを判定

        /// <summary>
        /// 文字列が小文字の英字かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が小文字の英字の場合はtrue、それ以外はfalse</returns>
        public static bool IsLowercaseAlphabet(string target)
        {
            return new Regex("^[a-z]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が大文字の英字かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が大文字の英字の場合はtrue、それ以外はfalse</returns>
        public static bool IsUppercaseAlphabet(string target)
        {
            return new Regex("^[A-Z]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が英字かどうかを判定します
        /// </summary>
        /// <remarks>大文字・小文字を区別しません</remarks>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が英字の場合はtrue、それ以外はfalse</returns>
        public static bool IsAlphabet(string target)
        {
            return new Regex("^[a-zA-Z]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が英数字かどうかを判定

        /// <summary>
        /// 文字列が小文字の英数字かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が小文字の英数字の場合はtrue、それ以外はfalse</returns>
        public static bool IsLowercaseAlphanumeric(string target)
        {
            return new Regex("^[0-9a-z]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が大文字の英数字かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が大文字の英数字の場合はtrue、それ以外はfalse</returns>
        public static bool IsUppercaseAlphanumeric(string target)
        {
            return new Regex("^[0-9A-Z]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が英数字かどうかを判定します
        /// </summary>
        /// <remarks>大文字・小文字を区別しません</remarks>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が英数字の場合はtrue、それ以外はfalse</returns>
        public static bool IsAlphanumeric(string target)
        {
            return new Regex("^[0-9a-zA-Z]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が半角英数字記号かどうかを判定

        /// <summary>
        /// 文字列が半角英数字記号かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角英数字記号の場合はtrue、それ以外はfalse</returns>
        public static bool IsASCII(string target)
        {
            return new Regex("^[\x20-\x7E]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が半角カタカナかどうかを判定

        /// <summary>
        /// 文字列が半角カタカナ（句読点～半濁点）かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角カタカナ（句読点～半濁点）の場合はtrue、それ以外はfalse</returns>
        public static bool IsHalfKatakanaPunctuation(string target)
        {
            return new Regex("^[\uFF61-\uFF9F]+$").IsMatch(target);
        }

        /// <summary>
        /// 文字列が半角カタカナ（「ｦ」～半濁点）かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角カタカナ（「ｦ」～半濁点）の場合はtrue、それ以外はfalse</returns>
        public static bool IsHalfKatakana(string target)
        {
            return new Regex("^[\uFF66-\uFF9F]+$").IsMatch(target);
        }

        #endregion

        #region 文字列が半角かどうかを判定

        /// <summary>
        /// 文字列が半角かどうかを判定します
        /// </summary>
        /// <remarks>半角の判定を長さで行います</remarks>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角の場合はtrue、それ以外はfalse</returns>
        public static bool IsHalfByLength(string target)
        {
            return target.Length == Encoding.GetEncoding("shift_jis").GetByteCount(target);
        }

        /// <summary>
        /// 文字列が半角かどうかを判定します
        /// </summary>
        /// <remarks>半角の判定を正規表現で行います。半角カタカナは「ｦ」～半濁点を半角とみなします</remarks>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角の場合はtrue、それ以外はfalse</returns>
        public static bool IsHalfByRegex(string target)
        {
            return new Regex("^[\u0020-\u007E\uFF66-\uFF9F]+$").IsMatch(target);
        }

        #endregion

        #region 文字列がひらがなかどうかを判定

        /// <summary>
        /// 文字列がひらがなかどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列がひらがなの場合はtrue、それ以外はfalse</returns>
        public static bool IsHiragana(string target)
        {
            return new Regex("^\\p{IsHiragana}+$").IsMatch(target);
        }

        #endregion

        #region 文字列が全角カタカナかどうかを判定

        /// <summary>
        /// 文字列が全角カタカナかどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が全角カタカナの場合はtrue、それ以外はfalse</returns>
        public static bool IsFullKatakana(string target)
        {
            return new Regex("^\\p{IsKatakana}+$").IsMatch(target);
        }

        #endregion

        #region 文字列がJIS X 0208 漢字第二水準までで構成されているかを判定

        /// <summary>
        /// 文字列がJIS X 0208 漢字第二水準までで構成されているかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="containsHalfKatakana">漢字第二水準までに半角カタカナを含む場合はtrue、それ以外はfalse</param>
        /// <returns>文字列がJIS X 0208 漢字第二水準までで構成されている場合はtrue、それ以外はfalse</returns>
        public static bool IsUntilJISKanjiLevel2(string target, bool containsHalfKatakana)
        {
            // 文字エンコーディングに「iso-2022-jp」を指定
            Encoding encoding = Encoding.GetEncoding("iso-2022-jp");

            // 文字列長を取得
            int length = target.Length;

            for (int i = 0; i < length; i++)
            {
                // 対象の部分文字列を取得
                string targetSubString = target.Substring(i, 1);

                // 半角英数字記号の場合
                if (IsASCII(targetSubString) == true)
                {
                    continue;
                }

                // 漢字第二水準までに半角カタカナを含まずかつ対象の部分文字列が半角カタカナの場合
                if (containsHalfKatakana == false &&
                    IsHalfKatakanaPunctuation(targetSubString) == true)
                {
                    return false;
                }

                // 対象部分文字列の文字コードバイト配列を取得
                byte[] targetBytes = encoding.GetBytes(targetSubString);

                // 要素数が「1」の場合は漢字第三水準以降の漢字が「?」に変換された
                if (targetBytes.Length == 1)
                {
                    return false;
                }

                // 文字コードバイト配列がJIS X 0208 漢字第二水準外の場合
                if (IsUntilJISKanjiLevel2(targetBytes) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 文字列がJIS X 0208 漢字第二水準までで構成されているかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列がJIS X 0208 漢字第二水準までで構成されている場合はtrue、それ以外はfalse</returns>
        /// <remarks>句読点～半濁点の半角カタカナはJIS X 0208 漢字第二水準外と判定します</remarks>
        public static bool IsUntilJISKanjiLevel2(string target)
        {
            return IsUntilJISKanjiLevel2(target, false);
        }

        /// <summary>
        /// 文字コードバイト配列がJIS X 0208 漢字第二水準までであるかを判定します
        /// </summary>
        /// <param name="targetBytes">文字コードバイト配列</param>
        /// <returns>文字コードバイト配列がJIS X 0208 漢字第二水準までである場合はtrue、それ以外はfalse</returns>
        private static bool IsUntilJISKanjiLevel2(byte[] targetBytes)
        {
            // 文字コードバイト配列の要素数が8ではない場合
            if (targetBytes.Length != 8)
            {
                return false;
            }

            // 区を取得
            int row = targetBytes[3] - 0x20;

            // 点を取得
            int cell = targetBytes[4] - 0x20;

            switch (row)
            {
                case 1: // 1区の場合
                    if (1 <= cell && cell <= 94)
                    {
                        // 1点～94点の場合
                        return true;
                    }

                    break;

                case 2: // 2区の場合
                    if (1 <= cell && cell <= 14)
                    {
                        // 1点～14点の場合
                        return true;
                    }
                    else if (26 <= cell && cell <= 33)
                    {
                        // 26点～33点の場合
                        return true;
                    }
                    else if (42 <= cell && cell <= 48)
                    {
                        // 42点～48点の場合
                        return true;
                    }
                    else if (60 <= cell && cell <= 74)
                    {
                        // 60点～74点の場合
                        return true;
                    }
                    else if (82 <= cell && cell <= 89)
                    {
                        // 82点～89点の場合
                        return true;
                    }
                    else if (cell == 94)
                    {
                        // 94点の場合
                        return true;
                    }

                    break;

                case 3: // 3区の場合
                    if (16 <= cell && cell <= 25)
                    {
                        // 16点～25点の場合
                        return true;
                    }
                    else if (33 <= cell && cell <= 58)
                    {
                        // 33点～58点の場合
                        return true;
                    }
                    else if (65 <= cell && cell <= 90)
                    {
                        // 65点～90点の場合
                        return true;
                    }

                    break;

                case 4: // 4区の場合
                    if (1 <= cell && cell <= 83)
                    {
                        // 1点～83点の場合
                        return true;
                    }

                    break;

                case 5: // 5区の場合
                    if (1 <= cell && cell <= 86)
                    {
                        // 1点～86点の場合
                        return true;
                    }

                    break;

                case 6: // 6区の場合
                    if (1 <= cell && cell <= 24)
                    {
                        // 1点～24点の場合
                        return true;
                    }
                    else if (33 <= cell && cell <= 56)
                    {
                        // 33点～56点の場合
                        return true;
                    }

                    break;

                case 7: // 7区の場合
                    if (1 <= cell && cell <= 33)
                    {
                        // 1点～33点の場合
                        return true;
                    }
                    else if (49 <= cell && cell <= 81)
                    {
                        // 49点～81点の場合
                        return true;
                    }

                    break;

                case 8: // 8区の場合
                    if (1 <= cell && cell <= 32)
                    {
                        // 1点～32点の場合
                        return true;
                    }

                    break;

                default:
                    if (16 <= row && row <= 46) // 16区～46区の場合
                    {
                        if (1 <= cell && cell <= 94)
                        {
                            // 1点～94点の場合
                            return true;
                        }
                    }
                    else if (row == 47) // 47区の場合
                    {
                        if (1 <= cell && cell <= 51)
                        {
                            // 1点～51点の場合
                            return true;
                        }
                    }
                    else if (48 <= row && row <= 83) // 48区～83区の場合
                    {
                        if (1 <= cell && cell <= 94)
                        {
                            // 1点～94点の場合
                            return true;
                        }
                    }
                    else if (row == 84) // 84区の場合
                    {
                        if (1 <= cell && cell <= 6)
                        {
                            // 1点～6点の場合
                            return true;
                        }
                    }

                    break;
            }

            return false;
        }

        #endregion
    }
}