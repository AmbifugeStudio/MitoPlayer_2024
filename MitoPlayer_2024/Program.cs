using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MitoPlayer_2024.Views;
using MitoPlayer_2024.Presenters;
using AxWMPLib;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using System.Data;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024
{
    internal static class Program
    {
        private static String ConnectionString;
        private static SettingDao SettingDao;
        private static ISetupView databaseSetupView { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // ClearDatabaseSettings();
            SettingDao = new SettingDao();

            ResultOrError result = IsDatabasePrepared();
            if (result.Success)
            {
                IMainView mainView = new MainView();
                MainPresenter mainPresenter = new MainPresenter(mainView, ConnectionString, SettingDao);
                Application.Run((Form)mainView);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private static ResultOrError IsDatabasePrepared()
        {
            ResultOrError result = new ResultOrError();

            //vannak-e lementett adatok
            String host = Properties.Settings.Default.Host;
            int portNumber = Properties.Settings.Default.Port;
            String userName = Properties.Settings.Default.UserName;
            String password = Properties.Settings.Default.Password;
            String database = Properties.Settings.Default.Database;

            String preConnectionString = String.Empty;
            bool isConnectionStringReady = false;

            //ha bármelyik hiányzik, be kell kérni új adatokat
            //ha mind ki van töltve, akkor ellenőrizni kell, hogy azok megfelelőek-e
            //ha nem megfelelő, akkor b ekell kérni új adatokat
            if (String.IsNullOrEmpty(host) ||
               portNumber <= 0 ||
               String.IsNullOrEmpty(userName) ||
               String.IsNullOrEmpty(password) ||
               String.IsNullOrEmpty(database))
            {
                if (ShowDatabaseSetupView())
                {
                    host = Properties.Settings.Default.Host;
                    portNumber = Properties.Settings.Default.Port;
                    userName = Properties.Settings.Default.UserName;
                    password = Properties.Settings.Default.Password;
                    database = Properties.Settings.Default.Database;

                    preConnectionString =
                      "Data Source=" + host + ";" +
                      "port=" + portNumber + ";" +
                      "username=" + userName + ";" +
                      "password=" + password + ";";
                    ConnectionString = preConnectionString +
                        "database=" + database + ";";

                    isConnectionStringReady = true;
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                preConnectionString =
                   "Data Source=" + host + ";" +
                   "port=" + portNumber + ";" +
                   "username=" + userName + ";" +
                   "password=" + password + ";";
                ConnectionString = preConnectionString +
                        "database=" + database + ";";

                if (SettingDao.IsConnectionStringValid(preConnectionString))
                {
                    isConnectionStringReady = true;
                }
                else
                {
                    if (ShowDatabaseSetupView())
                    {
                        isConnectionStringReady = true;
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }

            //ha a connection string megfelel, ellenőrizni kell, hogy létezik-e az adatbázis
            //ha már létezik, az azt jelenti, hogy már az inicializálás lefutott, tehát minden adat a helyén, indulhat a program
            //ha még nem létezik, akkor létre kell hozni az adatbázist és a táblastruktúrát
            //bármilyen hiba esetén az adatbázist törölni kell!
            if (isConnectionStringReady)
            {
                if (!SettingDao.IsDatabaseExists(preConnectionString))
                {
                    SettingDao = new SettingDao(ConnectionString);
                    result = SettingDao.CreateDatabase(preConnectionString);
                    if (result.Success)
                    {
                        result = SettingDao.CreateTableStructure();
                    }
                }
                else
                {
                    SettingDao = new SettingDao(ConnectionString);
                }
            }

            if (!result.Success) 
            {
                ClearDatabaseSettings();
                result = SettingDao.DeleteDatabase();
            }

            return result;
        }

        private static bool ShowDatabaseSetupView()
        {
            bool result = false;

            SetupView setupView = new SetupView();
            SetupPresenter presenter = new SetupPresenter(setupView);
            setupView.ShowDialog((SetupView)databaseSetupView);

            result = presenter.IsDatabaseConnectionReady;
            return result;
        }

        private static void ClearDatabaseSettings()
        {
            Properties.Settings.Default.Host = String.Empty;
            Properties.Settings.Default.Port = 0;
            Properties.Settings.Default.UserName = String.Empty;
            Properties.Settings.Default.Password = String.Empty;
            Properties.Settings.Default.Database = String.Empty;
            Properties.Settings.Default.Save();
        }

    }
}
