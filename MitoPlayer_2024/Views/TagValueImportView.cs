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
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }
       

        private void SetControlColors()
        {
            this.BackColor = CustomColor.BackColor;
            this.ForeColor = CustomColor.ForeColor;

            this.btnImport.BackColor = CustomColor.ButtonBackColor;
            this.btnImport.ForeColor = CustomColor.ForeColor;
            this.btnImport.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
            this.btnGenerate.BackColor = CustomColor.ButtonBackColor;
            this.btnGenerate.ForeColor = CustomColor.ForeColor;
            this.btnGenerate.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
            this.btnSave.BackColor = CustomColor.ButtonBackColor;
            this.btnSave.ForeColor = CustomColor.ForeColor;
            this.btnSave.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

            this.rtxtbScript.BackColor = CustomColor.ButtonBackColor;
            this.rtxtbScript.ForeColor = CustomColor.ForeColor;

            this.rtxtbTutorial.BackColor = CustomColor.ButtonBackColor;
            this.rtxtbTutorial.ForeColor = CustomColor.ForeColor;
        }


        public event EventHandler<Messenger> CloseViewEvent;
        public event EventHandler<Messenger> SaveScriptEvent;
        public event EventHandler GenerateScriptEvent;

        public void InitilizeScript(string script)
        {
            this.rtxtbScript.Text = script;
        }
        public void InitilizeTutorial(string tutorialText)
        {
            this.rtxtbTutorial.Text = tutorialText;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            CloseViewEvent?.Invoke(this, new Messenger() { StringField1 = this.rtxtbScript.Text });
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateScriptEvent?.Invoke(this, new EventArgs());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveScriptEvent?.Invoke(this, new Messenger() { StringField1 = this.rtxtbScript.Text });
        }

        
    }

}
