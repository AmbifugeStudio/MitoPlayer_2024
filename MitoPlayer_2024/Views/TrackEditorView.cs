using MitoPlayer_2024.Model;
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
    public partial class TrackEditorView : Form,ITrackEditorView
    {
        public TrackEditorView()
        {
            InitializeComponent();
        }

        #region SINGLETON

        public static TrackEditorView instance;
        public static TrackEditorView GetInstance(Form mainView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new TrackEditorView();
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
        {
        }

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
        }

        internal void CallExportToM3UEvent()
        {
            throw new NotImplementedException();
        }

        internal void CallExportToTXTEvent()
        {
            throw new NotImplementedException();
        }
    }
}
