using MitoPlayer_2024.Helpers;
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

        void SetTagListBindingSource(BindingSource tagList);
        void SetTagValueListBindingSource(BindingSource tagValueList, bool hasMultipleValues = false);
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
