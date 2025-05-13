using DevExpress.Blazor;

using Model.AllCommon;
using DBAccess.Model;
using Demo.Common;
using SiReports.CreateReports;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Pages
{

    public partial class DepositTable : Base
    {
        [Inject]
        /// <summary>JsRuntime</summary>
        protected IJSRuntime _JsRuntime { get; set; }

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>入出金表マスタリスト</summary>
        private IList<MDepositTable> MDepositTableList = new List<MDepositTable>();

        /// <summary>銀行マスタリスト</summary>
        private IList<MBank> MBankList = new List<MBank>();

        /// <summary>編集データ</summary>
        private MDepositTable EntryData { get; set; } = new MDepositTable();

        private string SuitNo { get; set; } = string.Empty;

        private long Kingaku { get; set; } = 100000;

        public bool EnabledButtonPrint { get; set; } = true;

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

            // 銀行マスタ一取得
            MBankList = _CommonDataService.GetMBankList();

            // 入出金一覧取得
            GetMDepositTableList();


            await Task.Delay(2);
        }

        /// <summary>
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            SelectedDataItem = e.DataItem;
            EntryData = (MDepositTable)e.EditModel;

            EntryData.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="e"></param>
        private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            EntryData = (MDepositTable)e.EditModel;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        private async Task UpdateDataAsync()
        {
            try
            {
                // 更新処理
                var cnt = this._CommonDataService.UpdateMDepositTable(EntryData);

                // 再読み込み
                // 入出金一覧取得
                GetMDepositTableList();

                // グリッド　リロード
                Grid.Reload();

                // ステート　チェンジ
                StateHasChanged();

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(JsRuntime, ex.Message);
            }
            await Task.Delay(10);
        }

        private async Task OnButtonSelect()
        {
            GetMDepositTableList();
            await Task.Delay(10);
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MDepositTable)e.DataItem;
            EntryData.ChangeSw = CDef.CHANGE_SW.DELETE.Code;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        /// <summary>
        /// 選択行変更処理
        /// </summary>
        /// <param name="e"></param>
        void OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (e.DataItem != null)
            {
                SelectedDataItem = e.DataItem;
            }
        }

        /// <summary>
        /// 入出金表読込
        /// </summary>
        private void GetMDepositTableList()
        {
            MDepositTableList = _CommonDataService.GetMDepositTableList();
            MDepositTableList = MDepositTableList.Where(x => x.SuitNo.Equals(SessionInfo._MSuit.SuitNo)).ToList();
            MDepositTableList = MDepositTableList.Where(x => (CompKingaku(x.AccountsPayable,Kingaku) ||
                                                              CompKingaku(x.AccountsReceivable, Kingaku))).ToList();
            int rowNo = 1;
            foreach (var data in MDepositTableList)
            {
                data.RowNo = rowNo;
                data.CompanyName = SessionInfo._MSuit._MCustomer.CompanyName;
                var list = MBankList.Where(x => x.BankCd.Equals(data.BankCd)).ToList();
                if(list.Count > 0)
                {
                    data.BankName = list[0].BankName;
                }
            }
        }

        public bool CompKingaku(long? a, long b)
        {
            long kingakiA = 0;
            long kingakiB= b;

            if (a.HasValue)
            {
                kingakiA = (long)a;
            }

            if(kingakiA > kingakiB)
            {
                return true;
            }
            return false;
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
        /// バッチOCR取込み
        /// </summary>
        private async Task OnButtonOcrImport()
        {
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("OcrImport");
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
        /// Excel 出力
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonExcel()
        {
            try
            {
                EnabledButtonPrint = false;

                var reports = new CreateReportsP001();

                var ret = reports.CreateReports(SessionInfo, MDepositTableList);

                if (ret.Status == SiReports.Model.PrintStatus.PrintOK)
                {
                    // 帳票ダウンロード
                    var fileName = Path.GetFileName(ret.ExcelFilePath);
                    await CommonTool.DownloadFile(this._JsRuntime, fileName);
                }

            }
            catch (Exception ex)
            {
                LogWriters.LogWriter.Error(ex);
            }
            finally
            {
                EnabledButtonPrint = true;
            }
            await Task.Delay(1);
        }
    }
}
