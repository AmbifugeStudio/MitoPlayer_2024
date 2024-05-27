using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }

        public Tag()
        {
            this.Id = -1;

        }

    }
}
