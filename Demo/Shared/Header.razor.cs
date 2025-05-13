using Microsoft.AspNetCore.Components;

namespace Demo.Shared
{
    public partial class Header
    {
        [Inject]
        /// <summary>ナビゲーションマネージャー</summary>
        private NavigationManager _NavigationManager { get; set; }

        public void ButtonLogout()
        {
            // 画面遷移
            _NavigationManager.NavigateTo("/", true);
        }
    }
}
