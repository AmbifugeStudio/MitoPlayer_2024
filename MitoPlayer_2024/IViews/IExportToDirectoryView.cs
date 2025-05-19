using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface IExportToDirectoryView
    {
        event EventHandler CloseViewWithOk;
        event EventHandler CloseViewWithCancel;
        event EventHandler BrowseEvent;
        event EventHandler<Messenger> SetRowNumberEvent;
        event EventHandler<Messenger> SetKeyCodeEvent;
        event EventHandler<Messenger> SetBpmNumberEvent;
        event EventHandler<Messenger> SetTrunkBpmEvent;
        event EventHandler<Messenger> SetTrunkedArtistEvent;
        event EventHandler<Messenger> SetTrunkedTitleEvent;
        event EventHandler<Messenger> SetArtistMinimumCharacterEvent;
        event EventHandler<Messenger> SetTitleMinimumCharacterEvent;
        void InitializeView(String path, bool isRowNumberChecked, bool isKeyCodeChecked, bool isBpmNumberChecked, bool isTrunkBpmChecked,
             bool isTrunkedArtistChecked, bool isTrunkedTitleChecked, decimal artistMinimumCharacter, decimal titleMinimumCharacter);
        void SetTrackListBindingSource(BindingSource trackList);
        void Show();
    }
}
