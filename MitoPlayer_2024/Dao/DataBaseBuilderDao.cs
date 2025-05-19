using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Models;
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;

namespace MitoPlayer_2024.Helpers
{
    public class DataBaseBuilderDao : BaseDao, IDataBaseBuilderDao
    {
        public DataBaseBuilderDao() { }
        public DataBaseBuilderDao(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool IsConnectionStringValid(String preConnectionString)
        {
            bool result = true;
            try
            {
                using (var connection = new SqliteConnection(preConnectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public bool IsDatabaseExists(string databaseFilePath)
        {
            return File.Exists(databaseFilePath);
        }
        public ResultOrError CreateDatabase(string databaseFilePath)
        {
            ResultOrError result = new ResultOrError();
            try
            {
                if (!File.Exists(databaseFilePath))
                {
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError("Database is not created.\n" + ex.Message);
            }
            return result;
        }
        public ResultOrError DeleteDatabase(string databaseFilePath)
        {
            ResultOrError result = new ResultOrError();
            try
            {
                if (File.Exists(databaseFilePath))
                {
                    File.Delete(databaseFilePath);
                }
            }
            catch (Exception ex)
            {
                result.AddError("Database is not deleted.\n" + ex.Message);
            }
            return result;
        }

        public ResultOrError CreateTableStructure()
        {
            ResultOrError result = new ResultOrError();

            if (result)
            {
                if (!this.TableIsExists(TableName.Profile.ToString()))
                    result = this.BuildProfileTable();
            }
            if (result)
            {
                if (!this.TableIsExists(TableName.Setting.ToString()))
                    result = this.BuildSettingTable();
            }

            if (result)
            {
                if (!this.TableIsExists(TableName.TrackProperty.ToString()))
                    result = this.BuildTrackPropertyTable();
            }
               
            if (result)
            {
                if (!this.TableIsExists(TableName.Playlist.ToString()))
                    result = this.BuildPlaylistTable();
            }
            
            if (result)
            {
                if (!this.TableIsExists(TableName.Track.ToString()))
                    result = this.BuildTrackTable();
            }
           
            if (result)
            {
                if (!this.TableIsExists(TableName.PlaylistContent.ToString()))
                    result = this.BuildPlaylistContentTable();
            }
           
            if (result)
            {
                if (!this.TableIsExists(TableName.Tag.ToString()))
                    result = this.BuildTagTable();
            }
            
            if (result)
            {
                if (!this.TableIsExists(TableName.TagValue.ToString()))
                    result = this.BuildTagValueTable();
            }
           
            if (result)
            {
                if (!this.TableIsExists(TableName.TrackTagValue.ToString()))
                    result = this.BuildTagTrackValueTable();
            }

            if (result)
            {
                if (!this.TableIsExists(TableName.TrainingData.ToString()))
                    result = this.BuildTrainingModelTable();
            }

            return result;
        }

        public bool TableIsExists(string tableName)
        {
            long count = 0;

            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@TableName; ";
                command.Parameters.AddWithValue("@TableName", tableName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = (long)reader[0];
                        break;
                    }
                }
            }
            return count > 0;
        }
        private ResultOrError BuildProfileTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE profile (
                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            IsActive INTEGER NOT NULL
                        );";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("Profile table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildSettingTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE setting ( 
                                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                            Name TEXT NOT NULL, 
                                            StringValue TEXT, 
                                            IntegerValue INTEGER, 
                                            DecimalValue REAL, 
                                            BooleanValue INTEGER, 
                                            ProfileId INTEGER
                                        );";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("Setting table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildTrackPropertyTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE trackproperty (
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        ColumnGroup TEXT NOT NULL, 
                                        Name TEXT NOT NULL, 
                                        Type TEXT NOT NULL, 
                                        IsEnabled INTEGER NOT NULL, 
                                        SortingId INTEGER NOT NULL, 
                                        ProfileId INTEGER NOT NULL
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("TrackProperty table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildPlaylistTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE playlist (
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        Name TEXT NOT NULL, 
                                        OrderInList INTEGER NOT NULL, 
                                        Hotkey INTEGER NOT NULL, 
                                        IsActive INTEGER NOT NULL, 
                                        IsModelTrainer INTEGER NOT NULL, 
                                        ProfileId INTEGER
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("Playlist table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildTrackTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE track (
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        Path TEXT NOT NULL, 
                                        FileName TEXT NOT NULL, 
                                        Artist TEXT, 
                                        Title TEXT, 
                                        Album TEXT, 
                                        Year INTEGER, 
                                        Length INTEGER, 
                                        Comment TEXT, 
                                        ProfileId INTEGER
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("Track table table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildPlaylistContentTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE playlistcontent (
                                        Id INTEGER NOT NULL PRIMARY KEY, 
                                        PlaylistId INTEGER NOT NULL, 
                                        TrackId INTEGER NOT NULL, 
                                        OrderInList INTEGER NOT NULL, 
                                        TrackIdInPlaylist INTEGER NOT NULL, 
                                        ProfileId INTEGER,
                                        FOREIGN KEY (PlaylistId) REFERENCES playlist (Id), 
                                        FOREIGN KEY (TrackId) REFERENCES track (Id)
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("PlaylistContent table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildTagTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tag ( 
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        Name TEXT NOT NULL, 
                                        TextColoring INTEGER NOT NULL,
                                        HasMultipleValues INTEGER NOT NULL,
                                        IsIntegrated INTEGER NOT NULL,
                                        OrderInList INTEGER NOT NULL, 
                                        ProfileId INTEGER NOT NULL
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("Tag table table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildTagValueTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tagvalue ( 
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        Name TEXT NOT NULL, 
                                        TagId INTEGER NOT NULL, 
                                        Color TEXT NOT NULL,
                                        Hotkey INTEGER NOT NULL, 
                                        ProfileId INTEGER NOT NULL,
                                        FOREIGN KEY (TagId) REFERENCES tag (Id)
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("TagValue table table is not created. \n" + ex.Message);
                }
            }
            return result;
        }
        private ResultOrError BuildTagTrackValueTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tracktagvalue ( 
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        TrackId INTEGER NOT NULL, 
                                        TagId INTEGER NOT NULL, 
                                        TagValueId INTEGER, 
                                        HasValue INTEGER NOT NULL,
                                        Value TEXT,
                                        ProfileId INTEGER NOT NULL,
                                        FOREIGN KEY (TrackId) REFERENCES track (Id), 
                                        FOREIGN KEY (TagId) REFERENCES tag (Id)
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("TrackProperty table table is not created. \n" + ex.Message);
                }
            }
            return result;
        } 
        private ResultOrError BuildTrainingModelTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new SqliteConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE trainingdata ( 
                                        Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        FilePath TEXT NOT NULL,
                                        TagId INTEGER NOT NULL,
                                        Name TEXT NOT NULL,
                                        CreateDate TEXT NOT NULL, 
                                        SampleCount INTEGER, 
                                        Balance REAL, 
                                        IsTemplate INTEGER NOT NULL,
                                        ExtractChromaFeatures INTEGER NOT NULL,             
                                        ExtractMFCCs INTEGER NOT NULL,
                                        ExtractSpectralContrast INTEGER NOT NULL,
                                        ExtractHPCP INTEGER NOT NULL,
                                        ExtractSpectralCentroid INTEGER NOT NULL,
                                        ExtractSpectralBandwidth INTEGER NOT NULL,
                                        HarmonicPercussiveSeparation INTEGER NOT NULL,
                                        ExtractTonnetzFeatures INTEGER NOT NULL,                                  
                                        ExtractZeroCrossingRate INTEGER NOT NULL,
                                        ExtractRmsEnergy INTEGER NOT NULL,
                                        ExtractPitch INTEGER NOT NULL,
                                        ProfileId INTEGER NOT NULL
                                    );";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    result.AddError("TrainingData table table is not created. \n" + ex.Message);
                }
            }
            return result;
        }

    }
}
