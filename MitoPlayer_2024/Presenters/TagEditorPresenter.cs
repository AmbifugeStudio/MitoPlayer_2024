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
        private ITagValueDao tagValueDao;
        private ISettingDao settingDao;
        private bool isEditMode = false;
        public Tag newTag;
        private int lastGeneratedTagId;

        public TagEditorPresenter(ITagEditorView tagEditorView, ITagValueDao tagValueDao, ISettingDao settingDao)
        {
            this.tagEditorView = tagEditorView;
            this.tagValueDao = tagValueDao;
            this.settingDao = settingDao;
            this.isEditMode = false;

            this.lastGeneratedTagId = this.settingDao.GetIntegerSetting(Settings.LastGeneratedTagId.ToString(), true);

            this.lastGeneratedTagId = this.lastGeneratedTagId + 1;
            ((TagEditorView)this.tagEditorView).SetTagName("New Tag " + this.lastGeneratedTagId.ToString());

            this.settingDao.SetIntegerSetting(Settings.LastGeneratedTagId.ToString(), this.lastGeneratedTagId, true);

            this.tagEditorView.CreateOrEditTag += CreateOrEditTag;
            this.tagEditorView.CloseEditor += CloseEditor;
        }
        public TagEditorPresenter(ITagEditorView tagEditorView, ITagValueDao tagValueDao, ISettingDao settingDao, Tag tag)
        {
            this.tagEditorView = tagEditorView;
            this.tagValueDao = tagValueDao;
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
                        Tag tag = this.tagValueDao.GetTagByName(e.StringField1);
                        if (tag != null)
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            try
                            {
                                this.newTag.Name = e.StringField1;
                                ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Tag hasn't been updated!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

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
                    List<Tag> tagList = this.tagValueDao.GetAllTag();
                    if (tagList != null && tagList.Count > 0)
                    {
                        if (tagList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            try
                            {
                                Tag tag = new Tag();
                                tag.Id = this.GetNewTagId();
                                tag.Name = e.StringField1;
                                this.newTag = tag;
                                ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Tag hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            Tag tag = new Tag();
                            tag.Id = this.GetNewTagId();
                            tag.Name = e.StringField1;
                            this.newTag = tag;
                            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.OK;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Tag hasn't been created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tag name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private int GetNewTagId()
        {
            int id = this.tagValueDao.GetLastObjectId(TableName.Tag.ToString());
            if (id == -1)
                return 0;
            else
                return id + 1;
        }

        private void CloseEditor(object sender, EventArgs e)
        {
            ((TagEditorView)this.tagEditorView).DialogResult = DialogResult.Cancel;
            ((TagEditorView)this.tagEditorView).Close();
        }
    }
}
