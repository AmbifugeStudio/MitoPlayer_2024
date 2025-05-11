using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Helpers.ErrorHandling;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class TagValuePresenter
    {
        private ITagValueView _view;
        private ITagValueImportView tagValueImportView;
        private ITagDao tagDao;
        private ITrackDao trackDao;

        private ISettingDao settingDao;
        private BindingSource tagListBindingSource { get; set; }
        private BindingSource tagValueListBindingSource { get; set; }
        private DataTable tagListTable { get; set; }
        private DataTable tagValueListTable { get; set; }
        private Tag currentTag { get; set; }
        private TagValue currentTagValue { get; set; }
        private bool IsInitialized { get; set; }

        public TagValuePresenter(ITagValueView tagValueEditorView, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            _view = tagValueEditorView;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            _view.CreateTag += CreateTag;
            _view.EditTag += EditTag;
            _view.DeleteTag += DeleteTag;
            _view.CreateTagValue += CreateTagValue;
            _view.EditTagValue += EditTagValue;
            _view.DeleteTagValue += DeleteTagValue;
            _view.CloseWithOk += CloseWitOk;
            _view.CloseWithCancel += CloseWitCancel;

            _view.SetCurrentTagId += SetCurrentTagId;
            _view.SetCurrentTagValueId += SetCurrentTagValueId;
            _view.OpenTagValueImportViewEvent += OpenTagValueImportViewEvent;
            _view.MoveTagListRowEvent += MoveTagListRowEvent;
        }

        private void MoveTagListRowEvent(object sender, Messenger e)
        {
            int sourceIndex = e.IntegerField1;
            int targetIndex = e.IntegerField2;

            if (sourceIndex == targetIndex || sourceIndex < 0 || targetIndex < 0)
            {
                return; // No move needed or invalid indices
            }

            // Create a list to hold the row to be moved
            List<DataRow> rowsToMove = new List<DataRow>();

            // Collect the row to be moved
            DataRow row = tagListTable.NewRow();
            row.ItemArray = tagListTable.Rows[sourceIndex].ItemArray;
            rowsToMove.Add(row);
            tagListTable.Rows.RemoveAt(sourceIndex);

            // Adjust the target index if necessary
            if (targetIndex > sourceIndex)
            {
                targetIndex -= rowsToMove.Count;
            }

            // Insert the row at the target index
            foreach (DataRow r in rowsToMove)
            {
                tagListTable.Rows.InsertAt(r, targetIndex);
                targetIndex++;
            }

            this.SaveTagList();
            this.InitializeTagDataTableContent();
        }

        private void SaveTagList()
        {
            List<Tag> taglist = this.ConvertTagListDataTableToList(tagListTable);
            int orderInList = 0;
            foreach (Tag tag in taglist)
            {
                tag.OrderInList = orderInList;
                this.tagDao.UpdateTag(tag);
                orderInList++;
            }
        }

        private List<Tag> ConvertTagListDataTableToList(DataTable dt)
        {
            List<Tag> tagList = new List<Tag>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Tag tag = new Tag();
                tag.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                tag.Name = dt.Rows[i]["Name"].ToString();
                tag.TextColoring = Convert.ToBoolean(dt.Rows[i]["TextColoring"]);
                tag.HasMultipleValues = Convert.ToBoolean(dt.Rows[i]["HasMultipleValues"]);
                tag.IsIntegrated = Convert.ToBoolean(dt.Rows[i]["IsIntegrated"]);
                tag.OrderInList = Convert.ToInt32(dt.Rows[i]["OrderInList"]);
                tagList.Add(tag);
            }

            return tagList;
        }

        public void Initialize()
        {
            try
            {
                this.InitializeTagDataTableColumns();
                this.InitializeTagValueDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void InitializeTagDataTableColumns()
        {
            this.tagListBindingSource = new BindingSource();
            this.tagListTable = new DataTable();
            this.tagListTable.Columns.Add("Id", typeof(Int32));
            this.tagListTable.Columns.Add("Name", typeof(String));
            this.tagListTable.Columns.Add("TextColoring", typeof(Boolean));
            this.tagListTable.Columns.Add("HasMultipleValues", typeof(Boolean));
            this.tagListTable.Columns.Add("IsIntegrated", typeof(Boolean));
            this.tagListTable.Columns.Add("OrderInList", typeof(Int32));

            this.tagListBindingSource.DataSource = tagListTable;
            this._view.InitializeTagListBindingSource(this.tagListBindingSource);
        }
        public void InitializeTagDataTableContent()
        {
            this.tagListTable.Rows.Clear();

            List<Tag> tagList = this.tagDao.GetAllTag().Value;
            if (tagList != null && tagList.Count > 0)
            {
                tagList = tagList.OrderBy(x => x.OrderInList).ToList();

                foreach (Tag tag in tagList)
                {
                    this.tagListTable.Rows.Add(
                        tag.Id, 
                        tag.Name, 
                        tag.TextColoring, 
                        tag.HasMultipleValues,
                        tag.IsIntegrated,
                        tag.OrderInList);
                }
               // this.currentTag = tagList[0];
            }

            this._view.ReloadTagListBindingSource(this.currentTag);
        }
        public void InitializeTagValueDataTable()
        {
            this.tagValueListBindingSource = new BindingSource();
            this.tagValueListTable = new DataTable();
            this.tagValueListTable.Columns.Add("Id", typeof(Int32));
            this.tagValueListTable.Columns.Add("Name", typeof(String));
            this.tagValueListTable.Columns.Add("Color", typeof(String));
            this.tagValueListTable.Columns.Add("Hotkey", typeof(int));

            this.tagValueListBindingSource.DataSource = tagValueListTable;
            this._view.InitializeTagValueListBindingSource(this.tagValueListBindingSource);
        }


        public void ReloadData()
        {
            try
            {
                this.InitializeTagDataTableContent();
                this.InitializeTagValueList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void InitializeTagValueList()
        {
            if (this.tagListTable != null && this.tagListTable.Rows != null && this.tagListTable.Rows.Count > 0 && this.currentTag != null)
            {
                this.tagValueListTable.Rows.Clear();

                List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(this.currentTag.Id).Value;
                if (tagValueList != null && tagValueList.Count > 0)
                {
                    foreach (TagValue tagValue in tagValueList)
                    {
                        this.tagValueListTable.Rows.Add(tagValue.Id, tagValue.Name, this.ColorToHex(tagValue.Color), tagValue.Hotkey);
                    }
                    this.currentTagValue = tagValueList[0];
                }

                this._view.ReloadTagValueListBindingSource();
            }
        }
        public string ColorToHex(Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        private void SetCurrentTagId(object sender, Messenger e)
        {
            this.SetCurrentTagId(e.IntegerField1);
        }
        private void SetCurrentTagId(int index)
        {
            if (this.tagListTable.Rows.Count > 0)
            {
                this.currentTag = this.tagDao.GetTag(Convert.ToInt32(this.tagListTable.Rows[index]["Id"])).Value;
                this._view.ReloadTagListBindingSource(this.currentTag);

                this.InitializeTagValueList();
            }
        }
        private void SetCurrentTagValueId(object sender, Messenger e)
        {
            if (this.currentTag != null)
            {
                int tagId = this.currentTag.Id;
                int tagValueId = Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]);
                this.currentTagValue = this.tagDao.GetTagValueByTagId(tagId, tagValueId).Value;
            }
        }

        private void CreateTag(object sender, EventArgs e)
        {
            TagEditorView tagEditorView = new TagEditorView();
            TagEditorPresenter presenter = new TagEditorPresenter(tagEditorView, this.tagDao, this.settingDao);
            if (tagEditorView.ShowDialog((TagValueView)this._view) == DialogResult.OK)
            {
                try
                {

                    this.CreateTag(presenter._newTag);

                    if (!presenter._newTag.HasMultipleValues)
                    {
                        TagValue tagValue = new TagValue();
                        tagValue.Name = "Default TagValue";
                        tagValue.TagId = presenter._newTag.Id;
                        tagValue.TagName = presenter._newTag.Name;
                        tagValue.Color = HexToColor("#FFFFFF");
                        tagValue.Hotkey = -1;
                        this.tagDao.CreateTagValue(tagValue);
                    }

                    this.currentTag = presenter._newTag;
                    this.tagListTable.Rows.Add(
                        presenter._newTag.Id,
                        presenter._newTag.Name,
                        presenter._newTag.TextColoring,
                        presenter._newTag.HasMultipleValues);
                    this.InitializeTagValueList();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void EditTag(object sender, Messenger e)
        {
            DataRow tagRow = this.tagListTable.Select("Id = " + Convert.ToInt32(this.tagListTable.Rows[e.IntegerField1]["Id"])).First();
            if (tagRow != null)
            {
                int id = (int)tagRow["Id"];

                Tag tag = this.tagDao.GetTag(id).Value;

                int tagIndex = this.tagListTable.Rows.IndexOf(tagRow);

                TagEditorView tagEditorView = new TagEditorView();
                TagEditorPresenter presenter = new TagEditorPresenter(tagEditorView, this.tagDao, this.settingDao, tag);
                if (tagEditorView.ShowDialog((TagValueView)this._view) == DialogResult.OK)
                {
                    this.tagDao.UpdateTag(presenter._newTag);

                    TrackProperty tpIndex = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"] + "TagValueId", ColumnGroup.TracklistColumns.ToString()).Value;
                    if (tpIndex != null)
                    {
                        tpIndex.Name = presenter._newTag.Name + "TagValueId";
                        this.settingDao.UpdateTrackProperty(tpIndex);
                    }

                    TrackProperty tp = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"], ColumnGroup.TracklistColumns.ToString()).Value;
                    if (tp != null)
                    {
                        tp.Name = presenter._newTag.Name;
                        this.settingDao.UpdateTrackProperty(tp);
                    }

                    this.tagListTable.Rows[tagIndex]["Name"] = presenter._newTag?.Name;
                    this.tagListTable.Rows[tagIndex]["TextColoring"] = presenter._newTag?.TextColoring;
                    this.tagListTable.Rows[tagIndex]["HasMultipleValues"] = presenter._newTag?.HasMultipleValues;

                    this.currentTag = presenter._newTag;
                    this._view.ReloadTagListBindingSource(this.currentTag);

                    this.InitializeTagValueList();
                }
            }
        }
        private void DeleteTag(object sender, Messenger e)
        {
            String tagName = this.tagListTable.Rows[e.IntegerField1]["Name"].ToString();
            if(tagName == "Key" || tagName =="Bpm")
            {
                MessageBox.Show("Key and Bpm tags should not be deleted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete the tag? All metadata of all track related to this Tag will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.trackDao.DeleteTrackTagValueByTagId(e.IntegerField1);

                    DataRow tagRow = this.tagListTable.Select("Id = " + Convert.ToInt32(this.tagListTable.Rows[e.IntegerField1]["Id"])).First();

                    this.trackDao.DeleteTrackTagValueByTagId((int)tagRow["Id"]);
                    this.tagDao.DeleteTagValuesByTagId((int)tagRow["Id"]);
                    this.tagDao.DeleteTag((int)tagRow["Id"]);

                    TrackProperty tp = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"], ColumnGroup.TracklistColumns.ToString()).Value;
                    if (tp != null)
                    {
                        this.settingDao.DeleteTrackProperty(tp.Id);
                    }

                    tp = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"] + "TagValueId", ColumnGroup.TracklistColumns.ToString()).Value;
                    if (tp != null)
                    {
                        this.settingDao.DeleteTrackProperty(tp.Id);
                    }

                    int selectedIndex = 0;
                    if (e.IntegerField1 == 0)
                    {
                        selectedIndex = 0;
                    }
                    else if (e.IntegerField1 == this.tagListTable.Rows.Count - 1)
                    {
                        selectedIndex = this.tagListTable.Rows.Count - 2;
                    }
                    else
                    {
                        selectedIndex = e.IntegerField1;
                    }

                    this.tagListTable.Rows.Remove(tagRow);

                    /* this.tagListBindingSource.DataSource = tagListTable;
                     this.tagValueView.SetTagListBindingSource(this.tagListBindingSource);*/

                    this.tagValueListTable.Rows.Clear();
                    /* this.tagValueListBindingSource.DataSource = tagValueListTable;
                     this.tagValueView.SetTagValueListBindingSource(this.tagValueListBindingSource);*/

                    if (this.tagListTable.Rows.Count > 0)
                        this.SetCurrentTagId(selectedIndex);
                }
            }

            
        }
        private void CreateTagValue(object sender, EventArgs e)
        {
            TagValueEditorView tagValueEditorView = new TagValueEditorView();
            TagValueEditorPresenter presenter = new TagValueEditorPresenter(tagValueEditorView, this.currentTag, this.tagDao, this.settingDao);
            if (tagValueEditorView.ShowDialog((TagValueView)this._view) == DialogResult.OK)
            {
                this.tagValueListTable.Rows.Add(presenter.newTagValue.Id, presenter.newTagValue.Name, this.ColorToHex(presenter.newTagValue.Color), presenter.newTagValue.Hotkey);
            }
        }
        private void EditTagValue(object sender, Messenger e)
        {
            DataRow tagValueRow = this.tagValueListTable.Select("Id = " + Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"])).First();
            if (tagValueRow != null)
            {
                int id = (int)tagValueRow["Id"];

                TagValue tagValue = this.tagDao.GetTagValue(id).Value;

                int tagValueIndex = this.tagValueListTable.Rows.IndexOf(tagValueRow);

                TagValueEditorView tagValueEditorView = new TagValueEditorView();
                TagValueEditorPresenter presenter = new TagValueEditorPresenter(tagValueEditorView, this.currentTag, this.tagDao, this.settingDao, tagValue);
                if (tagValueEditorView.ShowDialog((TagValueView)this._view) == DialogResult.OK)
                {
                    this.tagValueListTable.Rows[tagValueIndex]["Name"] = presenter.newTagValue?.Name;
                    this.tagValueListTable.Rows[tagValueIndex]["Color"] = ColorToHex(presenter.newTagValue.Color);
                    this.tagValueListTable.Rows[tagValueIndex]["Hotkey"] = presenter.newTagValue.Hotkey;

                    if (presenter.oldTagValue != null)
                    {
                        DataRow oldTagValueRow = this.tagValueListTable.Select("Id = " + presenter.oldTagValue.Id).First();
                        int oldTagValueIndex = this.tagValueListTable.Rows.IndexOf(oldTagValueRow);
                        this.tagValueListTable.Rows[oldTagValueIndex]["Hotkey"] = presenter.oldTagValue.Hotkey;
                    }
                }
            }
        }

        private void DeleteTagValue(object sender, Messenger e)
        {
            if(this.tagValueListTable.Rows.Count == 1)
            {
                MessageBox.Show("One tag value is mandatory and should not be deleted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete the tag value? All metadata of all track related to this tag value will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    DataRow tagValueRow = this.tagValueListTable.Select("Id = " + Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"])).First();

                    this.tagDao.DeleteTagValue(Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]));
                    this.trackDao.DeleteTagValueFromTrackTagValues(Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]));

                    this.tagValueListTable.Rows.Remove(tagValueRow);
                }
            }
        }

        private void OpenTagValueImportViewEvent(object sender, EventArgs e)
        {
            ResultOrError result = new ResultOrError();
            TagValueImportView tagValueImportView = new TagValueImportView();
            TagValueImportPresenter presenter = new TagValueImportPresenter(tagValueImportView, this.tagDao);
            tagValueImportView.ShowDialog((TagValueImportView)this.tagValueImportView);

            if(tagValueImportView.DialogResult == DialogResult.OK)
            {
                result = ImportTagsAndTagValues(presenter.TagNames, presenter.TagValueNames, presenter.ColorCodes);

                if (!result.Success)
                {
                    MessageBox.Show(result.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            

        }
        private ResultOrError ImportTagsAndTagValues(List<String> tagNames, List<List<String>> tagValueNames, List<List<String>> colorCodes)
        {
            ResultOrError result = new ResultOrError();
            if (result.Success)
            {
                if (tagNames == null || tagNames.Count == 0)
                {
                    result.AddError("No tag to process!");
                }
            }
            if (result.Success)
            {
                if (tagValueNames == null || tagValueNames.Count == 0)
                {
                    result.AddError("No tag value to process!");
                }
            }
            if (result.Success)
            {
                for (int i = 0; i <= tagNames.Count - 1; i++)
                {
                    if (tagValueNames[i] == null || tagValueNames[i].Count == 0)
                    {
                        result.AddError("No tag value to process to this tag!");
                        break;
                    }
                }
            }
            if (result.Success)
            {
                for (int i = 0; i <= tagNames.Count - 1; i++)
                {
                    Tag tag = new Tag();
                    String tagName = tagNames[i];
                    String[] tagNameParts = tagName.Split('(');
                    tag.Name = tagNameParts[0].Trim();
                    String tagProperties = tagNameParts.Length > 1 ? tagNameParts[1].Trim(')') : "";

                    // Set properties based on the tagProperties string
                    tag.TextColoring = tagProperties.Contains("Text");
                    tag.HasMultipleValues = tagProperties.Contains("HasMultipleValues");

                    result = this.CreateTag(tag);

                    if (result.Success)
                    {
                        for (int j = 0; j <= tagValueNames[i].Count - 1; j++)
                        {
                            TagValue tagValue = new TagValue();
                            tagValue.Name = tagValueNames[i][j];
                            tagValue.TagId = tag.Id;
                            tagValue.TagName = tag.Name;
                            tagValue.Color = HexToColor(colorCodes[i][j]);
                            tagValue.Hotkey = 0;

                            result = this.tagDao.CreateTagValue(tagValue);
                            if (!result.Success)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            this.Initialize();
            this.ReloadData();

            return result;
        }
        private Color HexToColor(string hexValue)
        {
            return System.Drawing.ColorTranslator.FromHtml(hexValue);
        }
        private ResultOrError CreateTag(Tag tag)
        {
            ResultOrError result = new ResultOrError();
            result = this.tagDao.CreateTag(tag);

            if (result.Success)
            {
                List<int> trackIdList = this.trackDao.GetAllTrackIdInList().Value;
                if (trackIdList != null && trackIdList.Count > 0)
                {
                    foreach (int trackId in trackIdList)
                    {
                        if (!this.trackDao.IsTrackTagValueAlreadyExists(trackId, tag.Id))
                        {
                            TrackTagValue ttv = new TrackTagValue();
                            ttv.TrackId = trackId;
                            ttv.TagId = tag.Id;
                            ttv.TagName = tag.Name;
                            ttv.TagValueId = -1;
                            ttv.TagValueName = String.Empty;
                            result = this.trackDao.CreateTrackTagValue(ttv);
                            if (!result.Success)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (result.Success)
            {
                TrackProperty tp = new TrackProperty();
                tp.Name = tag.Name;
                tp.Type = "System.String";
                tp.IsEnabled = true;
                tp.ColumnGroup = ColumnGroup.TracklistColumns.ToString();
                tp.SortingId = this.settingDao.GetNextTrackPropertySortingId().Value;
                result = this.settingDao.CreateTrackProperty(tp);

                tp.Name = tag.Name + "TagValueId";
                tp.Type = "System.Int32";
                tp.IsEnabled = false;
                tp.ColumnGroup = ColumnGroup.TracklistColumns.ToString();
                tp.SortingId = this.settingDao.GetNextTrackPropertySortingId().Value;
                result = this.settingDao.CreateTrackProperty(tp);
            }

            if (result.Success)
            {
                if (tag.HasMultipleValues)
                {
                    TagValue tv = new TagValue();
                    tv.TagId = tag.Id;
                    tv.TagName = tag.Name;
                    tv.Name = tag.Name;
                    tv.Color = Color.White;
                    result = this.tagDao.CreateTagValue(tv);
                }
            }
            return result;
        }
        private void CloseWitOk(object sender, EventArgs e)
        {
            ((TagValueView)this._view).DialogResult = DialogResult.OK;
            ((TagValueView)this._view).Close();
        }
        private void CloseWitCancel(object sender, EventArgs e)
        {
            ((TagValueView)this._view).DialogResult = DialogResult.Cancel;
            ((TagValueView)this._view).Close();
        }

        /*
        public void SelectFirstTagValue()
        {
            this.SetCurrentTagId(0);
        }

        
       
        private void SetCurrentTagId(int index)
        {
            if(this.tagListTable.Rows.Count > 0)
            {
                tagValueListTable.Rows.Clear();

                this.currentTag = this.tagDao.GetTag(Convert.ToInt32(this.tagListTable.Rows[index]["Id"]));
                List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(this.currentTag.Id);

                if (tagValueList != null && tagValueList.Count > 0)
                {
                    foreach (TagValue tagValue in tagValueList)
                    {
                        tagValueListTable.Rows.Add(tagValue.Id, tagValue.Name, this.ColorToHex(tagValue.Color), tagValue.Hotkey);
                    }
                }

                this.tagValueListBindingSource.DataSource = tagValueListTable;
                this.tagValueView.SetTagValueListBindingSource(this.tagValueListBindingSource, this.currentTag.HasMultipleValues);
            }
        }
        private void SetCurrentTagValueId(object sender, ListEventArgs e)
        {
            if (this.currentTag != null)
            {
                int tagId = this.currentTag.Id;
                int tagValueId = Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]);
                this.currentTagValue = this.tagDao.GetTagValueByTagId(tagId, tagValueId);
            }
           
        }
        
       
        
        
        
       
        
        */

    }
}
