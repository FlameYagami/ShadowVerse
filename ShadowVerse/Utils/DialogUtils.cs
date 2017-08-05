using Dialog;
using ShadowVerse.View;

namespace ShadowVerse.Utils
{
    internal class DialogUtils : BaseDialogUtils
    {
        public static void ShowPackCover()
        {
            var dlg = new PackCover {Owner = GetTopWindow()};
            dlg.ShowDialog();
        }
    }
}