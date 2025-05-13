using DBAccess.Context;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBAccess.Model;
using System.Net.Http.Headers;

namespace DBAccess.Service
{
    /// <summary>
    /// Api 通信サービス
    /// </summary>
    public class MyApiService
    {
        /// <summary> 
        /// API呼び出し（JSON + HTTPS対応）
        /// </summary>
        /// <param name="url">APIエンドポイント（例: https://api.smartread.jp/v4/job）</param>
        /// <param name="reqJson">JSON文字列</param>
        /// <param name="apiKey">APIキー（例: "your_api_key_here"）</param>
        /// <returns>APIレスポンスの文字列</returns>
        public async Task<string> CallApiJsonAsync(string url, string reqJson, string apiKey)
        {
            try
            {
                using var handler = new HttpClientHandler
                {
                    // 必要に応じてSSL証明書の検証設定をここで制御できる
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Authorization ヘッダーを追加
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);

                Console.WriteLine($"Authorizationヘッダー: {apiKey}");

                var content = new StringContent(reqJson, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                }

                var res = await response.Content.ReadAsStringAsync();
                return res;
            }
            catch (Exception ex)
            {
                LogWriters.LogWriter.Error(ex);
                throw;
            }
        }
    }
}