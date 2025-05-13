using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Demo.Pages {

    /// <summary>
    /// エラー画面
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel {

        /// <summary>リクエストID</summary>
        public string? RequestId { get; set; }

        /// <summary>表示リクエストID</summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>ロガー</summary>
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>エラーモデル</summary>
        public ErrorModel(ILogger<ErrorModel> logger) {
            _logger = logger;
        }

        /// <summary>リスエストID取得</summary>
        public void OnGet() {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}