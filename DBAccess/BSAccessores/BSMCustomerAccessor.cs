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

namespace DBAccess.BSAccessores
{
    /// <summary>
    /// MCustomerのDBAccessorクラスを定義します。
    /// </summary>
    public partial class BSMCustomerAccessor : CommonLogic
    {
        /// <summary>MCustomerのDao</summary>
        protected MCustomerDao Dao = null;
 
        /// <summary>DbContext</summary>
        protected ApplicationDbContext Context;
 
        /// <summary>DbContext</summary>
        public bool WhereFlg { get; set; } = false;

        /// <summary>DbContext</summary>
        public string TableDbName = "M_CUSTOMER"; 

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        
        public BSMCustomerAccessor(ApplicationDbContext context) : base(context)
        {
            Dao = new MCustomerDao(context);
            Context = context;
        }
        #endregion

        /// <summary>
        /// データ検索
        /// </summary>
        /// <returns>取得結果</returns>
        public IList<MCustomer> Get()
        {
            return Query<MCustomer>("SELECT * FROM M_CUSTOMER ");
        }
 
        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response MCustomerCUD(IList<MCustomer> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            foreach (var data in datalist)
            {
                var res = MCustomerCUD(data);
                if(!res.Status)
                {
                     return _Response;
                };
                _Response.Cnt += res.Cnt;
            }
            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="data">更新用データ</param>
        /// <returns>件数</returns>
        public virtual Response MCustomerCUD(MCustomer data)
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
            if(_Response.Cnt != 1)
            {
                 _Response.Message = MessageClass.EM0999;
                 return _Response;
            }
            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// データ更新処理
        /// </summary>
        /// <param name="dataList">更新用データリスト</param>
        /// <returns>件数</returns>
        public Response MCustomerBulkCUD(IList<MCustomer> datalist)
        {
            Response _Response = new Response()
            {
                Status = false,
                Cnt = 0,
                Message = string.Empty,
            };
            // 更新時刻
            DateTime now = DateTime.Now;
            // インサート用データ
            IList<MCustomer> inserList = new List<MCustomer>();
            // アップデート用データ
            IList<MCustomer> updateList = new List<MCustomer>();
            // デリート用データ
            IList<MCustomer> deleteList = new List<MCustomer>();

            // データ振分
            foreach (var data in datalist)
            {
                if (data.ChangeSw == CDef.CHANGE_SW.INSERT.Code)
                {
                    data.Id = GetSeq(TableDbName + "_id_seq");
                    data.RegisterUser = Context.UserId.ToString();
                    data.RegisterDatetime = now;
                    data.RegisterProcess = Context.Process;
                    data.UpdateUser = Context.UserId.ToString();
                    data.UpdateDatetime = now;
                    data.UpdateProcess = Context.Process;
                    inserList.Add(data);
                }
                else if (data.ChangeSw == CDef.CHANGE_SW.UPDATE.Code)
                {
                    data.UpdateUser = Context.UserId.ToString();
                    data.UpdateProcess = Context.Process;
                    updateList.Add(data);
                }
                else if (data.ChangeSw == CDef.CHANGE_SW.DELETE.Code)
                {
                    deleteList.Add(data);
                }
            }
            // 追加処理
            var insCnt = InsetdataList(inserList);
            _Response.Cnt += insCnt;

            // 更新処理
            var updCnt = UpdateList(updateList);
            _Response.Cnt += updCnt;

            // 削除処理
            foreach (var data in deleteList)
            {
                var res = MCustomerCUD(data);
                if(!res.Status)
                {
                     return _Response;
                };
                _Response.Cnt += res.Cnt;
            }

            _Response.Status = true;
            return _Response;
        }

        /// <summary>
        /// 更新対象のデータを読み込み
        /// </summary>
        /// <param name="data">更新データ</param>
        public MCustomer Find(MCustomer data)
        {
            var rec = Dao.Find(data.Id);
            if (rec == null)
            {
                 OriginalException.ExclusiveException("MCustomer Key=" + ((long)data.Id).ToString());
            }
            if (CompareFatetime(rec.UpdateDatetime, data.UpdateDatetime))
            {
                 OriginalException.ExclusiveException("MCustomer Key=" + ((long)data.Id).ToString());
            }
            return rec;
        }

        /// <summary>
        /// 存在チェックを行う
        /// </summary>
        /// <param name="data">更新データ</param>
        public bool Exist(long? id)
        {
            return Dao.Exist(id);
        }

        /// <summary>
        /// １０００件単位でインサート
        /// </summary>
        /// <param name="inserList">インサート用データリスト</param>
        /// <returns>更新件数</returns>
        public int InsetdataList(IList<MCustomer> inserList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MCustomer> dataList = new List<MCustomer>();
            for (int idx = 0; idx < inserList.Count; idx++)
            {
                dataList.Add(inserList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetInsertSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MCustomer>();
                }
            }
            if (dataList.Count != CommonContext.KousinCnt && dataList.Count > 0)
            {
                sql = GetInsertSql(dataList);
                cnt += Execute(sql);
            }
            if (inserList.Count != cnt)
            {
                OriginalException.ExclusiveException(sql);
            }
            return cnt;
        }

        /// <summary>
        /// １０００件単位でUPDATE
        /// </summary>
        /// <param name="inserList">UPDATE用データリスト</param>
        /// <returns>更新件数</returns>
        public int UpdateList(IList<MCustomer> UpdateList)
        {
            int cnt = 0;
            var sql = string.Empty;
            IList<MCustomer> dataList = new List<MCustomer>();
            for (int idx = 0; idx < UpdateList.Count; idx++)
            {
                dataList.Add(UpdateList[idx]);
                if (dataList.Count == CommonContext.KousinCnt)
                {
                    sql = GetUpdateSql(dataList);
                    cnt += Execute(sql);
                    dataList = new List<MCustomer>();
                }
            }
            if (dataList.Count != CommonContext.KousinCnt && dataList.Count > 0)
            {
                sql = GetUpdateSql(dataList);
                cnt += Execute(sql);
            }
            if (UpdateList.Count != cnt)
            {
                OriginalException.ExclusiveException(sql);
            }
            return cnt;
        }
        /// <summary>
        /// </summary>
        /// <param name="inserList">新規追加用データリスト</param>
        /// <returns>SQL文</returns>
        public string GetInsertSql(IList<MCustomer> inserList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

            var sql = new StringBuilder();
            sql.Append("INSERT INTO M_CUSTOMER (");
            sql.Append("        ID");
            sql.Append("        ,CUSTOMER_CD");
            sql.Append("        ,COMPANY_NAME");
            sql.Append("        ,COMPANY_KANA");
            sql.Append("        ,PRESIDENT_NAME_SEI");
            sql.Append("        ,PRESIDENT_KANA_SEI");
            sql.Append("        ,PRESIDENT_NAME_MEI");
            sql.Append("        ,PRESIDENT_KANA_MEI");
            sql.Append("        ,ZIP_CD");
            sql.Append("        ,ADDRESS_KEN");
            sql.Append("        ,ADDRESS_SI");
            sql.Append("        ,ADDRESS_BANTI");
            sql.Append("        ,NEAREST_STATION");
            sql.Append("        ,TEL");
            sql.Append("        ,FAX");
            sql.Append("        ,CELL_PHONE");
            sql.Append("        ,EMAIL");
            sql.Append("        ,URL");
            sql.Append("        ,INDUSTRY");
            sql.Append("        ,MONTHLY_SALES");
            sql.Append("        ,DATE_OF_BIRTH");
            sql.Append("        ,ACQUISITION_CONSULTATION_AMOUNT");
            sql.Append("        ,PURPOSE_OF_USE");
            sql.Append("        ,DATE_OF_COMPANY_ESTABLISHMENT");
            sql.Append("        ,CURRENT_ACCOUNT_UMU");
            sql.Append("        ,PAST_DISHONORED_UMU");
            sql.Append("        ,DATE_OF_DISHONORED");
            sql.Append("        ,CONSUMPTION_TAX_KBN");
            sql.Append("        ,WITHHOLDING_TAX_KBN");
            sql.Append("        ,SOCIAL_INSURANCE_KBN");
            sql.Append("        ,REGISTER_USER");
            sql.Append("        ,REGISTER_DATETIME");
            sql.Append("        ,REGISTER_PROCESS");
            sql.Append("        ,UPDATE_USER");
            sql.Append("        ,UPDATE_DATETIME");
            sql.Append("        ,UPDATE_PROCESS");
            sql.Append(")");
            strSql = sql.ToString();

            var strValueSql = string.Empty;
            foreach (var entity in inserList)
            {
                sql = new StringBuilder();
                if (strValueSql != string.Empty)
                {
                    sql.Append(", (");
                }
                else
                {
                    sql.Append(" VALUES(");
                }
                sql.Append(EditInsertSql(entity));
                sql.Append(")");
                strValueSql += sql.ToString();
            }
            return strSql + strValueSql;
        }

        /// <summary>
        /// Value句の作成
        /// </summary>
        /// <param name="entity">エンティティ</param>
        /// <returns>SQL</returns>
        public string EditInsertSql(MCustomer entity)
        {
            var sql = new StringBuilder();
            sql.Append(" " + GenericUtil.LongToString(entity.Id) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CustomerCd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CompanyName) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CompanyKana) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PresidentNameSei) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PresidentKanaSei) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PresidentNameMei) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PresidentKanaMei) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.ZipCd) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.AddressKen) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.AddressSi) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.AddressBanti) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.NearestStation) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Tel) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Fax) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CellPhone) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Email) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Url) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.Industry) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.MonthlySales) + ", ");
            sql.Append(" " + GenericUtil.DateToString(entity.DateOfBirth) + ", ");
            sql.Append(" " + GenericUtil.LongToString(entity.AcquisitionConsultationAmount) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PurposeOfUse) + ", ");
            sql.Append(" " + GenericUtil.DateToString(entity.DateOfCompanyEstablishment) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.CurrentAccountUmu) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.PastDishonoredUmu) + ", ");
            sql.Append(" " + GenericUtil.DateToString(entity.DateOfDishonored) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.ConsumptionTaxKbn) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.WithholdingTaxKbn) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.SocialInsuranceKbn) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.RegisterUser) + ", ");
            sql.Append(" " + GenericUtil.DateTimeToString(entity.RegisterDatetime) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.RegisterProcess) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.UpdateUser) + ", ");
            sql.Append(" " + GenericUtil.DateTimeToString(entity.UpdateDatetime) + ", ");
            sql.Append(" " + GenericUtil.StringToString(entity.UpdateProcess) + " ");
            return sql.ToString();
        }

        /// <summary>
        /// 更新用SQL作成
        /// </summary>
        /// <param name="updateList">更新用データリスト</param>
        /// <returns>SQL文</returns>
        public string GetUpdateSql(IList<MCustomer> updateList)
        {
            var now = DateTime.Now;
            var strSql = string.Empty;

            var sql = new StringBuilder();
            var strValueSql = string.Empty;
            foreach (var entity in updateList)
            {
                sql = new StringBuilder();
                if (strValueSql != string.Empty)
                {
                    sql.Append(";");
                }
                sql.Append(EditUpdateSql(entity));
                strValueSql += sql.ToString();
            }
            return strSql + strValueSql;
        }

        /// <summary>
        /// Update句の作成
        /// </summary>
        /// <param name="entity">エンティティ</param>
        /// <returns>SQL</returns>
        public string EditUpdateSql(MCustomer entity)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE M_CUSTOMER");
            sql.Append(" SET");
            sql.Append("   CUSTOMER_CD  = " + GenericUtil.StringToString(entity.CustomerCd));
            sql.Append(",   COMPANY_NAME  = " + GenericUtil.StringToString(entity.CompanyName));
            sql.Append(",   COMPANY_KANA  = " + GenericUtil.StringToString(entity.CompanyKana));
            sql.Append(",   PRESIDENT_NAME_SEI  = " + GenericUtil.StringToString(entity.PresidentNameSei));
            sql.Append(",   PRESIDENT_KANA_SEI  = " + GenericUtil.StringToString(entity.PresidentKanaSei));
            sql.Append(",   PRESIDENT_NAME_MEI  = " + GenericUtil.StringToString(entity.PresidentNameMei));
            sql.Append(",   PRESIDENT_KANA_MEI  = " + GenericUtil.StringToString(entity.PresidentKanaMei));
            sql.Append(",   ZIP_CD  = " + GenericUtil.StringToString(entity.ZipCd));
            sql.Append(",   ADDRESS_KEN  = " + GenericUtil.StringToString(entity.AddressKen));
            sql.Append(",   ADDRESS_SI  = " + GenericUtil.StringToString(entity.AddressSi));
            sql.Append(",   ADDRESS_BANTI  = " + GenericUtil.StringToString(entity.AddressBanti));
            sql.Append(",   NEAREST_STATION  = " + GenericUtil.StringToString(entity.NearestStation));
            sql.Append(",   TEL  = " + GenericUtil.StringToString(entity.Tel));
            sql.Append(",   FAX  = " + GenericUtil.StringToString(entity.Fax));
            sql.Append(",   CELL_PHONE  = " + GenericUtil.StringToString(entity.CellPhone));
            sql.Append(",   EMAIL  = " + GenericUtil.StringToString(entity.Email));
            sql.Append(",   URL  = " + GenericUtil.StringToString(entity.Url));
            sql.Append(",   INDUSTRY  = " + GenericUtil.StringToString(entity.Industry));
            sql.Append(",   MONTHLY_SALES  = " + GenericUtil.LongToString(entity.MonthlySales));
            sql.Append(",   DATE_OF_BIRTH  = " + GenericUtil.DateToString(entity.DateOfBirth));
            sql.Append(",   ACQUISITION_CONSULTATION_AMOUNT  = " + GenericUtil.LongToString(entity.AcquisitionConsultationAmount));
            sql.Append(",   PURPOSE_OF_USE  = " + GenericUtil.StringToString(entity.PurposeOfUse));
            sql.Append(",   DATE_OF_COMPANY_ESTABLISHMENT  = " + GenericUtil.DateToString(entity.DateOfCompanyEstablishment));
            sql.Append(",   CURRENT_ACCOUNT_UMU  = " + GenericUtil.StringToString(entity.CurrentAccountUmu));
            sql.Append(",   PAST_DISHONORED_UMU  = " + GenericUtil.StringToString(entity.PastDishonoredUmu));
            sql.Append(",   DATE_OF_DISHONORED  = " + GenericUtil.DateToString(entity.DateOfDishonored));
            sql.Append(",   CONSUMPTION_TAX_KBN  = " + GenericUtil.StringToString(entity.ConsumptionTaxKbn));
            sql.Append(",   WITHHOLDING_TAX_KBN  = " + GenericUtil.StringToString(entity.WithholdingTaxKbn));
            sql.Append(",   SOCIAL_INSURANCE_KBN  = " + GenericUtil.StringToString(entity.SocialInsuranceKbn));
            sql.Append(",   REGISTER_USER  = " + GenericUtil.StringToString(entity.RegisterUser));
            sql.Append(",   REGISTER_DATETIME  = " + GenericUtil.DateToString(entity.RegisterDatetime));
            sql.Append(",   REGISTER_PROCESS  = " + GenericUtil.StringToString(entity.RegisterProcess));
            sql.Append(",   UPDATE_USER  = " + GenericUtil.StringToString(entity.UpdateUser));
            sql.Append(",   UPDATE_DATETIME  = " + GenericUtil.DateToString(entity.UpdateDatetime));
            sql.Append(",   UPDATE_PROCESS  = " + GenericUtil.StringToString(entity.UpdateProcess));
            sql.Append(" WHERE id = " + GenericUtil.LongToString(entity.Id));
            if (WhereFlg)
            {
                  sql.Append(" AND   str(update_datetime, 'yyyyMMddHH24MISS') = '" + ((DateTime)entity.UpdateDatetime).ToString("yyyyMMddHHmmss") + "'");
            }
            return sql.ToString();
        }

        /// <summary>
        /// CSV作成
        /// </summary>
        /// <param name="dataList">データリスト</param>
        /// <param name="header">ヘッダー有無</param>
        /// <returns>SQL</returns>
        public string CreateCSV(IList<MCustomer> dataList, bool header = true)
        {
            var csv = new StringBuilder();
            if (header)
            {
                csv.Append("ID" + "\t");
                csv.Append("CUSTOMER_CD" + "\t");
                csv.Append("COMPANY_NAME" + "\t");
                csv.Append("COMPANY_KANA" + "\t");
                csv.Append("PRESIDENT_NAME_SEI" + "\t");
                csv.Append("PRESIDENT_KANA_SEI" + "\t");
                csv.Append("PRESIDENT_NAME_MEI" + "\t");
                csv.Append("PRESIDENT_KANA_MEI" + "\t");
                csv.Append("ZIP_CD" + "\t");
                csv.Append("ADDRESS_KEN" + "\t");
                csv.Append("ADDRESS_SI" + "\t");
                csv.Append("ADDRESS_BANTI" + "\t");
                csv.Append("NEAREST_STATION" + "\t");
                csv.Append("TEL" + "\t");
                csv.Append("FAX" + "\t");
                csv.Append("CELL_PHONE" + "\t");
                csv.Append("EMAIL" + "\t");
                csv.Append("URL" + "\t");
                csv.Append("INDUSTRY" + "\t");
                csv.Append("MONTHLY_SALES" + "\t");
                csv.Append("DATE_OF_BIRTH" + "\t");
                csv.Append("ACQUISITION_CONSULTATION_AMOUNT" + "\t");
                csv.Append("PURPOSE_OF_USE" + "\t");
                csv.Append("DATE_OF_COMPANY_ESTABLISHMENT" + "\t");
                csv.Append("CURRENT_ACCOUNT_UMU" + "\t");
                csv.Append("PAST_DISHONORED_UMU" + "\t");
                csv.Append("DATE_OF_DISHONORED" + "\t");
                csv.Append("CONSUMPTION_TAX_KBN" + "\t");
                csv.Append("WITHHOLDING_TAX_KBN" + "\t");
                csv.Append("SOCIAL_INSURANCE_KBN" + "\t");
                csv.Append("REGISTER_USER" + "\t");
                csv.Append("REGISTER_DATETIME" + "\t");
                csv.Append("REGISTER_PROCESS" + "\t");
                csv.Append("UPDATE_USER" + "\t");
                csv.Append("UPDATE_DATETIME" + "\t");
                csv.Append("UPDATE_PROCESS" + "\r\n");
            }
            foreach (var data in dataList)
            {
                csv.Append(data.Id.ToString() + "\t");
                csv.Append(data.CustomerCd + "\t");
                csv.Append(data.CompanyName + "\t");
                csv.Append(data.CompanyKana + "\t");
                csv.Append(data.PresidentNameSei + "\t");
                csv.Append(data.PresidentKanaSei + "\t");
                csv.Append(data.PresidentNameMei + "\t");
                csv.Append(data.PresidentKanaMei + "\t");
                csv.Append(data.ZipCd + "\t");
                csv.Append(data.AddressKen + "\t");
                csv.Append(data.AddressSi + "\t");
                csv.Append(data.AddressBanti + "\t");
                csv.Append(data.NearestStation + "\t");
                csv.Append(data.Tel + "\t");
                csv.Append(data.Fax + "\t");
                csv.Append(data.CellPhone + "\t");
                csv.Append(data.Email + "\t");
                csv.Append(data.Url + "\t");
                csv.Append(data.Industry + "\t");
                csv.Append(data.MonthlySales.ToString() + "\t");
                csv.Append(((DateTime)data.DateOfBirth).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.AcquisitionConsultationAmount.ToString() + "\t");
                csv.Append(data.PurposeOfUse + "\t");
                csv.Append(((DateTime)data.DateOfCompanyEstablishment).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.CurrentAccountUmu + "\t");
                csv.Append(data.PastDishonoredUmu + "\t");
                csv.Append(((DateTime)data.DateOfDishonored).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.ConsumptionTaxKbn + "\t");
                csv.Append(data.WithholdingTaxKbn + "\t");
                csv.Append(data.SocialInsuranceKbn + "\t");
                csv.Append(data.RegisterUser + "\t");
                csv.Append(((DateTime)data.RegisterDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.RegisterProcess + "\t");
                csv.Append(data.UpdateUser + "\t");
                csv.Append(((DateTime)data.UpdateDatetime).ToString("yyyy/MM/dd") + "\t");
                csv.Append(data.UpdateProcess + "\r\n");
            }
            return csv.ToString();
        }

        public virtual void DefaultSet(MCustomer data)
        {
        }

    }
}
