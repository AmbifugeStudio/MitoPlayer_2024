using MitoPlayer_2024.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MitoPlayer_2024.Helpers
{
    public static class LabelTimer
    {
        private static Timer timer;
        public static SettingDao settingDao;
        public static void DisplayLabel(IContainer _view,System.Windows.Forms.Label _label, String _message, decimal timeInSec)
        {
            if(timer == null)
            {
                timer = new Timer(_view);
                timer.Interval = (int)(timeInSec * 1000);
                timer.Start();
            }
            else
            {
                timer.Stop();
                timer.Interval = (int)(timeInSec * 1000);
                timer.Start();
            }

            _label.Show();
            _label.Text = _message;

            if (timer != null)
            {
                timer.Tick += (s, e) =>
                {
                    _label.Hide();
                    timer.Stop();
                    timer.Dispose();
                };
            }
           
        }

    }
}
