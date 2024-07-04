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
    public partial class TagValueEditorView : Form, ITagValueEditorView
    {
        public TagValueEditorView()
        {
            InitializeComponent();
            this.txtTagValueName.Focus();
            this.CenterToScreen();
        }

        public event EventHandler<ListEventArgs> CreateOrEditTagValue;
        public event EventHandler CloseEditor;
        public event EventHandler ChangeColor;
       

        public void SetTagValueName(String name, bool edit = false)
        {
            this.txtTagValueName.Text = name;
            if (edit)
            {
                this.Text = "Edit tag value";
            }
            else
            {
                this.Text = "Create tag value";
            }
        }
        public void SetColor(Color color)
        {
            this.pnlColor.BackColor = color;
        }
       
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CreateOrEditTagValue?.Invoke(this, new ListEventArgs() { StringField1 = txtTagValueName.Text });
        }

        private void txtTagValueName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CreateOrEditTagValue?.Invoke(this, new ListEventArgs() { StringField1 = txtTagValueName.Text });
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseEditor?.Invoke(this, new EventArgs());
            }
        }
        private void btnColorChange_Click(object sender, EventArgs e)
        {
            this.ChangeColor?.Invoke(this, new EventArgs());
        }


        private void pnlColor_Click(object sender, EventArgs e)
        {
            this.ChangeColor?.Invoke(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseEditor?.Invoke(this, new EventArgs());
        }
    }
}
