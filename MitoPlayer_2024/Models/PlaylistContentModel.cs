using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class PlaylistContentModel
    {
        //Fields
        private int id;
        private int playlistId;
        private int trackId;

        //Properties - Validations
        [DisplayName("Playlist Content Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }
        [DisplayName("Playlist Id")]
        public int PlaylistId
        {
            get => playlistId;
            set => playlistId = value;
        }
        [DisplayName("Track Id")]
        public int TrackId
        {
            get => trackId;
            set => trackId = value;
        }
    }
}
