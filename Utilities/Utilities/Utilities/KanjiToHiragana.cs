using System.Text;
using NMeCab;
using Microsoft.International.Converters;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Utilities
{
    public class Kanji
    {
        /// <summary>
        /// 漢字カタカナ変換
        /// </summary>
        /// <param name="text"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public string Henkan(string text, int opt = 0)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            var preText = text.Replace("－","ノ"); 

            MeCabParam mPara = new MeCabParam();
            mPara.DicDir = @"C:\Program Files (x86)\MeCab\dic\ipadic";

            MeCabTagger mTagger = MeCabTagger.Create(mPara);
            var moto = GetKakakana(mTagger.Parse(preText));
            if (opt == 0)
            {
                return moto;
            }
            else if (opt == 1)
            {
                return KanaConverter.KatakanaToHalfwidthKatakana(moto);

            }
            else if (opt == 2)
            {
                return KanaConverter.KatakanaToHiragana(moto);
            }
            return "";
        }

        /// <summary>
        /// カタカナ取得
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetKakakana(string text)
        {
            StringBuilder retKana = new StringBuilder ();
            string retKatakana = "";

            var dataArray = text.Split("\r\n"); 

            for(var c1 = 0; c1 < dataArray.Length - 2; c1++)
            {
                var textArray = dataArray[c1].Split(",");
                retKatakana = retKatakana + textArray[textArray.Length - 2];
            }
            return retKatakana;
        }
    }
}

