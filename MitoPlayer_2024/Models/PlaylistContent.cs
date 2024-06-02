using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public  class PlaylistContent
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        public int OrderInList { get; set; }
        public int TrackIdInPlaylist { get; set; }
        public int ProfileId { get; set; }

        public PlaylistContent() { }
    }
}
