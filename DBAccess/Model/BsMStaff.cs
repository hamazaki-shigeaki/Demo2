using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMStaffの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMStaff 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "スタッフID")]
        [Required(ErrorMessage = MessageClass.EM0013)]
        public string StaffId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "氏名")]
        [Required(ErrorMessage = MessageClass.EM0013)]
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Kana { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "性別")]
        [Required(ErrorMessage = MessageClass.EM0011)]
        public string Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfHire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SitenCd { get; set; }

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
        public void Copy(MStaff src)
        {
            this.Id = src.Id;
            this.StaffId = src.StaffId;
            this.FullName = src.FullName;
            this.Kana = src.Kana;
            this.Sex = src.Sex;
            this.DateOfHire = src.DateOfHire;
            this.SitenCd = src.SitenCd;
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
        public MStaff Reverse()
        {
            var dst = new MStaff();
            dst.Id = this.Id;
            dst.StaffId = this.StaffId;
            dst.FullName = this.FullName;
            dst.Kana = this.Kana;
            dst.Sex = this.Sex;
            dst.DateOfHire = this.DateOfHire;
            dst.SitenCd = this.SitenCd;
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
