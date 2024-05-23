using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface ISettingDao
    {
        void CreateStringSetting(String name, String value, bool withoutProfile = false);
        void CreateIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        void CreateDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        void CreateBooleanSetting(String name, Boolean value, bool withoutProfile = false);

        String GetStringSetting(string name, bool withoutProfile = false, bool intern = true);
        int GetIntegerSetting(string name, bool withoutProfile = false, bool intern = true);
        decimal GetDecimalSetting(string name, bool withoutProfile = false, bool intern = true);
        bool? GetBooleanSetting(string name, bool withoutProfile = false, bool intern = true);

        void SetStringSetting(String name, String value, bool withoutProfile = false);
        void SetIntegerSetting(String name, Int32 value, bool withoutProfile = false);
        void SetDecimalSetting(String name, Decimal value, bool withoutProfile = false);
        void SetBooleanSetting(String name, Boolean value, bool withoutProfile = false);

    }
}
