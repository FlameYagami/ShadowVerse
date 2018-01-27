using System.Collections.Generic;

namespace BahamutCardCrawler.Constant
{
    public class Dic
    {
        public static Dictionary<int, string> RaceDic = new Dictionary<int, string>
        {
            {1, "人"},
            {2, "神"},
            {3, "魔"}
        };

        public static Dictionary<int, string> RarityDic = new Dictionary<int, string>
        {
            {0, "LG"},
            {1, "SSR"},
            {2, "SR"},
            {3, "HR"},
            {4, "R"},
            {5, "HN"},
            {6, "N"}
        };

        public static Dictionary<int, string> HrefDic = new Dictionary<int, string>
        {
            {10, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20%8e%da%8e%bc%8e%de%8e%aa%8e%dd%8e%c4%8e%de"},
            {11, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20SS%8e%da%8e%b1"},
            {12, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20S%8e%da%8e%b1"},
            {13, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%da%8e%b1"},
            {14, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20%8e%da%8e%b1"},
            {15, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%c9%8e%b0%8e%cf%8e%d9"},
            {16, "http://seesaawiki.jp/mnga_bahamut/d/%bf%cd%c2%b0%c0%ad%20%8e%c9%8e%b0%8e%cf%8e%d9"},
            {20, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20%8e%da%8e%bc%8e%de%8e%aa%8e%dd%8e%c4%8e%de"},
            {21, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20SS%8e%da%8e%b1"},
            {22, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20S%8e%da%8e%b1"},
            {23, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%da%8e%b1"},
            {24, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20%8e%da%8e%b1"},
            {25, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%c9%8e%b0%8e%cf%8e%d9"},
            {26, "http://seesaawiki.jp/mnga_bahamut/d/%bf%c0%c2%b0%c0%ad%20%8e%c9%8e%b0%8e%cf%8e%d9"},
            {30, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20%8e%da%8e%bc%8e%de%8e%aa%8e%dd%8e%c4%8e%de"},
            {31, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20SS%8e%da%8e%b1"},
            {32, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20S%8e%da%8e%b1"},
            {33, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%da%8e%b1"},
            {34, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20%8e%da%8e%b1"},
            {35, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20%8e%ca%8e%b2%8e%c9%8e%b0%8e%cf%8e%d9"},
            {36, "http://seesaawiki.jp/mnga_bahamut/d/%cb%e2%c2%b0%c0%ad%20%8e%c9%8e%b0%8e%cf%8e%d9"}
        };

        /// <summary>
        ///     图标字典
        /// </summary>
        public static Dictionary<int, string> IconPathDic = new Dictionary<int, string>();

        /// <summary>
        ///     图鉴字典
        /// </summary>
        public static Dictionary<int, string> ImagesPathDic = new Dictionary<int, string>();
    }
}