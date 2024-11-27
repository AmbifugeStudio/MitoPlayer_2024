using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Model
{

    public class Track
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public int Year { get; set; }
        public double Length { get; set; }
        public bool IsMissing { get; set; }
        public int OrderInList { get; set; }
        public int TrackIdInPlaylist { get; set; }
        public int ProfileId { get; set; }
        public List<TrackTagValue> TrackTagValues { get; set; }
        public bool IsNew { get; set; }
        public string Key { get; set; }
        public float[] Features { get; set; }

        public Track() {
            this.Id = -1;
        }

    }
}
