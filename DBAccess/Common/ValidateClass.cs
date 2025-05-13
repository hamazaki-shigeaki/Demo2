namespace DBAccess.Common
{
    /// <summary>
    /// バリデーションの定義クラスです。
    /// </summary>
    public static class ValidateClass
    {
        /// <summary>半角数字のみ</summary>
        public const string R0001 = @"^[0-9]+$";

        /// <summary>数字7桁</summary>
        public const string R0002 = @"\d{7}";

        /// <summary>半角カナ</summary>
        public const string R0003 = @"^[ｱ-ﾞ ]+$";

        /// <summary>全角カナ</summary>
        public const string R0004 = @"^[ァ-ヴ!ー　]+$";

        /// <summary>半角のみ</summary>
        public const string R0005 = @"^[ -~｡-ﾟ]+$";

        /// <summary>全角のみ</summary>
        public const string R0006 = @"^[^ -~｡-ﾟ]+$";

        /// <summary>メールアドレス必須でない場合</summary>
        public const string R0007 = @"[\w!#$%&'*+/=?^_@{}\\|~-]+(\.[\w!#$%&'*+/=?^_{}\\|~-]+)*@([\w][\w-]*\.)+[\w][\w-]*";

        /// <summary>数字4桁</summary>
        public const string R0008 = @"\d{4}";

        /// <summary>数字3桁</summary>
        public const string R0009 = @"\d{3}";

        /// <summary>英大文字小文字記号含む8桁以上16桁まで</summary>
        public const string R0010 = @"^(?=.*?[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!-/:-@\[-`\{-~])(?!.*?(.)\1{4,})[0-9a-zA-Z!-/:-@\[-`\{-~]{8,16}$";

        /// <summary>数字13桁</summary>
        public const string R0011 = @"\d{13}";

        /// <summary>数字5桁</summary>
        public const string R0012 = @"\d{5}";

        /// <summary>数字8桁</summary>
        public const string R0013 = @"\d{8}";

        /// <summary>数字2桁</summary>
        public const string R0014 = @"\d{2}";
    }
}