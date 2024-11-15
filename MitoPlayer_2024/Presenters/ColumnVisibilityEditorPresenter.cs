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
        private IColumnVisibilityEditorView view;
        private ITrackDao trackDao;
        private ISettingDao settingDao;
        private List<String> allColumnList;
        private List<String> visibleColumnList;

        private DataTable trackPropertyTable { get; set; }
        private BindingSource trackPropertyListBindingSource { get; set; }
        public List<TrackProperty> TrackPropertyList { get; set; }

        public ColumnVisibilityEditorPresenter(IColumnVisibilityEditorView view, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.view = view;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.view.ChangeVisibility += ChangeVisibility;
            this.view.MoveUp += MoveUp;
            this.view.MoveDown += MoveDown;
            this.view.CloseViewWithCancel += CloseViewWithCancel;
            this.view.CloseViewWithOk += CloseViewWithOk;

            this.InitializeDataTables();
        }

        private void InitializeDataTables()
        {
            this.trackPropertyListBindingSource = new BindingSource();
            this.trackPropertyTable = new DataTable();
            this.trackPropertyTable.Columns.Add("Id", typeof(int));
            this.trackPropertyTable.Columns.Add("Name", typeof(string));
            this.trackPropertyTable.Columns.Add("IsEnabled", typeof(bool));

            List<TrackProperty> tpList = new List<TrackProperty>();
            tpList = this.settingDao.GetTrackPropertyListByColumnGroup(ColumnGroup.TracklistColumns.ToString());
            if (tpList != null && tpList.Count > 0)
            {
                tpList = tpList.OrderBy(x => x.SortingId).ToList();
                this.TrackPropertyList = tpList;

                for (int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
                {
                    this.trackPropertyTable.Rows.Add(this.TrackPropertyList[i].Id, this.TrackPropertyList[i].Name, this.TrackPropertyList[i].IsEnabled);
                }
            }

            this.trackPropertyListBindingSource.DataSource = this.trackPropertyTable;
            this.view.SetColumnListBindingSource(this.trackPropertyListBindingSource);
        }

        private void ChangeVisibility(object sender, Helpers.Messenger e)
        {
            int selectedIndex = 0;

            this.trackPropertyTable.Rows.Clear();
            for(int i = 0; i <= this.TrackPropertyList.Count - 1; i++)
            {
                if (this.TrackPropertyList[i].Id == e.IntegerField1)
                {
                    this.TrackPropertyList[i].IsEnabled = !this.TrackPropertyList[i].IsEnabled;
                    selectedIndex = i;
                }
                this.trackPropertyTable.Rows.Add(this.TrackPropertyList[i].Id, this.TrackPropertyList[i].Name, this.TrackPropertyList[i].IsEnabled);
            }

            this.trackPropertyListBindingSource.DataSource = this.trackPropertyTable;
            this.view.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        }

        private void MoveUp(object sender, Helpers.Messenger e)
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
            this.view.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        } 
        private void MoveDown(object sender, Helpers.Messenger e)
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
            this.view.SetColumnListBindingSource(this.trackPropertyListBindingSource, selectedIndex);
        }
        private void CloseViewWithOk(object sender, EventArgs e)
        {

            ((ColumnVisibilityEditorView)this.view).DialogResult = DialogResult.OK;
            ((ColumnVisibilityEditorView)this.view).Close();
        }
        private void CloseViewWithCancel(object sender, EventArgs e)
        {
            ((ColumnVisibilityEditorView)this.view).DialogResult = DialogResult.Cancel;
            ((ColumnVisibilityEditorView)this.view).Close();
        }
    }
}
