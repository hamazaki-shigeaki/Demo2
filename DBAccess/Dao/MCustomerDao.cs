using System;
using System.Text;
using Dapper;
using DBAccess.Model;
using DBAccess.Context;
using DBAccess.Logics;
using Model.CBean;

namespace DBAccess.Dao
{
    /// <summary>
    /// MCustomerのDaoクラスを定義します。
    /// </summary>
    public partial class MCustomerDao : CommonLogic
    {
        /// <summary>DBコンテキスト</summary>
        private ApplicationDbContext con = null;

        #region "コンストラクタ"
        public MCustomerDao(ApplicationDbContext context) : base(context)
        {
            con = context;
        }
        #endregion

        #region "Insert"
        /// <summary>
        /// Insert(MCustomer)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Insert(MCustomer entity)
        {
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
            sql.Append(") VALUES (");
            sql.Append("        @Id");
            sql.Append("        ,@CustomerCd");
            sql.Append("        ,@CompanyName");
            sql.Append("        ,@CompanyKana");
            sql.Append("        ,@PresidentNameSei");
            sql.Append("        ,@PresidentKanaSei");
            sql.Append("        ,@PresidentNameMei");
            sql.Append("        ,@PresidentKanaMei");
            sql.Append("        ,@ZipCd");
            sql.Append("        ,@AddressKen");
            sql.Append("        ,@AddressSi");
            sql.Append("        ,@AddressBanti");
            sql.Append("        ,@NearestStation");
            sql.Append("        ,@Tel");
            sql.Append("        ,@Fax");
            sql.Append("        ,@CellPhone");
            sql.Append("        ,@Email");
            sql.Append("        ,@Url");
            sql.Append("        ,@Industry");
            sql.Append("        ,@MonthlySales");
            sql.Append("        ,@DateOfBirth");
            sql.Append("        ,@AcquisitionConsultationAmount");
            sql.Append("        ,@PurposeOfUse");
            sql.Append("        ,@DateOfCompanyEstablishment");
            sql.Append("        ,@CurrentAccountUmu");
            sql.Append("        ,@PastDishonoredUmu");
            sql.Append("        ,@DateOfDishonored");
            sql.Append("        ,@ConsumptionTaxKbn");
            sql.Append("        ,@WithholdingTaxKbn");
            sql.Append("        ,@SocialInsuranceKbn");
            sql.Append("        ,@RegisterUser");
            sql.Append("        ,@RegisterDatetime");
            sql.Append("        ,@RegisterProcess");
            sql.Append("        ,@UpdateUser");
            sql.Append("        ,@UpdateDatetime");
            sql.Append("        ,@UpdateProcess");
            sql.Append(");");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("CustomerCd", entity.CustomerCd);
            param.Add("CompanyName", entity.CompanyName);
            param.Add("CompanyKana", entity.CompanyKana);
            param.Add("PresidentNameSei", entity.PresidentNameSei);
            param.Add("PresidentKanaSei", entity.PresidentKanaSei);
            param.Add("PresidentNameMei", entity.PresidentNameMei);
            param.Add("PresidentKanaMei", entity.PresidentKanaMei);
            param.Add("ZipCd", entity.ZipCd);
            param.Add("AddressKen", entity.AddressKen);
            param.Add("AddressSi", entity.AddressSi);
            param.Add("AddressBanti", entity.AddressBanti);
            param.Add("NearestStation", entity.NearestStation);
            param.Add("Tel", entity.Tel);
            param.Add("Fax", entity.Fax);
            param.Add("CellPhone", entity.CellPhone);
            param.Add("Email", entity.Email);
            param.Add("Url", entity.Url);
            param.Add("Industry", entity.Industry);
            param.Add("MonthlySales", entity.MonthlySales);
            param.Add("DateOfBirth", entity.DateOfBirth);
            param.Add("AcquisitionConsultationAmount", entity.AcquisitionConsultationAmount);
            param.Add("PurposeOfUse", entity.PurposeOfUse);
            param.Add("DateOfCompanyEstablishment", entity.DateOfCompanyEstablishment);
            param.Add("CurrentAccountUmu", entity.CurrentAccountUmu);
            param.Add("PastDishonoredUmu", entity.PastDishonoredUmu);
            param.Add("DateOfDishonored", entity.DateOfDishonored);
            param.Add("ConsumptionTaxKbn", entity.ConsumptionTaxKbn);
            param.Add("WithholdingTaxKbn", entity.WithholdingTaxKbn);
            param.Add("SocialInsuranceKbn", entity.SocialInsuranceKbn);
            param.Add("RegisterUser", con.UserId.ToString());
            param.Add("RegisterDatetime", now);
            param.Add("RegisterProcess", con.Process);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", now);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Update"
        /// <summary>
        /// Update(MCustomer)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Update(MCustomer entity)
        {
            var sql = new StringBuilder(); 
            sql.Append($"UPDATE M_CUSTOMER");
            sql.Append($"   SET CUSTOMER_CD = @CustomerCd");
            sql.Append($",       COMPANY_NAME = @CompanyName");
            sql.Append($",       COMPANY_KANA = @CompanyKana");
            sql.Append($",       PRESIDENT_NAME_SEI = @PresidentNameSei");
            sql.Append($",       PRESIDENT_KANA_SEI = @PresidentKanaSei");
            sql.Append($",       PRESIDENT_NAME_MEI = @PresidentNameMei");
            sql.Append($",       PRESIDENT_KANA_MEI = @PresidentKanaMei");
            sql.Append($",       ZIP_CD = @ZipCd");
            sql.Append($",       ADDRESS_KEN = @AddressKen");
            sql.Append($",       ADDRESS_SI = @AddressSi");
            sql.Append($",       ADDRESS_BANTI = @AddressBanti");
            sql.Append($",       NEAREST_STATION = @NearestStation");
            sql.Append($",       TEL = @Tel");
            sql.Append($",       FAX = @Fax");
            sql.Append($",       CELL_PHONE = @CellPhone");
            sql.Append($",       EMAIL = @Email");
            sql.Append($",       URL = @Url");
            sql.Append($",       INDUSTRY = @Industry");
            sql.Append($",       MONTHLY_SALES = @MonthlySales");
            sql.Append($",       DATE_OF_BIRTH = @DateOfBirth");
            sql.Append($",       ACQUISITION_CONSULTATION_AMOUNT = @AcquisitionConsultationAmount");
            sql.Append($",       PURPOSE_OF_USE = @PurposeOfUse");
            sql.Append($",       DATE_OF_COMPANY_ESTABLISHMENT = @DateOfCompanyEstablishment");
            sql.Append($",       CURRENT_ACCOUNT_UMU = @CurrentAccountUmu");
            sql.Append($",       PAST_DISHONORED_UMU = @PastDishonoredUmu");
            sql.Append($",       DATE_OF_DISHONORED = @DateOfDishonored");
            sql.Append($",       CONSUMPTION_TAX_KBN = @ConsumptionTaxKbn");
            sql.Append($",       WITHHOLDING_TAX_KBN = @WithholdingTaxKbn");
            sql.Append($",       SOCIAL_INSURANCE_KBN = @SocialInsuranceKbn");
            sql.Append($",       UPDATE_USER = @UpdateUser");
            sql.Append($",       UPDATE_DATETIME = SYSDATETIME() ");
            sql.Append($",       UPDATE_PROCESS = @UpdateProcess");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var now = DateTime.Now;
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("CustomerCd", entity.CustomerCd);
            param.Add("CompanyName", entity.CompanyName);
            param.Add("CompanyKana", entity.CompanyKana);
            param.Add("PresidentNameSei", entity.PresidentNameSei);
            param.Add("PresidentKanaSei", entity.PresidentKanaSei);
            param.Add("PresidentNameMei", entity.PresidentNameMei);
            param.Add("PresidentKanaMei", entity.PresidentKanaMei);
            param.Add("ZipCd", entity.ZipCd);
            param.Add("AddressKen", entity.AddressKen);
            param.Add("AddressSi", entity.AddressSi);
            param.Add("AddressBanti", entity.AddressBanti);
            param.Add("NearestStation", entity.NearestStation);
            param.Add("Tel", entity.Tel);
            param.Add("Fax", entity.Fax);
            param.Add("CellPhone", entity.CellPhone);
            param.Add("Email", entity.Email);
            param.Add("Url", entity.Url);
            param.Add("Industry", entity.Industry);
            param.Add("MonthlySales", entity.MonthlySales);
            param.Add("DateOfBirth", entity.DateOfBirth);
            param.Add("AcquisitionConsultationAmount", entity.AcquisitionConsultationAmount);
            param.Add("PurposeOfUse", entity.PurposeOfUse);
            param.Add("DateOfCompanyEstablishment", entity.DateOfCompanyEstablishment);
            param.Add("CurrentAccountUmu", entity.CurrentAccountUmu);
            param.Add("PastDishonoredUmu", entity.PastDishonoredUmu);
            param.Add("DateOfDishonored", entity.DateOfDishonored);
            param.Add("ConsumptionTaxKbn", entity.ConsumptionTaxKbn);
            param.Add("WithholdingTaxKbn", entity.WithholdingTaxKbn);
            param.Add("SocialInsuranceKbn", entity.SocialInsuranceKbn);
            param.Add("UpdateUser", con.UserId.ToString());
            param.Add("UpdateDatetime", entity.UpdateDatetime);
            param.Add("UpdateProcess", con.Process);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// Delete(MCustomer)
        /// </summary>
        /// <param name="entity">エンティティ</param>
        public int Delete(MCustomer entity)
        {
            var sql = new StringBuilder(); 
            sql.Append("DELETE FROM M_CUSTOMER");
            sql.Append($" WHERE id = @Id and update_datetime = @UpdateDatetime ");
            var param = new DynamicParameters();
            param.Add("Id", entity.Id);
            param.Add("UpdateDatetime", entity.UpdateDatetime);

            return Execute(sql.ToString(), param); 
        }
        #endregion

        #region "Find"
        /// <summary>
        /// Find
        /// </summary>
        /// <param name="id">ID</param>
        public MCustomer Find(long? id)
        {
            var cb = new MCustomerCB(); 
            cb.Query().SetId_Equal(id); 
            return ExecuteEntity<MCustomer>(cb); 
        }
        #endregion

        #region "Exist"
        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>取得結果(存在:=true,存在しない:=false)</returns>
        public bool Exist(long? id)
        {
            if(Find(id) == null) 
            {
               return false; 
            }
            return true;
        }
        #endregion
    }
}
