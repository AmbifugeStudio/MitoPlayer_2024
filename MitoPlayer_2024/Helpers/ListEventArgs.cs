using AxWMPLib;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class ListEventArgs : EventArgs
    {
        public AxWindowsMediaPlayer MediaPlayer { get; set; }
        public int SelectedTrackIndex { get; set; }
        public double CurrentPlayPosition { get; set; }

        public string[] DragAndDropFiles { get; set; }
        public DataGridViewRowCollection Rows { get; set; }

        public int ColumnIndex { get; set; }

        public String StringField1 { get; set; }
        public int IntegerField1 { get; set; }
        public Playlist Playlist { get; set; }
    }
   
}
