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
    public partial class ProfileEditorView : Form,IProfileEditorView
    {
        public ProfileEditorView()
        {
            InitializeComponent();
        }

        #region SINGLETON

        private static ProfileEditorView
            instance;

        //MDI nélkül kiveszed a containert a pm-ből
        public static ProfileEditorView GetInstance(Form mainView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new ProfileEditorView();
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
    }
}
