using System;
using System.Text;
using System.IO;
using System.Reflection;
using Dapper;

namespace DBAccess.Resource
{
    /// <summary>
    /// ＳＱＬ文テキストファイルを読み込むクラスを定義する。
    /// </summary>
    public static class SqlUtil
    {
        /// <summary>
        /// ＳＱＬ文読込
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>取得結果</returns>
        public static string GetSql(string fileName)
        {
            string result = "";
            // 現在実行しているアセンブリを取得する
            Assembly assembly = Assembly.GetExecutingAssembly();

            // アセンブリに埋め込まれているリソース"Sample.resources.hello.txt"のStreamを取得する
            using (var stream = assembly.GetManifestResourceStream("DBAccess.Resource." + fileName))
            {
                // Streamの内容をすべて読み込んで標準出力に表示する
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
                var textArray = result.Split("\r\n");
            }
            return result;
        }

        /// <summary>
        /// ＳＱＬ文読込（パラメータ付き）
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="parameters">条件パラメータ</param>
        /// <returns>取得結果</returns>
        public static string GetSelectParameters(string fileName, DynamicParameters parameters = null)
        {
            string result = "";
            // 現在実行しているアセンブリを取得する
            var assm = Assembly.GetExecutingAssembly();

            // アセンブリに埋め込まれているリソース"Sample.resources.hello.txt"のStreamを取得する
            using (var stream = assm.GetManifestResourceStream("DBAccess.Resource." + fileName))
            {

                // Streamの内容をすべて読み込んで標準出力に表示する
                var reader = new StreamReader(stream);

                result = reader.ReadToEnd();

            }
            var textArray = result.Split("\r\n");

            var sql = textArray[0];
            for (var index = 1; index < textArray.Length; index++)
            {
                var text = textArray[index];
                text = text.Replace("\r", "");
                text = text.Replace("\n", "");
                text = text.Remove(0, 1);
                if (text == "WHERE 1 = 1")
                {
                    sql += " " + textArray[index];
                }
                else
                {
                    var items = text.Split(" ");
                    if (items[0] == "WHERE" || items[0] == "AND" || items[0] == "OR")
                    {
                        foreach (var name in parameters.ParameterNames)
                        {
                            foreach (var item in items)
                            {
                                if (name.Equals(item.Replace("@", "")))
                                {
                                    sql += " " + textArray[index];
                                }
                            }
                        }
                    }
                    else
                    {
                        sql += " " + textArray[index];
                    }
                }
            }
            return sql;
        }

        /// <summary>
        /// ＳＱＬ文条件パラメータ名称修正
        /// </summary>
        /// <param name="sql">ＳＱＬ文</param>
        /// <returns>変換結果</returns>
        public static string SetPatameterSeq(string sql)
        {
            string retSql = sql;
            var tmp = retSql.Replace("\r", "");
            tmp = retSql.Replace("\n", "");
            var items = new StringBuilder();
            var strArray = tmp.Split(" ");

        RETRY:
            foreach (var str in strArray)
            {
                if (str.Length == 0)
                {
                    continue;
                }
                if (str.Substring(0, 1) == "@")
                {
                    int count = 0;
                    int index = retSql.IndexOf(str, 0);
                    while (index != -1)
                    {
                        count++;
                        index = retSql.IndexOf(str, index + str.Length);
                    }
                    if (count > 1)
                    {
                        index = 0;
                        for (var c = 1; c <= count; c++)
                        {
                            index = retSql.IndexOf(str, index + 1);
                            var p1 = str.Replace("\r", "");
                            p1 = p1.Replace("\n", "");
                            retSql = retSql.Insert(index + p1.Length, c.ToString());
                        }
                        strArray = retSql.Split(" ");
                        goto RETRY;
                    }
                }
            }
            return retSql;
        }
    }
}
