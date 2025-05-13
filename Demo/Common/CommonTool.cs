using System.Text.Json;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using DBAccess.Context;
using DBAccess.Model;
using LogWriters;

namespace Demo.Common
{
    /// <summary>
    /// 共通関数を定義します。
    /// </summary>
    public static class CommonTool
    {
        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        /// <param name="jsRuntime">JSランタイム</param>
        /// <param name="message">メッセージ</param>
        public static async Task ErrorMessage(IJSRuntime jsRuntime, string message)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Mes_Error", message);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
            }
        }

        /// <summary>
        /// サクセスメッセージ表示
        /// </summary>
        /// <param name="jsRuntime">JSランタイム</param>
        /// <param name="message">メッセージ</param>
        public static async Task SuccessMessage(IJSRuntime jsRuntime, string message)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Mes_Success", message);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
            }
        }

        /// <summary>
        /// 問い合わせメッセージ表示
        /// </summary>
        /// <param name="jsRuntime">JSランタイム</param>
        /// <param name="message">メッセージ</param>
        public static async Task<bool> ConfirmMessage(IJSRuntime jsRuntime, string message)
        {
            try
            {
                var confirmed = await jsRuntime.InvokeAsync<string>("Mes_Confirm", message);
                if (confirmed.ToString() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// インフォメーションメッセージ表示
        /// </summary>
        /// <param name="jsRuntime">JSランタイム</param>
        /// <param name="message">メッセージ</param>
        public static async Task InfomationMessage(IJSRuntime jsRuntime, string message)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("Mes_info", message);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
            }
        }

        /// <summary>
        /// Page 遷移
        /// </summary>
        /// <param name="helper">NavigationManager</param>
        /// <param name="Page">移動先ページ</param>
        /// <param name="opt">オプション　true：NewWindow</param>
        public static void OpenPage(NavigationManager helper, string Page, bool opt = false)
        {
            helper.NavigateTo(Page, opt);
        }

        /// <summary>
        /// 共通エラー処理
        /// </summary>
        /// <param name="ex">エクセプション</param>
        public static void CommonError(Exception ex)
        {
            try
            {
                LogWriter.Error(ex);
            }
            catch (Exception e)
            {
                LogWriter.Error(e);
                if (e.InnerException != null)
                {
                    LogWriter.Error(e.InnerException);
                }
            }
        }

        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        /// <param name="jsRuntime">JSランタイム</param>
        /// <param name="fileName">ファイル名</param>
        public static async Task DownloadFile(IJSRuntime jsRuntime, string fileName)
        {
            await jsRuntime.InvokeVoidAsync("DownLoad", fileName);
        }


    }
}