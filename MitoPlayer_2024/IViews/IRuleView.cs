using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.IViews
{
    public interface IRuleView
    {

        event EventHandler CreateRule;
        event EventHandler<Messenger> EditRule;
        event EventHandler<Messenger> DeleteRule;

        event EventHandler<Messenger> SetTagEvent;
        event EventHandler<Messenger> SetTagPercentEvent;

        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;

        void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary);
        void InitializeRuleList(DataTableModel model);
        void ReloadRuleList(DataTableModel model);
        void InitializeTagPanel(List<Tag> tagList);
    }

}
