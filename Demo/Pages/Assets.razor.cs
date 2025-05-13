using DBAccess.Model;
using Demo.Common;
using DevExpress.Blazor;
using Model.AllCommon;

namespace Demo.Pages
{
    public partial class Assets　: Base
    {
        /// <summary>債権マスタリスト</summary>
        private IList<MAssets> MAssetsList = new List<MAssets>();

        /// <summary>区分マスタリスト</summary>
        private IList<MKubun> MKubunList = new List<MKubun>();

        /// <summary>文書区分・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombDocClassificationList = new List<CommonCombBox>();

        /// <summary>グリッド</summary>
        private IGrid Grid { get; set; } = new DxGrid();

        /// <summary>編集データ</summary>
        private MAssets EntryData { get; set; } = new MAssets();

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }


        private string SuitNo { get; set; } = string.Empty;

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

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
            GetMAssetsList();


            await Task.Delay(2);
        }

        private void GetMAssetsList()
        {
            MAssetsList = _CommonDataService.GetMAssetsList();

            MAssetsList = MAssetsList.Where(x => string.Equals(x.SuitNo, SessionInfo._MSuit.SuitNo)).ToList();

            int rowNo = 0;
            foreach (var data in MAssetsList)
            {
                data.RowNo = rowNo;
                rowNo++;

                var list = CombDocClassificationList.Where(x => x.Value.Equals(data.AssetType)).ToList();
                if(list.Count > 0)
                {
                    data.StrAssetType = list[0].Text;
                }

            }
        }

        /// <summary>
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            SelectedDataItem = e.DataItem;
            EntryData = (MAssets)e.EditModel;

            if (e.IsNew)
            {
                EntryData.SuitNo = SessionInfo._MSuit.SuitNo;
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
            EntryData = (MAssets)e.EditModel;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        private async Task UpdateDataAsync()
        {
            try
            {
                // 更新処理
                var cnt = this._CommonDataService.UpdateMAssets(EntryData);

                // 再読み込み
                // 債権一覧取得
                GetMAssetsList();

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

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MAssets)e.DataItem;
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
    }
}
