using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.NonContainer;
using Tag = MitoPlayer_2024.Models.Tag;

namespace MitoPlayer_2024.Presenters
{
    public class TagValueEditorPresenter
    {
        private ITagValueEditorView tagValueEditorView;
        private ITagDao tagDao;
        private ISettingDao settingDao;
        private Tag currentTag;
        private bool isEditMode = false;
        public TagValue newTagValue;
        private int lastGeneratedTagValueId;
        private Color tagValueColor;
       
        public TagValueEditorPresenter(ITagValueEditorView tagValueEditorView,Tag tag, ITagDao tagDao, ISettingDao settingDao)
        {
            this.tagValueEditorView = tagValueEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.currentTag = tag;
            this.isEditMode = false;

            this.lastGeneratedTagValueId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedTagValueId.ToString(), true);

            this.lastGeneratedTagValueId = this.lastGeneratedTagValueId + 1;
            ((TagValueEditorView)this.tagValueEditorView).SetTagValueName("New Tag Value " + this.lastGeneratedTagValueId.ToString());
            ((TagValueEditorView)this.tagValueEditorView).SetColor(Color.White);
            
            this.settingDao.SetIntegerSetting(Settings.LastGeneratedTagValueId.ToString(), this.lastGeneratedTagValueId, true);

            this.tagValueEditorView.CreateOrEditTagValue += CreateOrEditTagValue;
            this.tagValueEditorView.CloseEditor += CloseEditor;
            this.tagValueEditorView.ChangeColor += ChangeColor;
            
        }

        public TagValueEditorPresenter(ITagValueEditorView tagValueEditorView, Tag tag, ITagDao tagDao, ISettingDao settingDao, TagValue tagValue)
        {
            this.tagValueEditorView = tagValueEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.currentTag = tag;
            this.newTagValue = tagValue;
            this.isEditMode = true;

            this.tagValueColor = newTagValue.Color;
            ((TagValueEditorView)this.tagValueEditorView).SetTagValueName(newTagValue.Name, true);
            ((TagValueEditorView)this.tagValueEditorView).SetColor(this.tagValueColor);
           
            this.tagValueEditorView.CreateOrEditTagValue += CreateOrEditTagValue;
            this.tagValueEditorView.CloseEditor += CloseEditor;
            this.tagValueEditorView.ChangeColor += ChangeColor;
           
        }

        private void ChangeColor(object sender, EventArgs e)
        {
            ColorDialog clrDialog = new ColorDialog();
            clrDialog.Color = this.tagValueColor;
            if (clrDialog.ShowDialog() == DialogResult.OK)
            {
                ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.None;
                this.tagValueColor = clrDialog.Color;
                ((TagValueEditorView)this.tagValueEditorView).SetColor(this.tagValueColor);
            }
        }
       

        private void CreateOrEditTagValue(object sender, Helpers.ListEventArgs e)
        {
            ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newTagValue.Name))
                    {
                        this.newTagValue.Color = this.tagValueColor;
                        ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        TagValue TagValue = this.tagDao.GetTagValueByName(this.currentTag.Id, e.StringField1);
                        if (TagValue != null)
                        {
                            MessageBox.Show("TagValue name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.newTagValue.Name = e.StringField1;
                            this.newTagValue.Color = this.tagValueColor;
                            ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("TagValue name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(this.currentTag.Id);
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        if (tagValueList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("TagValue name already exists in this Tag!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            TagValue tagValue = new TagValue();
                            tagValue.Id = this.tagDao.GetNextId(TableName.TagValue.ToString());
                            tagValue.Name = e.StringField1;
                            tagValue.TagId = this.currentTag.Id;
                            tagValue.TagName = this.currentTag.Name;
                            tagValue.Color = this.tagValueColor;
                            
                            this.newTagValue = tagValue;
                            ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        TagValue tagValue = new TagValue();
                        tagValue.Id = this.tagDao.GetNextId(TableName.TagValue.ToString());
                        tagValue.Name = e.StringField1;
                        tagValue.TagId = this.currentTag.Id;
                        tagValue.TagName = this.currentTag.Name;
                        tagValue.Color = this.tagValueColor;
                       
                        this.newTagValue = tagValue;
                        ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show("TagValue name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CloseEditor(object sender, EventArgs e)
        {
            ((TagValueEditorView)this.tagValueEditorView).DialogResult = DialogResult.Cancel;
            ((TagValueEditorView)this.tagValueEditorView).Close();
        }
    }
}
