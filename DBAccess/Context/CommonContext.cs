#define Windows

namespace DBAccess.Context
{
    /// <summary>
    /// 共通定数のクラス定義です。
    /// </summary>
    public static class CommonContext
    {
        /// <summary>サーバー環境リナックス</summary>
        public const string WebServerLinux = "Linux";

        /// <summary>ウインドウズ</summary>
        public const string WebServerWindows = "Windows";

        /// <summary>基本マスタKey</summary>
        public const int BasicKey = 999;

        /// <summary>画面タイトル</summary>
        public static string TitleName = "";

        /// <summary>Web環境</summary>
        public static string WebServer = "";

        /// <summary>DownLoadPath</summary>
        public static string DownLoadPath = "";

        /// <summary>PdfOutPutPath</summary>
        public static string PdfOutPutPath = "";

        /// <summary>初回表示判定用</summary>
        public static bool IsFastDisplay = true;

        /// <summary>ヘルプファイル</summary>
        public static string HelpFileName = "Help.pdf";

        /// <summary>バケット名</summary>
        public static string BucketName = "o-report";

        /// <summary>画面モード（新規）</summary>
        public static string ModeEntry = "Entry";

        /// <summary>画面モード（新規）</summary>
        public static string ModeEntryName = "新規";

        /// <summary>画面モード（変更）</summary>
        public static string ModeEdit = "Edit";

        /// <summary>画面モード（変更）</summary>
        public static string ModeEditName = "変更";

        /// <summary>判定フラグ：トラン</summary>
        public static string HantelFlg_Tran = "0";

        /// <summary>判定フラグ：マスター</summary>
        public static string HantelFlg_Master = "1";

        public static int KousinCnt;


        /// <summary>
        /// TemplateFolder
        /// </summary>
        public static string TemplateFolder = string.Empty;

        /// <summary>
        /// DownLoadFolder
        /// </summary>
        public static string DownLoadFolder = string.Empty;

        /// <summary>
        /// UploadFolder
        /// </summary>
        public static string UploadFolder = string.Empty;
    }
}