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
    }
}