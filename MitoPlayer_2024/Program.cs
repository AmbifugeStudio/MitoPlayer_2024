using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Presenters;
using MitoPlayer_2024.Views;
using System;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Windows.Forms;

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
            SQLitePCL.Batteries_V2.Init();


            File.AppendAllText("startup.log", $"App indult: {DateTime.Now}\n");

            SettingDao = new SettingDao();

            //TEST 
            //Properties.Settings.Default.Reset();
            //Properties.Settings.Default.Save();

            ResultOrError result = IsSQLiteDatabasePrepared();
            //ResultOrError result = IsDatabasePrepared();
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

        private static ResultOrError IsSQLiteDatabasePrepared()
        {
            ResultOrError result = new ResultOrError();

            String dbPath = "mitoplayer12dev.db";
            String connectionString = $"Data Source={dbPath};";

            try
            {
                if (!File.Exists(dbPath))
                {
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                    }

                    if (!File.Exists(dbPath))
                    {
                        result.AddError("Database could not be created.");
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError("Database could not be created: " + ex.Message);
            }

            try
            {
                if (result)
                {

                    SettingDao = new SettingDao(connectionString);

                    if (!DatabaseHasTables(dbPath))
                    {
                        

                        result = SettingDao.CreateTableStructure();

                        if (!result)
                        {
                            result.AddError("Datatables could not be created.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError("Datatables could not be created: " + ex.Message);
            }

            if (result)
            {
                ConnectionString = connectionString;
            }

            return result;
        }
        private static bool DatabaseHasTables(string dbPath)
        {
            if (!File.Exists(dbPath))
                return false;

            using (var conn = new SqliteConnection($"Data Source={dbPath};"))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' LIMIT 1;";

                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }

        private static ResultOrError IsDatabasePrepared()
        {
            ResultOrError result = new ResultOrError();
            String dbPath = "mitoplayer12dev";

            // check if saved data exists
            String host = Properties.Settings.Default.Host;
            int portNumber = !String.IsNullOrEmpty(Properties.Settings.Default.Port) ? int.Parse(Properties.Settings.Default.Port) : 0;
            String userName = Properties.Settings.Default.UserName;
            String password = Properties.Settings.Default.Password;
            String database = Properties.Settings.Default.Database;
            database = "mitoplayer12dev";
            String preConnectionString = String.Empty;
            bool isConnectionStringReady = false;

            // if any data is missing, new data must be requested
            // if all fields are filled, check whether the data is valid
            // if the data is not valid, new data must be requested
            if (String.IsNullOrEmpty(host) ||
               portNumber <= 0 ||
               String.IsNullOrEmpty(userName) ||
               String.IsNullOrEmpty(password) ||
               String.IsNullOrEmpty(database))
            {
                if (ShowDatabaseSetupView())
                {
                    host = Properties.Settings.Default.Host;
                    portNumber = int.Parse(Properties.Settings.Default.Port);
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
                    ClearDatabaseSettings();
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
                        ClearDatabaseSettings();
                        Application.Exit();
                    }
                }
            }

            // if the connection string is valid, check whether the database exists
            // if it already exists, that means initialization has already run, so all data is in place, and the program can start
            // if it doesn't exist yet, the database and the table structure must be created
            // in case of any error, the database must be deleted! 
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
                result = SettingDao.DeleteDatabase(dbPath);
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
            Properties.Settings.Default.Port = String.Empty;
            Properties.Settings.Default.UserName = String.Empty;
            Properties.Settings.Default.Password = String.Empty;
            Properties.Settings.Default.Database = String.Empty;
            Properties.Settings.Default.Save();
        }

    }
}
