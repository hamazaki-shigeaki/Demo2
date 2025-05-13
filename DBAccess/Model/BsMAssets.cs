using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMAssetsの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMAssets 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? AssetsId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuitNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Kingaku { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TransactionDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PaymentDestination { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Apply { get; set; }

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
        public void Copy(MAssets src)
        {
            this.Id = src.Id;
            this.AssetsId = src.AssetsId;
            this.SuitNo = src.SuitNo;
            this.AssetType = src.AssetType;
            this.Kingaku = src.Kingaku;
            this.DueDate = src.DueDate;
            this.PaymentMethod = src.PaymentMethod;
            this.TransactionDetails = src.TransactionDetails;
            this.Payer = src.Payer;
            this.PaymentDestination = src.PaymentDestination;
            this.Apply = src.Apply;
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
        public MAssets Reverse()
        {
            var dst = new MAssets();
            dst.Id = this.Id;
            dst.AssetsId = this.AssetsId;
            dst.SuitNo = this.SuitNo;
            dst.AssetType = this.AssetType;
            dst.Kingaku = this.Kingaku;
            dst.DueDate = this.DueDate;
            dst.PaymentMethod = this.PaymentMethod;
            dst.TransactionDetails = this.TransactionDetails;
            dst.Payer = this.Payer;
            dst.PaymentDestination = this.PaymentDestination;
            dst.Apply = this.Apply;
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
