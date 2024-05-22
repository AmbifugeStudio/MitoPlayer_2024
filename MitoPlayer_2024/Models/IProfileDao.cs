using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface IProfileDao
    {
        Profile GetProfile(int id);
        void CreateProfile(Profile profile);
    }
}
