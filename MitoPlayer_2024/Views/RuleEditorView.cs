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
    public partial class RuleEditorView : Form,IRuleEditorView
    {
        public RuleEditorView()
        {
            InitializeComponent();
        }

        #region SINGLETON

        public static RuleEditorView instance;
        public static RuleEditorView GetInstance(Form mainView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new RuleEditorView();
                instance.MdiParent = mainView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        private void btnOk_Click(object sender, EventArgs e)
        {

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

    }
}
