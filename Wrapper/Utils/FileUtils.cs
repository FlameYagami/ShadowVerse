using System;
using System.IO;
using System.Text;
using Wrapper.Constant;

namespace Wrapper.Utils
{
    public class FileUtils
    {
        public static string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception(StringConst.FileNotExits);
            var content = new StringBuilder();
            content.Append(File.ReadAllText(filePath));
            return content.ToString();
        }

        public static bool SaveFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                var sw = new StreamWriter(fs);
                sw.Write(content);
                sw.Close();
                fs.Close();
                fs.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}