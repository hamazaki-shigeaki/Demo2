using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MStaffの表示用テーブル定義クラスを表します。
    /// </summary>
    public class MStaff :BsMStaff
    {
        public string DisplaySex { get; set; }

        public string DisplaySitenCd { get; set; }
    }

}
