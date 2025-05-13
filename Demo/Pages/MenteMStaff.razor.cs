using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DevExpress.Blazor;

using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Service;
using Demo.Common;

namespace Demo.Pages
{
    /// <summary>
    /// スタッフ・マスター
    /// </summary>
    public partial class MenteMStaff : Base
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

        /// <summary>グリッド</summary>
        private IGrid? Grid { get; set; } = new DxGrid();

        /// <summary>スタッフマスタリスト</summary>
        private IList<MStaff> MStaffList = new List<MStaff>();

        /// <summary>編集データ</summary>
        private MStaff EntryData { get; set; } = new MStaff();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>支店名コンボ</summary>
        private IList<CommonCombBox> CombBankiList = new List<CommonCombBox>();

        /// <summary>性別コンボ</summary>
        private IList<CommonCombBox> CombSexiList = new List<CommonCombBox>();

        /// <summary>
        /// 銀行マスタリスト
        /// </summary>
        private IList<MBank> MBankList = new List<MBank>();

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMStaff 画面");

            // コンボボックスデータ設定
            using (var tool = new CreateListBoxData(this._CommonDataService))
            {
                CombSexiList = tool.CDefCreateListData(CDef.Sex.Values, 1, true);
            };

            // スタッフマスタ一覧取得
            GetMStaffList();

            await Task.Delay(2);
        }

        /// <summary>
        /// スタッフマスタ読込
        /// </summary>
        private void GetMStaffList()
        {
            // 銀行マスタ
            MBankList = _CommonDataService.GetMBankList();
            CombBankiList = new List<CommonCombBox>();
            foreach (var data in MBankList)
            {
                // 支店名
                CombBankiList.Add(new CommonCombBox() { Value = data.SitenCd, Text = data.SitenCd + ":" + data.BranchName });
            }


            MStaffList = _CommonDataService.GetMStaffList();
            MStaffList = MStaffList.OrderBy(x => x.StaffId).ToList();

            int rowNo = 1;
            foreach (var data in MStaffList)
            {
                data.RowNo = rowNo;
                // 性別
                if (!string.IsNullOrEmpty(data.Sex))
                {
                    var sexList = CombSexiList.Where(x => x.Value == data.Sex).ToList();
                    if (sexList.Count > 0)
                    {
                        data.DisplaySex = sexList[0].Text;
                    }
                }
                // 支店名
                if (!string.IsNullOrEmpty(data.SitenCd))
                {
                    var bankList = CombBankiList.Where(x => x.Value == data.SitenCd).ToList();
                    if (bankList.Count > 0)
                    {
                        data.DisplaySitenCd = bankList[0].Text;
                    }
                }
            }
        }

        /// <summary>
        /// 行選択
        /// </summary>
        /// <param name="e"></param>
        void OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            SelectedDataItem = e.DataItem;
        }

        /// <summary>
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            EntryData = (MStaff)e.EditModel;
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
            EntryData = (MStaff)e.EditModel;

            await UpdateDataAsync();
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MStaff)e.DataItem;
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
                var cnt = this._CommonDataService.UpdateMStaff(EntryData);

                // 再読み込み
                GetMStaffList();

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
    }
}
