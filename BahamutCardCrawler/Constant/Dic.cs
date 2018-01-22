using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahamutCardCrawler.Constant
{
    public class Dic
    {
        public static Dictionary<int, string> RaceDic = new Dictionary<int, string>
        {
            {0, "人"},
            {1, "神"},
            {2, "魔"}
        };

        public static Dictionary<int, string> RareDic = new Dictionary<int, string>
        {
            {0, "LG"},
            {1, "SSR"},
            {2, "SR"},
            {3, "HR"},
            {4, "R"},
            {5, "HN"},
            {6, "N"},
        };

        public static Dictionary<int, string> CgPathDic = new Dictionary<int, string>();
    }
}
