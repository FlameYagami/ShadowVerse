using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using BahamutCardCrawler.Utils;

namespace BahamutCardCrawler.Converter
{
    public class IconPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var md5 = value?.ToString();
            var cardModel = CardUtils.GetCardModel(md5);
            var iconUrl = cardModel.IconUrl;
            var iconPath = CardUtils.GetIconPath(cardModel);
            return File.Exists(iconPath) ? iconPath : iconUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}