using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class PreferencesView : Form, IPreferencesView
    {
        public event EventHandler CloseViewWithOkEvent;
        public event EventHandler CloseViewWithCancelEvent;
        public event EventHandler ClearDatabaseEvent;



        public PreferencesView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearDatabaseEvent?.Invoke(this, EventArgs.Empty);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseViewWithOkEvent?.Invoke(this, EventArgs.Empty);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancelEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
