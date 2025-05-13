using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMDepositTableの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMDepositTable 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? DepositId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuitNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BankCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SitenCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? AccessDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PaymentDestination { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? AccountsReceivable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SourcePayment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? AccountsPayable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Zandaka { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Memo { get; set; }

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
        public void Copy(MDepositTable src)
        {
            this.Id = src.Id;
            this.DepositId = src.DepositId;
            this.SuitNo = src.SuitNo;
            this.BankCd = src.BankCd;
            this.SitenCd = src.SitenCd;
            this.AccessDate = src.AccessDate;
            this.PaymentDestination = src.PaymentDestination;
            this.AccountsReceivable = src.AccountsReceivable;
            this.SourcePayment = src.SourcePayment;
            this.AccountsPayable = src.AccountsPayable;
            this.Zandaka = src.Zandaka;
            this.Apply = src.Apply;
            this.Memo = src.Memo;
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
        public MDepositTable Reverse()
        {
            var dst = new MDepositTable();
            dst.Id = this.Id;
            dst.DepositId = this.DepositId;
            dst.SuitNo = this.SuitNo;
            dst.BankCd = this.BankCd;
            dst.SitenCd = this.SitenCd;
            dst.AccessDate = this.AccessDate;
            dst.PaymentDestination = this.PaymentDestination;
            dst.AccountsReceivable = this.AccountsReceivable;
            dst.SourcePayment = this.SourcePayment;
            dst.AccountsPayable = this.AccountsPayable;
            dst.Zandaka = this.Zandaka;
            dst.Apply = this.Apply;
            dst.Memo = this.Memo;
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
