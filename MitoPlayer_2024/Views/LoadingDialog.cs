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
    public partial class LoadingDialog : Form
    {


        public LoadingDialog()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public void SetProcessDescription(String processDescription)
        {
            this.lblProcessDescription.Text = processDescription;
        }

    }
}
