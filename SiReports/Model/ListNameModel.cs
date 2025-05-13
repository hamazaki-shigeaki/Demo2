using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiReports.Model
{
    public class ListNameModel
    {
        /// <summary>
        /// RowNo
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// Type 0:folder 1:file
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// FullPath
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// ザイズ
        /// </summary>
        public long? FileSize { get; set; }

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
