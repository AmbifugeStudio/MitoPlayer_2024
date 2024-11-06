using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class TrackTagValue
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int? TagValueId { get; set; }
        public string TagValueName { get; set; }
        public bool HasMultipleValues { get; set; }
        public string Value { get; set; }
        public int ProfileId { get; set; }

        public TrackTagValue() { }
    }
}
