﻿using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Views
{
    public interface ITagEditorView
    {
        event EventHandler<Messenger> CreateOrEditTag;
        event EventHandler CloseEditor;
        event EventHandler<Messenger> ChangeTextColoring;
        event EventHandler<Messenger> ChangeHasMultipleValues;
        void Show();
    }
}
