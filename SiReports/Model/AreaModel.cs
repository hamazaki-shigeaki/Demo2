using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiReports.Model
{
    /// <summary>
    /// 名前定義領域
    /// </summary>
    public class AreaModel
    {
        /// <summary>
        /// 定義名
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Col { get; set; }
    }
}
