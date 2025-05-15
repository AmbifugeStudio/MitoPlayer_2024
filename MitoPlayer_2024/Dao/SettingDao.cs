using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;

namespace MitoPlayer_2024.Dao
{
    public class SettingDao : BaseDao, ISettingDao
    {
        private int profileId = -1;
        private DataBaseBuilderDao databaseBuilderDao { get; set; }

        public SettingDao()
        {
            this.databaseBuilderDao = new DataBaseBuilderDao();
        }
        public SettingDao(string connectionString)
        {
            this.connectionString = connectionString;
            this.databaseBuilderDao = new DataBaseBuilderDao(connectionString);
        }
        public int GetNextId(string tableName)
        {
            int lastId = -1;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    command.CommandText = $@"SELECT Id 
                                      FROM {tableName} 
                                      ORDER BY Id DESC 
                                      LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastId = reader.ReadInt("Id");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Error occurred while fetching the last ID from table: {tableName}", ex);
            }

            if(lastId == -1)
            {
                return 1;
            }
            else if(lastId > 0)
            {
                return lastId + 1;
            }
            else
            {
                return 1;
            }
        }

        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }
        public int GetProfileId()
        {
            return this.profileId;
        }
        public bool IsConnectionStringValid(String preConnectionString)
        {
            return this.databaseBuilderDao.IsConnectionStringValid(preConnectionString);
        }
        public bool IsDatabaseExists(String preConnectionString)
        {
            return this.databaseBuilderDao.IsDatabaseExists(preConnectionString);
        }
        public ResultOrError CreateDatabase(String preConnectionString)
        {
            return this.databaseBuilderDao.CreateDatabase(preConnectionString);
        }
        public ResultOrError DeleteDatabase(String databaseFilePath)
        {
            return this.databaseBuilderDao.DeleteDatabase(databaseFilePath);
        }
        public ResultOrError CreateTableStructure()
        {
            ResultOrError result = this.databaseBuilderDao.CreateTableStructure();
            if (result.Success)
            {
                result = this.InitializeGlobalSettings();
            }
            return result;
        }

        public ResultOrError CreateColumns()
        {
            ResultOrError result = new ResultOrError();
            String colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnNames.ToString()];
            String colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnTypes.ToString()];
            String colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.PlaylistColumnVisibility.ToString()];
            String[] colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
            String[] colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
            bool[] colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            ResultOrError<TrackProperty> tpResult = this.GetTrackPropertyByNameAndGroup(colNameArray[0], ColumnGroup.PlaylistColumns.ToString());
            
            if (tpResult)
            {
                if (tpResult.Value == null || tpResult.Value.Name == null)
                {
                    result = this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, ColumnGroup.PlaylistColumns.ToString());
                    
                    if (result)
                    {
                        colNames = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnNames.ToString()];
                        colTypes = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnTypes.ToString()];
                        colVisibility = System.Configuration.ConfigurationManager.AppSettings[Settings.TrackColumnVisibility.ToString()];
                        colNameArray = Array.ConvertAll(colNames.Split(','), s => s);
                        colTypeArray = Array.ConvertAll(colTypes.Split(','), s => s);
                        colVisibilityArray = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

                        result = this.InitializeColumns(colNameArray, colTypeArray, colVisibilityArray, ColumnGroup.TracklistColumns.ToString());
                    }
                }
            }
            else
            {
                result.AddError(tpResult.ErrorMessage);
            }

            return result;
        }

        public void InitializeKeys(ref String[] keyNameArray,ref String[] keyColorArray)
        {
            String keyNames = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyCodes.ToString()];
            String keyColors = System.Configuration.ConfigurationManager.AppSettings[Settings.KeyColors.ToString()];
            keyNameArray = Array.ConvertAll(keyNames.Split(','), s => s);
            keyColorArray = Array.ConvertAll(keyColors.Split(','), s => s);
        }
        public ResultOrError InitializeGlobalSettings()
        {
            ResultOrError result = this.InitializeIntegerSetting(Settings.LastGeneratedPlaylistId.ToString(), true);
            if (result)
                this.InitializeIntegerSetting(Settings.LastGeneratedProfileId.ToString(), true);
            if (result)
                this.InitializeIntegerSetting(Settings.LastGeneratedTagId.ToString(), true);
            if (result)
                this.InitializeIntegerSetting(Settings.LastGeneratedTagValueId.ToString(), true);
            return result;
        }
        public ResultOrError InitializeProfileSettings()
        {
            ResultOrError result = this.CreateColumns();

            //INNER SETTINGS
            if (result)
                this.InitializeStringSetting(Settings.LastOpenDirectoryPath.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.LastOpenFilesFilterIndex.ToString());
            if (result)
                this.InitializeStringSetting(Settings.PlaylistColumnVisibility.ToString());
            if (result)
                this.InitializeStringSetting(Settings.TrackColumnVisibility.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.CurrentPlaylistId.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.CurrentSelectorPlaylistId.ToString());

            //SETTING MENU
            if (result)
                this.InitializeBooleanSetting(Settings.AutomaticBpmImport.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.AutomaticKeyImport.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.ImportBpmFromVirtualDj.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.ImportKeyFromVirtualDj.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.PlayTrackAfterOpenFiles.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.PreviewPercentage.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsShortTrackColouringEnabled.ToString());
            if (result)
                this.InitializeDecimalSetting(Settings.ShortTrackColouringThreshold.ToString());

            //PLAYER SETTINGS
            if (result)
                this.InitializeBooleanSetting(Settings.IsShuffleEnabled.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.Volume.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsMuteEnabled.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsPreviewEnabled.ToString());

            //PLAYER FORM VIEW ELEMENT VISIBILITY
            if (result)
                this.InitializeBooleanSetting(Settings.IsTagEditorComponentDisplayed.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsOnlyPlayingRowModeEnabled.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsPlaylistListDisplayed.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsCoverImageComponentDisplayed.ToString());

            //EXPORT TO DIRECTORY
            if (result)
                this.InitializeStringSetting(Settings.LastExportDirectoryPath.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsRowNumberChecked.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsKeyCodeChecked.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsBpmNumberChecked.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsTrunkedBpmChecked.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsTrunkedArtistChecked.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.IsTrunkedTitleChecked.ToString());
            if (result)
                this.InitializeDecimalSetting(Settings.ArtistMinimumCharacter.ToString());
            if (result)
                this.InitializeDecimalSetting(Settings.TitleMinimumCharacter.ToString());

            //SELECTOR
            if (result)
                this.InitializeBooleanSetting(Settings.IsTrackListActive.ToString());

            //LIVE STREAM ANIMATION
            if (result)
                this.InitializeStringSetting(Settings.LiveStreamAnimationImagePath.ToString());
            if (result)
                this.InitializeBooleanSetting(Settings.PreventMusicPlayingWhileStream.ToString());

            //MODEL TRAINING
            if (result)
                this.InitializeBooleanSetting(Settings.IsLogMessageEnabled.ToString());
            if (result)
                this.InitializeDecimalSetting(Settings.LogMessageDisplayTime.ToString());
            if (result)
                this.InitializeIntegerSetting(Settings.TrainingModelBatchCount.ToString());
           
            if (result)
                this.InitializeBooleanSetting(Settings.IsTracklistDetailsDisplayed.ToString());

            return result;
        }
        private ResultOrError InitializeStringSetting(String settingName, bool withoutProfile = false)
        {
            ResultOrError result = new ResultOrError();
            string stringData = String.Empty;
            stringData = System.Configuration.ConfigurationManager.AppSettings[settingName];

            if (!this.IsSettingExists(settingName, withoutProfile))
            {
                result = this.CreateStringSetting(settingName, stringData, withoutProfile);
            }
            return result;
        }
        private ResultOrError InitializeIntegerSetting(String settingName, bool withoutProfile = false)
        {
            ResultOrError result = new ResultOrError();
            int integerData = -1;
            integerData = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.IsSettingExists(settingName, withoutProfile))
            {
                result = this.CreateIntegerSetting(settingName, integerData, withoutProfile);
            }
            return result;
        }
        private ResultOrError InitializeBooleanSetting(String settingName, bool withoutProfile = false)
        {
            ResultOrError result = new ResultOrError();
            bool boolData = false;
            boolData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.IsSettingExists(settingName, withoutProfile))
            {
                result = this.CreateBooleanSetting(settingName, boolData, withoutProfile);
            }
            return result;
        }
        private ResultOrError InitializeDecimalSetting(String settingName, bool withoutProfile = false)
        {
            ResultOrError result = new ResultOrError();
            decimal decimalData = -1;
            decimalData = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings[settingName]);

            if (!this.IsSettingExists(settingName, withoutProfile))
            {
                result = this.CreateDecimalSetting(settingName, decimalData, withoutProfile);
            }
            return result;
        }
        private bool IsSettingExists(string name, bool withoutProfile = false)
        {
            bool result = false;

            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT COUNT(*) 
                                FROM Setting 
                                WHERE Name = @Name 
                                AND ProfileId = @ProfileId";

                command.Parameters.AddWithValue("@Name", name);

                if (!withoutProfile)
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);
                else
                    command.Parameters.AddWithValue("@ProfileId", -1);

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    result = true;
                }
            }

            return result;
        }
        public ResultOrError CreateStringSetting(string name, string value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Setting (Name,StringValue,ProfileId) 
                                    VALUES (@Name,@StringValue,@ProfileId)";

                    command.Parameters.AddWithValue("@Name", name ?? "");
                    command.Parameters.AddWithValue("@StringValue", value ?? "");
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError CreateIntegerSetting(string name, int value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Setting (Name,IntegerValue,ProfileId) 
                                    VALUES (@Name,@IntegerValue,@ProfileId)";

                    command.Parameters.AddWithValue("@Name", name ?? "");
                    command.Parameters.AddWithValue("@IntegerValue", value);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError CreateDecimalSetting(string name, decimal value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Setting (Name,DecimalValue,ProfileId) 
                                    VALUES (@Name,@DecimalValue,@ProfileId)";

                    command.Parameters.AddWithValue("@Name", name ?? "");
                    command.Parameters.AddWithValue("@DecimalValue", value);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError CreateBooleanSetting(string name, bool value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Setting (Name,BooleanValue,ProfileId) 
                                    VALUES (@Name,@BooleanValue,@ProfileId)";

                    command.Parameters.AddWithValue("@Name", name ?? "");
                    command.Parameters.AddWithValue("@BooleanValue", value ? 1 :0);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        public ResultOrError<string> GetStringSetting(string name, bool withoutProfile = false)
        {
            var result = new ResultOrError<string> { Value = null };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT StringValue 
                                    FROM Setting 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = reader.ReadString("StringValue");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching StringSetting with Name [{name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<int> GetIntegerSetting(string name, bool withoutProfile = false)
        {
            var result = new ResultOrError<int> { Value = -1 };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"SELECT IntegerValue 
                                    FROM Setting 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = reader.ReadInt("IntegerValue");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching IntegerSetting with Name [{name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<decimal> GetDecimalSetting(string name, bool withoutProfile = false)
        {
            var result = new ResultOrError<decimal> { Value = -1 };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"SELECT DecimalValue 
                                    FROM Setting 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = reader.ReadDecimal("DecimalValue");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching DecimalSetting with Name [{name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<bool?> GetBooleanSetting(string name, bool withoutProfile = false)
        {
            var result = new ResultOrError<bool?> { Value = null };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"SELECT BooleanValue 
                                    FROM Setting 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = reader.ReadNullableBool("BooleanValue");
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching BooleanSetting with Name [{name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError SetStringSetting(string name, string value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Setting 
                                    SET StringValue = @StringValue 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@StringValue", value);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError SetIntegerSetting(string name, int value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Setting 
                                    SET IntegerValue = @IntegerValue 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@IntegerValue", value);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError SetDecimalSetting(string name, decimal value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Setting 
                                    SET DecimalValue = @DecimalValue 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@DecimalValue", value);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError SetBooleanSetting(string name, bool value, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Setting 
                                    SET BooleanValue = @BooleanValue 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@BooleanValue", value ? 1 : 0); 
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting [{name}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteSettings()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                   
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Setting 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Settings for ProfileId [{this.profileId}] are not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        private ResultOrError InitializeColumns(string[] colNames, string[] colTypes, bool[] colVisibility, string columnGroup)
        {
            ResultOrError result = new ResultOrError();
            for (int i = 0; i <= colNames.Length - 1; i++)
            {
                TrackProperty tp = new TrackProperty();
                tp.ColumnGroup = columnGroup;
                tp.Name = colNames[i];
                tp.Type = colTypes[i];
                tp.IsEnabled = colVisibility[i];
                tp.SortingId = i;
                tp.ProfileId = -1;
                result = this.CreateTrackProperty(tp);
                if (!result.Success)
                {
                    break;
                }
            }
            return result;
        }
        public ResultOrError<int> GetNextTrackPropertySortingId()
        {
            var result = new ResultOrError<int> { Value = -1 };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT SortingId 
                                    FROM TrackProperty 
                                    WHERE ProfileId = @ProfileId 
                                    ORDER BY SortingId DESC 
                                    LIMIT 1";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = reader.ReadInt("SortingId") + 1;
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching next SortingId for TrackProperty. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        public ResultOrError CreateTrackProperty(TrackProperty tp, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO TrackProperty (ColumnGroup,Name,Type,IsEnabled,SortingId,ProfileId) 
                                    VALUES (@ColumnGroup,@Name,@Type,@IsEnabled,@SortingId,@ProfileId)";

                    command.Parameters.AddWithValue("@ColumnGroup", tp.ColumnGroup ?? "");
                    command.Parameters.AddWithValue("@Name", tp.Name ?? "");
                    command.Parameters.AddWithValue("@Type", tp.Type ?? "");
                    command.Parameters.AddWithValue("@IsEnabled", tp.IsEnabled ? 1 : 0); 
                    command.Parameters.AddWithValue("@SortingId", tp.SortingId);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"TrackProperty [{tp.Name}] is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        public ResultOrError<TrackProperty> GetTrackProperty(int id, bool withoutProfile = false)
        {
            var result = new ResultOrError<TrackProperty> { Value = null };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"SELECT * FROM TrackProperty 
                                    WHERE Id = @Id AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = new TrackProperty
                            {
                                Id = reader.ReadInt("Id"),
                                ColumnGroup = reader.ReadString("ColumnGroup"),
                                Name = reader.ReadString("Name"),
                                Type = reader.ReadString("Type"),
                                IsEnabled = reader.ReadBool("IsEnabled"),
                                SortingId = reader.ReadInt("SortingId"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching TrackProperty with Id [{id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        public ResultOrError<TrackProperty> GetTrackPropertyByNameAndGroup(string name, string columnGroup, bool withoutProfile = false)
        {
            var result = new ResultOrError<TrackProperty> { Value = null };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT * FROM TrackProperty 
                                    WHERE Name = @Name 
                                    AND ColumnGroup = @ColumnGroup 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ColumnGroup", columnGroup);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = new TrackProperty
                            {
                                Id = reader.ReadInt("Id"),
                                ColumnGroup = reader.ReadString("ColumnGroup"),
                                Name = reader.ReadString("Name"),
                                Type = reader.ReadString("Type"),
                                IsEnabled = reader.ReadBool("IsEnabled"),
                                SortingId = reader.ReadInt("SortingId"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching TrackProperty by Name [{name}] and ColumnGroup [{columnGroup}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<List<TrackProperty>> GetTrackPropertyListByColumnGroup(string columnGroup, bool withoutProfile = false, bool withAndWithoutProfile = false)
        {
            var result = new ResultOrError<List<TrackProperty>> { Value = new List<TrackProperty>() };

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    if (!withAndWithoutProfile)
                    {
                        command.CommandText = @"SELECT * FROM TrackProperty 
                                        WHERE ColumnGroup = @ColumnGroup 
                                        AND ProfileId = @ProfileId 
                                        ORDER BY SortingId";
                        command.Parameters.AddWithValue("@ColumnGroup", columnGroup);
                        command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);
                    }
                    else
                    {
                        command.CommandText = @"SELECT * FROM TrackProperty 
                                        WHERE ColumnGroup = @ColumnGroup 
                                        ORDER BY SortingId";
                        command.Parameters.AddWithValue("@ColumnGroup", columnGroup);
                        command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tp = new TrackProperty
                            {
                                Id = reader.ReadInt("Id"),
                                ColumnGroup = reader.ReadString("ColumnGroup"),
                                Name = reader.ReadString("Name"),
                                Type = reader.ReadString("Type"),
                                IsEnabled = reader.ReadBool("IsEnabled"),
                                SortingId = reader.ReadInt("SortingId"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };

                            result.Value.Add(tp);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Error occurred while fetching TrackProperty list by ColumnGroup [{columnGroup}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdateTrackProperty(TrackProperty tp, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"UPDATE TrackProperty 
                                    SET ColumnGroup = @ColumnGroup, 
                                        Name = @Name, 
                                        Type = @Type, 
                                        IsEnabled = @IsEnabled, 
                                        SortingId = @SortingId 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", tp.Id);
                    command.Parameters.AddWithValue("@ColumnGroup", tp.ColumnGroup);
                    command.Parameters.AddWithValue("@Name", tp.Name);
                    command.Parameters.AddWithValue("@Type", tp.Type);
                    command.Parameters.AddWithValue("@IsEnabled", tp.IsEnabled ? 1 : 0); 
                    command.Parameters.AddWithValue("@SortingId", tp.SortingId);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"TrackProperty [{tp.Name}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteTrackProperty(int id, bool withoutProfile = false)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"DELETE FROM TrackProperty 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", withoutProfile ? -1 : this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"TrackProperty with ID [{id}] is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllTrackProperty()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"DELETE FROM TrackProperty 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"TrackProperties are not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearSettingTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Setting";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"Setting table has not been cleared. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearTrackPropertyTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM TrackProperty";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                string errorMessage = $"TrackProperty table has not been cleared. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

    }
}
