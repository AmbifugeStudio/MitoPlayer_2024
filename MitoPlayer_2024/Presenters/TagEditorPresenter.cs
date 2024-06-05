using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class TagEditorPresenter
    {
        private ITagEditorView tagEditorView;
        private ITagDao tagDao;
        private ISettingDao settingDao;
        private bool isEditMode = false;
        public Tag newTag;
        private int lastGeneratedTagId;

        public TagEditorPresenter(ITagEditorView tagEditorView, ITagDao tagDao, ISettingDao settingDao)
        {
            this.tagEditorView = tagEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedTagId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedTagId.ToString(), true);

            this.lastGeneratedTagId = this.lastGeneratedTagId + 1;
            ((TagEditorView)this.tagEditorView).SetTagName("New Tag " + this.lastGeneratedTagId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedTagId.ToString(), this.lastGeneratedTagId, true);

            this.tagEditorView.CreateOrEditTag += CreateOrEditTag;
            this.tagEditorView.CloseEditor += CloseEditor;
        }
        public TagEditorPresenter(ITagEditorView tagEditorView, ITagDao tagDao, ISettingDao settingDao, Tag tag)
        {
            this.tagEditorView = tagEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.newTag = tag;
            this.isEditMode = true;

            ((TagEditorView)this.tagEditorView).SetTagName(newTag.Name, true);

            this.tagEditorView.CreateOrEditTag += CreateOrEditTag;
            this.tagEditorView.CloseEditor += CloseEditor;
        }

        private void CreateOrEditTag(object sender, Helpers.ListEventArgs e)
        {
            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.None;

            if (this.isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this.newTag.Name))
                    {
                        ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Tag tag = this.tagDao.GetTagByName(e.StringField1);
                        if (tag != null)
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.newTag.Name = e.StringField1;
                            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tag name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    List<Tag> tagList = this.tagDao.GetAllTag();
                    if (tagList != null && tagList.Count > 0)
                    {
                        if (tagList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Tag tag = new Tag();
                            tag.Id = this.tagDao.GetNextId(TableName.Tag.ToString());
                            tag.Name = e.StringField1;
                            this.newTag = tag;
                            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        Tag tag = new Tag();
                        tag.Id = this.tagDao.GetNextId(TableName.Tag.ToString());
                        tag.Name = e.StringField1;
                        this.newTag = tag;
                        ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show("Tag name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CloseEditor(object sender, EventArgs e)
        {
            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.Cancel;
            ((TagEditorView)this.tagEditorView).Close();
        }
    }
}
