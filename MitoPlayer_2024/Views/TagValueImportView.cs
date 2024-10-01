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
    public partial class TagValueImportView : Form, ITagValueImportView
    {
        public TagValueImportView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public event EventHandler<ListEventArgs> CloseView;


        private void btnImport_Click(object sender, EventArgs e)
        {
            CloseView?.Invoke(this, new ListEventArgs() { StringField1 = this.rtxtbScript.Text });
        }

       
    }

}
