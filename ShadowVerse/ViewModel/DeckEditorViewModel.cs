using System.Data;
using ShadowVerse.Command;
using ShadowVerse.Utils;
using ShadowVerse.View;
using Wrapper.Utils;

namespace ShadowVerse.ViewModel
{
    public class DeckEditorViewModel
    {
        public static DataSet DsAllCache = new DataSet();

        public DeckEditorViewModel(DeckEditorWindow deckEditorWindow)
        {
            DeckEditorWindow = deckEditorWindow;
            CmdExit = new DelegateCommand {ExecuteCommand = Exit_Click};
            SqliteUtils.FillDataToDataSet(SqlUtils.GetQueryAllSql(), DsAllCache);
        }

        public static DeckEditorWindow DeckEditorWindow { get; set; }
        public DelegateCommand CmdExit { get; set; }

        public void Exit_Click(object obj)
        {
            DialogUtils.ShowPackCover();
        }
    }
}