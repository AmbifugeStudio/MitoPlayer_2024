using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Model;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using MitoPlayer_2024.Presenters;
using System.Xml.Linq;

namespace MitoPlayer_2024._Repositories
{
    public class PlaylistDao : BaseDao, IPlaylistDao
    {
        //Constructor
        public PlaylistDao(string connectionString)
        {
            this.connectionString = connectionString;
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
        public List<PlaylistModel> GetAllPlaylist()
        {
            List<PlaylistModel> playListList = new List<PlaylistModel>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist ORDER BY OrderInList";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var playlistModel = new PlaylistModel();
                        playlistModel.Id = (int)reader[0];
                        playlistModel.Name = reader[1].ToString();
                        playlistModel.OrderInList = (int)reader[2];
                        playListList.Add(playlistModel);
                    }
                }
                connection.Close();
            }
            return playListList;
        }

        /*
         * üres playlist létrehozása
         */
        public void CreatePlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO Playlist values (@Id, @Name, @OrderInList)";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlistModel.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistModel.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlistModel.OrderInList;
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
        public List<TrackModel> LoadPlaylist(PlaylistModel playlistModel)
        {
            List<TrackModel> trackList = new List<TrackModel>();

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
                   "plc.TrackIdInPlaylist " +
                   "FROM Playlist pll, PlaylistContent plc, Track tra " +
                   "WHERE pll.Id = plc.PlaylistId and plc.TrackId = tra.Id " +
                   "AND pll.Id = @PlaylistId " +
                   "ORDER BY plc.SortingId ";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistModel.Id;
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
                        var idInPlaylist = (int)reader[8];

                        TrackModel track = new TrackModel();
                        track.Id = trackId;
                        track.Path = trackPath;
                        track.FileName = trackFileName;
                        track.Artist = trackArtist;
                        track.Title = trackTitle;
                        track.Album = trackAlbum;
                        track.Year = trackYear;
                        track.Length = trackLength;
                        track.IdInPlaylist = idInPlaylist;

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
                command.CommandText = "DELETE FROM PlaylistContent WHERE PlaylistId = @PlaylistId ";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
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
                command.CommandText = "SELECT * FROM Playlist WHERE Name = @Name ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistName;
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

        public PlaylistModel GetPlaylistByName(String playlistName)
        {
            PlaylistModel playlistModel = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Playlist WHERE Name = @Name ";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistName;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlistModel = new PlaylistModel();
                        playlistModel.Id = (int)reader[0];
                        playlistModel.Name = reader[1].ToString();
                        playlistModel.OrderInList = (int)reader[2];
                        break;
                    }
                }
            }
            return playlistModel;
        }

        public void UpdatePlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 
                                            SET Name = @Name, OrderInList = @OrderInList
                                            WHERE Id = @Id";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlistModel.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistModel.Name;
                command.Parameters.Add("@PlaylistOrder", MySqlDbType.VarChar).Value = playlistModel.OrderInList;
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

        public void DeletePlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Playlist WHERE Id = @Id";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlistModel.Id;
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

            DeleteTracksFromPlaylist(playlistModel);
        }

        public void DeleteTracksFromPlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM PlaylistContent WHERE PlaylistId = @PlaylistId";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistModel.Id;
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



        /*
        public int GetPlaylistContentIdByPlaylistAndTracklist(int playlistId, int trackId)
        {
            int playlistContentId = -1;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Id FROM PlaylistContent WHERE PlaylistId = @PlaylistId and TrackId = @TrackId ";
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlistContentId = (int)reader[0];
                    }
                }
            }
            return playlistContentId;

        }

        //Methods



        public void UpdatePlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 
                                        SET Name=@Name, PlaylistOrder = @PlaylistOrder,IsCurrent= @IsCurrent 
                                        WHERE Id = @Id";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlistModel.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlistModel.Name;
                command.Parameters.Add("@PlaylistOrder", MySqlDbType.VarChar).Value = playlistModel.PlaylistOrder;
                command.Parameters.Add("@IsCurrent", MySqlDbType.Int32).Value = playlistModel.IsCurrent;
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Playlist updated succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        

        public void AddTracksToPlaylist(PlaylistModel playlistModel)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                for (int i = 0; i < playlistModel.TrackList.Count; i++)
                {
                    command.CommandText = "INSERT INTO PlaylistContent values (@PlaylistId, @TrackId)";
                    command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistModel.Id;
                    command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = playlistModel.TrackList[i].Id;
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Track inserted succesfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Track is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                connection.Close();
            }
        }
        */


    }
}
