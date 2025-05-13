using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MKubunの表示用テーブル定義クラスを表します。
    /// </summary>
    public class MKubun :BsMKubun
    {
        public string DspKubun { get; set; }

        [Display(Name = "区分")]
        [Required(ErrorMessage = MessageClass.EM0011)]
        public string StrKubunId { get; set; }
    }
}
