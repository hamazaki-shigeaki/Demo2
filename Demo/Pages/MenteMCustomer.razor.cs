using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DevExpress.Blazor;

using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Service;
using Demo.Common;

namespace Demo.Pages
{
    public partial class MenteMCustomer : Base
    {
        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>顧客マスタリスト</summary>
        private IList<MCustomer> MCustomerList = new List<MCustomer>();

        /// <summary>編集データ</summary>
        private MCustomer EntryData { get; set; } = new MCustomer();


        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMCustomer 画面");

            // 顧客マスタ一覧取得
            GetMCustomerList();

            await Task.Delay(2);
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
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            EntryData = (MCustomer)e.EditModel;
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
            EntryData = (MCustomer)e.EditModel;

            await UpdateDataAsync();
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MCustomer)e.DataItem;
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
                // 更新処理
                var cnt = this._CommonDataService.UpdateMCustomer(EntryData);

                // 再読み込み
                GetMCustomerList();

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
        }

        /// <summary>
        /// 顧客マスタ読込
        /// </summary>
        private void GetMCustomerList()
        {
            MCustomerList = _CommonDataService.GetMCustomerList();

            int rowNo = 1;
            foreach (var data in MCustomerList)
            {
                data.RowNo = rowNo;
            }
        }
    }
}
