using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class TagValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TagName { get; set; }
        public int TagId { get; set; }
        public int ProfileId { get; set; }
        public Color Color { get; set; }
        public int Hotkey { get; set; }
        public TagValue() { }

    }
}
