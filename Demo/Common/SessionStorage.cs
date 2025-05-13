using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using DBAccess.Model;

namespace Demo.Common
{
    /// <summary>
    /// セッションストレージクラス
    /// </summary>
    public static class SessionStorage
    {
        public const string Session_Key = "Session";

        /// <summary>
        /// セッション読込み
        /// </summary>
        /// <param name="sessionStorage">セッションストレージ</param>
        /// <returns>セッション情報</returns>
        public static async Task<SessionModel> SessionInfoRead(ProtectedLocalStorage sessionStorage)
        {
            var json = await sessionStorage.GetAsync<string>(Session_Key);
            if (json.Value == null)
            {
                return new SessionModel();
            }
            var jsonStr = json.Value.ToString();
            var retInfo = JsonSerializer.Deserialize<SessionModel>(jsonStr);
            return retInfo;
        }

        /// <summary>
        /// セッション書込み
        /// </summary> 
        /// <param name="sessionStorage">セッションストレージ</param>
        /// <param name="info">セッション情報</param>
        public static async Task SessionInfoWrire(ProtectedLocalStorage sessionStorage, SessionModel info)
        {
            var json = JsonSerializer.Serialize(info);
            await sessionStorage.SetAsync(Session_Key, json);
        }

        /// <summary>
        /// セッションクリア
        /// </summary> 
        /// <param name="sessionStorage">セッションストレージ</param>
        /// <param name="info">セッション情報</param>
        public static async Task SessionInfoDelete(ProtectedLocalStorage sessionStorage)
        {
            await sessionStorage.DeleteAsync(Session_Key);
        }
    }
}