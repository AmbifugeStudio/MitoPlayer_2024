using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MitoPlayer_2024.Model
{
    public interface ITrackDao
    {
        int GetNextId(String tableName);
        void SetProfileId(int profileId);
        int GetProfileId();

        #region PLAYLIST
        ResultOrError<bool> IsPlaylistNameAlreadyExists(String name);
        ResultOrError CreatePlaylist(Playlist playlist);
        ResultOrError<Playlist> GetPlaylist(int id);
        ResultOrError<Playlist> GetPlaylistByName(String name);
        ResultOrError<Playlist> GetActivePlaylist();
        ResultOrError<List<Playlist>> GetAllPlaylist();
        ResultOrError SetActivePlaylist(int id);
        ResultOrError UpdatePlaylist(Playlist playlist);
        ResultOrError DeletePlaylist(int id);
        ResultOrError DeleteAllPlaylist();
        ResultOrError ClearPlaylistTable();
        #endregion

        #region TRACK
        ResultOrError CreateTrack(Track track);
        ResultOrError<Track> GetTrackWithTags(int id, List<Tag> tagList);
        ResultOrError<Track> GetTrackWithTagsByPath(string path, List<Tag> tagList);
        ResultOrError<List<Track>> GetTracklistWithTagsByPlaylistId(int playlistId, List<Tag> tagList);
        ResultOrError<List<Track>> GetTracklistWithTagsFromDatabaseByParameters(String textFilter, List<TagValueFilter> tagValueFilterList, List<Tag> tagList, int resultSize);
        ResultOrError<List<int>> GetAllTrackIdInList();
        ResultOrError UpdateTrack(Track track);
        ResultOrError DeleteAllTrack();
        ResultOrError ClearTrackTable();
        #endregion

        #region PLAYLISTCONTENT
        ResultOrError<int> GetNextSmallestTrackIdInPlaylist();
        ResultOrError CreatePlaylistContent(PlaylistContent plc);
        ResultOrError CreatePlaylistContentBatch(List<PlaylistContent> playlistContents);
        ResultOrError<PlaylistContent> GetPlaylistContentByTrackIdInPlaylist(int trackIdInPlaylist);
        ResultOrError UpdatePlaylistContent(PlaylistContent plc);
        ResultOrError DeletePlaylistContentByPlaylistId(int playlistId);
        ResultOrError DeleteAllPlaylistContent();
        ResultOrError ClearPlaylistContentTable();
        #endregion

        #region TRACKTAGVALUE
        ResultOrError<bool> IsTrackTagValueAlreadyExists(int trackId, int tagId);
        ResultOrError CreateTrackTagValue(TrackTagValue ttv);
        ResultOrError<List<TrackTagValue>> LoadTrackTagValuesByTrackIds(List<int> trackIds, List<Tag> tagList);
        ResultOrError UpdateTrackTagValue(TrackTagValue trackTagValue);
        ResultOrError UpdateTrackTagValues(List<TrackTagValue> trackTagValueList);

        ResultOrError DeleteTagValueFromTrackTagValues(int tagValueId);
        ResultOrError DeleteTrackTagValueByTagId(int tagId);
        ResultOrError DeleteTrackTagValueByTrackId(int trackId);
        ResultOrError DeleteAllTrackTagValue();
        ResultOrError ClearTrackTagValueTable();
        #endregion

       /* List<TrainingData> GetAllTrainingData();
        ResultOrError CreateTrainingData(TrainingData trainingData);
        TrainingData GetTrainingData(int id);
        void DeleteTrainingData(int id);*/

    }
}
