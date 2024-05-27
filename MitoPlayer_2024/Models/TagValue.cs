using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class TagValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TagId { get; set; }
        public int ProfileId { get; set; }
        public TagValue()
        {
            this.Id = -1;

        }

    }
}
