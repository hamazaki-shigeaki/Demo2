using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using DevExpress.Blazor;
using GrapeCity.Documents.DX;
using static Model.AllCommon.CDef;

using Model.AllCommon;
using DBAccess.Service;
using DBAccess.Common;
using DBAccess.Model;
using Demo.Common;

namespace Demo.Pages
{
    /// <summary>
    /// 区分マスタ
    /// </summary>
    public partial class MenteMKubun : Base
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

        /// <summary>
        /// 区分マスタリスト
        /// </summary>
        private IList<MKubun> MKubunList = new List<MKubun>();

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>イネーブル　コード</summary>
        private bool EnabledCode { get; set; } = true;

        /// <summary>区分ID　リードONLY</summary>
        private bool ReadOnlyKubunId { get; set; } = false;

        /// <summary>登録用　編集データ</summary>
        private MKubun EntryData { get; set; } = new MKubun();

        /// <summary>区分ID</summary>
        private string KubunId { get; set; }

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>区分コンボ</summary>
        private IList<CommonCombBox> cmbKubun = new List<CommonCombBox>();

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <returns></returns>
        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMKubun 画面");

            // コンボボックスデータ設定
            using (var tool = new CreateListBoxData(this._CommonDataService))
            {
                cmbKubun = tool.GetKubunComb();
            };

            // 区分マスタ一覧取得
            GetMKubunList();

            await Task.Delay(2);
        }

        /// <summary>
        /// 区分マスタ読込
        /// </summary>
        private void GetMKubunList()
        {
            MKubunList = _CommonDataService.GetMKubunList();
            MKubunList = MKubunList.OrderBy(x => x.KubunId).ThenBy(x => x.DisplayOder).ToList();
            int rowNo = 1;
            foreach (var data in MKubunList)
            {
                data.RowNo = rowNo;
                data.StrKubunId = data.KubunId.ToString();
                var list = cmbKubun.Where(x => x.Value == data.StrKubunId).ToList();
                if(list.Count > 0)
                {
                    data.DspKubun = list[0].Text;
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
            EntryData = (MKubun)e.EditModel;
            if (e.IsNew)
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.INSERT.Code;
                EnabledCode = true;
                ReadOnlyKubunId = false;
            }
            else
            {
                EntryData.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
                EnabledCode = false;
                ReadOnlyKubunId = true;
            }
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="e"></param>
        private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            EntryData = (MKubun)e.EditModel;
            EntryData.KubunId = int.Parse(EntryData.StrKubunId);


            if (EntryData.ChangeSw == CDef.CHANGE_SW.INSERT.Code || EntryData.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
            {
                foreach (var data in MKubunList)
                {
                    if (EntryData.Id == null)
                    {
                        if (EntryData.KubunId == data.KubunId && EntryData.Code == data.Code)
                        {
                            await CommonTool.ErrorMessage(this._JsRuntime, MessageClass.EM0018);
                            e.Cancel = true;
                            return;
                        } 
                    }
                    else
                    {
                        if (EntryData.Id != data.Id)
                        {
                            if (EntryData.KubunId == data.KubunId && EntryData.Code == data.Code)
                            {
                                await CommonTool.ErrorMessage(this._JsRuntime, MessageClass.EM0018);
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            }

            await UpdateDataAsync();
            await Task.Delay(10);
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MKubun)e.DataItem;
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
                var cnt = this._CommonDataService.UpdateMKubun(EntryData);

                // 一覧データ読込
                GetMKubunList();

                // 検索条件絞込み
                SelectData();

                // ステートチェンジ
                StateHasChanged();

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(JsRuntime, ex.Message);
            }
        }

        /// <summary>
        /// 検索ボタン処理
        /// </summary>
        /// <returns></returns>
        private async Task OnSelect()
        {
            // 一覧データ読込
            GetMKubunList();

            // 検索条件絞込み
            SelectData();

            // グリッド初期化
            GridClear();

            await Task.Delay(1);
        }

        /// <summary>
        /// 検索条件絞込み
        /// </summary>
        private void SelectData()
        {
            if (!string.IsNullOrEmpty(KubunId))
            {
                var id = long.Parse(KubunId);
                MKubunList = MKubunList.Where(x => x.KubunId.Equals(id)).ToList();
            }
        }

        /// <summary>
        /// クリアボタン押下
        /// </summary>
        /// <returns></returns>
        private async Task OnClear()
        {
            KubunId = string.Empty;
            GridClear();
            await Task.Delay(1);
        }

        /// <summary>
        /// グリッド初期化
        /// </summary>
        private void GridClear()
        {
            Grid.ClearSort();
            Grid.ClearSelection();
            Grid.ClearFilter();
            OnFilterRow = false;
            OnGroupRow = false;
        }
    }
}