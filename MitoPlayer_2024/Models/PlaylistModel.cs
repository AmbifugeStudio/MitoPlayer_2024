using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MitoPlayer_2024.Model;

namespace MitoPlayer_2024.Models
{
    public class PlaylistModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderInList { get; set; }

        private List<TrackModel> trackList;

        public PlaylistModel()
        {
        }

        public PlaylistModel(int id, string name, int orderInList)
        {
            this.Id = id;
            this.Name = name;
            this.OrderInList = orderInList;
        }

    }
}
