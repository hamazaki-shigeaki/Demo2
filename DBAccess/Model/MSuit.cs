using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MSuitの表示用テーブル定義クラスを表します。
    /// </summary>
    public class MSuit : BsMSuit
    {
        public string SituationName { get; set; }

        public MCustomer _MCustomer { get; set; } = new();
    }

}
