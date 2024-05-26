using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface IColumnVisibilityEditorView
    {
        event EventHandler<ListEventArgs> AddColumn;
        event EventHandler<ListEventArgs> RemoveColumn;
        event EventHandler CloseViewWithOk;
        event EventHandler CloseViewWithCancel;

    }
}
