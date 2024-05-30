using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class ColumnVisibilityEditorPresenter
    {
        private IColumnVisibilityEditorView columnVisibilityEditorView;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private List<String> allColumnList;
        private List<String> visibleColumnList;

        private DataTable trackPropertyTable { get; set; }
        private BindingSource trackPropertyListBindingSource { get; set; }
        public List<TrackProperty> TrackPropertyList { get; set; }

        public ColumnVisibilityEditorPresenter(IColumnVisibilityEditorView columnVisibilityEditorView,ITrackDao trackDao, ISettingDao settingDao)
        {
            this.columnVisibilityEditorView = columnVisibilityEditorView;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.columnVisibilityEditorView.ChangeVisibility += ChangeVisibility;
            this.columnVisibilityEditorView.MoveUp += MoveUp;
            this.columnVisibilityEditorView.MoveDown += MoveDown;
            this.columnVisibilityEditorView.CloseViewWithCancel += CloseViewWithCancel;
            this.columnVisibilityEditorView.CloseViewWithOk += CloseViewWithOk;

            this.InitializeDataTables();
        }

        private void InitializeDataTables()
        {
            this.trackPropertyListBindingSource = new BindingSource();
            trackPropertyTable = new DataTable();
            trackPropertyTable.Columns.Add("Id", typeof(int));
            trackPropertyTable.Columns.Add("Name", typeof(string));
            trackPropertyTable.Columns.Add("IsEnabled", typeof(bool));

            this.TrackPropertyList = this.trackDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());

            if(this.TrackPropertyList != null && this.TrackPropertyList.Count > 0)
            {
                this.TrackPropertyList = this.TrackPropertyList.OrderBy(x => x.SortingId).ToList();
                foreach (TrackProperty tp in this.TrackPropertyList)
                {
                    trackPropertyTable.Rows.Add(tp.Id, tp.Name, tp.IsEnabled);
                }
            }

            this.trackPropertyListBindingSource.DataSource = trackPropertyTable;
            this.columnVisibilityEditorView.SetColumnListBindingSource(this.trackPropertyListBindingSource);
        }

        private void ChangeVisibility(object sender, Helpers.ListEventArgs e)
        {
            int selectedIndex = 0;
            trackPropertyTable.Rows.Clear();
            for(int i = 0;i<=this.TrackPropertyList.Count-1;i++)
            {
                if (this.TrackPropertyList[i].Id == e.IntegerField1)
                {
                    this.TrackPropertyList[i].IsEnabled = !this.TrackPropertyList[i].IsEnabled;
                    selectedIndex = i;
                }
                trackPropertyTable.Rows.Add(this.TrackPropertyList[i].Id, this.TrackPropertyList[i].Name, this.TrackPropertyList[i].IsEnabled);
            }

            this.trackPropertyListBindingSource.DataSource = trackPropertyTable;
            this.columnVisibilityEditorView.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        }

        private void MoveUp(object sender, Helpers.ListEventArgs e)
        {
            int selectedIndex = 0;
            trackPropertyTable.Rows.Clear();
            for (int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
            {
                if (this.TrackPropertyList[i].Id == e.IntegerField1)
                {
                    if(i > 0)
                    {
                        TrackProperty tp = this.TrackPropertyList[i - 1];
                        this.TrackPropertyList[i -1] = this.TrackPropertyList[i];
                        this.TrackPropertyList[i] = tp;
                        selectedIndex = i - 1;
                    }
                }
                
            }
            for (int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
            {
                trackPropertyTable.Rows.Add(this.TrackPropertyList[i].Id, this.TrackPropertyList[i].Name, this.TrackPropertyList[i].IsEnabled);
            }

            this.trackPropertyListBindingSource.DataSource = trackPropertyTable;
            this.columnVisibilityEditorView.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        } 
        private void MoveDown(object sender, Helpers.ListEventArgs e)
        {
            int selectedIndex = 0;
            trackPropertyTable.Rows.Clear();
            for (int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
            {
                if (this.TrackPropertyList[i].Id == e.IntegerField1)
                {
                    if (i < this.TrackPropertyList.Count - 1)
                    {
                        TrackProperty tp = this.TrackPropertyList[i + 1];
                        this.TrackPropertyList[i+1] = this.TrackPropertyList[i];
                        this.TrackPropertyList[i] = tp;
                        selectedIndex = i + 1;
                        break;
                    }
                }
            }
            for (int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
            {
                trackPropertyTable.Rows.Add(this.TrackPropertyList[i].Id, this.TrackPropertyList[i].Name, this.TrackPropertyList[i].IsEnabled);
            }

            this.trackPropertyListBindingSource.DataSource = trackPropertyTable;
            this.columnVisibilityEditorView.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        }
        private void CloseViewWithOk(object sender, EventArgs e)
        {
            if (this.TrackPropertyList != null && TrackPropertyList.Count > 0)
            {
                int sortingId = 0;
                foreach (TrackProperty tp in this.TrackPropertyList)
                {
                    tp.SortingId = sortingId;
                    sortingId++;
                    this.trackDao.UpdateTrackProperty(tp);
                }
            }

            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).DialogResult = DialogResult.OK;
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).Close();
        }
        private void CloseViewWithCancel(object sender, EventArgs e)
        {
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).DialogResult = DialogResult.Cancel;
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).Close();
        }
    }
}
