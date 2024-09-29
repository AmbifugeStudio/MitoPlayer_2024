using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MitoPlayer_2024.Views
{
    public partial class AboutView : Form, IAboutView
    {

        public event EventHandler CloseView;

        public AboutView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            CloseView?.Invoke(this, e);
        }

        private void AboutView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                CloseView?.Invoke(this, e);
            }
        }

        private void AboutView_Load(object sender, EventArgs e)
        {
            String version = "";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            this.lblVersion.Text = version;
        }
    }
}

