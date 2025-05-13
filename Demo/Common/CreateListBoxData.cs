using DBAccess.BSAccessores;
using DBAccess.Logics;
using DBAccess.Model;
using DBAccess.Service;
using Model.AllCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Common
{
    /// <summary>
    /// コンボボックス作成用のクラス定義です。
    /// </summary>
    public class CreateListBoxData : IDisposable
    {
        // コモンサービス
        private CommonDataService _CommonDataService;

        /// <summary>
        /// コンストラクト
        /// </summary>
        /// <param name="service">サービス</param>
        public CreateListBoxData(CommonDataService service)
        {
            _CommonDataService = service;
        }

        /// <summary>
        /// リストボックス用データ作成(Db用)
        /// </summary>
        /// <param name="param">リストボックス用パラメータ</param>
        /// <returns>リストデータ</returns>
        public virtual IList<CommonCombBox> DbCreateListData(CreateListBoxDataParam param)
        {
            var sql = new System.Text.StringBuilder("select ");
            int col = 0;
            foreach (var id in param.Id)
            {
                if (col > 0)
                {
                    sql.Append(" || ");
                }
                sql.Append(id);
                col++;
            }
            sql.Append(" as value, ");
            col = 0;
            foreach (var text in param.Text)
            {
                if (col > 0)
                {
                    sql.Append(" || ");
                }
                sql.Append(text);
                col++;
            }
            sql.Append(" as text");
            sql.Append(" from " + param.Table);
            var listData = new List<CommonCombBox>(); 
            using (var logic = new CommonLogic(_CommonDataService._Context))
            {
                listData = logic.Query<CommonCombBox>(sql.ToString()).ToList() ;
            };
            return MakeList(listData, param.Paturn, param.Blunk);
        }

        /// <summary>
        /// リストボックス用データ作成(Cdef用)
        /// </summary>
        /// <param name="cdefval">Cdef</param>
        /// <param name="paturn">編集パターン:0両方、1:コードのみ、2:名称のみ</param>
        /// <param name="blunk">先頭ブランクデータ追加有無</param>
        /// <returns>リストデータ</returns>
        public virtual IList<CommonCombBox> CDefCreateListData(dynamic cdefval, int paturn = 0, bool blunk = false)
        {
            IList<CommonCombBox> list = new System.Collections.Generic.List<CommonCombBox>();
            if (blunk)
            {
                var data = new CommonCombBox()
                {
                    Value = "",
                    Text = "",
                };
                list.Add(data);
            }
            foreach (dynamic val in cdefval)
            {
                var text = val.Code + ":" + val.Alias;
                switch (paturn)
                {
                    case 0:
                        break;
                    case 1:
                        text = val.Alias;
                        break;
                    case 2:
                        text = val.Code;
                        break;
                }
                var data = new CommonCombBox()
                {
                    Value = val.Code,
                    Text = text,
                };
                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// 性別コンボデータを取得します。
        /// </summary>
        /// <returns>コンボボックス用List</returns>
        public IList<CommonCombBox> GetSexComb()
        {
            return CDefCreateListData(CDef.Sex.Values, 0, true);
        }

        /// <summary>
        /// 有無コンボデータを取得します。
        /// </summary>
        /// <returns>コンボボックス用List</returns>
        public IList<CommonCombBox> GetUmuComb()
        {
            return CDefCreateListData(CDef.UmuFlg.Values, 0, true);
        }

        /// <summary>
        /// 税コンボデータを取得します。
        /// </summary>
        /// <returns>コンボボックス用List</returns>
        public IList<CommonCombBox> GetZeiComb()
        {
            return CDefCreateListData(CDef.ConsumptionTaxKbn.Values, 0, true);
        }

        /// <summary>
        /// 区分名称コンボデータを取得します。
        /// </summary>
        /// <returns>コンボボックス用List</returns>
        public IList<CommonCombBox> GetKubunComb()
        {
            return CDefCreateListData(CDef.KubunId.Values, 0, true);
        }

        /// <summary>
        /// リストボックス用データ作成
        /// </summary>
        /// <param name="motoList"></param>
        /// <param name="paturn"></param>
        /// <param name="blunk"></param>
        /// <returns></returns>
        public IList<CommonCombBox> MakeList(IList<CommonCombBox> motoList, int paturn, bool blunk)
        {
            IList<CommonCombBox> list = new System.Collections.Generic.List<CommonCombBox>();
            if (blunk)
            {
                var data = new CommonCombBox()
                {
                    Value = "",
                    Text = "",
                };
                list.Add(data);
            }
            foreach (var val in motoList)
            {
                var text = val.Value + ":" + val.Text;
                switch (paturn)
                {
                    case 0:
                        break;
                    case 1:
                        text = val.Value;
                        break;
                    case 2:
                        text = val.Text;
                        break;
                }
                var data = new CommonCombBox()
                {
                    Value = val.Value,
                    Text = text,
                };
                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// リストボックス用テキスト取得
        /// </summary>
        /// <param name="motoList"></param>
        /// <param name="paturn"></param>
        /// <param name="blunk"></param>
        /// <returns></returns>
        public string GetName(IEnumerable<CommonCombBox> combList, string value)
        {
            foreach (var data in combList)
            {
                if(data.Value == value)
                {
                    return data.Text; 
                }
            }
            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// コンボボックス表示データ取得用パラメータクラス
    /// </summary>
    public class CreateListBoxDataParam
    {
        public string Table { get; set; }
        public Array Id { get; set; }
        public Array Text { get; set; }
        public int Paturn { get; set; }
        public bool Blunk { get; set; }
    }
}
