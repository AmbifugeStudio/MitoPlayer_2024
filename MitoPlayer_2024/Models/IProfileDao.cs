using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface IProfileDao
    {
        #region PROFILE
        int GetNextId(String tableName);
        ResultOrError CreateProfile(Profile profile);
        Profile GetActiveProfile();
        Profile GetProfile(int id);
        Profile GetProfileByName(String name);
        List<Profile> GetAllProfile();
        void UpdateProfile(Profile profile);
        void DeleteProfile(int id);
        void ClearProfileTable();
        #endregion
    }
}
