using DBAccess.Model;
using Demo.Common;
using DevExpress.Blazor;
using Model.AllCommon;

namespace Demo.Pages
{

    public partial class OcrImportResult : Base
    {
        public string SuitNo { get; set; } = "S800000001";

        /// <summary>OCR取込リスト</summary>
        private IList<TOcr> TOcrList = new List<TOcr>();

        /// <summary>OCR取込リスト</summary>
        private IList<TOcrMoto> TOcrMotoList = new List<TOcrMoto>();

        /// <summary>入出金表マスタリスト</summary>
        private IList<MDepositTable> MDepositTableList = new List<MDepositTable>();

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>グリッド</summary>
        private IGrid GridMoto { get; set; } = new DxGrid();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>編集データ</summary>
        private TOcr EntryData { get; set; } = new TOcr();


        private string? ImagePath = null;


        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            OnButtonSelect();
            await Task.Delay(2);
        }



        /// <summary>
        /// 検索処理
        /// </summary>
        private void OnButtonSelect()
        {
            TOcrList = _CommonDataService.GetTOcrList();
            TOcrMotoList = _CommonDataService.GetTOcrMotoList();

            TOcrList = TOcrList.Where(x => x.SuitNo.Equals(SuitNo.Trim())).ToList();
            TOcrMotoList = TOcrMotoList.Where(x => x.SuitNo.Equals(SuitNo.Trim())).ToList();

            int rowno = 1;
            for(int c=0; c < TOcrList.Count; c++)
            {
                TOcrList[c].RowNo = rowno++;
            }
            rowno = 1;
            for (int c = 0; c < TOcrMotoList.Count; c++)
            {
                TOcrMotoList[c].RowNo = rowno++;
            }
            if(TOcrList.Count > 0)
            {
                ImagePath = @"./uploads/ybszvg1a.h5h.jpg";
            }
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
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            SelectedDataItem = e.DataItem;
            EntryData = (TOcr)e.EditModel;

            if (EntryData.ChangeSw != CDef.CHANGE_SW.INSERT.Code)
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
            }



            for (int c = 0; c < TOcrList.Count; c++)
            {
                if (TOcrList[c].Id == EntryData.Id)
                {
                    TOcrList[c] = EntryData;
                }
            }
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="e"></param>
        private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            EntryData = (TOcr)e.EditModel;
            //await UpdateDataAsync();
            await Task.Delay(10);
        }


        /// <summary>
        /// 登録ボタン
        /// </summary>
        private async Task EditModelSaving()
        {
            await UpdateDataAsync();

            MDepositTableList = new List<MDepositTable>();
            foreach (var data in TOcrList)
            {
                var id = _CommonDataService._CommonLogic.GetSeq("M_DEPOSIT_TABLE_id_seq");
                var mDepositTable = new MDepositTable()
                {
                    ChangeSw = CDef.CHANGE_SW.INSERT.Code,
                    Id = id,
                    DepositId = id,
                    SuitNo = SessionInfo._MSuit.SuitNo,
                    AccountsReceivable = data.AzukariKin,
                    AccountsPayable = data.SiharaiKin,
                    Zandaka = data.Zandaka,
                    Apply = data.Apply,
                };
                if (mDepositTable.AccountsPayable > 0)
                {
                    mDepositTable.SourcePayment = data.Apply2;
                }
                else
                {
                    mDepositTable.PaymentDestination = data.Apply2;
                }
                if (!string.IsNullOrEmpty(data.Yyyy) && !string.IsNullOrEmpty(data.Mm) && !string.IsNullOrEmpty(data.Dd))
                {
                    var date = data.Yyyy + "-" + data.Mm + "-" + data.Dd;
                    mDepositTable.AccessDate = DateTime.Parse(date);
                }


                MDepositTableList.Add(mDepositTable);
            }
            // 更新処理
            var cnt = this._CommonDataService.InsertMDepositTable(MDepositTableList);

            var page = await GetOyaPage();
            _NavigationManager.NavigateTo(page);
            await Task.Delay(10);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns></returns>
        private async Task UpdateDataAsync()
        {
            // 更新処理
            var cnt = this._CommonDataService.InsertOcr(TOcrList);
            cnt = this._CommonDataService.InsertOcrMoto(TOcrMotoList);
            await Task.Delay(10);
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (TOcr)e.DataItem;
            EntryData.ChangeSw = CDef.CHANGE_SW.DELETE.Code;
            await UpdateDataAsync();
            await Task.Delay(10);
        }
    }
}
