using System;
using System.IO;

namespace Wrapper
{
    public class PathManager
    {
        public static string RootPath = Directory.GetParent(Environment.CurrentDirectory).FullName + "\\";

        public static string PicturePath = RootPath + "picture\\";
        public static string ThumbnailPath = RootPath + "thumbnail\\";
        public static string DeckFolderPath = RootPath + "deck\\";
        public static string TexturesPath = RootPath + "textures\\";
        public static string BackgroundPath = TexturesPath + "Background.jpg";

        public static string AtkPath = $"{TexturesPath}ic_atk.png";
        public static string LifePath = $"{TexturesPath}ic_life.png";
        public static string Cost1Path = $"{TexturesPath}cost_1.png";
        public static string Cost2Path = $"{TexturesPath}cost_1.png";
        public static string Cost3Path = $"{TexturesPath}cost_1.png";
        public static string Cost4Path = $"{TexturesPath}cost_1.png";
        public static string Cost5Path = $"{TexturesPath}cost_1.png";
        public static string Cost6Path = $"{TexturesPath}cost_1.png";
        public static string Cost7Path = $"{TexturesPath}cost_1.png";
        public static string Cost8Path = $"{TexturesPath}cost_1.png";
        public static string Cost9Path = $"{TexturesPath}cost_1.png";
        public static string Cost10Path = $"{TexturesPath}cost_1.png";
    }
}