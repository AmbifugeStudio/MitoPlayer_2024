using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class TagEditorPresenter
    {
        private ITagEditorView _view;
        private ITagDao _tagDao;
        private ISettingDao _settingDao;
        private bool _isEditMode = false;
        public Tag _newTag;
        private bool _hasMultipleValues;
        private bool _textColoring;

        public TagEditorPresenter(ITagEditorView tagEditorView, ITagDao tagDao, ISettingDao settingDao)
        {
            _view = tagEditorView;
            _tagDao = tagDao;
            _settingDao = settingDao;

            _isEditMode = false;
            _textColoring = true;
            _hasMultipleValues = false;

            _view.CreateOrEditTag += CreateOrEditTag;
            _view.CloseEditor += CloseEditor;
            _view.ChangeHasMultipleValues += ChangeHasMultipleValues;
            _view.ChangeTextColoring += ChangeTextColoring;

            int lastGeneratedTagId = _settingDao.GetIntegerSetting(Settings.LastGeneratedTagId.ToString(), true).Value;
            lastGeneratedTagId = lastGeneratedTagId + 1;

            _settingDao.SetIntegerSetting(Settings.LastGeneratedTagId.ToString(), lastGeneratedTagId, true);

            ((TagEditorView)_view).SetTagName("New Tag " + lastGeneratedTagId.ToString());
            ((TagEditorView)_view).SetTextColoring(true);
        }
        public TagEditorPresenter(ITagEditorView tagEditorView, ITagDao tagDao, ISettingDao settingDao, Tag tag)
        {
            _view = tagEditorView;
            _tagDao = tagDao;
            _settingDao = settingDao;

            _newTag = tag;
            _isEditMode = true;

            _view.CreateOrEditTag += CreateOrEditTag;
            _view.CloseEditor += CloseEditor;
            _view.ChangeHasMultipleValues += ChangeHasMultipleValues;
            _view.ChangeTextColoring += ChangeTextColoring;

            ((TagEditorView)this._view).SetTagName(_newTag.Name, true);
            ((TagEditorView)this._view).SetHasMultipleValues(_newTag.HasMultipleValues, false);
            ((TagEditorView)this._view).SetTextColoring(_newTag.TextColoring);
        }

        private void ChangeHasMultipleValues(object sender, Messenger e)
        {
            this._hasMultipleValues = e.BooleanField1;
        } 
        private void ChangeTextColoring(object sender, Messenger e)
        {
            this._textColoring = e.BooleanField1;
        }

        private void CreateOrEditTag(object sender, Helpers.Messenger e)
        {
            ((TagEditorView)this._view).DialogResult = DialogResult.None;

            if (this._isEditMode)
            {
                if (!String.IsNullOrEmpty(e.StringField1))
                {
                    if (e.StringField1.Equals(this._newTag.Name))
                    {
                        this._newTag.TextColoring = this._textColoring;
                        this._newTag.HasMultipleValues = this._hasMultipleValues;
                        ((TagEditorView)this._view).DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Tag tag = this._tagDao.GetTagByName(e.StringField1).Value;
                        if (tag != null)
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this._newTag.Name = e.StringField1;
                            this._newTag.TextColoring = this._textColoring;
                            this._newTag.HasMultipleValues = this._hasMultipleValues;
                            ((TagEditorView)this._view).DialogResult = DialogResult.OK;
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
                    List<Tag> tagList = this._tagDao.GetAllTag().Value;
                    if (tagList != null && tagList.Count > 0)
                    {
                        if (tagList.Exists(x => x.Name.Equals(e.StringField1)))
                        {
                            MessageBox.Show("Tag name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Tag tag = new Tag();
                            tag.Id = this._settingDao.GetNextId(TableName.Tag.ToString());
                            tag.Name = e.StringField1;
                            tag.TextColoring = this._textColoring;
                            tag.HasMultipleValues = this._hasMultipleValues;
                            this._newTag = tag;
                            ((TagEditorView)this._view).DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        Tag tag = new Tag();
                        tag.Id = this._settingDao.GetNextId(TableName.Tag.ToString());
                        tag.Name = e.StringField1;
                        tag.TextColoring = this._textColoring;
                        tag.HasMultipleValues = this._hasMultipleValues;
                        this._newTag = tag;
                        ((TagEditorView)this._view).DialogResult = DialogResult.OK;
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
            ((TagEditorView)this._view).DialogResult = DialogResult.Cancel;
            ((TagEditorView)this._view).Close();
        }
    }
}
