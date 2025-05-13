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
    /// <summary>
    /// 相続案件一覧
    /// </summary>
    public partial class SuitList : Base
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

        /// <summary>ﾌｨﾙﾀｰ行表示</summary>
        private bool OnFilterRow { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool CompFlg { get; set; } = false;

        /// <summary>ｸﾞﾙｰﾌﾟ行表示</summary>
        private bool OnGroupRow { get; set; } = false;

        /// <summary>一覧Gridで選択された行</summary>
        private object? SelectedDataItem { get; set; }

        /// <summary>案件マスタリスト</summary>
        private IList<MSuit> MSuitList = new List<MSuit>();

        /// <summary>案件マスタリスト(絞込み元)</summary>
        private IList<MSuit> MotoSuitList = new List<MSuit>();

        /// <summary>顧客マスタリスト</summary>
        private IList<MCustomer> MCustomerList = new List<MCustomer>();

        /// <summary>顧客コード・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombCustomerList = new List<CommonCombBox>();

        /// <summary>分類・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombBunruiList = new List<CommonCombBox>();

        /// <summary>ステータス・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombStatusList = new List<CommonCombBox>();

        /// <summary>ラベル　ポジション</summary>
        LabelPosition LabelPosition { get; set; }

        /// <summary>区分マスタリスト</summary>
        private IList<MKubun> MKubunList = new List<MKubun>();

        // 検索条件入力項目

        /// <summary>案件ID</summary>
        private string SuitId { get; set; } = string.Empty;

        /// <summary>勘定店CD</summary>
        private string KanjyoutenCd { get; set; } = string.Empty;

        /// <summary>CIF№</summary>
        private string CIFNo { get; set; } = string.Empty;

        /// <summary>案件区分</summary>
        private string SuitKbn { get; set; } = string.Empty;

        /// <summary>被相続人氏名</summary>
        private string HiSouzkuninName { get; set; } = string.Empty;

        /// <summary>金額</summary>
        private int? DecimalValue { get; set; } = null;

        /// <summary>チェック01</summary>
        bool Checked01 { get; set; } = true;

        /// <summary>チェック02</summary>
        bool Checked02 { get; set; } = true;

        /// <summary>チェック03</summary>
        CheckBoxContentAlignment Alignment { get; set; }

        /// <summary>新規登録表示</summary>
        public bool EntryPouUpVisible { get; set; } = false;

        /// <summary> 編集データ</summary>
        private MSuit EntryData { get; set; } = new MSuit();

        /// <summary>状況・コンボボックス用リスト</summary>
        private IList<CommonCombBox> CombSituationList = new List<CommonCombBox>();

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

            GetMSuitList();

            await Task.Delay(2);
        }

        /// <summary>
        /// 案件マスタ一覧データ取得
        /// </summary>
        private void GetMSuitList()
        {
            MCustomerList = _CommonDataService.GetMCustomerList();

            int rowNo = 0;
            foreach (var data in MCustomerList)
            {
                data.RowNo = rowNo;
                rowNo++;
            }

            MSuitList = _CommonDataService.GetMSuitList().OrderBy(x => x.SuitNo).ToList();

            if (!CompFlg)
            {
                MSuitList = MSuitList.Where(x=> x.Situation != CDef.Situation.Situation_03.Code).ToList();
            }

            rowNo = 0;
            foreach (var data in MSuitList)
            {
                data.RowNo = rowNo;
                var list = CombSituationList.Where(x => x.Value.Equals(data.Situation)).ToList();
                if(list.Count > 0)
                {
                    data.SituationName = list[0].Text;
                }
                var list2 = MCustomerList.Where(x => x.CustomerCd.Equals(data.CustomerCd)).ToList();
                if (list2.Count > 0)
                {
                    data._MCustomer = list2[0];
                }
                rowNo++;
            }

        }

        /// <summary>
        /// 行編集
        /// </summary>
        /// <param name="e">イベント</param>
        private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            SelectedDataItem = e.DataItem;
            EntryData = (MSuit)e.EditModel;

            EntryData.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
            EntryData._MCustomer.ChangeSw = CDef.CHANGE_SW.UPDATE.Code;
        }

        /// <summary>
        /// 登録ボタン
        /// </summary>
        /// <param name="e"></param>
        private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            EntryData = (MSuit)e.EditModel;
            await UpdateDataAsync();
            await Task.Delay(10);
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="e">イベント/param>
        private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            EntryData = (MSuit)e.DataItem;
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
                var cnt = this._CommonDataService.UpdateMSuit(EntryData);

                // 案件マスタ読み込み
                GetMSuitList();

                // 条件絞込み
                JyokenSelect();

                // グリッド　クリア
                //GridClear();

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
        /// 先頭行に移動
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonFastRow()
        {
            Grid.MakeRowVisible(0);

            await Task.Delay(20);
        }

        /// <summary>
        /// 最終行に移動
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonLastRow()
        {
            Grid.MakeRowVisible(MSuitList.Count - 1);

            await Task.Delay(20);
        }

        /// <summary>
        /// クリアボタン押下
        /// </summary>
        /// <returns></returns>
        private async Task OnClear()
        {
            Grid.ClearSort();
            Grid.ClearSelection();
            Grid.ClearFilter();
            OnFilterRow = false;
            OnGroupRow = false;
        }

        /// <summary>
        /// 検索ボタン処理
        /// </summary>
        /// <returns></returns>
        private async Task OnSelect()
        {
            // セッション情報クリア
            await SessionClear();

            // 案件マスタ読み込み
            GetMSuitList();

            // 条件絞込み
            JyokenSelect();

            // グリッド　クリア
            GridClear();

            await Task.Delay(1);
        }

        /// <summary>
        /// セッション情報　選択情報クリア
        /// </summary>
        private async Task SessionClear()
        {
            // 選択クリア
            SessionInfo._MSuit = null;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
        }

        /// <summary>
        /// 検索条件による絞込み
        /// </summary>
        private void JyokenSelect()
        {
            // 検索条件により絞込み
        }

        /// <summary>
        /// グリッド　クリア
        /// </summary>
        private void GridClear()
        {
            // グリッドクリア
            Grid.ClearSort();
            Grid.ClearSelection();
            Grid.ClearFilter();
            OnFilterRow = false;
            OnGroupRow = false;
        }

        /// <summary>
        /// 徴求書類ボタン押下処理
        /// </summary>
        /// <returns></returns>
        private async Task OnButtonDocsearch()
        {
            if (SelectedDataItem == null)
            {
                await CommonTool.ErrorMessage(JsRuntime, MessageClass.EM0019);
                return;
            }
            SessionInfo._MSuit = (MSuit)SelectedDataItem;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);

            await JumpPage("Docsearch");
        }


        /// <summary>
        /// 資産明細ボタン押下
        /// </summary>
        private async Task OnButtonAssetDetails()
        {
            if (SelectedDataItem == null)
            {
                await CommonTool.ErrorMessage(JsRuntime, MessageClass.EM0019);
                return;
            }
            SessionInfo._MSuit = (MSuit)SelectedDataItem;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("AssetDetails");
        }

        /// <summary>
        /// 処理中チェックボックス変更処理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task OnChangeChecked01(bool value)
        {
            Checked01 = value;
            Check();
            await Task.Delay(2);
        }

        /// <summary>
        /// 完了チェックボックス変更処理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task OnChangeChecked02(bool value)
        {
            Checked02 = value;
            Check();
            await Task.Delay(2);
        }

        /// <summary>
        /// チェックボック変更処理
        /// </summary>
        private void Check()
        {
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
        /// 入出金表
        /// </summary>
        private async Task OnButtonDepositTable()
        {
            if (SelectedDataItem == null)
            {
                await CommonTool.ErrorMessage(JsRuntime, MessageClass.EM0019);
                return;
            }
            SessionInfo._MSuit = (MSuit)SelectedDataItem;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("DepositTable");
        }

        /// <summary>
        /// 預かり書類
        /// </summary>
        private async Task OnButtonDocument()
        {
            if (SelectedDataItem == null)
            {
                await CommonTool.ErrorMessage(JsRuntime, MessageClass.EM0019);
                return;
            }
            SessionInfo._MSuit = (MSuit)SelectedDataItem;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("Document");
        }


        /// <summary>
        /// 債権管理
        /// </summary>
        private async Task OnButtonSaiken()
        {
            if (SelectedDataItem == null)
            {
                await CommonTool.ErrorMessage(JsRuntime, MessageClass.EM0019);
                return;
            }
            SessionInfo._MSuit = (MSuit)SelectedDataItem;
            await SessionStorage.SessionInfoWrire(_ProtectedLocalStorage, SessionInfo);
            await JumpPage("Assets");
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        private async Task OnButtonPrint()
        {
            try
            {
                await Task.Delay(50);
            }
            catch (Exception ex)
            {
                await CommonTool.ErrorMessage(this._JsRuntime, ex.Message);
            }
        }

        /// <summary>
        /// チェックボックス変更
        /// </summary>
        /// <param name="value">値/param>
        private void OnCheckedChanged(bool value)
        {
            CompFlg = value;
            GetMSuitList();
        }
    }
}
