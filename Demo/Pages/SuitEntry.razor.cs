using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DevExpress.Blazor;

using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Service;
using Demo.Common;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Blazor.PdfViewerServer;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.Import.Doc;
using System.Reflection;
using DevExpress.Blazor.Reporting.Models;
using DevExpress.CodeParser;

namespace Demo.Pages
{
    /// <summary>
    /// 相続案件一覧
    /// </summary>
    public partial class SuitEntry : Base
    {
        [Inject]
        /// <summary>JsRuntime</summary>
        protected IJSRuntime _JsRuntime { get; set; }

        /// <summary>CommonDataService</summary>
        [Inject]
        /// <summary>DBアクセスサービス</summary>
        protected CommonDataService _CommonDataService { get; set; }

        [Inject]
        /// <summary>ナビゲーションマネージャー</summary>
        private NavigationManager _NavigationManager { get; set; }

        private bool PdfPopUpVisible = false;

        private string filePath;

        byte[] DocumentContent { get; set; }

        private bool PopupVisible { get; set; } = false;

        /// <summary> 編集データ</summary>
        private MSuit EntryData { get; set; } = new MSuit();

        /// <summary>状況・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombSituationList = new List<CommonCombBox>();

        /// <summary>有無・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombUmeist = new List<CommonCombBox>();

        /// <summary>顧客コード・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombCustomerList = new List<CommonCombBox>();

        /// <summary>税区分・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombZeiKbnList = new List<CommonCombBox>();

        /// <summary>区分マスタリスト</summary>
        private IList<MKubun> MKubunList = new List<MKubun>();

        /// <summary>顧客マスタリスト</summary>
        private IList<MCustomer> MCustomerList = new List<MCustomer>();

        /// <summary>PDFファイルパス</summary>
        string PdfPath { get; set; } = @"./wwwroot/Upload/仮審査申し込み用紙.pdf";

        /// <summary>PDFビュワー</summary>
        SfPdfViewerServer pdfViewer = new();

        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        private bool pdfViewerOn = false;
        private bool JpgViewerOn = false;

        private string[] ImageUrl { get; set; } = { "" };

        private string FileName { get; set; } = string.Empty;

        bool UploadVisible { get; set; } = false;

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("SuitEntry 画面");


            // 区分マスタ
            MKubunList = _CommonDataService.GetMKubunList();

            // 顧客マスタ
            MCustomerList = _CommonDataService.GetMCustomerList();

            int rowNo = 1;
            foreach (var data in MCustomerList)
            {
                data.RowNo = rowNo;
            }

            // コンボボックスデータ設定
            using (var tool = new CreateListBoxData(this._CommonDataService))
            {
                // 状況コンボ
                var list = MKubunList.Where(x => x.KubunId == 1).ToList();
                CombSituationList = new List<CommonCombBox>();
                foreach (var data in list)
                {
                    CombSituationList.Add(new CommonCombBox() { Value = data.Code, Text = data.Name });
                }
                CombSituationList = tool.MakeList(CombSituationList, 2, true);

                // コンボボックスデータ設定
                CombUmeist = tool.GetUmuComb();
                CombZeiKbnList = tool.GetZeiComb();

                // 顧客コード
                foreach (var data in MCustomerList)
                {
                    var newData = new CommonCombBox()
                    {
                        Text = data.CompanyName,
                        Value = data.CustomerCd
                    };
                    CombCustomerList.Add(newData);
                }
                CombCustomerList = tool.MakeList(CombCustomerList, 0, true);
            };
            EntryData.Situation = CDef.Situation.Situation_01.Code;

            await Task.Delay(2);
        }

        /// <summary>
        /// 確定ボタン・バリデーションエラー処理
        /// </summary>
        private void HandleValidSubmit()
        {
        }

        /// <summary>
        /// 確定ボタン・バリデーションエラー処理
        /// </summary>
        private void HandleInvalidSubmit()
        {
            Console.WriteLine("バリデーションチェック：エラー");
        }

