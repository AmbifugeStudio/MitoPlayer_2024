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
        void SetProfileId(int profileId);
        Playlist GetActivePlaylist();
        void SetActivePlaylist(int playlistId);

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
        
        int GetNextLastSmallestTrackIdInPlaylist();
        List<Playlist> GetDefaultPlaylist(String defaultPlaylistName);


        void DeleteAllPlaylistFromProfile();
        void DeleteAllPlaylistFromProfile(int id);
        void DeleteAllPlaylistContentFromProfile(int id);
        void ClearPlaylistTable();
        void ClearPlaylistContentTable();
    }
}
