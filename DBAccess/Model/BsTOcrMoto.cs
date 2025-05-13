using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsTOcrMotoの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsTOcrMoto 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuitNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ImportDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Seqno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Yyyy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Dd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Apply2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? SiharaiKin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? AzukariKin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Zandaka { get; set; }

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
        public void Copy(TOcrMoto src)
        {
            this.Id = src.Id;
            this.SuitNo = src.SuitNo;
            this.ImportDate = src.ImportDate;
            this.Seqno = src.Seqno;
            this.Yyyy = src.Yyyy;
            this.Mm = src.Mm;
            this.Dd = src.Dd;
            this.Apply = src.Apply;
            this.Apply2 = src.Apply2;
            this.SiharaiKin = src.SiharaiKin;
            this.AzukariKin = src.AzukariKin;
            this.Zandaka = src.Zandaka;
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
        public TOcrMoto Reverse()
        {
            var dst = new TOcrMoto();
            dst.Id = this.Id;
            dst.SuitNo = this.SuitNo;
            dst.ImportDate = this.ImportDate;
            dst.Seqno = this.Seqno;
            dst.Yyyy = this.Yyyy;
            dst.Mm = this.Mm;
            dst.Dd = this.Dd;
            dst.Apply = this.Apply;
            dst.Apply2 = this.Apply2;
            dst.SiharaiKin = this.SiharaiKin;
            dst.AzukariKin = this.AzukariKin;
            dst.Zandaka = this.Zandaka;
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
