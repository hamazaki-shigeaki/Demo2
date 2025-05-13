using DBAccess.Context;
using DBAccess.Model;
using DBAccess.Service;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.AllCommon;
using System.Text.RegularExpressions;

namespace Demo.Pages
{
    public partial class Login
    {

        /// <summary>
        /// 画面初期化イベント処理
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
            await Task.Delay(10);
        }

        /// <summary>
        /// ログインボタン処理
        /// </summary>
        private async Task HandleValidSubmit()
        {
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
        }
    }
}
