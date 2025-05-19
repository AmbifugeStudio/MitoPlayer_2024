using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.IViews
{
    public interface IRulesView
    {

        event EventHandler AddTag;
        event EventHandler  RemoveTag;
        event EventHandler CreateRule;
        event EventHandler EditRule;
        event EventHandler RemoveRule;

        event EventHandler CloseWitOk;
        event EventHandler CloseWitCancel;

        event EventHandler SetCurrentTagId;
        event EventHandler SetCurrentRuleId;

        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;

        void InitializeTagListBindingSource(BindingSource tagListBindingSource);
        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<string, Dictionary<string, Color>> tagValueDictionary);
        void ReloadTagListBindingSource(Tag currentTag);
    }

}
