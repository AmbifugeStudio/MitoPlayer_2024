using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

        public void SetProfileId(int profileId)
        {
            this.profileId = profileId;
        }
        public int GetProfileId()
        {
            return this.profileId;
        }

        public int GetNextId(string tableName)
        {
            int lastId = -1;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
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
            catch (SQLiteException ex)
            {
                Logger.Error($"Error occurred while fetching the last ID from table: {tableName}", ex);
            }

            if (lastId == -1)
            {
                return 1;
            }
            else if (lastId > 0)
            {
                return lastId + 1;
            }
            else
            {
                return 1;
            }
        }
        #region PLAYLIST
        public ResultOrError<bool> IsPlaylistNameAlreadyExists(string name)
        {
            var result = new ResultOrError<bool> { Value = false };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT 1 FROM Playlist 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Value = true;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while checking if playlist name exists: {name}. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError CreatePlaylist(Playlist playlist)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"INSERT INTO Playlist (Name,OrderInList,Hotkey,IsActive,IsModelTrainer,ProfileId ) 
                                    VALUES (@Name,@OrderInList,@Hotkey,@IsActive,@IsModelTrainer,@ProfileId )";

                    command.Parameters.AddWithValue("@Name", playlist.Name);
                    command.Parameters.AddWithValue("@OrderInList", playlist.OrderInList);
                    command.Parameters.AddWithValue("@Hotkey", playlist.Hotkey);
                    command.Parameters.AddWithValue("@IsActive", playlist.IsActive ? 1 : 0);
                    command.Parameters.AddWithValue("@IsModelTrainer", playlist.IsModelTrainer ? 1 : 0);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while creating playlist [{playlist.Name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<Playlist> GetPlaylist(int id)
        {
            var result = new ResultOrError<Playlist>();
            Playlist playlist = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Playlist 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            playlist = new Playlist
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                Hotkey = reader.ReadInt("Hotkey"),
                                IsActive = reader.ReadBool("IsActive"),
                                IsModelTrainer = reader.ReadBool("IsModelTrainer"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }

                if (playlist == null)
                {
                    result.AddError($"Playlist with ID [{id}] not found.");
                }
                else
                {
                    result.Value = playlist;
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching playlist with ID [{id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<Playlist> GetPlaylistByName(string name)
        {
            var result = new ResultOrError<Playlist>();
            Playlist playlist = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Playlist 
                                    WHERE Name = @Name 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            playlist = new Playlist
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                Hotkey = reader.ReadInt("Hotkey"),
                                IsActive = reader.ReadBool("IsActive"),
                                IsModelTrainer = reader.ReadBool("IsModelTrainer"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }

                if (playlist == null)
                {
                    result.AddError($"Playlist with name [{name}] not found.");
                }
                else
                {
                    result.Value = playlist;
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching playlist with name [{name}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<Playlist> GetActivePlaylist()
        {
            var result = new ResultOrError<Playlist>();
            Playlist playlist = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Playlist 
                                    WHERE IsActive = 1 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            playlist = new Playlist
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                Hotkey = reader.ReadInt("Hotkey"),
                                IsActive = reader.ReadBool("IsActive"),
                                IsModelTrainer = reader.ReadBool("IsModelTrainer"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }

                if (playlist == null)
                {
                    //result.AddError($"Active playlist not found.");
                }
                else
                {
                    result.Value = playlist;
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching active playlist. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<List<Playlist>> GetAllPlaylist()
        {
            var result = new ResultOrError<List<Playlist>> { Value = new List<Playlist>() };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * FROM Playlist 
                                    WHERE ProfileId = @ProfileId 
                                    ORDER BY OrderInList";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var playlist = new Playlist
                            {
                                Id = reader.ReadInt("Id"),
                                Name = reader.ReadString("Name"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                Hotkey = reader.ReadInt("Hotkey"),
                                IsActive = reader.ReadBool("IsActive"),
                                IsModelTrainer = reader.ReadBool("IsModelTrainer"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };

                            result.Value.Add(playlist);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = "Error occurred while fetching all playlists. \n" + ex.Message;
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError SetActivePlaylist(int id)
        {
            var result = new ResultOrError();

            // Deaktiváljuk az összes playlist-et, mielőtt az aktívat beállítjuk
            this.InactiveAllPlaylist();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Playlist 
                                    SET IsActive = 1 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while updating playlist with ID [{id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError InactiveAllPlaylist()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE Playlist 
                                    SET IsActive = 0 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while updating playlists for ProfileId [{this.profileId}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdatePlaylist(Playlist playlist)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
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

                    command.Parameters.AddWithValue("@Id", playlist.Id);
                    command.Parameters.AddWithValue("@Name", playlist.Name);
                    command.Parameters.AddWithValue("@OrderInList", playlist.OrderInList);
                    command.Parameters.AddWithValue("@Hotkey", playlist.Hotkey);
                    command.Parameters.AddWithValue("@IsActive", playlist.IsActive ? 1 : 0);
                    command.Parameters.AddWithValue("@IsModelTrainer", playlist.IsModelTrainer ? 1 : 0);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while updating playlist with ID [{playlist.Id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeletePlaylist(int id)
        {
            var result = new ResultOrError();

            // Delete content of Playlist via PlaylistId
            this.DeletePlaylistContentByPlaylistId(id);

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Playlist 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while deleting playlist with ID [{id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllPlaylist()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Playlist 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while deleting all playlists for ProfileId [{this.profileId}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearPlaylistTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Playlist";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = "Error occurred while clearing the Playlist table. \n" + ex.Message;
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        #endregion

        #region TRACK
        public ResultOrError CreateTrack(Model.Track track)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Track (Path,FileName,Artist,Title,Album,Year,Length,Comment,ProfileId) 
                                    VALUES (@Path,@FileName,@Artist,@Title,@Album,@Year,@Length,@Comment,@ProfileId)";

                    command.Parameters.AddWithValue("@Path", track.Path);
                    command.Parameters.AddWithValue("@FileName", track.FileName);
                    command.Parameters.AddWithValue("@Artist", track.Artist);
                    command.Parameters.AddWithValue("@Title", track.Title);
                    command.Parameters.AddWithValue("@Album", track.Album);
                    command.Parameters.AddWithValue("@Year", track.Year);
                    command.Parameters.AddWithValue("@Length", track.Length);
                    command.Parameters.AddWithValue("@Comment", track.Comment);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while inserting track with Title [{track.Title}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<Model.Track> GetTrackWithTags(int id, List<Tag> tagList)
        {
            var result = new ResultOrError<Model.Track>();
            Model.Track track = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;

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
                        tra.Comment, 
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

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (track == null)
                            {
                                track = new Model.Track
                                {
                                    Id = reader.ReadInt("TrackId"),
                                    Path = reader.ReadString("Path"),
                                    FileName = reader.ReadString("FileName"),
                                    Artist = reader.ReadString("Artist"),
                                    Title = reader.ReadString("Title"),
                                    Album = reader.ReadString("Album"),
                                    Year = reader.ReadInt("Year"),
                                    Length = reader.ReadInt("Length"),
                                    Comment = reader.ReadString("Comment"),
                                    ProfileId = reader.ReadInt("TrackProfileId"),
                                    TrackTagValues = new List<TrackTagValue>()
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("TagValueId")))
                            {
                                var tagValue = new TrackTagValue
                                {
                                    Id = reader.ReadInt("TagValueId"),
                                    TrackId = reader.ReadInt("TrackId"),
                                    TagId = reader.ReadInt("TagId"),
                                    TagValueId = reader.ReadNullableInt("TagValueId"),
                                    HasMultipleValues = reader.ReadBool("HasValue"),
                                    Value = reader.ReadString("Value"),
                                    TagName = reader.ReadString("TagName"),
                                    TagValueName = reader.ReadString("TagValueName"),
                                    ProfileId = reader.ReadInt("TagProfileId")
                                };

                                track.TrackTagValues.Add(tagValue);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching track with ID [{id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            if (track == null)
                result.AddError($"Track with ID [{id}] not found.");
            else
                result.Value = track;

            return result;
        }
        public ResultOrError<Model.Track> GetTrackWithTagsByPath(string path, List<Tag> tagList)
        {
            var result = new ResultOrError<Model.Track>();
            Model.Track track = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;

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
                        tra.Comment, 
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

                    command.Parameters.AddWithValue("@Path", path);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (track == null)
                            {
                                track = new Model.Track
                                {
                                    Id = reader.ReadInt("TrackId"),
                                    Path = reader.ReadString("Path"),
                                    FileName = reader.ReadString("FileName"),
                                    Artist = reader.ReadString("Artist"),
                                    Title = reader.ReadString("Title"),
                                    Album = reader.ReadString("Album"),
                                    Year = reader.ReadInt("Year"),
                                    Length = reader.ReadInt("Length"),
                                    Comment = reader.ReadString("Comment"),
                                    ProfileId = reader.ReadInt("TrackProfileId"),
                                    TrackTagValues = new List<TrackTagValue>()
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("TagValueId")))
                            {
                                var tagValue = new TrackTagValue
                                {
                                    Id = reader.ReadInt("TagValueId"),
                                    TrackId = reader.ReadInt("TrackId"),
                                    TagId = reader.ReadInt("TagId"),
                                    TagValueId = reader.ReadNullableInt("TagValueId"),
                                    HasMultipleValues = reader.ReadBool("HasValue"),
                                    Value = reader.ReadString("Value"),
                                    TagName = reader.ReadString("TagName"),
                                    TagValueName = reader.ReadString("TagValueName"),
                                    ProfileId = reader.ReadInt("TagProfileId")
                                };

                                track.TrackTagValues.Add(tagValue);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching track by path [{path}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            if (track == null)
                result.AddError($"Track with path [{path}] not found.");
            else
                result.Value = track;

            return result;
        }
        public ResultOrError<List<Model.Track>> GetTracklistWithTagsByPlaylistId(int playlistId, List<Tag> tagList)
        {
            var result = new ResultOrError<List<Model.Track>> { Value = new List<Model.Track>() };
            var trackDictionary = new Dictionary<int, Model.Track>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;

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
                                    tra.Comment, 
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

                    command.Parameters.AddWithValue("@PlaylistId", playlistId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int trackId = reader.ReadInt("TrackId");

                            if (!trackDictionary.TryGetValue(trackId, out var track))
                            {
                                track = new Model.Track
                                {
                                    Id = trackId,
                                    Path = reader.ReadString("Path"),
                                    FileName = reader.ReadString("FileName"),
                                    Artist = reader.ReadString("Artist"),
                                    Title = reader.ReadString("Title"),
                                    Album = reader.ReadString("Album"),
                                    Year = reader.ReadInt("Year"),
                                    Length = reader.ReadInt("Length"),
                                    Comment = reader.ReadString("Comment"),
                                    OrderInList = reader.ReadInt("OrderInList"),
                                    TrackIdInPlaylist = reader.ReadInt("TrackIdInPlaylist"),
                                    ProfileId = reader.ReadInt("TrackProfileId"),
                                    TrackTagValues = new List<TrackTagValue>()
                                };
                                result.Value.Add(track);
                                trackDictionary[trackId] = track;
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("TagValueId")))
                            {
                                var tagValue = new TrackTagValue
                                {
                                    Id = reader.ReadInt("TagValueId"),
                                    TrackId = trackId,
                                    TagId = reader.ReadInt("TagId"),
                                    TagValueId = reader.ReadNullableInt("TagValueId"),
                                    HasMultipleValues = reader.ReadBool("HasValue"),
                                    Value = reader.ReadString("Value"),
                                    TagName = reader.ReadString("TagName"),
                                    TagValueName = reader.ReadString("TagValueName"),
                                    ProfileId = reader.ReadInt("TagProfileId")
                                };
                                track.TrackTagValues.Add(tagValue);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching tracks for playlist with ID [{playlistId}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<List<int>> GetAllTrackIdInList()
        {
            var result = new ResultOrError<List<int>> { Value = new List<int>() };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT Id 
                                    FROM Track 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Value.Add(reader.ReadInt("Id"));
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching track IDs for ProfileId [{this.profileId}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdateTrack(Track track)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"UPDATE Track SET 
                                    Path = @Path, 
                                    FileName = @FileName, 
                                    Artist = @Artist, 
                                    Title = @Title, 
                                    Album = @Album, 
                                    Year = @Year, 
                                    Length = @Length, 
                                    Comment = @Comment 
                                    WHERE Id = @Id 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", track.Id);
                    command.Parameters.AddWithValue("@Path", track.Path ?? ""); 
                    command.Parameters.AddWithValue("@FileName", track.FileName ?? "");
                    command.Parameters.AddWithValue("@Artist", track.Artist ?? ""); 
                    command.Parameters.AddWithValue("@Title", track.Title ?? "");
                    command.Parameters.AddWithValue("@Album", track.Album ?? ""); 
                    command.Parameters.AddWithValue("@Year", track.Year); 
                    command.Parameters.AddWithValue("@Length", track.Length); 
                    command.Parameters.AddWithValue("@Comment", track.Comment ?? ""); 
                    command.Parameters.AddWithValue("@ProfileId", this.profileId); 

                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while updating track with ID [{track.Id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllTrack()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM Track 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while deleting tracks for ProfileId [{this.profileId}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearTrackTable()
        {
            var result = new ResultOrError();

            // Delete Playlist and TrackTagValue tables
            this.ClearPlaylistTable();
            this.ClearTrackTagValueTable();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM Track";

                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = "Error occurred while clearing the Track table. \n" + ex.Message;
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        #endregion

        #region PLAYLISTCONTENT
        public ResultOrError<int> GetNextSmallestTrackIdInPlaylist()
        {
            var result = new ResultOrError<int> { Value = 0 }; // Alapértelmezett érték

            List<int> trackIds = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT TrackIdInPlaylist 
                                    FROM PlaylistContent 
                                    ORDER BY TrackIdInPlaylist";

                    using (var reader = command.ExecuteReader())
                    {
                        trackIds = new List<int>();
                        while (reader.Read())
                        {
                            trackIds.Add(reader.ReadInt("TrackIdInPlaylist"));
                        }
                    }
                }

                if (trackIds == null || trackIds.Count == 0)
                {
                    return result;
                }

                for (int i = 0; i <= trackIds.Count - 1; i++)
                {
                    if (i != trackIds[i])
                    {
                        return new ResultOrError<int> { Value = i }; 
                    }
                }

                result.Value = trackIds.Count;
            }
            catch (SQLiteException ex)
            {
                result.AddError($"Error occurred while fetching the next smallest TrackId from PlaylistContent. \n{ex.Message}");
                Logger.Error("Error occurred while fetching the next smallest TrackId from PlaylistContent.", ex);
            }

            return result;
        }

        public ResultOrError CreatePlaylistContent(PlaylistContent plc)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO PlaylistContent (Id,PlaylistId,TrackId,OrderInList,TrackIdInPlaylist,ProfileId) 
                                            VALUES (@Id,@PlaylistId,@TrackId,@OrderInList,@TrackIdInPlaylist,@ProfileId)";

                    command.Parameters.AddWithValue("@Id", plc.Id);
                    command.Parameters.AddWithValue("@PlaylistId", plc.PlaylistId);
                    command.Parameters.AddWithValue("@TrackId", plc.TrackId);
                    command.Parameters.AddWithValue("@OrderInList", plc.OrderInList);
                    command.Parameters.AddWithValue("@TrackIdInPlaylist", plc.TrackIdInPlaylist);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while inserting playlistcontent with ID [{plc.Id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError CreatePlaylistContentBatch(List<PlaylistContent> playlistContents)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var plc in playlistContents)
                        {
                            using (var command = new SQLiteCommand("INSERT INTO PlaylistContent (Id, PlaylistId, TrackId, OrderInList, TrackIdInPlaylist) " +
                                "                       VALUES (@Id, @PlaylistId, @TrackId, @OrderInList, @TrackIdInPlaylist)", connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Id", plc.Id);
                                command.Parameters.AddWithValue("@PlaylistId", plc.PlaylistId);
                                command.Parameters.AddWithValue("@TrackId", plc.TrackId);
                                command.Parameters.AddWithValue("@OrderInList", plc.OrderInList);
                                command.Parameters.AddWithValue("@TrackIdInPlaylist", plc.TrackIdInPlaylist);

                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while inserting PlaylistContent batch. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<PlaylistContent> GetPlaylistContentByTrackIdInPlaylist(int trackIdInPlaylist)
        {
            var result = new ResultOrError<PlaylistContent>();
            PlaylistContent playlistContent = null;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT * 
                                    FROM PlaylistContent 
                                    WHERE TrackIdInPlaylist = @TrackIdInPlaylist";

                    command.Parameters.AddWithValue("@TrackIdInPlaylist", trackIdInPlaylist);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            playlistContent = new PlaylistContent
                            {
                                Id = reader.ReadInt("Id"),
                                PlaylistId = reader.ReadInt("PlaylistId"),
                                TrackId = reader.ReadInt("TrackId"),
                                OrderInList = reader.ReadInt("OrderInList"),
                                TrackIdInPlaylist = reader.ReadInt("TrackIdInPlaylist"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                        }
                    }
                }

                if (playlistContent == null)
                {
                    result.AddError($"No PlaylistContent found for TrackIdInPlaylist: {trackIdInPlaylist}");
                }
                else
                {
                    result.Value = playlistContent;
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while fetching PlaylistContent by TrackIdInPlaylist [{trackIdInPlaylist}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdatePlaylistContent(PlaylistContent plc)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"UPDATE PlaylistContent SET 
                                    OrderInList = @OrderInList, 
                                    TrackIdInPlaylist = @TrackIdInPlaylist
                                    WHERE Id = @Id 
                                    AND PlaylistId = @PlaylistId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", plc.Id);
                    command.Parameters.AddWithValue("@PlaylistId", plc.PlaylistId);
                    command.Parameters.AddWithValue("@TrackId", plc.TrackId);
                    command.Parameters.AddWithValue("@OrderInList", plc.OrderInList);
                    command.Parameters.AddWithValue("@TrackIdInPlaylist", plc.TrackIdInPlaylist);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"PlaylistContent is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }

        public ResultOrError DeletePlaylistContentByPlaylistId(int playlistId)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM PlaylistContent 
                                    WHERE PlaylistId = @PlaylistId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@PlaylistId", playlistId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"PlaylistContent is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllPlaylistContent()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM PlaylistContent 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"PlaylistContent is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearPlaylistContentTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM PlaylistContent";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"PlaylistContent is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        #endregion

        #region TRACKTAGVALUE
        public ResultOrError<bool> IsTrackTagValueAlreadyExists(int trackId, int tagId)
        {
            var result = new ResultOrError<bool> { Value = false }; // Alapértelmezett érték

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"SELECT Id 
                                    FROM TrackTagValue 
                                    WHERE TrackId = @TrackId 
                                    AND TagId = @TagId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TrackId", trackId);
                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    // Az ExecuteScalar() nem tér vissza null értékkel, ha nincs találat, ezért ellenőrizzük, hogy > 0
                    if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    {
                        result.Value = true; // Ha van találat, igazra állítjuk
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while checking if TrackTagValue already exists for TrackId: {trackId} and TagId: {tagId}. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
 
        public ResultOrError CreateTrackTagValue(TrackTagValue ttv)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO TrackTagValue (TrackId,TagId,TagValueId,HasValue,Value,ProfileId) 
                                    VALUES (@TrackId,@TagId,@TagValueId,@HasValue,@Value,@ProfileId)";

                    command.Parameters.AddWithValue("@TrackId", ttv.TrackId);
                    command.Parameters.AddWithValue("@TagId", ttv.TagId);
                    command.Parameters.AddWithValue("@TagValueId", ttv.TagValueId);
                    command.Parameters.AddWithValue("@HasValue", ttv.HasMultipleValues ? 1 : 0);
                    command.Parameters.AddWithValue("@Value", ttv.Value ?? "");
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"TrackTagValue is not inserted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError<List<TrackTagValue>> LoadTrackTagValuesByTrackIds(List<int> trackIds, List<Tag> tagList)
        {
            var result = new ResultOrError<List<TrackTagValue>> { Value = new List<TrackTagValue>() };

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    var trackIdsParameter = string.Join(",", trackIds);
                    var tagIdsParameter = string.Join(",", tagList.Select(tag => tag.Id));
                    command.CommandText = $@"
                                SELECT 
                                ttv.Id AS Id, 
                                ttv.TrackId AS TrackId, 
                                ttv.TagId AS TagId, 
                                ttv.TagValueId AS TagValueId, 
                                ttv.HasValue AS HasValue, 
                                ttv.Value AS Value, 
                                t.Name AS TagName, 
                                tv.Name AS TagValueName, 
                                ttv.ProfileId AS ProfileId 
                                FROM 
                                TrackTagValue ttv
                                LEFT JOIN Tag t ON ttv.TagId = t.Id
                                LEFT JOIN TagValue tv ON ttv.TagValueId = tv.Id
                                WHERE 
                                ttv.TrackId IN ({trackIdsParameter})
                                AND ttv.TagId IN ({tagIdsParameter})
                                AND ttv.ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tagValue = new TrackTagValue
                            {
                                Id = reader.ReadInt("Id"),
                                TrackId = reader.ReadInt("TrackId"),
                                TagId = reader.ReadInt("TagId"),
                                TagValueId = reader.ReadNullableInt("TagValueId"),
                                HasMultipleValues = reader.ReadBool("HasValue"),
                                Value = reader.ReadString("Value"),
                                TagName = reader.ReadString("TagName"),
                                TagValueName = reader.ReadString("TagValueName"),
                                ProfileId = reader.ReadInt("ProfileId")
                            };
                            result.Value.Add(tagValue);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while loading TrackTagValues by TrackIds. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdateTrackTagValue(TrackTagValue ttv)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"UPDATE TrackTagValue 
                                    SET TagValueId = @TagValueId, 
                                        HasValue = @HasValue, 
                                        Value = @Value
                                    WHERE Id = @Id 
                                    AND TrackId = @TrackId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@Id", ttv.Id);
                    command.Parameters.AddWithValue("@TagValueId", ttv.TagValueId);
                    command.Parameters.AddWithValue("@HasValue", ttv.HasMultipleValues ? 1 : 0); 
                    command.Parameters.AddWithValue("@Value", ttv.Value ?? "");
                    command.Parameters.AddWithValue("@TrackId", ttv.TrackId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"Error occurred while updating TrackTagValue with ID [{ttv.Id}]. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError UpdateTrackTagValues(List<TrackTagValue> trackTagValueList)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    
                    foreach (TrackTagValue ttv in trackTagValueList)
                    {
                        command.Connection = connection;
                        command.CommandText = @"UPDATE TrackTagValue 
                                        SET TrackId = @TrackId, 
                                            TagId = @TagId, 
                                            TagValueId = @TagValueId, 
                                            HasValue = @HasValue, 
                                            Value = @Value  
                                        WHERE Id = @Id 
                                        AND ProfileId = @ProfileId";

                        command.Parameters.AddWithValue("@TrackId", ttv.TrackId);
                        command.Parameters.AddWithValue("@TagId", ttv.TagId);
                        command.Parameters.AddWithValue("@TagValueId", ttv.TagValueId);
                        command.Parameters.AddWithValue("@HasValue", ttv.HasMultipleValues ? 1 : 0);
                        command.Parameters.AddWithValue("@Value", ttv.Value ?? ""); 
                        command.Parameters.AddWithValue("@ProfileId", this.profileId);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = "Error occurred while updating TrackTagValues. \n" + ex.Message;
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteTagValueFromTrackTagValues(int tagValueId)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"UPDATE TrackTagValue 
                                    SET TagValueId = -1
                                    WHERE TagValueId = @TagValueId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TagValueId", tagValueId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"TrackTagValue with TagValueId [{tagValueId}] is not updated. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteTrackTagValueByTagId(int tagId)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TrackTagValue 
                                    WHERE TagId = @TagId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TagId", tagId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"TrackTagValue with TagId [{tagId}] is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteTrackTagValueByTrackId(int trackId)
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TrackTagValue 
                                    WHERE TrackId = @TrackId 
                                    AND ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@TrackId", trackId);
                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"TrackTagValue with TrackId [{trackId}] is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError DeleteAllTrackTagValue()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM TrackTagValue 
                                    WHERE ProfileId = @ProfileId";

                    command.Parameters.AddWithValue("@ProfileId", this.profileId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = $"TrackTagValue with ProfileId [{this.profileId}] is not deleted. \n{ex.Message}";
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        public ResultOrError ClearTrackTagValueTable()
        {
            var result = new ResultOrError();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                using (var command = new SQLiteCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM TrackTagValue";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                string errorMessage = "TrackTagValue is not deleted. \n" + ex.Message;
                result.AddError(errorMessage);
                Logger.Error(errorMessage, ex);
            }

            return result;
        }
        #endregion

        public List<TrainingData> GetAllTrainingData()
        {
            List<TrainingData> trainingDataList = new List<TrainingData>();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TrainingData 
                                        WHERE ProfileId = @ProfileId 
                                        ORDER BY CreateDate DESC";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trainingData = new TrainingData();
                        trainingData.Id = (int)reader[0];
                        trainingData.FilePath = (string)reader[1];
                        trainingData.TagId = (int)reader[2];
                        trainingData.Name = (string)reader[3];
                        trainingData.CreateDate = Convert.ToDateTime(reader[4]);
                        trainingData.SampleCount = (int)reader[5];
                        trainingData.Balance = (Decimal)reader[6];
                        trainingData.IsTemplate = Convert.ToBoolean(reader[7]);
                        trainingData.ExtractChromaFeatures = Convert.ToBoolean(reader[8]);
                        trainingData.ExtractMFCCs = Convert.ToBoolean(reader[9]);
                        trainingData.ExtractSpectralContrast = Convert.ToBoolean(reader[10]);
                        trainingData.ExtractHPCP = Convert.ToBoolean(reader[11]);
                        trainingData.ExtractSpectralCentroid = Convert.ToBoolean(reader[12]);
                        trainingData.ExtractSpectralBandwidth = Convert.ToBoolean(reader[13]);
                        trainingData.HarmonicPercussiveSeparation = Convert.ToBoolean(reader[14]);
                        trainingData.ExtractTonnetzFeatures = Convert.ToBoolean(reader[15]);
                        trainingData.ExtractZeroCrossingRate = Convert.ToBoolean(reader[16]);
                        trainingData.ExtractRmsEnergy = Convert.ToBoolean(reader[17]);
                        trainingData.ExtractPitch = Convert.ToBoolean(reader[18]);
                        trainingData.ProfileId = (int)reader[19];
                        trainingDataList.Add(trainingData);
                    }
                }
                connection.Close();
            }
            return trainingDataList;
        }
        public TrainingData GetTrainingData(int id)
        {
            TrainingData trainingData = new TrainingData();

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT * FROM TrainingData 
                                        WHERE ProfileId = @ProfileId 
                                        AND Id = @Id  
                                        ORDER BY CreateDate DESC";

                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;
                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trainingData.Id = (int)reader[0];
                        trainingData.FilePath = (string)reader[1];
                        trainingData.TagId = (int)reader[2];
                        trainingData.Name = (string)reader[3];
                        trainingData.CreateDate = Convert.ToDateTime(reader[4]);
                        trainingData.SampleCount = (int)reader[5];
                        trainingData.Balance = (Decimal)reader[6];
                        trainingData.IsTemplate = Convert.ToBoolean(reader[7]);
                        trainingData.ExtractChromaFeatures = Convert.ToBoolean(reader[8]);
                        trainingData.ExtractMFCCs = Convert.ToBoolean(reader[9]);
                        trainingData.ExtractSpectralContrast = Convert.ToBoolean(reader[10]);
                        trainingData.ExtractHPCP = Convert.ToBoolean(reader[11]);
                        trainingData.ExtractSpectralCentroid = Convert.ToBoolean(reader[12]);
                        trainingData.ExtractSpectralBandwidth = Convert.ToBoolean(reader[13]);
                        trainingData.HarmonicPercussiveSeparation = Convert.ToBoolean(reader[14]);
                        trainingData.ExtractTonnetzFeatures = Convert.ToBoolean(reader[15]);
                        trainingData.ExtractZeroCrossingRate = Convert.ToBoolean(reader[16]);
                        trainingData.ExtractRmsEnergy = Convert.ToBoolean(reader[17]);
                        trainingData.ExtractPitch = Convert.ToBoolean(reader[18]);
                        trainingData.ProfileId = (int)reader[19];

                        break;
                    }
                }
                connection.Close();
            }
            return trainingData;
        }
        public ResultOrError CreateTrainingData(TrainingData trainingData)
        {
            ResultOrError result = new ResultOrError();
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO trainingdata values ( 
                                        @Id, 
                                        @FilePath,
                                        @TagId,
                                        @Name, 
                                        @CreateDate, 
                                        @SampleCount, 
                                        @Balance,
                                        @IsTemplate,
                                        @ExtractChromaFeatures,             
                                        @ExtractMFCCs,
                                        @ExtractSpectralContrast,
                                        @ExtractHPCP,
                                        @ExtractSpectralCentroid,
                                        @ExtractSpectralBandwidth,
                                        @HarmonicPercussiveSeparation,
                                        @ExtractTonnetzFeatures,                                  
                                        @ExtractZeroCrossingRate,
                                        @ExtractRmsEnergy,
                                        @ExtractPitch,
                                        @ProfileId )";

                command.Parameters.Add("@Id", MySqlDbType.Int32).Value = trainingData.Id;
                command.Parameters.Add("@FilePath", MySqlDbType.VarChar).Value = trainingData.FilePath;
                command.Parameters.Add("@TagId", MySqlDbType.Int32).Value = trainingData.TagId;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = trainingData.Name;
                command.Parameters.Add("@CreateDate", MySqlDbType.DateTime).Value = trainingData.CreateDate;
                command.Parameters.Add("@SampleCount", MySqlDbType.Int32).Value = trainingData.SampleCount;
                command.Parameters.Add("@Balance", MySqlDbType.Decimal).Value = trainingData.Balance;
                command.Parameters.Add("@IsTemplate", MySqlDbType.Bit).Value = trainingData.IsTemplate;
                command.Parameters.Add("@ExtractChromaFeatures", MySqlDbType.Bit).Value = trainingData.ExtractChromaFeatures;
                command.Parameters.Add("@ExtractMFCCs", MySqlDbType.Bit).Value = trainingData.ExtractMFCCs;
                command.Parameters.Add("@ExtractSpectralContrast", MySqlDbType.Bit).Value = trainingData.ExtractSpectralContrast;
                command.Parameters.Add("@ExtractHPCP", MySqlDbType.Bit).Value = trainingData.ExtractHPCP;
                command.Parameters.Add("@ExtractSpectralCentroid", MySqlDbType.Bit).Value = trainingData.ExtractSpectralCentroid;
                command.Parameters.Add("@ExtractSpectralBandwidth", MySqlDbType.Bit).Value = trainingData.ExtractSpectralBandwidth;
                command.Parameters.Add("@HarmonicPercussiveSeparation", MySqlDbType.Bit).Value = trainingData.HarmonicPercussiveSeparation;
                command.Parameters.Add("@ExtractTonnetzFeatures", MySqlDbType.Bit).Value = trainingData.ExtractTonnetzFeatures;
                command.Parameters.Add("@ExtractZeroCrossingRate", MySqlDbType.Bit).Value = trainingData.ExtractZeroCrossingRate;
                command.Parameters.Add("@ExtractRmsEnergy", MySqlDbType.Bit).Value = trainingData.ExtractRmsEnergy;
                command.Parameters.Add("@ExtractPitch", MySqlDbType.Bit).Value = trainingData.ExtractPitch;
                command.Parameters.Add("@ProfileId", MySqlDbType.Int32).Value = this.profileId;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    result.AddError("TrainingData [" + trainingData.Name + " " + trainingData.CreateDate.ToShortDateString() + " " + trainingData.CreateDate.ToShortTimeString() + "] is not inserted. \n" + ex.Message);
                }
                connection.Close();
            }
            return result;
        }
        public void DeleteTrainingData(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"DELETE FROM TrainingData 
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
                    MessageBox.Show("TrainingData is not deleted. \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.Close();
            }
        }

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