        /// <summary>
        /// 新規登録
        /// </summary>
        private async Task OnButtonEntry()
        {
            // 入力チェック

            // 登録処理
            try
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.INSERT.Code;
                if (string.IsNullOrEmpty(EntryData._MCustomer.CustomerCd))
                {
                    EntryData._MCustomer.ChangeSw = CDef.CHANGE_SW.INSERT.Code;
                }
                else
                {
                    EntryData._MCustomer.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
                }

                // 更新処理
                var cnt = this._CommonDataService.UpdateMSuit(EntryData);

                // スタータス更新
                StateHasChanged();

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(this._JsRuntime, ex.Message);
            }


            // 一覧に遷移
            await JumpPage("SuitList");
        }

        protected void SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            if (files.ToList().Count > 0)
            {
                UploadVisible = true;

                foreach (var data in files)
                {
                    FileName = data.Name;
                }
            }
            else
            {
                UploadVisible = false;
            }

            InvokeAsync(StateHasChanged);
        }
        protected string GetUploadUrl(string url)
        {
            return NavigationManager.ToAbsoluteUri(url).AbsoluteUri;
        }

        private void OnButtonCopy()
        {
            EntryData = new MSuit();
            EntryData._MCustomer = MCustomerList[0];
            EntryData.Situation = CDef.Situation.Situation_01.Code;

            EntryData._MCustomer.Id = null;
            EntryData._MCustomer.CustomerCd = null;

            PopupVisible = false;

            PdfPath = @"./wwwroot/Upload/" + FileName;

            pdfViewerOn = false;
            if (FileName.Contains(".pdf"))
            {
                pdfViewerOn = true;
            }
        }

        private void OnButtonClose()
        {
            PopupVisible = false;

        }

        /// <summary>
        /// 取込み
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonTorikomi()
        {
            PopupVisible = true;
            await Task.Delay(10);
        }

        /// <summary>
        /// 顧客コンボ変更
        /// </summary>
        /// <param name="args"></param>
        void OnSelectedCityChanged(SelectedDataItemChangedEventArgs<CommonCombBox> args)
        {
            pdfViewerOn = false;

            if (args.DataItem != null)
            {
                var list = MCustomerList
                   .Where(x => string.Equals(x.CustomerCd, args.DataItem.Value?.ToString(), StringComparison.Ordinal))
                   .ToList();
                if (list.Count > 0)
                {
                    EntryData._MCustomer = list[0];
                }
            }
        }

        protected void OnCustomizeToolbar(ToolbarModel toolbarModel)
        {
            var printToolbarItem = toolbarModel.AllItems.Where(i => i.Id == ToolbarItemId.Print).FirstOrDefault();
            if (printToolbarItem != null)
            {
                printToolbarItem.IconCssClass = "print-btn";
            }
            toolbarModel.AllItems.Clear();

            var downloadToolbarItem = new ToolbarItem
            {
                Text = "Download",
                AdaptiveText = "Download",
                BeginGroup = true,
                Id = "Download",
                IconCssClass = "download-btn",
                Click = async (args) =>
                {
                    await pdfViewer.DownloadAsync();
                }
            };
        }

        private string imageDataUrl;
        private string imagePath;

        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;


            if (file != null)
            {
                var uploadsFolder = "/wwwroot/Upload";
                Directory.CreateDirectory(uploadsFolder); // フォルダがなければ作成

                FileName = file.Name;
                var filePath = Path.Combine(uploadsFolder, FileName);

                await using var stream = file.OpenReadStream(5 * 1024 * 1024); // 最大5MB
                await using var fileStream = File.Create(filePath);
                await stream.CopyToAsync(fileStream);

                // ブラウザで表示するためのURL
                imagePath = @"./wwwroot/Upload/77.jpg";

            }
        }
    }
}
