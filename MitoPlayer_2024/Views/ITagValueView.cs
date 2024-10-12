using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface ITagValueView
    {
        void InitializeTagListBindingSource(BindingSource tagList);
        void InitializeTagValueListBindingSource(BindingSource tagValueList);
        void ReloadTagListBindingSource(Tag tag);
        void ReloadTagValueListBindingSource();
        event EventHandler CreateTag;
        event EventHandler<ListEventArgs> EditTag;
        event EventHandler<ListEventArgs> DeleteTag;
        event EventHandler<ListEventArgs> CreateTagValue;
        event EventHandler<ListEventArgs> EditTagValue;
        event EventHandler<ListEventArgs> DeleteTagValue;
        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;
        event EventHandler<ListEventArgs> SetCurrentTagId;
        event EventHandler<ListEventArgs> SetCurrentTagValueId;
        event EventHandler OpenTagValueImportViewEvent;
        void Show();
    }
}
