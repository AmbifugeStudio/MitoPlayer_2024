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
        public bool CellOnly { get; set; }
        public bool HasMultipleValues { get; set; }
        public bool Integrated { get; set; }

        public Tag(){
            this.Integrated = false;
            this.CellOnly = false;
            this.HasMultipleValues = false;
        }

    }
}
