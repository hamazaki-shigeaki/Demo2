using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MDepositTableの表示用テーブル定義クラスを表します。
    /// </summary>
    public class MDepositTable :BsMDepositTable
    {
        /// <summary>
        /// 
        /// </summary>
        public string BankName { get; set; }

        public string CompanyName { get; set; }
    }
}
