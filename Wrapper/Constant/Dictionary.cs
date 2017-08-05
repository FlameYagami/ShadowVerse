using System.Collections.Generic;

namespace Wrapper.Constant
{
    public class Dictionary
    {
        // Common
        public static Dictionary<string, string> AbilityTypeDic = new Dictionary<string, string>
        {
            {"Lv", "Lv"},
            {"射程", "【常】射程"},
            {"绝界", "【常】绝界"},
            {"起始卡", "【常】起始卡"},
            {"生命恢复", "【常】生命恢复"},
            {"虚空使者", "【常】虚空使者"},
            {"进化原力", "【自】进化原力"},
            {"零点优化", "【※】零点优化"}
        };

        public static Dictionary<string, string> ImgCampPathDic = new Dictionary<string, string>
        {
            {"红", PathManager.TexturesPath + "Red.png"},
            {"蓝", PathManager.TexturesPath + "Blue.png"},
            {"白", PathManager.TexturesPath + "White.png"},
            {"黑", PathManager.TexturesPath + "Black.png"},
            {"绿", PathManager.TexturesPath + "Green.png"},
            {"无", PathManager.TexturesPath + "Void.png"}
        };

        public static Dictionary<int, string> PackDic = new Dictionary<int, string>
        {
            {100, "Base"},
            {101, "STD"},
            {102, "DRK"},
            {103, "ROB"},
            {104, "TOG"},
            {105, "WLD"}
        };
    }
}