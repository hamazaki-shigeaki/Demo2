using System;
using System.Text;
using System.Collections.Generic;
 
using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Common;
using DBAccess.Context;
using DBAccess.Logics;
using DBAccess.Dao;
using Utilities;
using DBAccess.BSAccessores;

namespace DBAccess.DBAccessores
{
    /// <summary>
    /// MSuitのDBAccessorクラスを定義します。
    /// </summary>
    public partial class MSuitAccessor : BSMSuitAccessor
    {
        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>

        public MSuitAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new MSuitDao(context);
            Context = context;
        }
        #endregion

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public override Response MSuitCUD(MSuit data)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            if (string.IsNullOrEmpty(data.ChangeSw))
            {
                _Response.Message = MessageClass.EM0997;
                return _Response;
            }
            if (data.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
            {
                data.Id = GetSeq(TableDbName + "_id_seq");
                data.SuitNo = "S" + ((long)data.Id).ToString("000000000");

                DefaultSet(data);
                _Response.Cnt += Dao.Insert(data);
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
            {
                DefaultSet(data);
                _Response.Cnt += Dao.Update(data);
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.DELETE.Code)
            {
                _Response.Cnt += Dao.Delete(data);
            }
            if (_Response.Cnt != 1)
            {
                _Response.Message = MessageClass.EM0999;
                return _Response;
            }
            _Response.Status = true;
            return _Response;
        }

    }
}
