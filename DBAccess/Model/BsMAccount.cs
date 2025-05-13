using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMAccountの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMAccount 
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
        public string Kamoku { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KouzaNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Zandaka { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CifNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Kana { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CotionFlg { get; set; }

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
        public void Copy(MAccount src)
        {
            this.Id = src.Id;
            this.BankCd = src.BankCd;
            this.SitenCd = src.SitenCd;
            this.Kamoku = src.Kamoku;
            this.KouzaNo = src.KouzaNo;
            this.Zandaka = src.Zandaka;
            this.CifNo = src.CifNo;
            this.Kana = src.Kana;
            this.CotionFlg = src.CotionFlg;
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
        public MAccount Reverse()
        {
            var dst = new MAccount();
            dst.Id = this.Id;
            dst.BankCd = this.BankCd;
            dst.SitenCd = this.SitenCd;
            dst.Kamoku = this.Kamoku;
            dst.KouzaNo = this.KouzaNo;
            dst.Zandaka = this.Zandaka;
            dst.CifNo = this.CifNo;
            dst.Kana = this.Kana;
            dst.CotionFlg = this.CotionFlg;
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
