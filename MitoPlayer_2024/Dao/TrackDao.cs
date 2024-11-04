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
using System.Linq;
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
        public ResultOrError CreatePlaylist(Playlist playlist)
        {
            ResultOrError result = new ResultOrError();
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
                                        @Hotkey, 
                                        @IsActive,
                                        @IsModelTrainer,
                                        @ProfileId )";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlist.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlist.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlist.OrderInList;
                command.Parameters.Add("@Hotkey", MySqlDbType.Int32).Value = playlist.Hotkey;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = playlist.IsActive;
                command.Parameters.Add("@IsModelTrainer", MySqlDbType.Bit).Value = playlist.IsModelTrainer;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("Playlist [" + playlist.Name + "] is not inserted. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
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
                        playlist.Hotkey = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.IsModelTrainer = Convert.ToBoolean(reader[5]);
                        playlist.ProfileId = (int)reader[6];
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
                        playlist.Hotkey = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.IsModelTrainer = Convert.ToBoolean(reader[5]);
                        playlist.ProfileId = (int)reader[6];
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
                        playlist.Hotkey = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.IsModelTrainer = Convert.ToBoolean(reader[5]);
                        playlist.ProfileId = (int)reader[6];
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
                        playlist.Hotkey = (int)reader[3];
                        playlist.IsActive = Convert.ToBoolean(reader[4]);
                        playlist.IsModelTrainer = Convert.ToBoolean(reader[5]);
                        playlist.ProfileId = (int)reader[6];
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
                                        Hotkey = @Hotkey, 
                                        IsActive = @IsActive,
                                        IsModelTrainer = @IsModelTrainer

                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = playlist.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = playlist.Name;
                command.Parameters.Add("@OrderInList", MySqlDbType.Int32).Value = playlist.OrderInList;
                command.Parameters.Add("@Hotkey", MySqlDbType.Int32).Value = playlist.Hotkey;
                command.Parameters.Add("@IsActive", MySqlDbType.Bit).Value = playlist.IsActive;
                command.Parameters.Add("@IsModelTrainer", MySqlDbType.Bit).Value = playlist.IsModelTrainer;
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
        public void CreateTrack(Model.Track track)
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
        }
        public Model.Track GetTrackWithTags(int id, List<Tag> tagList)
        {
            Model.Track track = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                var tagIdsParameter = string.Join(",", tagList.Select(tag => tag.Id));
                command.CommandText = $@"
                                    SELECT 
                                    tra.Id AS TrackId, 
                                    tra.Path, 
                                    tra.FileName, 
                                    tra.Artist, 
                                    tra.Title, 
                                    tra.Album, 
                                    tra.Year, 
                                    tra.Length, 
                                    tra.ProfileId AS TrackProfileId,
                                    ttv.Id AS TagValueId, 
                                    ttv.TagId, 
                                    ttv.TagValueId, 
                                    ttv.HasValue, 
                                    ttv.Value, 
                                    t.Name AS TagName, 
                                    tv.Name AS TagValueName, 
                                    ttv.ProfileId AS TagProfileId
                                    FROM 
                                    Track tra
                                    LEFT JOIN TrackTagValue ttv ON tra.Id = ttv.TrackId AND ttv.TagId IN ({tagIdsParameter})
                                    LEFT JOIN Tag t ON ttv.TagId = t.Id
                                    LEFT JOIN TagValue tv ON ttv.TagValueId = tv.Id
                                    WHERE 
                                    tra.Id = @Id 
                                    AND tra.ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (track == null)
                        {
                            track = new Model.Track
                            {
                                Id = reader.GetInt32(0),
                                Path = reader.GetString(1),
                                FileName = reader.GetString(2),
                                Artist = reader.GetString(3),
                                Title = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Album = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Year = reader.GetInt32(6),
                                Length = reader.GetInt32(7),
                                ProfileId = reader.GetInt32(8),
                                TrackTagValues = new List<TrackTagValue>()
                            };
                        }

                        if (!reader.IsDBNull(9))
                        {
                            var tagValue = new TrackTagValue
                            {
                                Id = reader.GetInt32(9),
                                TrackId = reader.GetInt32(0),
                                TagId = reader.GetInt32(10),
                                TagValueId = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11),
                                HasValue = reader.GetBoolean(12),
                                Value = reader.IsDBNull(13) ? null : reader.GetString(13),
                                TagName = reader.IsDBNull(14) ? null : reader.GetString(14),
                                TagValueName = reader.IsDBNull(15) ? null : reader.GetString(15),
                                ProfileId = reader.GetInt32(16)
                            };
                            track.TrackTagValues.Add(tagValue);
                        }
                    }
                }
            }

            return track;
        }
        public Model.Track GetTrackWithTagsByPath(string path, List<Tag> tagList)
        {
            Model.Track track = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                var tagIdsParameter = string.Join(",", tagList.Select(tag => tag.Id));
                command.CommandText = $@"
                                        SELECT 
                                        tra.Id AS TrackId, 
                                        tra.Path, 
                                        tra.FileName, 
                                        tra.Artist, 
                                        tra.Title, 
                                        tra.Album, 
                                        tra.Year, 
                                        tra.Length, 
                                        tra.ProfileId AS TrackProfileId,
                                        ttv.Id AS TagValueId, 
                                        ttv.TagId, 
                                        ttv.TagValueId, 
                                        ttv.HasValue, 
                                        ttv.Value, 
                                        t.Name AS TagName, 
                                        tv.Name AS TagValueName, 
                                        ttv.ProfileId AS TagProfileId
                                        FROM 
                                        Track tra
                                        LEFT JOIN TrackTagValue ttv ON tra.Id = ttv.TrackId AND ttv.TagId IN ({tagIdsParameter})
                                        LEFT JOIN Tag t ON ttv.TagId = t.Id
                                        LEFT JOIN TagValue tv ON ttv.TagValueId = tv.Id
                                        WHERE 
                                        tra.Path = @Path 
                                        AND tra.ProfileId = @ProfileId";

                command.Parameters.Add("@Path", MySqlDbType.VarChar).Value = path;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (track == null)
                        {
                            track = new Model.Track
                            {
                                Id = reader.GetInt32(0),
                                Path = reader.GetString(1),
                                FileName = reader.GetString(2),
                                Artist = reader.GetString(3),
                                Title = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Album = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Year = reader.GetInt32(6),
                                Length = reader.GetInt32(7),
                                ProfileId = reader.GetInt32(8),
                                TrackTagValues = new List<TrackTagValue>()
                            };
                        }

                        if (!reader.IsDBNull(9))
                        {
                            var tagValue = new TrackTagValue
                            {
                                Id = reader.GetInt32(9),
                                TrackId = reader.GetInt32(0),
                                TagId = reader.GetInt32(10),
                                TagValueId = reader.IsDBNull(11) ? (int?) null : reader.GetInt32(11),
                                HasValue = reader.GetBoolean(12),
                                Value = reader.IsDBNull(13) ? null : reader.GetString(13),
                                TagName = reader.IsDBNull(14) ? null : reader.GetString(14),
                                TagValueName = reader.IsDBNull(15) ? null : reader.GetString(15),
                                ProfileId = reader.GetInt32(16)
                            };
                            track.TrackTagValues.Add(tagValue);
                        }
                    }
                }
            }

            return track;
        }
        public List<Model.Track> GetTracklistWithTagsByPlaylistId(int playlistId, List<Tag> tagList)
        {
            var trackList = new List<Model.Track>();
            var trackDictionary = new Dictionary<int, Model.Track>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                var tagIdsParameter = string.Join(",", tagList.Select(tag => tag.Id));
                command.CommandText = $@"
                                        SELECT 
                                        tra.Id AS TrackId, 
                                        tra.Path, 
                                        tra.FileName, 
                                        tra.Artist, 
                                        tra.Title, 
                                        tra.Album, 
                                        tra.Year, 
                                        tra.Length, 
                                        plc.OrderInList, 
                                        plc.TrackIdInPlaylist, 
                                        plc.ProfileId AS TrackProfileId,
                                        ttv.Id AS TagValueId, 
                                        ttv.TagId, 
                                        ttv.TagValueId, 
                                        ttv.HasValue, 
                                        ttv.Value, 
                                        t.Name AS TagName, 
                                        tv.Name AS TagValueName, 
                                        ttv.ProfileId AS TagProfileId
                                        FROM 
                                        Playlist pll
                                        JOIN PlaylistContent plc ON pll.Id = plc.PlaylistId
                                        JOIN Track tra ON plc.TrackId = tra.Id
                                        LEFT JOIN TrackTagValue ttv ON tra.Id = ttv.TrackId AND ttv.TagId IN ({tagIdsParameter})
                                        LEFT JOIN Tag t ON ttv.TagId = t.Id
                                        LEFT JOIN TagValue tv ON ttv.TagValueId = tv.Id
                                        WHERE 
                                        pll.Id = @PlaylistId 
                                        AND plc.ProfileId = @ProfileId 
                                        AND pll.ProfileId = @ProfileId 
                                        AND tra.ProfileId = @ProfileId 
                                        ORDER BY 
                                        plc.OrderInList";

                command.Parameters.Add("@PlaylistId", MySqlDbType.Int32).Value = playlistId;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int trackId = reader.GetInt32(0);

                        if (!trackDictionary.TryGetValue(trackId, out var track))
                        {
                            track = new Model.Track
                            {
                                Id = trackId,
                                Path = reader.GetString(1),
                                FileName = reader.GetString(2),
                                Artist = reader.GetString(3),
                                Title = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Album = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Year = reader.GetInt32(6),
                                Length = reader.GetInt32(7),
                                OrderInList = reader.GetInt32(8),
                                TrackIdInPlaylist = reader.GetInt32(9),
                                ProfileId = reader.GetInt32(10),
                                TrackTagValues = new List<TrackTagValue>()
                            };
                            trackList.Add(track);
                            trackDictionary[trackId] = track;
                        }

                        if (!reader.IsDBNull(11))
                        {
                            var tagValue = new TrackTagValue
                            {
                                Id = reader.GetInt32(11),
                                TrackId = trackId,
                                TagId = reader.GetInt32(12),


                                TagValueId = reader.IsDBNull(13) ? (int?)null : reader.GetInt32(13),
                                HasValue = reader.GetBoolean(14),
                                Value = reader.IsDBNull(15) ? null : reader.GetString(15),
                                TagName = reader.IsDBNull(16) ? null : reader.GetString(16),
                                TagValueName = reader.IsDBNull(17) ? null : reader.GetString(17),

                                ProfileId = reader.GetInt32(18)
                            };
                            track.TrackTagValues.Add(tagValue);
                        }
                    }
                }
            }

            return trackList;
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
        public PlaylistContent GetPlaylistContentByTrackIdInPlaylist(int trackIdInPlaylist)
        {
            PlaylistContent result = null;

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * 
                                        FROM PlaylistContent 
                                        WHERE TrackIdInPlaylist = @TrackIdInPlaylist ";
                command.Parameters.Add("@TrackIdInPlaylist", MySqlDbType.Int32).Value = trackIdInPlaylist;

                using (var reader = command.ExecuteReader())
                {
                    result = new PlaylistContent();
                    while (reader.Read())
                    {
                        result.Id = (int)reader[0];
                        result.PlaylistId = (int)reader[1];
                        result.TrackId = (int)reader[2];
                        result.OrderInList = (int)reader[3];
                        result.TrackIdInPlaylist = (int)reader[4];
                        result.ProfileId = (int)reader[5];
                    }
                }
                connection.Close();
            }

            return result;
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
        public ResultOrError CreateTrackTagValue(TrackTagValue ttv)
        {
            ResultOrError result = new ResultOrError();
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
                                        @HasValue, 
                                        @Value, 
                                        @ProfileId )";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = ttv.Id;
                command.Parameters.Add("@TrackId", MySqlDbType.VarChar).Value = ttv.TrackId;
                command.Parameters.Add("@TagId", MySqlDbType.VarChar).Value = ttv.TagId;
                command.Parameters.Add("@TagValueId", MySqlDbType.VarChar).Value = ttv.TagValueId;
                command.Parameters.Add("@HasValue", MySqlDbType.Bit).Value = ttv.HasValue;
                command.Parameters.Add("@Value", MySqlDbType.VarChar).Value = ttv.Value;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("TrackTagValue is not inserted. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        public List<TrackTagValue> LoadTrackTagValuesByTrackIds(List<int> trackIds, List<Tag> tagList)
        {
            var tagValues = new List<TrackTagValue>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                var trackIdsParameter = string.Join(",", trackIds);
                var tagIdsParameter = string.Join(",", tagList.Select(tag => tag.Id));
                command.CommandText = $@"
                                        SELECT 
                                        ttv.Id, 
                                        ttv.TrackId, 
                                        ttv.TagId, 
                                        ttv.TagValueId, 
                                        ttv.HasValue, 
                                        ttv.Value, 
                                        t.Name, 
                                        tv.Name, 
                                        ttv.ProfileId 
                                        FROM 
                                        TrackTagValue ttv
                                        LEFT JOIN Tag t ON ttv.TagId = t.Id
                                        LEFT JOIN TagValue tv ON ttv.TagValueId = tv.Id
                                        WHERE 
                                        ttv.TrackId IN ({trackIdsParameter})
                                        AND ttv.TagId IN ({tagIdsParameter})
                                        AND ttv.ProfileId = @ProfileId";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tagValue = new TrackTagValue
                        {
                            Id = reader.GetInt32(0),
                            TrackId = reader.GetInt32(1),
                            TagId = reader.GetInt32(2),
                            TagValueId = reader.GetInt32(3),
                            HasValue = reader.GetBoolean(4),
                            Value = reader.GetString(5),
                            TagName = reader.GetString(6),
                            TagValueName = reader.GetString(7),
                            ProfileId = reader.GetInt32(8)
                        };
                        tagValues.Add(tagValue);
                    }
                }
            }

            return tagValues;
        }
        public void UpdateTrackTagValue(TrackTagValue ttv)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"UPDATE TrackTagValue 
                                        SET TagValueId = @TagValueId, 
                                        HasValue = @HasValue, 
                                        Value = @Value  

                                        WHERE Id = @Id 
                                        AND TrackId = @TrackId 
                                        AND ProfileId = @ProfileId";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = ttv.Id;
                command.Parameters.Add("@TagValueId", MySqlDbType.Int32).Value = ttv.TagValueId;
                command.Parameters.Add("@HasValue", MySqlDbType.Bit).Value = ttv.HasValue;
                command.Parameters.Add("@Value", MySqlDbType.VarChar).Value = ttv.Value;
                command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = ttv.TrackId;
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
                                            TagValueId = @TagValueId, 
                                            HasValue = @HasValue, 
                                            Value = @Value  

                                            WHERE Id = @Id 
                                            AND ProfileId = @ProfileId";

                    command.Parameters.Add("@TrackId", MySqlDbType.Int32).Value = ttv.TrackId;
                    command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = ttv.TagId;
                    command.Parameters.Add("@TagValueId", MySqlDbType.Int32).Value = ttv.TagValueId;
                    command.Parameters.Add("@HasValue", MySqlDbType.Bit).Value = ttv.HasValue;
                    command.Parameters.Add("@Value", MySqlDbType.VarChar).Value = ttv.Value;
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
