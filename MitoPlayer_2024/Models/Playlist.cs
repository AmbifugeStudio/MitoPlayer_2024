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
        public int QuickListGroup { get; set; }
        public int ProfileId { get; set; }
        public bool IsActive { get; set; }

        public Playlist()
        {
        }

        public Playlist(int id, string name, int orderInList, int quickListGroup,int profileId, bool isActive)
        {
            this.Id = id;
            this.Name = name;
            this.OrderInList = orderInList;
            this.QuickListGroup = quickListGroup;
            this.ProfileId = profileId;
            this.IsActive = isActive;
        }

    }
}
