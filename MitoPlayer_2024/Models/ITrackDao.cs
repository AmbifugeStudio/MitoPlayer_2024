using MitoPlayer_2024.Helpers;
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
        ResultOrError CreatePlaylist(Playlist playlist);
        Playlist GetPlaylist(int id);
        Playlist GetPlaylistByName(String name);
        Playlist GetActivePlaylist();
        List<Playlist> GetAllPlaylist();
        void SetActivePlaylist(int id);
        void UpdatePlaylist(Playlist playlist);
        void DeletePlaylist(int id);
        void DeleteAllPlaylist();
        void ClearPlaylistTable();
        #endregion

        #region TRACK
        void CreateTrack(Track track);
        Track GetTrackWithTags(int id, List<Tag> tagList);
        Track GetTrackWithTagsByPath(string path, List<Tag> tagList);
        List<Track> GetTracklistWithTagsByPlaylistId(int playlistId, List<Tag> tagList);
        List<int> GetAllTrackIdInList();
        void DeleteAllTrack();
        void ClearTrackTable();
        #endregion

        #region PLAYLISTCONTENT
        int GetNextSmallestTrackIdInPlaylist();
        void CreatePlaylistContent(PlaylistContent plc);
        void CreatePlaylistContentBatch(List<PlaylistContent> playlistContents);
        PlaylistContent GetPlaylistContentByTrackIdInPlaylist(int trackIdInPlaylist);
        void UpdatePlaylistContent(PlaylistContent plc);
        void DeletePlaylistContentByPlaylistId(int playlistId);
        void DeleteAllPlaylistContent();
        void ClearPlaylistContentTable();
        #endregion

        #region TRACKTAGVALUE
        bool IsTrackTagValueAlreadyExists(int trackId, int tagId);
        ResultOrError CreateTrackTagValue(TrackTagValue ttv);
        List<TrackTagValue> LoadTrackTagValuesByTrackIds(List<int> trackIds, List<Tag> tagList);
        void UpdateTrackTagValue(TrackTagValue trackTagValue);
        void UpdateTrackTagValues(List<TrackTagValue> trackTagValueList);
        
        void DeleteTagValueFromTrackTagValues(int tagValueId);
        void DeleteTrackTagValueByTagId(int tagId);
        void DeleteTrackTagValueByTrackId(int trackId);
        void DeleteAllTrackTagValue();
        void ClearTrackTagValueTable();
        #endregion
    }
}
