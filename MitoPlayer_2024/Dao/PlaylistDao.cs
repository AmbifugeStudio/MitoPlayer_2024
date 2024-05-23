using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MitoPlayer_2024.Dao
{
    public class PlaylistDao : BaseDao, IPlaylistDao
    {
        public int ProfileId = -1;
        public PlaylistDao(string connectionString, int profileId)
        {
            this.connectionString = connectionString;
            this.ProfileId = profileId;
        }

        public int GetNextLastSmallestTrackIdInPlaylist()
        {
            int result = 0;
            List<int> trackIds = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT TrackIdInPlaylist FROM PlaylistContent ORDER BY TrackIdInPlaylist ";
                using (var reader = command.ExecuteReader())
                {
                    trackIds = new List<int>();
                    while (reader.Read())
                    {
                        trackIds.Add((int)reader[0]);
                    }
                }
            }
            if(trackIds == null || trackIds.Count == 0)
            {
                return result;
            }
            else
            {
                for(int i = 0; i<= trackIds.Count -1; i++)
                {
                    if(i != trackIds[i])
                    {
                        return i;
                    }
                }
                result = trackIds.Count;
            }

            return result;
        }

        /*
         * univerzális Id lekérő
         */
        public int GetLastObjectId(String tableName)
        {
            int lastId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM " + tableName + " ORDER BY Id desc LIMIT 1";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastId = (int)reader[0];
                    }
                }
            }
            return lastId;
        }

        #region LOADING
        /*
         * az összes playlist lekérése
         */
        public List<Playlist> GetAllPlaylist()
        {
            List<Playlist> playListList = new List<Playlist>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE ProfileId = @ProfileId ORDER BY OrderInList";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = reader[1].ToString();
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playListList.Add(playlist);
                    }
                }
                connection.Close();
            }
            return playListList;
        }

        public List<Playlist> GetDefaultPlaylist(String defaultPlaylistName)
        {
            List<Playlist> playListList = new List<Playlist>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE Name = @Name AND ProfileId = @ProfileId ORDER BY OrderInList";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = defaultPlaylistName;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = reader[1].ToString();
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playListList.Add(playlist);
                    }
                }
                connection.Close();
            }
            return playListList;
        }

        /*
         * üres playlist létrehozása
         */
        public void CreatePlaylist(Playlist playlist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Playlist values (@Id, @Name, @OrderInList, @QuickListGroup, @ProfileId)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlist.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlist.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlist.OrderInList;
                command.Parameters.Add("@QuickListGroup", MySqlDbType.Int32).Value = playlist.QuickListGroup;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                   // MessageBox.Show("Playlist created succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        /*
         * a playlist-hez tartozó számok lekérése
         */
        public List<Track> LoadPlaylist(int id)
        {
            List<Track> trackList = new List<Track>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT " +
                   "tra.Id, " +
                   "tra.Path, " +
                   "tra.FileName, " +
                   "tra.Artist, " +
                   "tra.Title, " +
                   "tra.Album, " +
                   "tra.Year, " +
                   "tra.Length, " +
                   "plc.OrderInList, " +
                   "plc.TrackIdInPlaylist " +
                   "FROM Playlist pll, PlaylistContent plc, Track tra " +
                   "WHERE pll.Id = plc.PlaylistId and plc.TrackId = tra.Id " +
                   "AND pll.Id = @PlaylistId " +
                   "AND plc.ProfileId = @ProfileId " +
                   "AND pll.ProfileId = @ProfileId " +
                   "AND tra.ProfileId = @ProfileId " +
                   "ORDER BY plc.OrderInList ";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trackId = (int)reader[0];
                        var trackPath = reader[1].ToString();
                        var trackFileName = reader[2].ToString();
                        var trackArtist = reader[3].ToString();
                        var trackTitle = reader[4].ToString();
                        var trackAlbum = reader[5].ToString();
                        var trackYear = (int)reader[6];
                        var trackLength = (int)reader[7];
                        var orderInList = (int)reader[8];
                        var trackIdInPlaylist = (int)reader[9];

                        Track track = new Track();
                        track.Id = trackId;
                        track.Path = trackPath;
                        track.FileName = trackFileName;
                        track.Artist = trackArtist;
                        track.Title = trackTitle;
                        track.Album = trackAlbum;
                        track.Year = trackYear;
                        track.Length = trackLength;
                        track.OrderInList = orderInList;
                        track.TrackIdInPlaylist = trackIdInPlaylist;

                        trackList.Add(track);
                    }
                }
                connection.Close();

                return trackList;
            }
        }

        #endregion

        public void DeletePlaylistContent(int playlistId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM PlaylistContent WHERE PlaylistId = @PlaylistId AND ProfileId = @ProfileId ";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
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


        public bool ValidatePlaylistName(String playlistName)
        {
            String name = "";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistName;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader[0].ToString();
                    }
                }
            }
            if (String.IsNullOrEmpty(name))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public Playlist GetPlaylistByName(int id)
        {
            Playlist playlist = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE Id = @Id AND  ProfileId = @ProfileId ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = reader[1].ToString();
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        break;
                    }
                }
            }
            return playlist;
        }


        public Playlist GetPlaylistByName(String playlistName)
        {
            Playlist playlist = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE Name = @Name AND ProfileId = @ProfileId";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistName;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = reader[1].ToString();
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        break;
                    }
                }
            }
            return playlist;
        }

        public void UpdatePlaylist(Playlist playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 
                                            SET Name = @Name, OrderInList = @OrderInList, QuickListGroup = @QuickListGroup
                                            WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlistModel.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistModel.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlistModel.OrderInList;
                command.Parameters.Add("@QuickListGroup", MySqlDbType.Int32).Value = playlistModel.QuickListGroup;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

        public void DeletePlaylist(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Playlist WHERE Id = @Id AND ProfileId = @ProfileId";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
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

            DeleteTracksFromPlaylist(id);
        }

        public void DeleteAllPlaylist()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Playlist WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
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

        public void DeleteTracksFromPlaylist(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM PlaylistContent WHERE PlaylistId = @PlaylistId AND ProfileId = @ProfileId";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.ProfileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

    }
}
