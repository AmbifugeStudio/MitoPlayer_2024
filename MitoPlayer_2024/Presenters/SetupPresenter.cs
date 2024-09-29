
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class SetupPresenter
    {
        private ISetupView setupView;
        public bool IsDatabaseConnectionReady { get; set; }

        public SetupPresenter(ISetupView view)
        {
            this.setupView = view;
            this.setupView.CloseWithOk += SetupView_CloseWithOk;
        }

        private void SetupView_CloseWithOk(object sender, Helpers.ListEventArgs e)
        {
            String host = e.StringField1;
            String port = e.StringField2;
            String userName = e.StringField3;
            String password = e.StringField4;
            String database = System.Configuration.ConfigurationManager.AppSettings["DefaultDatabaseName"];
            int portNumber = 0;
            bool result = true;

            if (result)
            {
                if (String.IsNullOrEmpty(host))
                {
                    result = false;
                    MessageBox.Show("Host is missing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (result)
            {
                if (String.IsNullOrEmpty(port))
                {
                    result = false;
                    MessageBox.Show("Port is missing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        Int32.TryParse(port, out portNumber);
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                   
                }
            }
            if (result)
            {
                if (String.IsNullOrEmpty(userName))
                {
                    result = false;
                    MessageBox.Show("User name is missing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (result)
            {
                if (String.IsNullOrEmpty(password))
                {
                    result = false;
                    MessageBox.Show("Password is missing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (result)
            {
                String connectionString = "Data Source=" + host + ";port=" + portNumber + ";username=" + userName + ";password=" + password + ";";

                try
                {
                    using (var connection = new MySqlConnection(connectionString))
                    using (var command = new MySqlCommand())
                    {
                        connection.Open();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    MessageBox.Show("Database connection is invalid!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
            this.IsDatabaseConnectionReady = result;

            if (result)
            {
                Properties.Settings.Default.Host = host;
                Properties.Settings.Default.Port = portNumber;
                Properties.Settings.Default.UserName = userName;
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.Database = database;
                Properties.Settings.Default.Save();
                ((SetupView)this.setupView).Close();
            }
        }
    }
}
