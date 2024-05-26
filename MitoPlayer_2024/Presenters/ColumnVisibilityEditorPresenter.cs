using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class ColumnVisibilityEditorPresenter
    {
        private IColumnVisibilityEditorView columnVisibilityEditorView;
        private ISettingDao settingDao;
        private List<String> allColumnList;
        private List<String> visibleColumnList;
        public bool[] ColumnVisibilityArray;

        private String[] tracklistColumnNames = null;

        public ColumnVisibilityEditorPresenter(IColumnVisibilityEditorView columnVisibilityEditorView, ISettingDao settingDao)
        {
            this.columnVisibilityEditorView = columnVisibilityEditorView;
            this.settingDao = settingDao;

            String colNames = this.settingDao.GetStringSetting(Settings.TrackColumnNames.ToString(), true);
            tracklistColumnNames = Array.ConvertAll(colNames.Split(','), s => s);

            String colVisibility = this.settingDao.GetStringSetting(Settings.TrackColumnVisibility.ToString(), true);
            bool[] tracklistColumnVisibility = Array.ConvertAll(colVisibility.Split(','), s => Boolean.Parse(s));

            allColumnList = new List<String>();
            visibleColumnList = new List<String>();

            for (int i = 0; i <= tracklistColumnNames.Count() - 1; i++)
            {
                if (tracklistColumnVisibility[i])
                {
                    visibleColumnList.Add(tracklistColumnNames[i]);
                }
                else
                {
                    allColumnList.Add(tracklistColumnNames[i]);
                }
            }

            visibleColumnList = visibleColumnList.OrderBy(x => x).ToList();
            allColumnList = allColumnList.OrderBy(x => x).ToList();

            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetVisibleColumnList(visibleColumnList);
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetAllColumnList(allColumnList);

            this.columnVisibilityEditorView.AddColumn += AddColumn;
            this.columnVisibilityEditorView.RemoveColumn += RemoveColumn;
            this.columnVisibilityEditorView.CloseViewWithCancel += CloseViewWithCancel;
            this.columnVisibilityEditorView.CloseViewWithOk += CloseViewWithOk;
        }
        
        private void AddColumn(object sender, Helpers.ListEventArgs e)
        {
            String column = e.StringField1;
            visibleColumnList.Add(column);
            allColumnList.Remove(column);
            visibleColumnList = visibleColumnList.OrderBy(x => x).ToList();
            allColumnList = allColumnList.OrderBy(x => x).ToList();
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetVisibleColumnList(visibleColumnList);
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetAllColumnList(allColumnList);
        }
        private void RemoveColumn(object sender, Helpers.ListEventArgs e)
        {
            String column = e.StringField1;
            visibleColumnList.Remove(column);
            allColumnList.Add(column);
            visibleColumnList = visibleColumnList.OrderBy(x => x).ToList();
            allColumnList = allColumnList.OrderBy(x => x).ToList();
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetVisibleColumnList(visibleColumnList);
            ((ColumnVisibilityEditorView)this.columnVisibilityEditorView).SetAllColumnList(allColumnList);
        }
        private void CloseViewWithOk(object sender, EventArgs e)
        {
            String columnVisibilityString = "";

            foreach (String column in tracklistColumnNames)
            {
                if (this.visibleColumnList.Contains(column))
                {
                    columnVisibilityString = columnVisibilityString + "true,";
                }
                else
                {
                    columnVisibilityString = columnVisibilityString + "false,";
                }

            }
            columnVisibilityString = columnVisibilityString.Remove(columnVisibilityString.Length - 1);
            this.settingDao.SetStringSetting(Settings.TrackColumnVisibility.ToString(), columnVisibilityString, true);

            this.ColumnVisibilityArray = Array.ConvertAll(columnVisibilityString.Split(','), s => Boolean.Parse(s));

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
