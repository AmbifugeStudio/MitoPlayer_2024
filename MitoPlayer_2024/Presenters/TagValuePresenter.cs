using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class TagValuePresenter
    {
        private ITagValueView tagValueEditorView;
        private ITagValueDao tagValueDao;

        private ISettingDao settingDao;
        private BindingSource tagListBindingSource { get; set; }
        private BindingSource tagValueListBindingSource { get; set; }
        private DataTable tagListTable { get; set; }
        private DataTable tagValueListTable { get; set; }
        private Tag currentTag { get; set; }
        private TagValue currentTagValue { get; set; }

        public TagValuePresenter(ITagValueView tagValueEditorView, ITagValueDao tagValueDao, ISettingDao settingDao)
        {
            this.tagValueEditorView = tagValueEditorView;
            this.tagValueDao = tagValueDao;
            this.settingDao = settingDao;

            this.tagValueEditorView.CreateTag += CreateTag;
            this.tagValueEditorView.EditTag += EditTag;
            this.tagValueEditorView.DeleteTag += DeleteTag;
            this.tagValueEditorView.CreateTagValue += CreateTagValue;
            this.tagValueEditorView.EditTagValue += EditTagValue;
            this.tagValueEditorView.DeleteTagValue += DeleteTagValue;
            this.tagValueEditorView.CloseWithOk += CloseWitOk;
            this.tagValueEditorView.CloseWithCancel += CloseWitCancel;
            this.tagValueEditorView.SetCurrentTagId += SetCurrentTagId;
            this.tagValueEditorView.SetCurrentTagValueId += SetCurrentTagValueId;

            this.InitializeDataTables();
            this.tagValueEditorView.Show();
        }
        private void InitializeDataTables()
        {
            this.tagListBindingSource = new BindingSource();
            this.tagListTable = new DataTable();
            this.tagListTable.Columns.Add("Id", typeof(Int32));
            this.tagListTable.Columns.Add("Name", typeof(String));

            this.tagValueListBindingSource = new BindingSource();
            this.tagValueListTable = new DataTable();
            this.tagValueListTable.Columns.Add("Id", typeof(Int32));
            this.tagValueListTable.Columns.Add("Name", typeof(String));

            List<Tag> tagList = this.tagValueDao.GetAllTag();
            if(tagList != null && tagList.Count > 0)
            {
                foreach(Tag tag in tagList)
                {
                    tagListTable.Rows.Add(tag.Id, tag.Name);
                }
            }

            this.tagListBindingSource.DataSource = tagListTable;
            this.tagValueEditorView.SetTagListBindingSource(this.tagListBindingSource);
            this.tagValueListBindingSource.DataSource = tagValueListTable;
            this.tagValueEditorView.SetTagValueListBindingSource(this.tagValueListBindingSource);
        }
        private void SetCurrentTagId(object sender, ListEventArgs e)
        {
            tagValueListTable.Rows.Clear();

            this.currentTag = this.tagValueDao.GetTag(Convert.ToInt32(this.tagListTable.Rows[e.IntegerField1]["Id"]));
            List<TagValue> tagValueList = this.tagValueDao.GetTagValuesByTagId(this.currentTag.Id);
            
            if (tagValueList != null && tagValueList.Count > 0)
            {
               
                foreach (TagValue tag in tagValueList)
                {
                    tagValueListTable.Rows.Add(tag.Id, tag.Name);
                }
            }
  
            this.tagValueListBindingSource.DataSource = tagValueListTable;
            this.tagValueEditorView.SetTagValueListBindingSource(this.tagValueListBindingSource);
        }
        private void SetCurrentTagValueId(object sender, ListEventArgs e)
        {
            if (this.currentTag != null)
            {
                int tagId = this.currentTag.Id;
                int tagValueId = Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]);
                this.currentTagValue = this.tagValueDao.GetTagValueByTagIdAndTagValueId(tagId, tagValueId);
            }
           
        }
        private void CreateTag(object sender, EventArgs e)
        {
            TagEditorView tagEditorView = new TagEditorView();
            TagEditorPresenter presenter = new TagEditorPresenter(tagEditorView, this.tagValueDao, this.settingDao);
            if (tagEditorView.ShowDialog((TagValueView)this.tagValueEditorView) == DialogResult.OK)
            {
                this.tagValueDao.CreateTag(presenter.newTag);
                this.settingDao.AddColumn(presenter.newTag.Name, "System.String", true, ColumnGroup.TracklistColumns.ToString());
                this.tagListTable.Rows.Add(presenter.newTag.Id, presenter.newTag.Name);
            }
        }
        private void EditTag(object sender, ListEventArgs e)
        {
            DataRow tagRow = this.tagListTable.Select("Id = " + Convert.ToInt32(this.tagListTable.Rows[e.IntegerField1]["Id"])).First();
            if(tagRow != null)
            {
                Tag tag = new Tag();
                tag.Id = (int)tagRow["Id"];
                tag.Name = (string)tagRow["Name"];

                int tagIndex = this.tagListTable.Rows.IndexOf(tagRow);

                TagEditorView tagEditorView = new TagEditorView();
                TagEditorPresenter presenter = new TagEditorPresenter(tagEditorView, this.tagValueDao, this.settingDao, tag);
                if (tagEditorView.ShowDialog((TagValueView)this.tagValueEditorView) == DialogResult.OK)
                {
                    this.tagValueDao.UpdateTag(presenter.newTag);
                    
                    TrackProperty tp = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"], ColumnGroup.TracklistColumns.ToString());
                    if(tp != null)
                    {
                        tp.Name = presenter.newTag.Name;
                        this.settingDao.UpdateColumn(tp);
                    }

                    this.tagListTable.Rows[tagIndex]["Name"] = presenter.newTag?.Name;
                    this.tagListBindingSource.DataSource = tagListTable;
                    this.tagValueEditorView.SetTagListBindingSource(this.tagListBindingSource);
                }
            }
        }
        private void DeleteTag(object sender, ListEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete the tag? All metadata of all track related to this Tag will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.tagValueDao.DeleteTag(e.IntegerField1);

                DataRow tagRow = this.tagListTable.Select("Id = " + Convert.ToInt32(this.tagListTable.Rows[e.IntegerField1]["Id"])).First();
                TrackProperty tp = this.settingDao.GetTrackPropertyByNameAndGroup((string)tagRow["Name"], ColumnGroup.TracklistColumns.ToString());
                if (tp != null)
                {
                    this.settingDao.DeleteColumn(tp.Id);
                }

                this.tagListTable.Rows.Remove(tagRow);
                this.tagListBindingSource.DataSource = tagListTable;
                this.tagValueEditorView.SetTagListBindingSource(this.tagListBindingSource);
                this.tagValueListTable.Rows.Clear();
                this.tagValueListBindingSource.DataSource = tagValueListTable;
                this.tagValueEditorView.SetTagValueListBindingSource(this.tagValueListBindingSource);
            }
        }
        private void CreateTagValue(object sender, EventArgs e)
        {
            TagValueEditorView tagValueEditorView = new TagValueEditorView();
            TagValueEditorPresenter presenter = new TagValueEditorPresenter(tagValueEditorView, this.currentTag.Id, this.tagValueDao, this.settingDao);
            if (tagValueEditorView.ShowDialog((TagValueView)this.tagValueEditorView) == DialogResult.OK)
            {
                this.tagValueDao.CreateTagValue(this.currentTag.Id,presenter.newTagValue);
                this.tagValueListTable.Rows.Add(presenter.newTagValue.Id, presenter.newTagValue.Name);
            }
        }
        private void EditTagValue(object sender, ListEventArgs e)
        {
            DataRow tagValueRow = this.tagValueListTable.Select("Id = " + Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"])).First();
            if (tagValueRow != null)
            {
                TagValue tagValue = new TagValue();
                tagValue.Id = (int)tagValueRow["Id"];
                tagValue.Name = (string)tagValueRow["Name"];

                int tagValueIndex = this.tagValueListTable.Rows.IndexOf(tagValueRow);

                TagValueEditorView tagValueEditorView = new TagValueEditorView();
                TagValueEditorPresenter presenter = new TagValueEditorPresenter(tagValueEditorView, this.currentTag.Id, this.tagValueDao, this.settingDao, tagValue);
                if (tagValueEditorView.ShowDialog((TagValueView)this.tagValueEditorView) == DialogResult.OK)
                {
                    this.tagValueDao.UpdateTagValue(presenter.newTagValue);
                    this.tagValueListTable.Rows[tagValueIndex]["Name"] = presenter.newTagValue?.Name;

                    this.tagValueListBindingSource.DataSource = tagValueListTable;
                    this.tagValueEditorView.SetTagValueListBindingSource(this.tagValueListBindingSource);
                }
            }
        }
        private void DeleteTagValue(object sender, ListEventArgs e)
        {
            if(MessageBox.Show("Do you really want to delete the tag value? All metadata of all track related to this tag value will be deleted!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                

                DataRow tagValueRow = this.tagValueListTable.Select("Id = " + Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"])).First();
                

                this.tagValueDao.DeleteTagValue(Convert.ToInt32(this.tagValueListTable.Rows[e.IntegerField1]["Id"]));
                this.tagValueListTable.Rows.Remove(tagValueRow);

                this.tagValueListBindingSource.DataSource = tagValueListTable;
                this.tagValueEditorView.SetTagValueListBindingSource(this.tagValueListBindingSource);
            }
        }
        private void CloseWitOk(object sender, EventArgs e)
        {
            ((TagValueView)this.tagValueEditorView).DialogResult = DialogResult.OK;
            ((TagValueView)this.tagValueEditorView).Close();
        }
        private void CloseWitCancel(object sender, EventArgs e)
        {
            ((TagValueView)this.tagValueEditorView).DialogResult = DialogResult.Cancel;
            ((TagValueView)this.tagValueEditorView).Close();
        }

 
    }
}
