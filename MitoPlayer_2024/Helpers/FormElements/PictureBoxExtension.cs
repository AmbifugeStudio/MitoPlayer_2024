using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class PictureBoxExtension : PictureBox
    {

        public bool IsMainCover { get; set; }
        public bool IsNew { get; set; }
        public bool IsOld { get; set; }
        public String FilePath { get; set; }
        public int TrackIdInPlaylist { get; set; }


        public PictureBoxExtension()
        {

        }
    }
}
