using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Models
{
    public interface ISettingDao
    {
        #region INITIALIZE
        bool IsConnectionStringValid(String preConnectionString);
        bool IsDatabaseExists(String preConnectionString);
        ResultOrError CreateDatabase(String preConnectionString);
        ResultOrError CreateTableStructure();
        ResultOrError CreateColumns();
        ResultOrError InitializeProfileSettings();
        void InitializeKeys(ref String[] keyNameArray,ref String[] keyColorArray);
        void SetProfileId(int profileId);
        int GetProfileId();

        ResultOrError DeleteDatabase();
        #endregion

        #region SETTINGS
        int GetNextId(String tableName);
        ResultOrError CreateStringSetting(int id, String name, String value, bool withoutProfile = false);
        ResultOrError CreateIntegerSetting(int id, String name, Int32 value, bool withoutProfile = false);
        ResultOrError CreateDecimalSetting(int id, String name, Decimal value, bool withoutProfile = false);
        ResultOrError CreateBooleanSetting(int id, String name, Boolean value, bool withoutProfile = false);
        String GetStringSetting(string name, bool withoutProfile = false);
        int GetIntegerSetting(string name, bool withoutProfile = false);
        decimal GetDecimalSetting(string name, bool withoutProfile = false);
        bool? GetBooleanSetting(string name, bool withoutProfile = false);
        ResultOrError SetStringSetting(String name, String value, bool withoutProfile = false);
        ResultOrError SetIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        ResultOrError SetDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        ResultOrError SetBooleanSetting(String name, Boolean value, bool withoutProfile = false);
        ResultOrError DeleteSettings();
        #endregion

        #region COLUMNS
        int GetNextTrackPropertySortingId();
        ResultOrError CreateTrackProperty(TrackProperty tp, bool withoutProfile = false);
        TrackProperty GetTrackProperty(int id, bool withoutProfile = false);
        TrackProperty GetTrackPropertyByNameAndGroup(string name, string group, bool withoutProfile = false);
        List<TrackProperty> GetTrackPropertyListByColumnGroup(String columnGroup, bool withoutProfile = false, bool withAndWithoutProfile = false);
        ResultOrError UpdateTrackProperty(TrackProperty tp, bool withoutProfile = false);
        ResultOrError DeleteTrackProperty(int id, bool withoutProfile = false);
        ResultOrError DeleteAllTrackProperty();
        ResultOrError ClearSettingTable();
        ResultOrError ClearTrackPropertyTable();
        #endregion

    }
}
