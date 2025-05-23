﻿using MitoPlayer_2024.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class HarmonizerView : Form, IHarmonizerView
    {
        public HarmonizerView()
        {
            InitializeComponent();
        }
        #region SINGLETON

        public static HarmonizerView instance;
        public static HarmonizerView GetInstance(Form mainView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new HarmonizerView();
                instance.MdiParent = mainView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
        #endregion



        internal void CallCreatePlaylistEvent()
        {

        }

        internal void CallLoadPlaylistEvent()
        {}

        internal void CallRenamePlaylistEvent()
        {
           }

        internal void CallDeletePlaylistEvent()
        {
           }

        internal void CallPlayTrackEvent()
        {
            }

        internal void CallPauseTrackEvent()
        {
           }

        internal void CallStopTrackEvent()
        {
           }

        internal void CallPrevTrackEvent()
        {
          }

        internal void CallNextTrackEvent()
        {
         }

        internal void CallRandomTrackEvent()
        {
          } internal void CallExportToM3UEvent()
        {
          }
        internal void CallExportToTXTEvent()
        {
        }

        internal void CallExportToDirectoryEvent()
        {
            throw new NotImplementedException();
        }
    }
}
