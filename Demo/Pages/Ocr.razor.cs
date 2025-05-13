using Microsoft.AspNetCore.Components.Forms;
using DBAccess.Model;
using DevExpress.Blazor;
using DevExpress.CodeParser;
using Model.AllCommon;
using GrapeCity.Documents.Excel;
using DevExpress.ClipboardSource.SpreadsheetML;
using SiReports.Tools;


namespace Demo.Pages
{
    /// <summary>
    /// 画像アップロードのクラスです
    /// </summary>
    public partial class Ocr : Base
    {

        /// <summary>OCR取込リスト</summary>
        private IList<TOcr> TOcrList = new List<TOcr>();

        /// <summary>OCR取込リスト</summary>
        private IList<TOcrMoto> TOcrMotoList = new List<TOcrMoto>();

        private string SuitNo { get; set; } = string.Empty;

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>入出金表マスタリスト</summary>
        private IList<MDepositTable> MDepositTableList = new List<MDepositTable>();

        /// <summary>編集データ</summary>
        private TOcr EntryData { get; set; } = new TOcr();

        private string Memo { get; set; } = string.Empty;

        private string? ImagePath;

        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMCustomer 画面");

            if (SessionInfo._MSuit != null)
            {
                SuitNo = SessionInfo._MSuit.SuitNo;
            }
            await Task.Delay(2);
        }


        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file.ContentType != "image/jpeg")
            {
                ImagePath = null;
                return;
            }

            var uploadsFolder = Path.Combine(@"./wwwroot/", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetRandomFileName() + ".jpg";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024).CopyToAsync(stream);
            ImagePath = $"uploads/{fileName}";
            object[,] values = new object[30,2];
            int rowCnt = 25;


            GrapeCity.Documents.Excel.Workbook _Workbook = new GrapeCity.Documents.Excel.Workbook();
            switch (e.File.Name){
                case "555169919872794935_2025-04-25_02_01_02.jpg":
                    _Workbook = DioDocsReports.OpenTemplate(@"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Excel\Test001.xlsx");
                    break;
                case "556625685985099947_2025-04-25_02_00_30.jpg":
                    _Workbook = DioDocsReports.OpenTemplate(@"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Excel\Test002.xlsx");
                    break;
                case "557927699381748018_(1)_2025-04-25_02_01_41.jpg":
                    _Workbook = DioDocsReports.OpenTemplate(@"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Excel\Test003.xlsx");
                    break;
                case "557991729341923330_(1)_2025-04-25_01_59_25.jpg":
                    _Workbook = DioDocsReports.OpenTemplate(@"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Excel\Test004.xlsx");
                    break;
                case "IMG_0449_2025-04-25_02_00_02.jpeg":
                    _Workbook = DioDocsReports.OpenTemplate(@"C:\OBS\git\Deposits-Withdrawals\Demo\Demo\wwwroot\Excel\Test005.xlsx");
                    break;
            }
            IWorksheet worksheet = _Workbook.Worksheets["Sheet1"];

            switch (e.File.Name)
            {
                case "555169919872794935_2025-04-25_02_01_02.jpg":
                    values = (object[,])worksheet.Range["A1:H25"].Value;
                    rowCnt = 25;
                    break;
                case "556625685985099947_2025-04-25_02_00_30.jpg":
                    values = (object[,])worksheet.Range["A1:H26"].Value;
                    rowCnt = 26;
                    break;
                case "557927699381748018_(1)_2025-04-25_02_01_41.jpg":
                    values = (object[,])worksheet.Range["A1:H17"].Value;
                    rowCnt = 17;
                    break;
                case "557991729341923330_(1)_2025-04-25_01_59_25.jpg":
                    values = (object[,])worksheet.Range["A1:H27"].Value;
                    rowCnt = 27;
                    break;
                case "IMG_0449_2025-04-25_02_00_02.jpeg":
                    rowCnt = 30;
                    values = (object[,])worksheet.Range["A1:H30"].Value;
                    break;
            }

            var importDate = DateTime.Now;
            TOcrList = new List<TOcr>();
            for (int c = 1; c < rowCnt; c++)
            {
                TOcrList.Add(
                    new TOcr()
                    {
                        ChangeSw = CDef.CHANGE_SW.INSERT.Code,
                        Id = c,
                        ImportDate = importDate,
                        Seqno = c,
                        SuitNo = SuitNo,
                        Yyyy = values[c, 0]?.ToString() ?? "",
                        Mm = values[c, 1]?.ToString() ?? "",
                        Dd = values[c, 2]?.ToString() ?? "",
                        Apply = values[c, 3]?.ToString() ?? "",
                        Apply2 = values[c, 4]?.ToString() ?? "",
                        SiharaiKin = ToLong(values[c, 5]),
                        AzukariKin = ToLong(values[c, 6]),
                        Zandaka = ToLong(values[c, 7]),
                    }
                );
            }

            TOcrMotoList = new List<TOcrMoto>();
            for (int c = 1; c < rowCnt; c++)
            {
                TOcrMotoList.Add(
                    new TOcrMoto()
                    {
                        ChangeSw = CDef.CHANGE_SW.INSERT.Code,
                        Id = c,
                        ImportDate = importDate,
                        Seqno = c,
                        SuitNo = SuitNo,
                        Yyyy = values[c, 0]?.ToString() ?? "",
                        Mm = values[c, 1]?.ToString() ?? "",
                        Dd = values[c, 2]?.ToString() ?? "",
                        Apply = values[c, 3]?.ToString() ?? "",
                        Apply2 = values[c, 4]?.ToString() ?? "",
                        SiharaiKin = ToLong(values[c, 5]),
                        AzukariKin = ToLong(values[c, 6]),
                        Zandaka = ToLong(values[c, 7]),
                    }
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private long ToLong(object value)
        {
            if (value == null) return 0;
            if (value is long) return (long)value;
            if (value is double) return (long)(double)value;
            if (long.TryParse(value.ToString(), out long result)) return result;
            return 0;
        }

        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
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
                    Apply = data.Apply + " " + data.Apply2,
                };
                if(mDepositTable.AccountsPayable > 0)
                {
                    mDepositTable.SourcePayment = data.Apply2;
                }
                else
                {
                    mDepositTable.PaymentDestination = data.Apply2;
                }
                if(!string.IsNullOrEmpty(data.Yyyy) && !string.IsNullOrEmpty(data.Mm) && !string.IsNullOrEmpty(data.Dd))
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