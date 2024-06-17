using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
        public int GetProfileId()
        {
            return this.profileId;
        }

        #region PLAYLIST
        public bool IsPlaylistNameAlreadyExists(String name)
        {
            String result = String.Empty;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Playlist 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                        break;
                    }
                }
            }
            if (!String.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CreatePlaylist(Playlist playlist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO Playlist values ( 
                                        @Id, 
                                        @Name, 
                                        @OrderInList, 
                                        @QuickListGroup, 
                                        @IsActive, 
                                        @ProfileId )";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlist.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlist.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlist.OrderInList;
                command.Parameters.Add("@QuickListGroup", MySqlDbType.Int32).Value = playlist.QuickListGroup;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = playlist.IsActive;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Playlist is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public Playlist GetPlaylist(int id)
        {
            Playlist playlist = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Playlist 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId ";
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = (string)reader[1];
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.ProfileId = (int)reader[5];
                        break;
                    }
                }
                connection.Close();
            }
            return playlist;
        }
        public Playlist GetPlaylistByName(String name)
        {
            Playlist playlist = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Playlist 
                                        WHERE Name = @Name 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = (string)reader[1];
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.ProfileId = (int)reader[5];
                        break;
                    }
                }
                connection.Close();
            }
            return playlist;
        }
        public Playlist GetActivePlaylist()
        {
            Playlist playlist = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Playlist 
                                        WHERE IsActive = true 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = (string)reader[1];
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.ProfileId = (int)reader[5];
                        break;
                    }
                }
                connection.Close();
            }
            return playlist;
        }
        public List<Playlist> GetAllPlaylist()
        {
            List<Playlist> playListList = new List<Playlist>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Playlist 
                                        WHERE ProfileId = @ProfileId 
                                        ORDER BY OrderInList";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var playlist = new Playlist();
                        playlist.Id = (int)reader[0];
                        playlist.Name = (string)reader[1];
                        playlist.OrderInList = (int)reader[2];
                        playlist.QuickListGroup = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.ProfileId = (int)reader[5];
                        playListList.Add(playlist);
                    }
                }
                connection.Close();
            }
            return playListList;
        }
        public void SetActivePlaylist(int id)
        {
            this.InactiveAllPlaylist();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 
                                        SET IsActive = true
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
        public void InactiveAllPlaylist()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 
                                        SET IsActive = false
                                        WHERE ProfileId = @ProfileId";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
        public void UpdatePlaylist(Playlist playlist)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE Playlist 

                                        SET Name = @Name, 
                                        OrderInList = @OrderInList, 
                                        QuickListGroup = @QuickListGroup, 
                                        IsActive = @IsActive

                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlist.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlist.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlist.OrderInList;
                command.Parameters.Add("@QuickListGroup", MySqlDbType.Int32).Value = playlist.QuickListGroup;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = playlist.IsActive;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
            this.DeletePlaylistContentByPlaylistId(id);

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Playlist 
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
        public void DeleteAllPlaylist()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Playlist 
                                        WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
        public void ClearPlaylistTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM Playlist ";
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
        #endregion

        #region TRACK
        public void CreateTrack(Track track)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Track values ( 
                                        @Id, 
                                        @Path, 
                                        @FileName, 
                                        @Artist, 
                                        @Title, 
                                        @Album, 
                                        @Year, 
                                        @Length, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = track.Id;
                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = track.Path;
                command.Parameters.Add("@FileName", MySqlDbType.VarChar).Value = track.FileName;
                command.Parameters.Add("@Artist", MySqlDbType.VarChar).Value = track.Artist;
                command.Parameters.Add("@Title", MySqlDbType.VarChar).Value = track.Title;
                command.Parameters.Add("@Album", MySqlDbType.VarChar).Value = track.Album;
                command.Parameters.Add("@Year", MySqlDbType.Int32).Value = track.Year;
                command.Parameters.Add("@Length", MySqlDbType.Int32).Value = track.Length;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }

            if (track.TrackTagValues != null && track.TrackTagValues.Count > 0)
            {
                foreach (TrackTagValue ttv in track.TrackTagValues)
                {
                    this.CreateTrackTagValue(ttv);
                }
            }
        }
        public Track GetTrackByPath(string path, List<Tag> tagList)
        {
            Track track = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM Track 
                                        WHERE Path = @Path 
                                        AND ProfileId = @ProfileId ";

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
                        track.ProfileId = (int)reader[8];
                        break;
                    }
                }
            }

            if (track != null)
                track.TrackTagValues = this.LoadTrackTagValuesByTrackId(track.Id, tagList);

            return track;
        }
        public List<Track> GetTracklistByPlaylistId(int playlistId, List<Tag> tagList)
        {
            List<Track> trackList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT 
                                        tra.Id, 
                                        tra.Path, 
                                        tra.FileName, 
                                        tra.Artist, 
                                        tra.Title, 
                                        tra.Album, 
                                        tra.Year, 
                                        tra.Length, 
                                        plc.OrderInList, 
                                        plc.TrackIdInPlaylist, 
                                        plc.ProfileId 
                                        
                                        FROM Playlist pll, 
                                        PlaylistContent plc, 
                                        Track tra 

                                        WHERE pll.Id = plc.PlaylistId 
                                        AND plc.TrackId = tra.Id 
                                        AND pll.Id = @PlaylistId 
                                        AND plc.ProfileId = @ProfileId 
                                        AND pll.ProfileId = @ProfileId 
                                        AND tra.ProfileId = @ProfileId 
                                        ORDER BY plc.OrderInList ";

                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    trackList = new List<Track>();
                    while (reader.Read())
                    {
                        Track track = new Track();
                        track.Id = (int)reader[0];
                        track.Path = (string)reader[1];
                        track.FileName = (string)reader[2];
                        track.Artist = (string)reader[3];
                        track.Title =  (string)reader[4];
                        track.Album = (string)reader[5];
                        track.Year = (int)reader[6];
                        track.Length = (int)reader[7];
                        track.OrderInList = (int)reader[8];
                        track.TrackIdInPlaylist = (int)reader[9];
                        track.ProfileId = (int)reader[10];
                        trackList.Add(track);
                    }
                }
                connection.Close();

                foreach (Track track in trackList)
                {
                    track.TrackTagValues = this.LoadTrackTagValuesByTrackId(track.Id, tagList);
                }
                return trackList;
            }
        }
        public List<int> GetAllTrackIdInList()
        {
            List<int> trackIdList = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT Id 
                                        FROM Track 
                                        WHERE ProfileId = @ProfileId ";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    trackIdList = new List<int>();
                    while (reader.Read())
                    {
                        trackIdList.Add((int)reader[0]);
                    }
                }
                connection.Close();
            }
            return trackIdList;
        }
        public void DeleteAllTrack()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM Track 
                                        WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void ClearTrackTable()
        {
            this.ClearPlaylistTable();
            this.ClearTrackTagValueTable();

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
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Track is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        #endregion

        #region PLAYLISTCONTENT
        public int GetNextSmallestTrackIdInPlaylist()
        {
            int result = 0;
            List<int> trackIds = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT TrackIdInPlaylist 
                                        FROM PlaylistContent 
                                        ORDER BY TrackIdInPlaylist ";
                using (var reader = command.ExecuteReader())
                {
                    trackIds = new List<int>();
                    while (reader.Read())
                    {
                        trackIds.Add((int)reader[0]);
                    }
                }
            }
            if (trackIds == null || trackIds.Count == 0)
            {
                return result;
            }
            else
            {
                for (int i = 0; i <= trackIds.Count - 1; i++)
                {
                    if (i != trackIds[i])
                    {
                        return i;
                    }
                }
                result = trackIds.Count;
            }

            return result;
        }
        public void CreatePlaylistContent(PlaylistContent plc)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO PlaylistContent values ( 
                                        @Id, 
                                        @PlaylistId, 
                                        @TrackId, 
                                        @OrderInList,
                                        @TrackIdInPlaylist, 
                                        @ProfileId)";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = plc.Id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = plc.PlaylistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = plc.TrackId;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = plc.OrderInList;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = plc.TrackIdInPlaylist;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void UpdatePlaylistContent(PlaylistContent plc)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE PlaylistContent SET 
                                        @OrderInList, 
                                        @TrackIdInPlaylist 

                                        WHERE Id = @Id 
                                        AND PlaylistId = @PlaylistId 
                                        AND ProfileId = @ProfileId; ";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = plc.Id;
                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = plc.PlaylistId;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = plc.TrackId;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = plc.OrderInList;
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = plc.TrackIdInPlaylist;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeletePlaylistContentByPlaylistId(int playlistId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM PlaylistContent 
                                        WHERE PlaylistId = @PlaylistId 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteAllPlaylistContent()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM PlaylistContent 
                                        WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void ClearPlaylistContentTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM PlaylistContent ";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("PlaylistContent is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        #endregion

        #region TRACKTAGVALUE
        public bool IsTrackTagValueAlreadyExists(int trackId, int tagId)
        {
            bool result = false;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT Id 
                                        FROM TrackTagValue 
                                        WHERE TrackId = @TrackId 
                                        AND TagId = @TagId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    result = true;
                }
                connection.Close();
            }

            return result;
        }
        public void CreateTrackTagValue(TrackTagValue ttv)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO TrackTagValue VALUES ( 
                                        @Id, 
                                        @TrackId, 
                                        @TagId, 
                                        @TagValueId, 
                                        @ProfileId )";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = ttv.Id;
                command.Parameters.Add("@TrackId", MySqlDbType.VarChar).Value = ttv.TrackId;
                command.Parameters.Add("@TagId", MySqlDbType.VarChar).Value = ttv.TagId;
                command.Parameters.Add("@TagValueId", MySqlDbType.VarChar).Value = ttv.TagValueId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackTagValue is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public List<TrackTagValue> LoadTrackTagValuesByTrackId(int trackId, List<Tag> tagList)
        {
            List<TrackTagValue> trackTagValueList = new List<TrackTagValue>();

            if (tagList != null && tagList.Count > 0)
            {
                foreach (Tag tag in tagList)
                {
                    using (var connection = new MySqlConnection(connectionString))
                    using (var command = new MySqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"SELECT 
                                                ttv.Id, 
                                                ttv.TrackId, 
                                                ttv.TagId,
                                                ttv.TagValueId,
                                                t.Name,
                                                tv.Name,
                                                ttv.ProfileId
                                                 
                                                FROM TrackTagValue ttv
                                                LEFT JOIN Tag t
                                                ON ttv.TagId=t.Id
                                                LEFT JOIN TagValue tv
                                                ON ttv.TagValueId= tv.Id
                                                 
                                                WHERE ttv.TrackId = @TrackId
                                                AND ttv.TagId = @TagId
                                                AND ttv.ProfileId = @ProfileId ";

                        command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                        command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tag.Id;
                        command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                        using (var reader = command.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {
                                TrackTagValue ttv = new TrackTagValue();
                                ttv.Id = (int)reader[0];
                                ttv.TrackId = (int)reader[1];
                                ttv.TagId = (int)reader[2];
                                ttv.TagValueId = (int)reader[3];
                                ttv.TagName = (string)reader[4];
                                ttv.TagValueName = Convert.ToString(reader[5]);
                                ttv.ProfileId = (int)reader[6];
                                trackTagValueList.Add(ttv);
                            }
                        }
                    }
                }
            }
            return trackTagValueList;
        }
        public void UpdateTrackTagValues(List<TrackTagValue> trackTagValueList)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                foreach (TrackTagValue ttv in trackTagValueList)
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE TrackTagValue 
                                            SET TrackId = @TrackId, 
                                            TagId = @TagId, 
                                            TagValueId = @TagValueId 

                                            WHERE Id = @Id 
                                            AND ProfileId = @ProfileId";

                    command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = ttv.TrackId;
                    command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = ttv.TagId;
                    command.Parameters.Add("@TagValueId", MySqlDbType.Int32).Value = ttv.TagValueId;
                    command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("TrackTagValue is not inserted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                connection.Close();
            }
        }

        public void DeleteTagValueFromTrackTagValues(int tagValueId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TrackTagValue 
                                            SET TagValueId = -1

                                            WHERE TagValueId = @TagValueId 
                                            AND ProfileId = @ProfileId";

                command.Parameters.Add("@TagValueId", MySqlDbType.Int32).Value = tagValueId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackTagValue is not updated. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteTrackTagValueByTagId(int tagId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackTagValue 
                                        WHERE TagId = @TagId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = tagId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackTagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteTrackTagValueByTrackId(int trackId)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackTagValue 
                                        WHERE TrackId = @TrackId 
                                        AND ProfileId = @ProfileId ";

                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = trackId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackTagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        public void DeleteAllTrackTagValue()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrackTagValue 
                                        WHERE ProfileId = @ProfileId ";
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

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
        public void ClearTrackTagValueTable()
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM TrackTagValue ";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("TrackTagValue is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }
        #endregion

        /*
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
           if (trackIds == null || trackIds.Count == 0)
           {
               return result;
           }
           else
           {
               for (int i = 0; i <= trackIds.Count - 1; i++)
               {
                   if (i != trackIds[i])
                   {
                       return i;
                   }
               }
               result = trackIds.Count;
           }

           return result;
       }
       */
    }
}
