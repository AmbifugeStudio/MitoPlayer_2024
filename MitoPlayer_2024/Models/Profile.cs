using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Profile()
        {
        }

        public Profile(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
