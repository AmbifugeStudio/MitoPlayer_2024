using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface ISettingDao
    {
        void CreateStringSetting(String name, String value);
        void CreateIntegerSetting(String name, Int32 value);
        void CreateDecimalSetting(String name, Decimal value);
        void CreateBooleanSetting(String name, Boolean value);
        String GetStringSettingByName(string name);
        int GetIntegerSettingByName(string name);
        decimal GetDecimalSettingByName(string name);
        bool? GetBooleanSettingByName(string name);
        void SetStringSetting(String name, String value);
        void SetIntegerSetting(String name, Int32 value);
        void SetDecimalSetting(String name, Decimal value);
        void SetBooleanSetting(String name, Boolean value);
    }
}
