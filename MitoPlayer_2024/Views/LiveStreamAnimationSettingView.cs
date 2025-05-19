using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class LiveStreamAnimationSettingView : Form, ILiveStreamAnimationSettingView
    {
        public event EventHandler BrowseDirectoryEvent;
        public event EventHandler<Messenger> SetImagePathEvent;

        public event EventHandler CloseViewWithOkEvent;
        public event EventHandler CloseViewWithCancelEvent;

        public LiveStreamAnimationSettingView()
        {
            this.InitializeComponent();
            this.SetControlColors();
            this.CenterToScreen();
        }

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");

        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.btnBrowse.BackColor = this.BackgroundColor;
            this.btnBrowse.ForeColor = this.FontColor;
            this.btnBrowse.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnOk.BackColor = this.BackgroundColor;
            this.btnOk.ForeColor = this.FontColor;
            this.btnOk.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnCancel.BackColor = this.BackgroundColor;
            this.btnCancel.ForeColor = this.FontColor;
            this.btnCancel.FlatAppearance.BorderColor = this.ButtonBorderColor;
        }

        public void InitializeView(Messenger messenger)
        {
            this.txtbImagePath.Text = messenger.StringField1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.BrowseDirectoryEvent?.Invoke(this, EventArgs.Empty);
        }
        private void SetImagePath()
        {
            this.SetImagePathEvent?.Invoke(this, new Messenger { StringField1 = this.txtbImagePath.Text });
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.SetImagePath();
            this.CloseViewWithOkEvent?.Invoke(this, EventArgs.Empty);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancelEvent?.Invoke(this, EventArgs.Empty);
        }

        
    }
}
