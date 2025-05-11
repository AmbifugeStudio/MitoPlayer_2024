using MitoPlayer_2024.Helpers.ErrorHandling;
using System;
using System.Collections.Generic;

namespace MitoPlayer_2024.Models
{
    public interface ISettingDao
    {
        #region INITIALIZE
        int GetNextId(string tableName);
        bool IsConnectionStringValid(String preConnectionString);
        bool IsDatabaseExists(String preConnectionString);
        ResultOrError CreateDatabase(String preConnectionString);
        ResultOrError CreateTableStructure();
        ResultOrError CreateColumns();
        ResultOrError InitializeProfileSettings();
        void InitializeKeys(ref String[] keyNameArray,ref String[] keyColorArray);
        void SetProfileId(int profileId);
        int GetProfileId();

        ResultOrError DeleteDatabase(String databaseFilePath);
        #endregion

        #region SETTINGS
        ResultOrError CreateStringSetting(String name, String value, bool withoutProfile = false);
        ResultOrError CreateIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        ResultOrError CreateDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        ResultOrError CreateBooleanSetting(String name, Boolean value, bool withoutProfile = false);
        ResultOrError<String> GetStringSetting(string name, bool withoutProfile = false);
        ResultOrError<int> GetIntegerSetting(string name, bool withoutProfile = false);
        ResultOrError<decimal> GetDecimalSetting(string name, bool withoutProfile = false);
        ResultOrError<bool?> GetBooleanSetting(string name, bool withoutProfile = false);
        ResultOrError SetStringSetting(String name, String value, bool withoutProfile = false);
        ResultOrError SetIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        ResultOrError SetDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        ResultOrError SetBooleanSetting(String name, Boolean value, bool withoutProfile = false);
        ResultOrError DeleteSettings();
        #endregion

        #region COLUMNS
        ResultOrError<int> GetNextTrackPropertySortingId();
        ResultOrError CreateTrackProperty(TrackProperty tp, bool withoutProfile = false);
        ResultOrError<TrackProperty> GetTrackProperty(int id, bool withoutProfile = false);
        ResultOrError<TrackProperty> GetTrackPropertyByNameAndGroup(string name, string group, bool withoutProfile = false);
        ResultOrError<List<TrackProperty>> GetTrackPropertyListByColumnGroup(String columnGroup, bool withoutProfile = false, bool withAndWithoutProfile = false);
        ResultOrError UpdateTrackProperty(TrackProperty tp, bool withoutProfile = false);
        ResultOrError DeleteTrackProperty(int id, bool withoutProfile = false);
        ResultOrError DeleteAllTrackProperty();
        ResultOrError ClearSettingTable();
        ResultOrError ClearTrackPropertyTable();
        #endregion

    }
}
