using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace MitoPlayer_2024.Dao
{
    public class SettingDao : BaseDao, ISettingDao
    {
        private int profileId = -1;
        private DataBaseBuilderDao databaseBuilderDao { get; set; }
        public SettingDao(string connectionString)
        {
            this.connectionString = connectionString;
            this.databaseBuilderDao = new DataBaseBuilderDao(connectionString);
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }
        public void InitializeFirstRun()
        {
            if (!this.databaseBuilderDao.TableIsExists("Setting"))
            {
                if (this.databaseBuilderDao.BuildDatabase())
                {
                    this.CreateBooleanSetting(this.GetNextSettingId(), Settings.FirstRun.ToString(), true, true);

                    this.InitializeGlobalSettings();

                    String colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnNames.ToString()];
                    String colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnTypes.ToString()];
                    String colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnVisibility.ToString()];
                    String[] colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
                    String[] colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
                    bool[] colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));
                    this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, "PlaylistColumns");

                    colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnNames.ToString()];
                    colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnTypes.ToString()];
                    colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnVisibility.ToString()];
                    colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
                    colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
                    colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));
                    this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, "TracklistColumns");
                }
            }
        }
        
        public bool SettingExists(string name, bool withoutProfile = false)
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
            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedPlaylistId.ToString());
            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedProfileId.ToString());
            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedTagId.ToString());
            this.InitializeGlobalIntegerSetting(Settings.LastGeneratedTagValueId.ToString());
        }
        private void InitializeGlobalStringSetting(String settingName)
        {
            String stringData = String.Empty;
            stringData = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (!this.SettingExists(settingName,true))
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

            if (!this.SettingExists(settingName,true))
            {
                this.CreateIntegerSetting(this.GetNextSettingId(),settingName, integerData, true);
            }
            else
            {
                this.SetIntegerSetting(settingName, integerData, true);
            }
        }
        private void InitializeGlobalBooleanSetting(String settingName)
        {
            Boolean boolData = false;
            boolData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.SettingExists(settingName, true))
            {
                this.CreateBooleanSetting(this.GetNextSettingId(), settingName, boolData, true);
            }
            else
            {
                this.SetBooleanSetting(settingName, boolData, true);
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

            if (!this.SettingExists(settingName))
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

            if (!this.SettingExists(settingName))
            {
                this.CreateIntegerSetting(this.GetNextSettingId(), settingName, integerData);
            }
            else
            {
                this.SetIntegerSetting(settingName, integerData);
            }
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

        public void InitializeColumns(string[] colNames, string[] colTypes, bool[] colVisibility, string columnGroup)
        {
            for (int i = 0; i <= colNames.Length - 1; i++)
            {
                TrackProperty tp = new TrackProperty();
                tp.Id = this.GetNextTrackPropertyId();
                tp.ColumnGroup = columnGroup;
                tp.Name = colNames[i];
                tp.Type = colTypes[i];
                tp.IsEnabled = colVisibility[i];
                tp.SortingId = i;
                tp.ProfileId = -1;

                this.CreateTrackProperty(tp);
            }
        }
        public int GetNextTrackPropertyId()
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM TrackProperty ORDER BY Id desc LIMIT 1";
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
        public int GetNextSortingIdInColumnGroup()
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT SortingId FROM TrackProperty WHERE ColumnGroup = @ColumnGroup ORDER BY SortingId desc LIMIT 1";
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = ColumnGroup.TracklistColumns.ToString();
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
        public void CreateTrackProperty(TrackProperty tp)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO TrackProperty (Id, ColumnGroup, Name, Type, IsEnabled, SortingId, ProfileId) " +
                    "VALUES (@Id, @ColumnGroup, @Name, @Type, @IsEnabled, @SortingId, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tp.Id;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = tp.ColumnGroup;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tp.Name;
                command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = tp.Type;
                command.Parameters.Add("@IsEnabled", MySqlDbType.Bit).Value = tp.IsEnabled;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = tp.SortingId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = tp.ProfileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackProperty is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void AddColumn(String name, String type, bool isEnable, string columnGroup, bool withoutProfile = false)
        {
            TrackProperty tp = new TrackProperty();
            tp.Id = this.GetNextTrackPropertyId();
            tp.ColumnGroup = columnGroup;
            tp.Name = name;
            tp.Type = type;
            tp.IsEnabled = isEnable;
            tp.SortingId = this.GetNextSortingIdInColumnGroup();
            if (withoutProfile)
            {
                tp.ProfileId = -1;
            }
            else
            {
                tp.ProfileId = this.profileId;
            }
            this.CreateTrackProperty(tp);
        }
        
        public TrackProperty GetTrackPropertyByNameAndGroup(string name, string group)
        {
            TrackProperty tp = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM TrackProperty WHERE Name = @Name AND ColumnGroup = @ColumnGroup ";
                using (var reader = command.ExecuteReader())
                {
                    tp = new TrackProperty();
                    while (reader.Read())
                    {
                        tp.Id = (int)reader[0];
                        tp.ColumnGroup = (string)reader[1];
                        tp.Name = (string)reader[2];
                        tp.Type = (string)reader[3];
                        tp.IsEnabled = (bool)reader[4];
                        tp.SortingId = (int)reader[5];
                        tp.ProfileId = (int)reader[6];
                    }
                }
            }
            return tp;
        }

        public void UpdateColumn(TrackProperty tp, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TrackProperty SET ColumnGroup = @ColumnGroup, Name = @Name, Type = @Type,
                                        IsEnable = @IsEnable, SortingId = @SortingId WHERE Id = @Id AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tp.Id;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = tp.ColumnGroup;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tp.Name;
                command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = tp.Type;
                command.Parameters.Add("@IsEnabled", MySqlDbType.Bit).Value = tp.IsEnabled;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = tp.SortingId;

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
                    MessageBox.Show("TrackProperty is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteColumn(int id, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackProperty WHERE Id = @Id AND ProfileId = @ProfileId  ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
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
                    MessageBox.Show("TrackProperty is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
    }
}
