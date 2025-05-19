using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class PlaylistCreatorPresenter
    {
        private IPlaylistCreatorView view;
        private ITrackDao trackDao;
        private ITagDao tagDao;
        private ISettingDao settingDao;

        private BindingSource playlistListBindingSource;
        private BindingSource tracklistBindingSource;
        private BindingSource selectorTracklistBindingSource;

        private DataTable playlistListTable;
        private DataTable tracklistTable;
        private DataTable selectorTracklistTable;

        private List<Tag> tagList;
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary;

        private int currentPlaylistId;
        private int currentSelectorPlaylistId;
        private int currentTrackId;
        private int currentSelectorTrackId;

        public PlaylistCreatorPresenter(IPlaylistCreatorView view, ITrackDao trackDao, ITagDao tagDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.tagDao = tagDao;
            this.settingDao = settingDao;

            this.view.CreatePlaylistEvent += CreatePlaylistEvent;
            this.view.EditPlaylistEvent += EditPlaylistEvent;
            this.view.DeletePlaylistEvent += DeletePlaylistEvent;
            this.view.LoadPlaylistEvent += LoadPlaylistEvent;
            this.view.LoadSelectedPlaylistEvent += LoadSelectedPlaylistEvent;
            this.view.SelectPlaylistEvent += SelectPlaylistEvent;
            
            this.view.SelectTrackEvent += SelectTrackEvent;
        }
        public void Initialize()
        {
            view.DisablePlaylistListSelection();
            view.DisableTracklistSelection();
            view.DisableSelectorTracklistSelection();

            InitializePlaylistList();
            InitializeTagList();
            InitializeTracklist();
            InitializeSelectorTracklist();
        }

        public void InitializePlaylistList()
        {
            InitializePlaylistListColumns();
            InitializePlaylistListRows();
            InitializePlaylistListOnView();
        }
        private void InitializePlaylistListColumns()
        {
            currentPlaylistId = 0;
            playlistListBindingSource = new BindingSource();
            playlistListTable = new DataTable();

            List<TrackProperty> tpList = settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.PlaylistColumns.ToString()).Value;
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    playlistListTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
            }
        }
        private void InitializePlaylistListRows()
        {
            playlistListTable.Clear();

            List<Playlist> playlistList = trackDao.GetAllPlaylist().Value;
            if (playlistList != null && playlistList.Count > 0)
            {
                playlistList = playlistList.OrderBy(x => x.OrderInList).ToList();
                foreach (Playlist playlist in playlistList)
                {
                    DataRow dataRow = playlistListTable.NewRow();
                    dataRow["Id"] = playlist.Id;
                    dataRow["Name"] = playlist.Name;
                    playlistListTable.Rows.Add(dataRow);
                }
            }
        }
        private void InitializePlaylistListOnView()
        {
            playlistListBindingSource.DataSource = playlistListTable;
            DataTableModel model = new DataTableModel() { BindingSource = playlistListBindingSource };
            view.InitializePlaylistList(model);
        }

        private void UpdatePlaylistList()
        {
            view.DisablePlaylistListSelection();
            UpdatePlaylistListRows();
            UpdatePlaylistListOnView();
            view.EnablePlaylistListSelection();
        }
        private void UpdatePlaylistListRows()
        {
            playlistListTable.Clear();

            List<Playlist> playlistList = trackDao.GetAllPlaylist().Value;
            if (playlistList != null && playlistList.Count > 0)
            {
                playlistList = playlistList.OrderBy(x => x.OrderInList).ToList();
                foreach (Playlist playlist in playlistList)
                {
                    DataRow dataRow = playlistListTable.NewRow();
                    dataRow["Id"] = playlist.Id;
                    dataRow["Name"] = playlist.Name;
                    playlistListTable.Rows.Add(dataRow);
                }
            }
        }
        private void UpdatePlaylistListOnView()
        {
            playlistListBindingSource.DataSource = playlistListTable;
            DataTableModel model = new DataTableModel() { BindingSource = playlistListBindingSource };
            view.UpdatePlaylistList(model);
        }

        private void InitializeTagList()
        {
            tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();
            List<Tag> tagList = tagDao.GetAllTag().Value;
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;
                List<TagValue> tagValueList = new List<TagValue>();
                foreach (Tag tag in this.tagList)
                {
                    tagValueList = tagDao.GetTagValuesByTagId(tag.Id).Value;
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        Dictionary<String, Color> tvDic = new Dictionary<String, Color>();
                        foreach (TagValue tv in tagValueList)
                        {
                            tvDic.Add(tv.Name, tv.Color);
                        }
                        tagValueDictionary.Add(tag.Name, tvDic);
                    }
                }
            }
        }

        public void InitializeTracklist()
        {
            InitializeTracklistColumns();
            InitializeTracklistRows();
            InitializeTracklistOnView();
        }
        private void InitializeTracklistColumns()
        {
            currentTrackId = 0;
            tracklistBindingSource = new BindingSource();
            tracklistTable = new DataTable();

            List<TrackProperty> tpList = settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString()).Value;
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    tracklistTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
            }
        }
        private void InitializeTracklistRows()
        {
            tracklistTable.Clear();

            List<Track> trackList = trackDao.GetTracklistWithTagsByPlaylistId(currentPlaylistId, tagList).Value;
            if (trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.OrderInList).ToList();
                foreach (Track track in trackList)
                {
                    DataRow dataRow = tracklistTable.NewRow();
                    dataRow["Id"] = track.Id;
                    dataRow["Album"] = track.Album;
                    dataRow["Artist"] = track.Artist;
                    dataRow["Title"] = track.Title;
                    dataRow["Year"] = track.Year.ToString();
                    dataRow["Length"] = this.LengthToString(track.Length);
                    dataRow["IsMissing"] = track.IsMissing;
                    dataRow["Path"] = track.Path;
                    dataRow["FileName"] = track.FileName;
                    dataRow["OrderInList"] = track.OrderInList;
                    dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;

                    if (track.TrackTagValues != null)
                    {
                        foreach (TrackTagValue ttv in track.TrackTagValues)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                dataRow[ttv.TagName] = ttv.Value;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                            else
                            {
                                dataRow[ttv.TagName] = ttv.TagValueName;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                        }
                    }

                    tracklistTable.Rows.Add(dataRow);
                }
            }
        }
        private String LengthToString(double length)
        {
            TimeSpan t = TimeSpan.FromSeconds(length);
            String lengthInString = "";

            if (t.Hours > 0)
            {
                lengthInString = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            }
            else
            {
                if (t.Minutes > 0)
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                }
                else
                {
                    lengthInString = string.Format("{0:D2}:{1:D2}", 0, t.Seconds);
                }

            }
            return lengthInString;
        }
        private void InitializeTracklistOnView()
        {
            tracklistBindingSource.DataSource = tracklistTable;
            DataTableModel model = new DataTableModel() { BindingSource = tracklistBindingSource };
            view.InitializeTracklist(model);
        }

        private void UpdateTracklist()
        {
            view.DisableTracklistSelection();
            UpdateTracklistRows();
            UpdateTracklistOnView();
            view.EnableTracklistSelection();
        }
        private void UpdateTracklistRows()
        {
            tracklistTable.Clear();

            List<Track> trackList = trackDao.GetTracklistWithTagsByPlaylistId(currentPlaylistId, tagList).Value;
            if (trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.OrderInList).ToList();
                foreach (Track track in trackList)
                {
                    DataRow dataRow = tracklistTable.NewRow();
                    dataRow["Id"] = track.Id;
                    dataRow["Album"] = track.Album;
                    dataRow["Artist"] = track.Artist;
                    dataRow["Title"] = track.Title;
                    dataRow["Year"] = track.Year.ToString();
                    dataRow["Length"] = this.LengthToString(track.Length);
                    dataRow["IsMissing"] = track.IsMissing;
                    dataRow["Path"] = track.Path;
                    dataRow["FileName"] = track.FileName;
                    dataRow["OrderInList"] = track.OrderInList;
                    dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;

                    if (track.TrackTagValues != null)
                    {
                        foreach (TrackTagValue ttv in track.TrackTagValues)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                dataRow[ttv.TagName] = ttv.Value;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                            else
                            {
                                dataRow[ttv.TagName] = ttv.TagValueName;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                        }
                    }

                    tracklistTable.Rows.Add(dataRow);
                }
            }
        }
        private void UpdateTracklistOnView()
        {
            tracklistBindingSource.DataSource = tracklistTable;
            DataTableModel model = new DataTableModel() { BindingSource = tracklistBindingSource };
            view.UpdateTracklist(model);
        }

        public void InitializeSelectorTracklist()
        {
            InitializeSelectorTracklistColumns();
            InitializeSelectorTracklistRows();
            InitializeSelectorTracklistOnView();
        }
        private void InitializeSelectorTracklistColumns()
        {
            currentSelectorTrackId = 0;
            selectorTracklistBindingSource = new BindingSource();
            selectorTracklistTable = new DataTable();

            List<TrackProperty> tpList = settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString()).Value;
            if (tpList != null && tpList.Count > 0)
            {
                foreach (TrackProperty tp in tpList)
                {
                    selectorTracklistTable.Columns.Add(tp.Name, Type.GetType(tp.Type));
                }
            }
        }
        private void InitializeSelectorTracklistRows()
        {
            selectorTracklistTable.Clear();

            List<Track> trackList = trackDao.GetTracklistWithTagsByPlaylistId(currentSelectorPlaylistId, tagList).Value;
            if (trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.OrderInList).ToList();
                foreach (Track track in trackList)
                {
                    DataRow dataRow = tracklistTable.NewRow();
                    dataRow["Id"] = track.Id;
                    dataRow["Album"] = track.Album;
                    dataRow["Artist"] = track.Artist;
                    dataRow["Title"] = track.Title;
                    dataRow["Year"] = track.Year.ToString();
                    dataRow["Length"] = this.LengthToString(track.Length);
                    dataRow["IsMissing"] = track.IsMissing;
                    dataRow["Path"] = track.Path;
                    dataRow["FileName"] = track.FileName;
                    dataRow["OrderInList"] = track.OrderInList;
                    dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;

                    if (track.TrackTagValues != null)
                    {
                        foreach (TrackTagValue ttv in track.TrackTagValues)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                dataRow[ttv.TagName] = ttv.Value;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                            else
                            {
                                dataRow[ttv.TagName] = ttv.TagValueName;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                        }
                    }

                    selectorTracklistTable.Rows.Add(dataRow);
                }
            }
        }
        private void InitializeSelectorTracklistOnView()
        {
            selectorTracklistBindingSource.DataSource = selectorTracklistTable;
            DataTableModel model = new DataTableModel() { BindingSource = selectorTracklistBindingSource };
            view.InitializeSelectorTracklist(model);
        }

        private void UpdateSelectorTracklist()
        {
            view.DisableSelectorTracklistSelection();
            UpdateSelectorTracklistRows();
            UpdateSelectorTracklistOnView();
            view.EnableSelectorTracklistSelection();
        }
        private void UpdateSelectorTracklistRows()
        {
            selectorTracklistTable.Clear();

            List<Track> trackList = trackDao.GetTracklistWithTagsByPlaylistId(currentSelectorPlaylistId, tagList).Value;
            if (trackList != null && trackList.Count > 0)
            {
                trackList = trackList.OrderBy(x => x.OrderInList).ToList();
                foreach (Track track in trackList)
                {
                    DataRow dataRow = tracklistTable.NewRow();
                    dataRow["Id"] = track.Id;
                    dataRow["Album"] = track.Album;
                    dataRow["Artist"] = track.Artist;
                    dataRow["Title"] = track.Title;
                    dataRow["Year"] = track.Year.ToString();
                    dataRow["Length"] = this.LengthToString(track.Length);
                    dataRow["IsMissing"] = track.IsMissing;
                    dataRow["Path"] = track.Path;
                    dataRow["FileName"] = track.FileName;
                    dataRow["OrderInList"] = track.OrderInList;
                    dataRow["TrackIdInPlaylist"] = track.TrackIdInPlaylist;

                    if (track.TrackTagValues != null)
                    {
                        foreach (TrackTagValue ttv in track.TrackTagValues)
                        {
                            if (ttv.HasMultipleValues)
                            {
                                dataRow[ttv.TagName] = ttv.Value;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                            else
                            {
                                dataRow[ttv.TagName] = ttv.TagValueName;
                                dataRow[ttv.TagName + "TagValueId"] = ttv.TagValueId;
                            }
                        }
                    }

                    selectorTracklistTable.Rows.Add(dataRow);
                }
            }
        }
        private void UpdateSelectorTracklistOnView()
        {
            selectorTracklistBindingSource.DataSource = selectorTracklistTable;
            DataTableModel model = new DataTableModel() { BindingSource = selectorTracklistBindingSource };
            view.UpdateSelectorTracklist(model);
        }


        private void CreatePlaylistEvent(object sender, Messenger e)
        {

        }
        private void EditPlaylistEvent(object sender, Messenger e)
        {

        }
        private void DeletePlaylistEvent(object sender, Messenger e)
        {

        }
        private void LoadPlaylistEvent(object sender, Messenger e)
        {

        }
        private void LoadSelectedPlaylistEvent(object sender, Messenger e)
        {

        }
        private void SelectPlaylistEvent(object sender, Messenger e)
        {

        }
        private void SelectTrackEvent(object sender, Messenger e)
        {

        }
    }
}
