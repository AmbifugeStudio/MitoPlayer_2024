using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Model
{
    public interface ITrackDao
    {
        int GetNextId(String tableName);
        void SetProfileId(int profileId);
        int GetProfileId();

        #region PLAYLIST
        bool IsPlaylistNameAlreadyExists(String name);
        void CreatePlaylist(Playlist playlist);
        Playlist GetPlaylist(int id);
        Playlist GetPlaylistByName(String name);
        Playlist GetActivePlaylist();
        List<Playlist> GetAllPlaylist();
        void SetActivePlaylist(int id);
        void UpdatePlaylist(Playlist playlist);
        void DeletePlaylist(int id);
        void DeleteAllPlaylist(bool withoutProfile = false);
        void ClearPlaylistTable();
        #endregion

        #region TRACK
        void CreateTrack(Track track);
        Track GetTrackByPath(string path, List<Tag> tagList);
        List<Track> GetTracklistByPlaylistId(int playlistId, List<Tag> tagList);
        List<int> GetAllTrackIdInList();
        void DeleteAllTrack(bool withoutProfile = false);
        void ClearTrackTable();
        #endregion

        #region PLAYLISTCONTENT
        void CreatePlaylistContent(PlaylistContent plc);
        void UpdatePlaylistContent(PlaylistContent plc);
        void DeletePlaylistContentByPlaylistId(int playlistId);
        void DeleteAllPlaylistContent(bool withoutProfile = false);
        void ClearPlaylistContentTable();
        #endregion

        #region TRACKTAGVALUE
        bool IsTrackTagValueAlreadyExists(int trackId, int tagId);
        void CreateTrackTagValue(TrackTagValue ttv);
        List<TrackTagValue> LoadTrackTagValuesByTrackId(int trackId, List<Tag> tagList);
        void UpdateTrackTagValues(List<TrackTagValue> trackTagValueList);
        void DeleteTrackTagValueByTagId(int tagId);
        void DeleteTrackTagValueByTrackId(int trackId);
        void DeleteAllTrackTagValue(bool withoutProfile = false);
        void ClearTrackTagValueTable();
        #endregion
    }
}
