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
        public bool IsActive { get; set; }
        public bool IsModelTrainer { get; set; }
        public int ProfileId { get; set; }
        public int Hotkey { get; set; }

        public Playlist() { }

    }
}
