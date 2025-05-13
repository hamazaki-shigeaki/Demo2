using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MDocumentの表示用テーブル定義クラスを表します
    /// </summary>
    public class MDocument :BsMDocument
    {
        public string StrDocClassification { get; set; }
    }

}
