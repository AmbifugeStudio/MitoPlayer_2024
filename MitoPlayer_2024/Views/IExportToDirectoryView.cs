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
        event EventHandler<ListEventArgs> SetRowNumberEvent;
        event EventHandler<ListEventArgs> SetKeyCodeEvent;
        event EventHandler<ListEventArgs> SetBpmNumberEvent;
        event EventHandler<ListEventArgs> SetTrunkBpmEvent;
        event EventHandler<ListEventArgs> SetTrunkedArtistEvent;
        event EventHandler<ListEventArgs> SetTrunkedTitleEvent;
         event EventHandler<ListEventArgs> SetArtistMinimumCharacterEvent;
         event EventHandler<ListEventArgs> SetTitleMinimumCharacterEvent;
        void InitializeView(String path, bool isRowNumberChecked, bool isKeyCodeChecked, bool isBpmNumberChecked, bool isTrunkBpmChecked,
             bool isTrunkedArtistChecked, bool isTrunkedTitleChecked, decimal artistMinimumCharacter, decimal titleMinimumCharacter);
        void SetTrackListBindingSource(BindingSource trackList);
        void Show();
    }
}
