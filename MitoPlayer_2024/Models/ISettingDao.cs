using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface ISettingDao
    {
        void SetProfileId(int profileId);
        void InitializeGlobalSettings();
        void InitializeProfileSettings();
        
        void CreateStringSetting(int id, String name, String value, bool withoutProfile = false);
        void CreateIntegerSetting(int id, String name, Int32 value, bool withoutProfile = false);
        void CreateDecimalSetting(int id, String name, Decimal value, bool withoutProfile = false);
        void CreateBooleanSetting(int id, String name, Boolean value, bool withoutProfile = false);

        String GetStringSetting(string name, bool withoutProfile = false);
        int GetIntegerSetting(string name, bool withoutProfile = false);
        decimal GetDecimalSetting(string name, bool withoutProfile = false);
        bool? GetBooleanSetting(string name, bool withoutProfile = false);

        void SetStringSetting(String name, String value, bool withoutProfile = false);
        void SetIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        void SetDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        void SetBooleanSetting(String name, Boolean value, bool withoutProfile = false);


        void ClearSettingTable();

    }
}
