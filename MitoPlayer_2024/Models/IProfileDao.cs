using MitoPlayer_2024.Helpers.ErrorHandling;
using System;
using System.Collections.Generic;

namespace MitoPlayer_2024.Models
{
    public interface IProfileDao
    {
        #region PROFILE
        ResultOrError CreateProfile(Profile profile);
        ResultOrError<Profile> GetActiveProfile();
        ResultOrError<Profile> GetProfile(int id);
        ResultOrError<Profile> GetProfileByName(String name);
        ResultOrError<List<Profile>> GetAllProfile();
        ResultOrError UpdateProfile(Profile profile);
        ResultOrError DeleteProfile(int id);
        ResultOrError ClearProfileTable();
        #endregion
    }
}
