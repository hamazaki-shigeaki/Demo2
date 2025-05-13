using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMBankの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMBank 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

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
        public string BankName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BankNameKana { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BranchNameKana { get; set; }

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
        public void Copy(MBank src)
        {
            this.Id = src.Id;
            this.BankCd = src.BankCd;
            this.SitenCd = src.SitenCd;
            this.BankName = src.BankName;
            this.BankNameKana = src.BankNameKana;
            this.BranchName = src.BranchName;
            this.BranchNameKana = src.BranchNameKana;
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
        public MBank Reverse()
        {
            var dst = new MBank();
            dst.Id = this.Id;
            dst.BankCd = this.BankCd;
            dst.SitenCd = this.SitenCd;
            dst.BankName = this.BankName;
            dst.BankNameKana = this.BankNameKana;
            dst.BranchName = this.BranchName;
            dst.BranchNameKana = this.BranchNameKana;
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
