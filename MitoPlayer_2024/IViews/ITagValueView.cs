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
        event EventHandler<Messenger> EditTag;
        event EventHandler<Messenger> DeleteTag;
        event EventHandler<Messenger> CreateTagValue;
        event EventHandler<Messenger> EditTagValue;
        event EventHandler<Messenger> DeleteTagValue;
        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;
        event EventHandler<Messenger> SetCurrentTagId;
        event EventHandler<Messenger> SetCurrentTagValueId;
        event EventHandler OpenTagValueImportViewEvent;
        event EventHandler<Messenger> MoveTagListRowEvent;
        void Show();
    }
}
