using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MitoPlayer_2024.Dao
{
    public class SettingDao : BaseDao, ISettingDao
    {
        private int profileId = -1;
        public SettingDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }



        public int GetNextSettingId()
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM Setting ORDER BY Id desc LIMIT 1";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastId = (int)reader[0];
                    }
                }
            }
            return lastId + 1;
        }

        public void InitializeGlobalSettings()
        {
            this.InitializeGlobalStringSetting(Settings.PlaylistColumnNames.ToString());
            this.InitializeGlobalStringSetting(Settings.PlaylistColumnTypes.ToString());
            this.InitializeGlobalStringSetting(Settings.PlaylistColumnVisibility.ToString());
            this.InitializeGlobalStringSetting(Settings.TrackColumnNames.ToString());
            this.InitializeGlobalStringSetting(Settings.TrackColumnTypes.ToString());
            this.InitializeGlobalStringSetting(Settings.TrackColumnVisibility.ToString());

            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedPlaylistId.ToString());
            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedProfileId.ToString());
        }
        private void InitializeGlobalStringSetting(String settingName)
        {
            String stringData = String.Empty;
            stringData = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (!this.StringSettingExists(settingName,true))
            {
                this.CreateStringSetting(this.GetNextSettingId(),settingName, stringData, true);
            }
            else
            {
                this.SetStringSetting(settingName, stringData,true);
            }
        }
        private void InitializeGlobalIntegerSetting(String settingName)
        {
            int integerData = -1;
            integerData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.IntegerSettingExists(settingName,true))
            {
                this.CreateIntegerSetting(this.GetNextSettingId(),settingName, integerData, true);
            }
            else
            {
                this.SetIntegerSetting(settingName, integerData, true);
            }
        }

        public void InitializeProfileSettings()
        {
            this.InitializeStringSetting(Settings.LastOpenDirectoryPath.ToString());
            this.InitializeStringSetting(Settings.PlaylistColumnVisibility.ToString());
            this.InitializeStringSetting(Settings.TrackColumnVisibility.ToString());

            this.InitializeIntegerSetting(Settings.Volume.ToString());
            this.InitializeIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString());
        }
        private void InitializeStringSetting(String settingName)
        {
            String stringData = String.Empty;
            stringData = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (!this.StringSettingExists(settingName))
            {
                this.CreateStringSetting(this.GetNextSettingId(),settingName, stringData);
            }
            else
            {
                this.SetStringSetting(settingName, stringData);
            }
        }
        private void InitializeIntegerSetting(String settingName)
        {
            int integerData = -1;
            integerData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.IntegerSettingExists(settingName))
            {
                this.CreateIntegerSetting(this.GetNextSettingId(), settingName, integerData);
            }
            else
            {
                this.SetIntegerSetting(settingName, integerData);
            }
        }

        public bool StringSettingExists(string name, bool withoutProfile = false)
        {
            bool result = false;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    result = true;
                }
            }

            return result;
        }
        public bool IntegerSettingExists(string name, bool withoutProfile = false)
        {
            bool result = false;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public void CreateStringSetting(int id, String name, String value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Id, Name, StringValue, ProfileId) VALUES (@Id, @Name, @StringValue, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                if (!withoutProfile)
                {
                   command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateIntegerSetting(int id, String name, Int32 value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Id, Name, IntegerValue, ProfileId) VALUES (@Id, @Name, @IntegerValue, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                if (!withoutProfile)
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateDecimalSetting(int id, String name, Decimal value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Id, Name, DecimalValue, ProfileId) VALUES (@Id, @Name, @DecimalValue, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;

                if (!withoutProfile)
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void CreateBooleanSetting(int id, String name, Boolean value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Setting (Id, Name, BooleanValue, ProfileId) VALUES (@Id, @Name, @BooleanValue, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                if (!withoutProfile)
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public String GetStringSetting(string name, bool withoutProfile = false)
        {
            String result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT StringValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            return result;
        }
        public int GetIntegerSetting(string name, bool withoutProfile = false)
        {
            int result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT IntegerValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (int)reader[0];
                    }
                }
            }

            return result;
        }
        public decimal GetDecimalSetting(string name, bool withoutProfile = false)
        {
            decimal result = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT DecimalValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (decimal)reader[0];
                    }
                }
            }

            return result;
        }
        public bool? GetBooleanSetting(string name, bool withoutProfile = false)
        {
            bool? result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT BooleanValue FROM Setting WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (bool)reader[0];
                    }
                }
            }

            return result;
        }
       
        public void SetStringSetting(String name, String value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Setting SET StringValue = @StringValue WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void SetIntegerSetting(String name, Int32 value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Setting SET IntegerValue = @IntegerValue WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }

        }
        public void SetDecimalSetting(String name, Decimal value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Setting SET DecimalValue = @DecimalValue WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }

        }
        public void SetBooleanSetting(String name, Boolean value, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Setting SET BooleanValue = @BooleanValue WHERE Name = @Name AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;
                if (!withoutProfile)
                {
                    
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                }
                else
                {
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }

        }

        public void ClearSettingTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Setting ";
                try
                {
                    command.ExecuteNonQuery();
                    //MessageBox.Show("Track deleted succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
    }
}
