using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MitoPlayer_2024.Helpers
{
    public class DataBaseBuilderDao : BaseDao, IDataBaseBuilderDao
    {
        public DataBaseBuilderDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool BuildDatabase()
        {
            bool settingTableReady = false;
            if(!this.TableIsExists(TableName.Profile.ToString()))
                this.BuildProfileTable();
            if (!this.TableIsExists(TableName.Playlist.ToString()))
                this.BuildPlaylistTable();
            if (!this.TableIsExists(TableName.Track.ToString()))
                this.BuildTrackTable();
            if (!this.TableIsExists(TableName.PlaylistContent.ToString()))
                this.BuildPlaylistContentTable();
            if (!this.TableIsExists(TableName.Tag.ToString()))
                this.BuildTagTable();
            if (!this.TableIsExists(TableName.TagValue.ToString()))
                this.BuildTagValueTable();
            if (!this.TableIsExists(TableName.TrackProperty.ToString()))
                this.BuildTrackPropertyTable();
            if (!this.TableIsExists(TableName.Setting.ToString()))
                settingTableReady = this.BuildSettingTable();
            return settingTableReady;
        }
        public bool TableIsExists(string tableName)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "(select case when exists((select * from information_schema.tables where table_name = @TableName)) then 1 else 0 end)";
                command.Parameters.Add("@TableName", MySqlDbType.VarChar).Value = tableName;
                return ((int)command.ExecuteScalar() == 1);
            }
        }
        private bool BuildSettingTable()
        {
            bool success = true;
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE setting (Id int(11) NOT NULL,Name varchar(50) NOT NULL,StringValue varchar(255) NOT NULL,IntegerValue int(11) NOT NULL,DecimalValue decimal(10,0) NOT NULL,BooleanValue tinyint(4) NOT NULL,ProfileId int(11) NOT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    success = false;
                    MessageBox.Show("Setting table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
            return success;
        }
        private void BuildProfileTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE profile (Id int(11) NOT NULL,Name varchar(50) NOT NULL,IsActive tinyint(4) NOT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Profile table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildPlaylistTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE playlist (Id int(11) NOT NULL,Name varchar(50) NOT NULL,OrderInList int(11) NOT NULL,QuickListGroup int(11) NOT NULL,ProfileId int(11) DEFAULT NULL,IsActive tinyint(4) NOT NULL, PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildTrackTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE track (Id int(11) NOT NULL,Path varchar(255) NOT NULL,FileName varchar(100) NOT NULL,Artist varchar(100) DEFAULT NULL,Title varchar(100) DEFAULT NULL,Album varchar(100) DEFAULT NULL,Year int(11) DEFAULT NULL,Length int(11) DEFAULT NULL,ProfileId int(11) DEFAULT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildPlaylistContentTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE playlistcontent (Id int(11) NOT NULL,PlaylistId int(11) NOT NULL,TrackId int(11) NOT NULL,OrderInList int(11) NOT NULL,TrackIdInPlaylist int(11) NOT NULL,ProfileId int(11) DEFAULT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildTagTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE tag (Id int(11) NOT NULL,Name varchar(50) NOT NULL,ProfileId int(11) NOT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Tag table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildTagValueTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE tagvalue (Id int(11) NOT NULL,Name varchar(50) NOT NULL,TagId int(11) NOT NULL,ProfileId int(11) NOT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TagValue table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        private void BuildTrackPropertyTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "CREATE TABLE trackproperty (Id int(11) NOT NULL,ColumnGroup varchar(50) NOT NULL,Name varchar(50) NOT NULL,Type varchar(50) NOT NULL,IsEnabled tinyint(4) NOT NULL,SortingId int(11) NOT NULL,ProfileId int(11) NOT NULL,  PRIMARY KEY (Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackProperty table is not created. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        

     
    }
}
