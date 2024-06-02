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
        public static void DisplayLabel(IContainer _view, System.Windows.Forms.Label _label, String _message)
        {
            _label.Show();
            _label.Text = _message;

            var t = new Timer(_view);
            t.Interval = 3000;
            t.Start();

            t.Tick += (s, e) =>
            {
                _label.Hide();
                t.Stop();
                t.Dispose();
            };
        }

    }
}
