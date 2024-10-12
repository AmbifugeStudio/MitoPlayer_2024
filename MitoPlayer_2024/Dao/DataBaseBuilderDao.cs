using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MitoPlayer_2024.Helpers
{
    public class DataBaseBuilderDao : BaseDao, IDataBaseBuilderDao
    {
        public DataBaseBuilderDao()
        {
        }
        public DataBaseBuilderDao(String connectionString)
        {
            this.connectionString = connectionString;
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

        public bool IsConnectionStringValid(String preConnectionString)
        {
            bool result = true;
            try
            {
                using (var connection = new MySqlConnection(preConnectionString))
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public bool IsDatabaseExists(String preConnectionString)
        {
            String tableName = String.Empty;

            using (var connection = new MySqlConnection(preConnectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT schema_name FROM information_schema.schemata WHERE schema_name LIKE '" + Properties.Settings.Default.Database + "' ";

                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        tableName = Convert.ToString(reader[0]);
                        break;
                    }
                }

                connection.Close();
            }
            return !String.IsNullOrEmpty(tableName);
        }
        public ResultOrError CreateDatabase(String preConnectionString)
        {
            ResultOrError result = new ResultOrError();

            using (var connection = new MySqlConnection(preConnectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE DATABASE IF NOT EXISTS " + Properties.Settings.Default.Database + ";";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Database is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        public ResultOrError DeleteDatabase()
        {
            ResultOrError result = new ResultOrError();

            using (var connection = new MySqlConnection(this.connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DROP DATABASE " + Properties.Settings.Default.Database + ";";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Database is not deleted. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        public ResultOrError CreateTableStructure()
        {
            ResultOrError result = new ResultOrError();

            if (result.Success)
            {
                if (!this.TableIsExists(TableName.Profile.ToString()))
                    result = this.BuildProfileTable();
            }
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.Setting.ToString()))
                    result = this.BuildSettingTable();
            }

            if (result.Success)
            {
                if (!this.TableIsExists(TableName.TrackProperty.ToString()))
                    result = this.BuildTrackPropertyTable();
            }
               
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.Playlist.ToString()))
                    result = this.BuildPlaylistTable();
            }
            
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.Track.ToString()))
                    result = this.BuildTrackTable();
            }
           
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.PlaylistContent.ToString()))
                    result = this.BuildPlaylistContentTable();
            }
           
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.Tag.ToString()))
                    result = this.BuildTagTable();
            }
            
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.TagValue.ToString()))
                    result = this.BuildTagValueTable();
            }
           
            if (result.Success)
            {
                if (!this.TableIsExists(TableName.TrackTagValue.ToString()))
                    result = this.BuildTagTrackValueTable();
            }
           

            return result;
        }

        public bool TableIsExists(string tableName)
        {
            long count = 0;

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
            string database = builder.Database;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT count(*) FROM information_schema.tables WHERE table_schema = @Database AND table_name = @TableName ";
                command.Parameters.Add("@TableName", MySqlDbType.VarChar).Value = tableName;
                command.Parameters.Add("@Database", MySqlDbType.VarChar).Value = database;

                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        count = (long)reader[0];
                        break;
                    }
                }

                connection.Close();
            }
            return count > 0;
        }
        private ResultOrError BuildProfileTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE profile ( 

                                        Id int(11) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        IsActive tinyint(4) NOT NULL, 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Profile table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildSettingTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE setting ( 

                                        Id int(11) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        StringValue varchar(255), 
                                        IntegerValue int(11), 
                                        DecimalValue decimal(10,0), 
                                        BooleanValue tinyint(4), 
                                        ProfileId int(11), 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Setting table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildTrackPropertyTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE trackproperty (

                                        Id int(11) NOT NULL, 
                                        ColumnGroup varchar(50) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        Type varchar(50) NOT NULL, 
                                        IsEnabled tinyint(4) NOT NULL, 
                                        SortingId int(11) NOT NULL, 
                                        ProfileId int(11) NOT NULL, 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("TrackProperty table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildPlaylistTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE playlist (

                                        Id int(11) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        OrderInList int(11) NOT NULL, 
                                        Hotkey int(11) NOT NULL, 
                                        IsActive tinyint(4) NOT NULL, 
                                        ProfileId int(11) DEFAULT NULL, 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Playlist table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildTrackTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE track (

                                        Id int(11) NOT NULL, 
                                        Path varchar(255) NOT NULL, 
                                        FileName varchar(100) NOT NULL, 
                                        Artist varchar(100) DEFAULT NULL, 
                                        Title varchar(100) DEFAULT NULL, 
                                        Album varchar(100) DEFAULT NULL, 
                                        Year int(11) DEFAULT NULL, 
                                        Length int(11) DEFAULT NULL, 
                                        ProfileId int(11) DEFAULT NULL, 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Track table table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildPlaylistContentTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE playlistcontent (

                                        Id int(11) NOT NULL, 
                                        PlaylistId int(11) NOT NULL, 
                                        TrackId int(11) NOT NULL, 
                                        OrderInList int(11) NOT NULL, 
                                        TrackIdInPlaylist int(11) NOT NULL, 
                                        ProfileId int(11) DEFAULT NULL, 

                                        PRIMARY KEY (Id), 

                                        KEY PlaylistId (PlaylistId), 
                                        KEY TrackId (TrackId), 
                                        CONSTRAINT playlistcontent_ibfk_1 FOREIGN KEY (PlaylistId) REFERENCES playlist (Id), 
                                        CONSTRAINT playlistcontent_ibfk_2 FOREIGN KEY (TrackId) REFERENCES track (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("PlaylistContent table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildTagTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tag ( 

                                        Id int(11) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        TextColoring tinyint(4) NOT NULL,
                                        HasMultipleValues tinyint(4) NOT NULL,
                                        Integrated tinyint(4) NOT NULL,
                                        ProfileId int(11) NOT NULL, 

                                        PRIMARY KEY (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Tag table table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildTagValueTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tagvalue ( 

                                        Id int(11) NOT NULL, 
                                        Name varchar(50) NOT NULL, 
                                        TagId int(11) NOT NULL, 
                                        Color varchar(25) NOT NULL,
                                        Hotkey int(11) NOT NULL, 
                                        ProfileId int(11) NOT NULL, 

                                        PRIMARY KEY (Id), 
                                               
                                        KEY TagId (TagId), 
                                        CONSTRAINT tagvalue_ibfk_1 FOREIGN KEY (TagId) REFERENCES tag (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("TagValue table table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        private ResultOrError BuildTagTrackValueTable()
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"CREATE TABLE tracktagvalue ( 

                                        Id int(11) NOT NULL, 
                                        TrackId int(11) NOT NULL, 
                                        TagId int(11) NOT NULL, 
                                        TagValueId int(11) DEFAULT NULL, 
                                        HasValue tinyint(4) NOT NULL,
                                        Value varchar(100) DEFAULT NULL,
                                        ProfileId int(11) NOT NULL, 

                                        PRIMARY KEY (Id), 

                                        KEY TrackId (TrackId), 
                                        KEY TagId (TagId), 
                                        CONSTRAINT tracktagvalue_ibfk_1 FOREIGN KEY (TrackId) REFERENCES track (Id), 
                                        CONSTRAINT tracktagvalue_ibfk_2 FOREIGN KEY (TagId) REFERENCES tag (Id)) 

                                        ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("TrackProperty table table is not created. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }


    }
}
