﻿using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public interface IPlaylistEditorView
    {

        event EventHandler<Messenger> ChangeName;
        event EventHandler<Messenger> ChangeHotkey;
        event EventHandler<Messenger> ChangeIsModelTrainer;
        event EventHandler CloseWithOk;
        event EventHandler CloseWithCancel;

    }
}
