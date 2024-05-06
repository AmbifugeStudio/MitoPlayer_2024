using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MitoPlayer_2024.Model;

namespace MitoPlayer_2024.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderInList { get; set; }

        private List<Track> trackList;

        public Playlist()
        {
        }

        public Playlist(int id, string name, int orderInList)
        {
            this.Id = id;
            this.Name = name;
            this.OrderInList = orderInList;
        }

    }
}
