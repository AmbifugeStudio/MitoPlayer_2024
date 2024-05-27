using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface ITagEditorView
    {
        event EventHandler<ListEventArgs> CreateOrEditTag;
        event EventHandler CloseEditor;
        void Show();
    }
}
