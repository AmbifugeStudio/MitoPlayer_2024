using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class AboutPresenter
    {
        private IAboutView aboutView;
        public Playlist newPlaylist;

        public AboutPresenter(IAboutView aboutView)
        {
            this.aboutView = aboutView;
            this.aboutView.CloseView += AboutView_CloseView;
        }

        private void AboutView_CloseView(object sender, EventArgs e)
        {
            ((AboutView)this.aboutView).Close();
        }

    }
}
