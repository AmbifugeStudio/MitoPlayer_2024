using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MitoPlayer_2024.Views;
using MitoPlayer_2024.Presenters;
using AxWMPLib;

namespace MitoPlayer_2024
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IMainView mainView = new MainView();
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;

            MainPresenter mainPresenter = new MainPresenter(mainView, sqlConnectionString);
            mainPresenter.Initialize();

            Application.Run((Form)mainView);
        }
    }
}
