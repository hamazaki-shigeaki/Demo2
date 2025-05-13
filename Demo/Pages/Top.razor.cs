using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DevExpress.Blazor;

using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Service;
using Demo.Common;

namespace Demo.Pages
{
    public partial class Top : Base
    {
        public int Kensu01 { get; set; } = 0;
        public int Kensu02 { get; set; } = 0;
        public int Kensu03 { get; set; } = 0;
        public string DispKensu01 { get; set; } = string.Empty;
        public string DispKensu02 { get; set; } = string.Empty;
        public string DispKensu03 { get; set; } = string.Empty;

        /// <summary>案件マスタリスト</summary>
        private IList<MSuit> MSuitList = new List<MSuit>();

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("Top 画面");

            Kensu01 = 0;
            Kensu02 = 0;
            Kensu03 = 0;
            MSuitList = _CommonDataService.GetMSuitList();
            foreach (var data in MSuitList)
            {
                if (data.Situation == CDef.Situation.Situation_01.Code)
                {
                    Kensu01++;
                }
                if (data.Situation == CDef.Situation.Situation_02.Code)
                {
                    Kensu02++;
                }
                if (data.Situation == CDef.Situation.Situation_03.Code)
                {
                    Kensu03++;
                }
            }
            DispKensu01 = Kensu01.ToString() + " 件";
            DispKensu02 = Kensu02.ToString() + " 件";
            DispKensu03 = Kensu03.ToString() + " 件";
        }

        /// <summary>
        /// OCR結果確認
        /// </summary>
        private async Task OnOcrImportResult()
        {
            await JumpPage("OcrImportResult");
        }
    }
}