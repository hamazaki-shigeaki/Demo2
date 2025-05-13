using System.Security.Cryptography;

namespace Demo.Common
{
    /// <summary>
    /// 暗号化　クラスです。
    /// </summary>
    public class MyEncryptClass
    {
        /// <summary>
        /// 文字列暗号化
        /// </summary>
        /// <param name="plain_text">暗号化対象文字列</param>
        /// <param name="key">暗号化Key</param>
        /// <param name="iv">IV</param>
        /// <returns></returns>
        static public string MyEncrypt(string plain_text, byte[] key, byte[] iv)
        {
            // 暗号化した文字列格納用
            string encrypted_str;

            // Aesオブジェクトを作成
            using (Aes aes = Aes.Create())
            {
                // Encryptorを作成
                using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                {
                    // 出力ストリームを作成
                    using (MemoryStream out_stream = new MemoryStream())
                    {
                        // 暗号化して書き出す
                        using (CryptoStream cs = new CryptoStream(out_stream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                // 出力ストリームに書き出し
                                sw.Write(plain_text);
                            }
                        }
                        // Base64文字列にする
                        byte[] result = out_stream.ToArray();
                        encrypted_str = Convert.ToBase64String(result);
                    }
                }
            }

            return encrypted_str;
        }


        // 復号する関数
        // 引数は暗号化されたテキスト(Base64)、暗号化キー、初期化ベクトル
        /// <summary>
        /// 複合化
        /// </summary>
        /// <param name="base64_text">複合対象文字列</param>
        /// <param name="key">暗号化Key</param>
        /// <param name="iv">IV</param>
        /// <returns></returns>
        static public string MyDecrypt(string base64_text, byte[] key, byte[] iv)
        {
            string plain_text;

            // Base64文字列をバイト型配列に変換
            byte[] cipher = Convert.FromBase64String(base64_text);

            // AESオブジェクトを作成
            using (Aes aes = Aes.Create())
            {
                // 復号器を作成
                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
                {
                    // 入力ストリームを作成
                    using (MemoryStream in_stream = new MemoryStream(cipher))
                    {
                        // 一気に復号
                        using (CryptoStream cs = new CryptoStream(in_stream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                plain_text = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return plain_text;
        }
    }
}
