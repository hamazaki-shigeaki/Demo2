using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMCustomerの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMCustomer 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CustomerCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyKana { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PresidentNameSei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PresidentKanaSei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PresidentNameMei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PresidentKanaMei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ZipCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressKen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressSi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressBanti { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NearestStation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? MonthlySales { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? AcquisitionConsultationAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PurposeOfUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfCompanyEstablishment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentAccountUmu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PastDishonoredUmu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfDishonored { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConsumptionTaxKbn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WithholdingTaxKbn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SocialInsuranceKbn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegisterUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? RegisterDatetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegisterProcess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateDatetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateProcess { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [NotMapped]
        public int    RowNo { get; set; }

        /// <summary>
        /// チェンジSW
        /// </summary>
        [NotMapped]
        public string ChangeSw { get; set; }


        /// <summary>
        /// コピー処理
        /// </summary>
        /// <param name=src>コピー元</param>
        public void Copy(MCustomer src)
        {
            this.Id = src.Id;
            this.CustomerCd = src.CustomerCd;
            this.CompanyName = src.CompanyName;
            this.CompanyKana = src.CompanyKana;
            this.PresidentNameSei = src.PresidentNameSei;
            this.PresidentKanaSei = src.PresidentKanaSei;
            this.PresidentNameMei = src.PresidentNameMei;
            this.PresidentKanaMei = src.PresidentKanaMei;
            this.ZipCd = src.ZipCd;
            this.AddressKen = src.AddressKen;
            this.AddressSi = src.AddressSi;
            this.AddressBanti = src.AddressBanti;
            this.NearestStation = src.NearestStation;
            this.Tel = src.Tel;
            this.Fax = src.Fax;
            this.CellPhone = src.CellPhone;
            this.Email = src.Email;
            this.Url = src.Url;
            this.Industry = src.Industry;
            this.MonthlySales = src.MonthlySales;
            this.DateOfBirth = src.DateOfBirth;
            this.AcquisitionConsultationAmount = src.AcquisitionConsultationAmount;
            this.PurposeOfUse = src.PurposeOfUse;
            this.DateOfCompanyEstablishment = src.DateOfCompanyEstablishment;
            this.CurrentAccountUmu = src.CurrentAccountUmu;
            this.PastDishonoredUmu = src.PastDishonoredUmu;
            this.DateOfDishonored = src.DateOfDishonored;
            this.ConsumptionTaxKbn = src.ConsumptionTaxKbn;
            this.WithholdingTaxKbn = src.WithholdingTaxKbn;
            this.SocialInsuranceKbn = src.SocialInsuranceKbn;
            this.RegisterUser = src.RegisterUser;
            this.RegisterDatetime = src.RegisterDatetime;
            this.RegisterProcess = src.RegisterProcess;
            this.UpdateUser = src.UpdateUser;
            this.UpdateDatetime = src.UpdateDatetime;
            this.UpdateProcess = src.UpdateProcess;
            this.ChangeSw = src.ChangeSw;
        }

        /// <summary>
        /// 複写処理
        /// </summary>
        /// <returns>複写データ</returns>
        public MCustomer Reverse()
        {
            var dst = new MCustomer();
            dst.Id = this.Id;
            dst.CustomerCd = this.CustomerCd;
            dst.CompanyName = this.CompanyName;
            dst.CompanyKana = this.CompanyKana;
            dst.PresidentNameSei = this.PresidentNameSei;
            dst.PresidentKanaSei = this.PresidentKanaSei;
            dst.PresidentNameMei = this.PresidentNameMei;
            dst.PresidentKanaMei = this.PresidentKanaMei;
            dst.ZipCd = this.ZipCd;
            dst.AddressKen = this.AddressKen;
            dst.AddressSi = this.AddressSi;
            dst.AddressBanti = this.AddressBanti;
            dst.NearestStation = this.NearestStation;
            dst.Tel = this.Tel;
            dst.Fax = this.Fax;
            dst.CellPhone = this.CellPhone;
            dst.Email = this.Email;
            dst.Url = this.Url;
            dst.Industry = this.Industry;
            dst.MonthlySales = this.MonthlySales;
            dst.DateOfBirth = this.DateOfBirth;
            dst.AcquisitionConsultationAmount = this.AcquisitionConsultationAmount;
            dst.PurposeOfUse = this.PurposeOfUse;
            dst.DateOfCompanyEstablishment = this.DateOfCompanyEstablishment;
            dst.CurrentAccountUmu = this.CurrentAccountUmu;
            dst.PastDishonoredUmu = this.PastDishonoredUmu;
            dst.DateOfDishonored = this.DateOfDishonored;
            dst.ConsumptionTaxKbn = this.ConsumptionTaxKbn;
            dst.WithholdingTaxKbn = this.WithholdingTaxKbn;
            dst.SocialInsuranceKbn = this.SocialInsuranceKbn;
            dst.RegisterUser = this.RegisterUser;
            dst.RegisterDatetime = this.RegisterDatetime;
            dst.RegisterProcess = this.RegisterProcess;
            dst.UpdateUser = this.UpdateUser;
            dst.UpdateDatetime = this.UpdateDatetime;
            dst.UpdateProcess = this.UpdateProcess;
            dst.ChangeSw = this.ChangeSw;
            return dst;
        }

    }
}
