using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public class UserSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
       // [DefaultSettingValue("white")]
        public String LastOpenDirectoryPath
        {
            get
            {
                return ((String)this["LastOpenDirectoryPath"]);
            }
            set
            {
                this["LastOpenDirectoryPath"] = (String)value;
            }
        }
        [UserScopedSetting()]
        public int LastOpenFilesFilterIndex
        {
            get
            {
                return ((int)this["LastOpenFilesFilterIndex"]);
            }
            set
            {
                this["LastOpenFilesFilterIndex"] = (int)value;
            }
        }
    }
}
