using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class TrackProperty
    {
        public int Id { get; set; }
        public string ColumnGroup { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsEnabled { get; set; }
        public int SortingId { get; set; }
        public int ProfileId { get; set; }

        public TrackProperty()
        {


        }
    }
}
