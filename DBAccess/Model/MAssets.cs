using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AutoMapper;
using DBAccess.Model;

namespace DBAccess.Model
{
    /// <summary>
    /// MAssetsの表示用テーブル定義クラスを表します。
    /// </summary>
    public class MAssets :BsMAssets
    {
        public string StrAssetType { get; set; }
    }

}
