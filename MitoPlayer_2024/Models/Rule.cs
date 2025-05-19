using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public int TagValueId1 { get; set; }
        public int TagValueId2 { get; set; }
        public int Percent { get; set; }

    }
}
