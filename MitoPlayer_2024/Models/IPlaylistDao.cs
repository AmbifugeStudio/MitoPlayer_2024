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
        void CreatePlaylist(Playlist playlist);
        List<Track> LoadPlaylist(int id);
        int GetLastObjectId(String tableName);
        void DeletePlaylistContent(int id);
        bool ValidatePlaylistName(String name);
        Playlist GetPlaylistByName(String name);
        Playlist GetPlaylistByName(int id);
        void UpdatePlaylist(Playlist playlist);
        void DeletePlaylist(int id);
        void DeleteTracksFromPlaylist(int id);
        void DeleteAllPlaylist();
        int GetNextLastSmallestTrackIdInPlaylist();

    }
}
