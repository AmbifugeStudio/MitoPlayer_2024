using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IProfileEditorView
    {
        event EventHandler<ListEventArgs> CreateOrEditProfile;
        event EventHandler CloseProfileEditor;

        void Show();
    }
}
