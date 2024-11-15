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
            this.InitializeComponent();
            this.SetControlColors();
            this.txtTagName.Focus();
            this.CenterToScreen();
        }

        public event EventHandler<Messenger> CreateOrEditTag;
        public event EventHandler CloseEditor;
        public event EventHandler<Messenger> ChangeTextColoring;
        public event EventHandler<Messenger> ChangeHasMultipleValues;

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;

        }

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
            bool textColoring = rdbtnText.Checked;
            this.CreateOrEditTag?.Invoke(this, new Messenger() { StringField1 = txtTagName.Text, BooleanField1 = textColoring });
        }

        private void txtTagName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool textColoring = rdbtnText.Checked;
                this.CreateOrEditTag?.Invoke(this, new Messenger() { StringField1 = txtTagName.Text, BooleanField1 = textColoring });
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseEditor?.Invoke(this, new EventArgs());
            }
        }
     
        public void SetHasMultipleValues(bool hasMultipleValues, bool enabled)
        {
            this.chbHasMultipleValues.Checked = hasMultipleValues;
            this.chbHasMultipleValues.Enabled = enabled;
        }   
        public void SetTextColoring(bool textColoring)
        {
            if (textColoring)
            {
                this.rdbtnText.Checked = true;
                this.rdbtnField.Checked = false;
            }
            else
            {
                this.rdbtnText.Checked = false;
                this.rdbtnField.Checked = true;
            }
            
        }

        private void rdbtnText_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbtnText.Checked)
            {
                this.rdbtnField.Checked = false;
                this.ChangeTextColoring?.Invoke(this, new Messenger()
                {
                    BooleanField1 = true
                });
            }
        }

        private void rdbtnField_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbtnField.Checked)
            {
                this.rdbtnText.Checked = false;
                this.ChangeTextColoring?.Invoke(this, new Messenger()
                {
                    BooleanField1 = false
                });
            }
        }
        private void chbHasMultipleValues_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeHasMultipleValues?.Invoke(this, new Messenger()
            {
                BooleanField1 = this.chbHasMultipleValues.Checked
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseEditor?.Invoke(this, new EventArgs());
        }

        
    }
}
