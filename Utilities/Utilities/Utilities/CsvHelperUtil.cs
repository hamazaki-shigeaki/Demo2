using System.Text;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using Microsoft.VisualBasic;

namespace Utilities
{
    public static class CsvHelperUtil
    {

        /// <summary>
        /// 任意CSVHELPERデータを任意のリストクラスにインポートする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csv"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static dynamic ImportOfData<T>(CsvReader csv, dynamic list) where T : class
        {
            // return csv.GetRecords<T>();

            while (csv.Read())
            {

                var rec = csv.GetRecord<T>();
                if (rec == null)
                {
                    continue;
                }
                list.Add(rec);
            }

            return list;
        }

   
    }
}
