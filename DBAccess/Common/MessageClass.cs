
namespace DBAccess.Model
{
    /// <summary>
    /// メッセージのクラス定義です。
    /// </summary>
    public static class MessageClass
    {
        public const string M0001 = "更新が完了しました。";
        public const string CM0001 = "更新します、よろしいですか？";
        public const string CM0002 = "削除します、よろしいですか？";
        public const string EM0001 = "{0}は必須入力です。";
        public const string EM0002 = "{0}は{1}文字以内で入力してください。";
        public const string EM0003 = "ユーザーIDは存在しません。";
        public const string EM0004 = "パスワードが一致しません。";
        public const string EM0005 = "{0}を正しく選択して下さい。";
        public const string EM0006 = "パスワードを半角英字大文字と半角英字小文字と半角数字それぞれ1文字以上含む8文字以上で入力してください。";
        public const string EM0007 = "{0}は数字のみです。";
        public const string EM0008 = "{0}は7桁の数字で入力してください。";
        public const string EM0009 = "{0}は10桁以上の数字で入力してください。";
        public const string EM0010 = "正しいメールアドレスを入力してください。";
        public const string EM0011 = "{0}を選択して下さい。";
        public const string EM0012 = "{0}は全角カナで入力して下さい。";
        public const string EM0013 = "{0}を入力して下さい。";
        public const string EM0014 = "他のマスターで使用しています、削除できません。";
        public const string EM0015 = "{0}は {1:d} から {2:d} の間の数字を入力してください。";
        public const string EM0016 = "認証エラー";
        public const string EM0017 = "鍵NOが未登録です。";
        public const string EM0018 = "コードが重複しています。";
        public const string EM0019 = "行が選択されていません。";
        public const string EM0020 = "{0}は半角カナで入力して下さい。";
        public const string EM0996 = "登録エラーが発生しました。しばらく待ってから再度試してみて下さい。";
        public const string EM0997 = "パラメータエラーです。";
        public const string EM0998 = "既に登録済みです。";
        public const string EM0999 = "他者によりすでにデータが更新もしくは削除されている可能性があるため\n処理を中断しました。\n現在開いているページを再度メニューから選択し、画面を更新してください。\nその後、必要であれば操作をやり直してください。";
        public const string EM1000 = "システムエラーです。システム管理者までご連絡ください。";
    }
}