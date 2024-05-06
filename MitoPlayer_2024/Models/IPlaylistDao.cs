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
        List<Playlist> GetAllPlaylist();
        void CreatePlaylist(Playlist playlistModel);
        List<Track> LoadPlaylist(Playlist playlistModel);
        int GetLastObjectId(String tableName);
        void DeletePlaylistContent(int playlistId);
        bool ValidatePlaylistName(String playlistName);
        Playlist GetPlaylistByName(String playlistName);
        void UpdatePlaylist(Playlist playlistModel);
        void DeletePlaylist(Playlist playlistModel);
        void DeleteTracksFromPlaylist(Playlist playlistModel);

    }
}
