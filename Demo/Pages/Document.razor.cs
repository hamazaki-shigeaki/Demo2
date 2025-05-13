using DevExpress.Blazor;

using DBAccess.Model;
using Demo.Common;
using Syncfusion.Blazor.PdfViewerServer;
using Model.AllCommon;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using DevExpress.ClipboardSource.SpreadsheetML;
using Org.BouncyCastle.Bcpg;

namespace Demo.Pages
{

    public partial class Document : Base
    {
        [Inject]
        /// <summary>JsRuntime</summary>
        protected IJSRuntime _JsRuntime { get; set; }

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ポップアップ画面表示</summary>
        bool PopupVisible { get; set; } = false;


        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>入出金表マスタリスト</summary>
        private IList<MDocument> MDocumentList = new List<MDocument>();

        /// <summary>区分マスタリスト</summary>
        private IList<MKubun> MKubunList = new List<MKubun>();

        /// <summary>文書区分・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombDocClassificationList = new List<CommonCombBox>();

        /// <summary>編集データ</summary>
        private MDocument EntryData { get; set; } = new MDocument();

        private string SuitNo { get; set; } = string.Empty;

        /// <summary>PDFファイルパス</summary>
        string PdfPath { get; set; } = @"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Document";

        /// <summary>PDF画面表示</summary>
        bool PopupPdfVisible { get; set; } = false;

        /// <summary>PDFビュワー</summary>
        SfPdfViewerServer pdfViewer;

        bool UploadVisible { get; set; } = false;

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMCustomer 画面");

            if (SessionInfo._MSuit != null)
            {
                SuitNo = SessionInfo._MSuit.SuitNo + " / " + SessionInfo._MSuit._MCustomer.CompanyName; ;
            }

            // 区分マスタ
            MKubunList = _CommonDataService.GetMKubunList();

            // コンボボックスデータ設定
            using (var tool = new CreateListBoxData(this._CommonDataService))
            {
                // 文書区分コンボ
                var list = MKubunList.Where(x => x.KubunId == 5).ToList();
                CombDocClassificationList = new List<CommonCombBox>();
                foreach (var data in list)
                {
                    CombDocClassificationList.Add(new CommonCombBox() { Value = data.Code, Text = data.Name });
                }
                CombDocClassificationList = tool.MakeList(CombDocClassificationList, 2, true);

            };

            GetMDocumentList();

            await Task.Delay(2);
        }

        /// <summary>
        /// 預かり書類読込
        /// </summary>
        private void GetMDocumentList()
        {
            MDocumentList = _CommonDataService.GetMDocumentList();
            MDocumentList = MDocumentList.Where(x => x.SuitNo.Equals(SessionInfo._MSuit.SuitNo)).ToList();                // 状況コンボ
            
            var docList = MKubunList.Where(x => x.KubunId == 5).ToList();

            int rowNo = 1;
            foreach (var data in MDocumentList)
            {
                data.RowNo = rowNo;
                var list = docList.Where(x => x.Code.Equals(data.DocClassification)).ToList();
                if(list.Count > 0){
                    data.StrDocClassification = list[0].Name;
                }
            }
        }

        /// <summary>
        /// OCR取込み
        /// </summary>
        private async Task OnButtonOcr()
        {
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("Ocr");
        }

        /// <summary>
        /// クリアボタン押下
        /// </summary>
        /// <returns></returns>
        private async Task OnClear()
        {
            // グリッド初期化
            Grid.ClearSort();
            Grid.ClearSelection();
            Grid.ClearFilter();
            OnFilterRow = false;
            OnGroupRow = false;

            await Task.Delay(1);
        }
        /// <summary>
        /// PDF表示ボタン処理
        /// </summary>
        /// <param name="data"></param>
        private void OnClickPdfOpen(object data)
        {
            var mDocument = (MDocument)data;
            if (string.IsNullOrEmpty(mDocument.FileName))
            {
                PdfPath = string.Empty;
                PopupPdfVisible = false;
            }
            else
            {
                PdfPath = mDocument.FolderName + @"/" + mDocument.FileName;
                PopupPdfVisible = true;
            }
        }

        /// <summary>
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            EntryData = (MDocument)e.EditModel;
            if (e.IsNew)
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.INSERT.Code;
            }
            else
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
            }
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="e"></param>
        private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            EntryData = (MDocument)e.EditModel;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MDocument)e.DataItem;
            EntryData.ChangeSw = CDef.CHANGE_SW.DELETE.Code;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private async Task UpdateDataAsync()
        {
            try
            {
                if (EntryData.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
                {
                    EntryData.SuitNo = this.SessionInfo._MSuit.SuitNo;
                    EntryData.Id = this._CommonDataService._CommonLogic.GetSeq("M_DOCUMENT_id_seq");
                    EntryData.DocumentNo = ((long)EntryData.Id).ToString();
                }

                // 更新処理
                var cnt = this._CommonDataService.UpdateMDocument(EntryData);

                // 文書マスタ読み込み
                GetMDocumentList();

                // スタータス更新
                StateHasChanged();

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(this._JsRuntime, ex.Message);
            }
        }

        /// <summary>
        /// PDFファイルアップロード
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonUploadPdf()
        {
            PopupVisible = true;
            await Task.Delay(10);
        }

        /// <summary>
        /// PDFファイルアップロード
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonUpload()
        {
            PopupVisible = true;
            await Task.Delay(10);
        }
        protected string GetUploadUrl(string url)
        {
            return NavigationManager.ToAbsoluteUri(url).AbsoluteUri;
        }

        protected void SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            if (files.ToList().Count > 0)
            {
                UploadVisible = true;

                foreach (var data in files)
                {
                    EntryData.FileName = data.Name;
                    EntryData.FolderName = @"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Document";
                }
            }
            else
            {
                UploadVisible = false;
            }

            InvokeAsync(StateHasChanged);
        }

        private void OnButtonClose()
        {
            PopupVisible = false;

        }

    }
}
