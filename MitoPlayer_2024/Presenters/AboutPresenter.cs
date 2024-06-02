using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class AboutPresenter
    {
        private IAboutView view;
        public Playlist newPlaylist;

        public AboutPresenter(IAboutView view)
        {
            this.view = view;
            this.view.CloseView += AboutView_CloseView;
        }

        private void AboutView_CloseView(object sender, EventArgs e)
        {
            ((AboutView)this.view).Close();
        }

    }
}
