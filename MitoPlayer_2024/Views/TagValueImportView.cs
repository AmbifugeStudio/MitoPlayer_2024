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
        //Dark Color Theme
        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridHeaderColor = System.Drawing.ColorTranslator.FromHtml("#36373a");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color WhiteColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnImport.BackColor = this.ButtonColor;
            this.btnImport.ForeColor = this.FontColor;
            this.btnImport.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.rtxtbScript.BackColor = this.ButtonColor;
            this.rtxtbScript.ForeColor = this.FontColor;

            this.rtxtbTutorial.BackColor = this.ButtonColor;
            this.rtxtbTutorial.ForeColor = this.FontColor;
        }


        public event EventHandler<ListEventArgs> CloseView;


        private void btnImport_Click(object sender, EventArgs e)
        {
            CloseView?.Invoke(this, new ListEventArgs() { StringField1 = this.rtxtbScript.Text });
        }

       
    }

}
