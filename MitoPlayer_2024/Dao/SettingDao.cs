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
        public override int GetNextId(String tableName)
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT Id 
                                        FROM " + tableName + " " +
                                        "ORDER BY Id " +
                                        "desc LIMIT 1";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastId = (int)reader[0];
                    }
                }
                connection.Close();
            }
            return lastId + 1;
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }
        public void InitializeFirstRun()
        {
            if (!this.databaseBuilderDao.TableIsExists(TableName.Setting.ToString()))
            {
                if (this.databaseBuilderDao.BuildDatabase())
                {
                    this.CreateBooleanSetting(this.GetNextId(TableName.Setting.ToString()), Settings.FirstRun.ToString(), true, true);

                    this.InitializeGlobalSettings();

                    String colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnNames.ToString()];
                    String colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnTypes.ToString()];
                    String colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnVisibility.ToString()];
                    String[] colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
                    String[] colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
                    bool[] colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));
                    this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, ColumnGroup.PlaylistColumns.ToString());

                    colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnNames.ToString()];
                    colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnTypes.ToString()];
                    colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnVisibility.ToString()];
                    colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
                    colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
                    colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));
                    this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, ColumnGroup.TracklistColumns.ToString());
                }
            }
        }
        public void InitializeGlobalSettings()
        {
            this.InitializeIntegerSetting(Settings.LastGeneratedPlaylistId.ToString(), true);
            this.InitializeIntegerSetting(Settings.LastGeneratedProfileId.ToString(), true);
            this.InitializeIntegerSetting(Settings.LastGeneratedTagId.ToString(), true);
            this.InitializeIntegerSetting(Settings.LastGeneratedTagValueId.ToString(), true);
        }
        public void InitializeProfileSettings()
        {
            this.InitializeStringSetting(Settings.LastOpenDirectoryPath.ToString());
            this.InitializeStringSetting(Settings.PlaylistColumnVisibility.ToString());
            this.InitializeStringSetting(Settings.TrackColumnVisibility.ToString());

            this.InitializeIntegerSetting(Settings.Volume.ToString());
            this.InitializeIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString());
        }
        private void InitializeStringSetting(String settingName, bool withoutProfile = false)
        {
            string stringData = String.Empty;
            stringData = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (!this.SettingExists(settingName,true))
            {
                this.CreateStringSetting(this.GetNextId(TableName.Setting.ToString()),settingName, stringData, withoutProfile);
            }
            else
            {
                this.SetStringSetting(settingName, stringData, withoutProfile);
            }
        }
        private void InitializeIntegerSetting(String settingName, bool withoutProfile = false)
        {
            int integerData = -1;
            integerData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.SettingExists(settingName,true))
            {
                this.CreateIntegerSetting(this.GetNextId(TableName.Setting.ToString()),settingName, integerData, withoutProfile);
            }
            else
            {
                this.SetIntegerSetting(settingName, integerData, withoutProfile);
            }
        }
        private void InitializeBooleanSetting(String settingName, bool withoutProfile = false)
        {
            bool boolData = false;
            boolData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.SettingExists(settingName, true))
            {
                this.CreateBooleanSetting(this.GetNextId(TableName.Setting.ToString()), settingName, boolData, withoutProfile);
            }
            else
            {
                this.SetBooleanSetting(settingName, boolData, withoutProfile);
            }
        }
        private void InitializeDecimalSetting(String settingName, bool withoutProfile = false)
        {
            decimal decimalData = -1;
            decimalData = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.SettingExists(settingName, true))
            {
                this.CreateDecimalSetting(this.GetNextId(TableName.Setting.ToString()), settingName, decimalData, withoutProfile);
            }
            else
            {
                this.SetDecimalSetting(settingName, decimalData, withoutProfile);
            }
        }
        private bool SettingExists(string name, bool withoutProfile = false)
        {
            bool result = false;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT COUNT(*) 
                                        FROM Setting 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
               
                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

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
                command.CommandText = @"INSERT INTO Setting ( 
                                        Id, 
                                        Name, 
                                        StringValue, 
                                        ProfileId) 

                                        VALUES ( 
                                        @Id, 
                                        @Name, 
                                        @StringValue, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;

                if (!withoutProfile)
                   command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"INSERT INTO Setting ( 
                                        Id, 
                                        Name, 
                                        IntegerValue, 
                                        ProfileId) 

                                        VALUES ( 
                                        @Id, 
                                        @Name, 
                                        @IntegerValue, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"INSERT INTO Setting ( 
                                        Id, 
                                        Name, 
                                        DecimalValue, 
                                        ProfileId) 

                                        VALUES ( 
                                        @Id, 
                                        @Name, 
                                        @DecimalValue, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"INSERT INTO Setting ( 
                                        Id, 
                                        Name, 
                                        BooleanValue, 
                                        ProfileId) 

                                        VALUES ( 
                                        @Id, 
                                        @Name, 
                                        @BooleanValue, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public string GetStringSetting(string name, bool withoutProfile = false)
        {
            string result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT StringValue 
                                        FROM Setting 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
                connection.Close();
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
                command.CommandText = @"SELECT IntegerValue 
                                        FROM Setting 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;

                if (!withoutProfile)
                   command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (int)reader[0];
                    }
                }
                connection.Close();
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
                command.CommandText = @"SELECT DecimalValue 
                                        FROM Setting 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = (decimal)reader[0];
                    }
                }
                connection.Close();
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
                command.CommandText = @"SELECT BooleanValue 
                                        FROM Setting 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = Convert.ToBoolean(reader[0]);
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
                command.CommandText = @"UPDATE Setting 
                                        SET StringValue = @StringValue 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@StringValue", MySqlDbType.VarChar).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"UPDATE Setting 
                                        SET IntegerValue = @IntegerValue 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@IntegerValue", MySqlDbType.Int32).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"UPDATE Setting 
                                        SET DecimalValue = @DecimalValue 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@DecimalValue", MySqlDbType.Decimal).Value = value;
                
                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                command.CommandText = @"UPDATE Setting 
                                        SET BooleanValue = @BooleanValue 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@BooleanValue", MySqlDbType.Bit).Value = value;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteSettings()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Setting 
                                        WHERE ProfileId = @ProfileId  ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        private void InitializeColumns(string[] colNames, string[] colTypes, bool[] colVisibility, string columnGroup)
        {
            for (int i = 0; i <= colNames.Length - 1; i++)
            {
                TrackProperty tp = new TrackProperty();
                tp.Id = this.GetNextId(TableName.TrackProperty.ToString());
                tp.ColumnGroup = columnGroup;
                tp.Name = colNames[i];
                tp.Type = colTypes[i];
                tp.IsEnabled = colVisibility[i];
                tp.SortingId = i;
                tp.ProfileId = -1;
                this.CreateTrackProperty(tp, true);
            }
        }
        public void CreateTrackProperty(TrackProperty tp, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO TrackProperty ( 
                                        Id, 
                                        ColumnGroup, 
                                        Name, 
                                        Type, 
                                        IsEnabled, 
                                        SortingId, 
                                        ProfileId) 

                                        VALUES ( 
                                        @Id, 
                                        @ColumnGroup, 
                                        @Name, 
                                        @Type, 
                                        @IsEnabled, 
                                        @SortingId, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tp.Id;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = tp.ColumnGroup;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tp.Name;
                command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = tp.Type;
                command.Parameters.Add("@IsEnabled", MySqlDbType.Bit).Value = tp.IsEnabled;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = tp.SortingId;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

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
        public TrackProperty GetTrackPropertyByNameAndGroup(string name, string columnGroup, bool withoutProfile = false)
        {
            TrackProperty tp = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TrackProperty 
                                        WHERE Name = @Name 
                                        AND ColumnGroup = @ColumnGroup ";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = columnGroup;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

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
        public List<TrackProperty> GetTrackPropertyListByColumnGroup(string columnGroup, bool withoutProfile = false, bool withAndWithoutProfile = false)
        {
            List<TrackProperty> tpList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TrackProperty 
                                        WHERE ColumnGroup = @ColumnGroup ";

                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = columnGroup;

                if (!withAndWithoutProfile)
                {
                    if (!withoutProfile)
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                    else
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;
                }              

                using (var reader = command.ExecuteReader())
                {
                    tpList = new List<TrackProperty>();
                    while (reader.Read())
                    {
                        TrackProperty tp = new TrackProperty();
                        tp.Id = (int)reader[0];
                        tp.ColumnGroup = (string)reader[1];
                        tp.Name = (string)reader[2];
                        tp.Type = (string)reader[3];
                        tp.IsEnabled = Convert.ToBoolean(reader[4]);
                        tp.SortingId = (int)reader[5];
                        tp.ProfileId = (int)reader[6];
                        tpList.Add(tp);
                    }
                }
            }
            return tpList;
        }

        public void UpdateTrackProperty(TrackProperty tp, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TrackProperty 

                                        SET ColumnGroup = @ColumnGroup, 
                                        Name = @Name, 
                                        Type = @Type, 
                                        IsEnabled = @IsEnabled, 
                                        SortingId = @SortingId 

                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = tp.Id;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = tp.ColumnGroup;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = tp.Name;
                command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = tp.Type;
                command.Parameters.Add("@IsEnabled", MySqlDbType.Bit).Value = tp.IsEnabled;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = tp.SortingId;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

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

        public void DeleteTrackProperty(int id, bool withoutProfile = false)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackProperty 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId  ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;

                if (!withoutProfile)
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                else
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = -1;

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
        public void DeleteAllTrackProperty()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackProperty 
                                        WHERE ProfileId = @ProfileId  ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackProperty is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Setting is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void ClearTrackPropertyTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM TrackProperty ";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackProperty is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        /*  public int GetNextSortingIdInColumnGroup()
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
          }*/

        /* public void CreateTrackProperty(String name, String type, bool isEnable, string columnGroup, bool withoutProfile = false)
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
         }*/












    }
}
