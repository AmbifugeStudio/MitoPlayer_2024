using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface IColumnVisibilityEditorView
    {
        void SetColumnListBindingSource(BindingSource columnList, int selectedIndex = 0);
        event EventHandler<ListEventArgs> ChangeVisibility;
        event EventHandler<ListEventArgs> MoveUp;
        event EventHandler<ListEventArgs> MoveDown;
        event EventHandler CloseViewWithOk;
        event EventHandler CloseViewWithCancel;

    }
}
