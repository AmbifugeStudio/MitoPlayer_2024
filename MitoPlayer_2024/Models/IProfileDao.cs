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
        Profile GetProfileByName(String name);
        void CreateProfile(Profile profile);
        void UpdateProfile(Profile profile);
        void DeleteProfile(int id);
        List<Profile> GetAllProfile();
        int GetLastObjectId(String tableName);
    }
}
