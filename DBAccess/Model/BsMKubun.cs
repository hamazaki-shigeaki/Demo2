using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMKubunの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMKubun 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? KubunId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? DisplayOder { get; set; }

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
        public void Copy(MKubun src)
        {
            this.Id = src.Id;
            this.KubunId = src.KubunId;
            this.Code = src.Code;
            this.Name = src.Name;
            this.DisplayOder = src.DisplayOder;
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
        public MKubun Reverse()
        {
            var dst = new MKubun();
            dst.Id = this.Id;
            dst.KubunId = this.KubunId;
            dst.Code = this.Code;
            dst.Name = this.Name;
            dst.DisplayOder = this.DisplayOder;
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
