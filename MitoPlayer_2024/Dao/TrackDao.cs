using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class TrackDao: BaseDao, ITrackDao
    {
        private int profileId = -1;
        public TrackDao(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }

        #region OPEN FILES

        public Track GetTrackByPath(String path)
        {
            Track track = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Track WHERE Path = @Path AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = path;
                command.Parameters.Add("@ProfileId", MySqlDbType.VarChar).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        track = new Track();
                        track.Id = (int)reader[0];
                        track.Path = reader[1].ToString();
                        track.FileName = reader[2].ToString();
                        track.Artist = reader[3].ToString();
                        track.Title = reader[4].ToString();
                        track.Album = reader[5].ToString();
                        track.Year = (int)reader[6];
                        track.Length = (int)reader[7];
                    }
                }
            }
            return track;
        }
        /*
         * új szám hozzáadása az adatbázishoz
         */
        public void AddTrackToDatabase(Track track)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Track values (@Id, @Path, @FileName, @Artist, @Title, @Album, @Year, @Length, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = track.Id;
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = track.Path;
                command.Parameters.Add("@FileName", MySqlDbType.VarChar).Value = track.FileName;
                command.Parameters.Add("@Artist", MySqlDbType.VarChar).Value = track.Artist;
                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = track.Title;
                command.Parameters.Add("@Album", MySqlDbType.VarChar).Value = track.Album;
                command.Parameters.Add("@Year", MySqlDbType.Int32).Value = track.Year;
                command.Parameters.Add("@Length", MySqlDbType.Int32).Value = track.Length;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.ExecuteNonQuery();
            }
        }
        /*
         * szám hozzáadása playlist-hez
         */
        public void AddTrackToPlaylist(int id, int playlistId, int trackId, int orderInList, int trackIdInPlaylist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO PlaylistContent values (@Id, @PlaylistId, @TrackId, @OrderInList, @TrackIdInPlaylist, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = orderInList;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = trackIdInPlaylist;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.ExecuteNonQuery();
            }
        }

        #endregion

        public void ClearTrackTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Track ";
                try
                {
                    command.ExecuteNonQuery();
                    //MessageBox.Show("Track deleted succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public void DeleteAllTrackFromProfile(int profileId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Track WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Object is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }



        public List<TrackProperty> GetTrackPropertyListByColumnGroup(String columnGroup)
        {
            List<TrackProperty> tpList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM TrackProperty WHERE ColumnGroup = @ColumnGroup ";
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = columnGroup;
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
                        tp.SortingId =(int)reader[5];
                        tp.ProfileId = (int)reader[6];
                        tpList.Add(tp);
                    }
                }
            }
            return tpList;
        }
        public void UpdateTrackProperty(TrackProperty trackProperty)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TrackProperty 
                                            SET ColumnGroup = @ColumnGroup, Name = @Name, Type = @Type, IsEnabled = @IsEnabled, 
                                            SortingId = @SortingId, ProfileId = @ProfileId 
                                            WHERE Id = @Id";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = trackProperty.Id;
                command.Parameters.Add("@ColumnGroup", MySqlDbType.VarChar).Value = trackProperty.ColumnGroup;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = trackProperty.Name;
                command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = trackProperty.Type;
                command.Parameters.Add("@IsEnabled", MySqlDbType.Bit).Value = trackProperty.IsEnabled;
                command.Parameters.Add("@SortingId", MySqlDbType.Int32).Value = trackProperty.SortingId;
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
    }
}
