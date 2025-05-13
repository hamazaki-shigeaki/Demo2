using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DBAccess.Model;
using DBAccess.Service;
using Demo.Common;

namespace Demo.Pages
{
    /// <summary>
    /// ページ共通クラス
    /// </summary>
    public partial class Base : ComponentBase
    {
        #region 変数
        /// <summary>IJSRuntime</summary>
        [Inject]
        /// <summary>JavaScript Runtime</summary>
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>NavigationManager</summary>
        [Inject]
        /// <summary>ナビゲーションマネージャー</summary>
        public NavigationManager _NavigationManager { get; set; }

        /// <summary>セッションストレージ</summary>
        [Inject]
        public ProtectedLocalStorage _ProtectedLocalStorage { set; get; }


        /// <summary>CommonDataService</summary>
        [Inject]
        /// <summary>DBアクセスサービス</summary>
        protected CommonDataService _CommonDataService { get; set; }

       /// <summary>セッション情報</summary>
        public SessionModel SessionInfo { get; set; } = new();

        /// <summary>ページ名</summary>
        public string MyPage;

        /// <summary>エラーメッセージ</summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>情報メッセージ</summary>
        public string InfoMessage { get; set; } = string.Empty;
        #endregion

        #region メイン画面　メソッド

        /// <summary>
        /// 画面初期化イベント処理
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var name = this.ToString();
            var className = name.Split(".");
            this.MyPage = className[className.Length - 1];
            _CommonDataService._Context.Process = this.MyPage;
            await Task.Delay(2);
        }

        /// <summary>
        /// 描画完了後処理
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(200);

                // 初回はセッションストレージから読込
                SessionInfo = await SessionStorage.SessionInfoRead(_ProtectedLocalStorage);
                await Init();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 初期処理(各画面の初期化オリジナル処理再定義用)
        /// </summary>
        protected virtual async Task Init()
        {
            await Task.Delay(1);
        }

        /// <summary>
        /// 画面遷移
        /// </summary>
        /// <param name="page">飛先</param>
        public virtual async Task JumpPage(string page)
        {
            if (page == "") return;
            await SaveOyaPage(MyPage);
            _NavigationManager.NavigateTo(page);
        }

        /// <summary>
        /// 親画面保存処理
        /// </summary>
        /// <param name="page">頁</param>
        public async Task SaveOyaPage(string page)
        {
            SessionInfo.PrePage.Add(page);
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
        }

        /// <summ
        /// 戻るボタン処理
        /// </summary>
        public virtual async Task OnButtonReturn()
        {
            var page = await GetOyaPage();
            _NavigationManager.NavigateTo(page);
        }

        /// <summary>
        /// 親画面取出処理
        /// </summary>
        public virtual async Task<string> GetOyaPage()
        {
            string[] pageArray = new string[20];
            if (SessionInfo.PrePage.Count == 0)
            {
                return "Top";
            }
            else
            {
                var page = SessionInfo.PrePage[SessionInfo.PrePage.Count - 1];
                SessionInfo.PrePage.Remove(page);
                await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
                return page;
            }
        }

        /// <summary>
        /// エラー処理
        /// </summary>
        /// <param name="message">メッセージ</param>
        public virtual async Task ErrorSyori(string message)
        {
            await CommonTool.ErrorMessage(JsRuntime, message);
            await JsRuntime.InvokeVoidAsync("Logout");
        }
        #endregion
    }
}