using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BahamutCardCrawler.Converter
{
    public class ImagePathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var imageUrl = values[0].ToString();
            var imagePath = values[1].ToString();
            return new BitmapImage(new Uri(File.Exists(imagePath) ? imagePath : imageUrl, UriKind.RelativeOrAbsolute));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}