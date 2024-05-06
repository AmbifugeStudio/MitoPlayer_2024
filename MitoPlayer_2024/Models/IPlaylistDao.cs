using MitoPlayer_2024.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface IPlaylistDao
    {
        List<PlaylistModel> GetAllPlaylist();
        void CreatePlaylist(PlaylistModel playlistModel);
        List<TrackModel> LoadPlaylist(PlaylistModel playlistModel);
        int GetLastObjectId(String tableName);

        void DeletePlaylistContent(int playlistId);

        bool ValidatePlaylistName(String playlistName);
        PlaylistModel GetPlaylistByName(String playlistName);
        void UpdatePlaylist(PlaylistModel playlistModel);
        void DeletePlaylist(PlaylistModel playlistModel);
        void DeleteTracksFromPlaylist(PlaylistModel playlistModel);

        /*

        int GetPlaylistContentIdByPlaylistAndTracklist(int playlistId, int trackId);

        int GetLastPlaylistContentId();
        TrackModel GetTrackByPath(String path);
       
        void UpdatePlaylist(PlaylistModel playlistModel);
        void DeletePlaylist(PlaylistModel playlistModel);
        void AddTracksToPlaylist(PlaylistModel playlistModel);
        */


    }
}
