using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Routing;
using Demo.Common;
using DBAccess.Service;
using DBAccess.Context;
using Demo.ViewModel;

namespace Demo.Shared
{
    /// <summary>
    /// メインレイアウト
    /// </summary>
    public partial class MainLayout
    {
        /// <summary>詳細画面表示用パラメータ</summary>
        [Parameter]
        public bool IsFastDisplay { get; set; } = true;

        [Inject]
        /// <summary>セッションストレージ</summary>
        private ProtectedLocalStorage sessionStorage { set; get; }

        [Inject]
        /// <summary>Javascriptランタイム</summary>
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        /// <summary>ナビゲーションマネージャー</summary>
        private NavigationManager _NavigationManager { get; set; }


        /// <summary>入力用ビューモデル</summary>
        public MainLayoutViewModel FormModel = new MainLayoutViewModel();

        /// <summary>CommonDataService</summary>
        [Inject]
        /// <summary>DBアクセスサービス</summary>
        protected CommonDataService _CommonDataService { get; set; }

        string? NavMenuCssClass { get; set; }
        bool _isMobileLayout;
        bool IsMobileLayout
        {
            get => _isMobileLayout;
            set
            {
                _isMobileLayout = value;
                IsSidebarExpanded = !_isMobileLayout;
            }
        }

        bool _isSidebarExpanded = true;
        bool IsSidebarExpanded
        {
            get => _isSidebarExpanded;
            set
            {
                if (_isSidebarExpanded != value)
                {
                    NavMenuCssClass = value ? "expand" : "collapse";
                    _isSidebarExpanded = value;
                }
            }
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void OnInitialized()
        {
            SessionStorage.SessionInfoDelete(sessionStorage);

        }

        /// <summary>
        /// 描画完了後処理
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _CommonDataService._Context.UserId = "SYSTEM";
                await Task.Delay(10);
            }
        }

        /// <summary>
        /// ロケーション変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        async void OnLocationChanged(object? sender, LocationChangedEventArgs args)
        {
            if (IsMobileLayout)
            {
                IsSidebarExpanded = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// ログインボタン処理
        /// </summary>
        private async Task HandleValidSubmit()
        {
            try
            {
                // 画面遷移
                NavigationManager.NavigateTo("./Top");

                IsFastDisplay = false;
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(JSRuntime, ex.Message);
            }
        }


        /// <summary>
        /// バリデーションエラー処理
        /// </summary>
        private void HandleInvalidSubmit()
        {
            Console.WriteLine("HandleInvalidSubmit");
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

    }
}
