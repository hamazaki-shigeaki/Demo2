using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// BsMDocumentの表示用テーブル定義クラスを表します。
    /// </summary>
    public partial class BsMDocument 
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocumentNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuitNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocClassification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "文書名")]
        [Required(ErrorMessage = MessageClass.EM0013)]
        public string DocName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

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
        public void Copy(MDocument src)
        {
            this.Id = src.Id;
            this.DocumentNo = src.DocumentNo;
            this.SuitNo = src.SuitNo;
            this.DocClassification = src.DocClassification;
            this.DocName = src.DocName;
            this.Contents = src.Contents;
            this.FolderName = src.FolderName;
            this.FileName = src.FileName;
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
        public MDocument Reverse()
        {
            var dst = new MDocument();
            dst.Id = this.Id;
            dst.DocumentNo = this.DocumentNo;
            dst.SuitNo = this.SuitNo;
            dst.DocClassification = this.DocClassification;
            dst.DocName = this.DocName;
            dst.Contents = this.Contents;
            dst.FolderName = this.FolderName;
            dst.FileName = this.FileName;
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
