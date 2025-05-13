using Model.AllCommon;
using DBAccess.Model;
using DBAccess.Common;
using DBAccess.Context;
using DBAccess.Dao;
using DBAccess.BSAccessores;
using Utilities;
using DBAccess.Logics;
using DBAccess.Service;

namespace DBAccess.DBAccessores
{
    public partial class MCustomerAccessor : BSMCustomerAccessor
    {
        public MCustomerAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new MCustomerDao(context);
            Context = context;
        }

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="data">更新用データ</param>
        /// <returns>件数</returns>
        public override Response MCustomerCUD(MCustomer data)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty
            };
            if (string.IsNullOrEmpty(data.ChangeSw))
            {
                _Response.Status = true;
                return _Response;
            }
            if (data.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
            {
                if (!data.Id.HasValue)
                {
                    data.Id = GetSeq(TableDbName + "_id_seq");
                    data.CustomerCd = ((long)data.Id).ToString("0000000000");
                }
                DefaultSet(data);
                _Response.Cnt += Dao.Insert(data);
                _Response.Key = data.Id;
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
            {
                DefaultSet(data);
                _Response.Cnt += Dao.Update(data);
                _Response.Key = data.Id;
            }
            else if (data.ChangeSw == CDef.CHANGE_SW.DELETE.Code)
            {
                _Response.Cnt += Dao.Delete(data);
                _Response.Key = data.Id;
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
