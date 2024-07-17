using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface ITagValueEditorView
    {
        event EventHandler<ListEventArgs> ChangeName;
        event EventHandler ChangeColor;
        event EventHandler<ListEventArgs> ChangeHotkey;

        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;
    }
}
