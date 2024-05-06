using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024
{
    public partial class MainView : Form, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public event EventHandler ShowPlaylistView;

        public event EventHandler RemoveMissingTracks;
        public event EventHandler RemoveDuplicatedTracks;

        public event EventHandler OpenFiles;
        public event EventHandler OpenDirectory;

        public event EventHandler PlayTrack;
        public event EventHandler PauseTrack;
        public event EventHandler StopTrack;
        public event EventHandler PrevTrack;
        public event EventHandler NextTrack;
        public event EventHandler RandomTrack;

        public event EventHandler OrderByTitle;
        public event EventHandler OrderByArtist;
        public event EventHandler OrderByFileName;
        public event EventHandler Reverse;
        public event EventHandler Shuffle;
        public event EventHandler Clear;

        private void btnPlayer_Click(object sender, EventArgs e)
        {
            ShowPlaylistView?.Invoke(this, EventArgs.Empty);
        }

        private void removeDeadItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveMissingTracks?.Invoke(this, EventArgs.Empty);
        }

        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveDuplicatedTracks?.Invoke(this, EventArgs.Empty);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFiles?.Invoke(this, EventArgs.Empty);
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDirectory?.Invoke(this, EventArgs.Empty);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopTrack?.Invoke(this, EventArgs.Empty);
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PauseTrack?.Invoke(this, EventArgs.Empty);
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayTrack?.Invoke(this, EventArgs.Empty);
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrevTrack?.Invoke(this, EventArgs.Empty);
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextTrack?.Invoke(this, EventArgs.Empty);
        }

        private void orderByArtistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderByArtist?.Invoke(this, EventArgs.Empty);
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomTrack?.Invoke(this, EventArgs.Empty);
        }

        private void orderByFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderByFileName?.Invoke(this, EventArgs.Empty);
        }

        private void orderByTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderByTitle?.Invoke(this, EventArgs.Empty);
        }

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reverse?.Invoke(this, EventArgs.Empty);
        }

        private void shuffleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shuffle?.Invoke(this, EventArgs.Empty);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear?.Invoke(this, EventArgs.Empty);
        }
    }
}
