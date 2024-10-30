using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public class ImageExtension
    {
        public Image Image {  get; set; }

        public String FilePath { get; set; }
        public bool IsMainCover {  get; set; }
        public int TrackIdInPlaylist { get; set; }

        public ImageExtension()
        {
            this.IsMainCover = false;
        }
    }
}
