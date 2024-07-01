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
    public partial class TagEditorView : Form, ITagEditorView
    {
        public TagEditorView()
        {
            InitializeComponent();
            this.txtTagName.Focus();
            this.CenterToScreen();
        }

        public event EventHandler<ListEventArgs> CreateOrEditTag;
        public event EventHandler CloseEditor;
        public event EventHandler<ListEventArgs> ChangeCellOnly;

        public void SetTagName(String name, bool edit = false)
        {
            this.txtTagName.Text = name;
            if (edit)
            {
                this.Text = "Edit tag";
            }
            else
            {
                this.Text = "Create tag";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CreateOrEditTag?.Invoke(this, new ListEventArgs() { StringField1 = txtTagName.Text, BooleanField1 = chbCellOnly.Checked });
        }

        private void txtTagName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CreateOrEditTag?.Invoke(this, new ListEventArgs() { StringField1 = txtTagName.Text, BooleanField1 = chbCellOnly.Checked });
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseEditor?.Invoke(this, new EventArgs());
            }
        }
        public void SetCellOnly(bool value)
        {
            this.chbCellOnly.Checked = value;
        }
        private void chbCellOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCellOnly?.Invoke(this, new ListEventArgs()
            {
                BooleanField1 = this.chbCellOnly.Checked
            });
        }
    }
}
