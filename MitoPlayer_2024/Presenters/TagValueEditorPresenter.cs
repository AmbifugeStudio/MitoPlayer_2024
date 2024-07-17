using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.CodeDom;
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
        private ITagValueEditorView view;
        private ITagDao tagDao;
        private ISettingDao settingDao;
        private Tag currentTag;
       
        public TagValue newTagValue;
        private String tagValueName;
        private Color tagValueColor;
        private int tagValueHotkey;
        public TagValue oldTagValue;
       
        public TagValueEditorPresenter(ITagValueEditorView tagValueEditorView,Tag tag, ITagDao tagDao, ISettingDao settingDao)
        {
            this.view = tagValueEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.currentTag = tag;
            this.oldTagValue = null;

            this.tagValueName = "New Tag Value " + this.settingDao.GetNextId(TableName.TagValue.ToString());
            this.tagValueColor = Color.White;
            this.tagValueHotkey = 0;
            ((TagValueEditorView)this.view).SetTagValueName(this.tagValueName);
            ((TagValueEditorView)this.view).SetColor(this.tagValueColor);
            ((TagValueEditorView)this.view).SetHotkey(this.tagValueHotkey);
  
            this.view.ChangeName += ChangeName;
            this.view.ChangeColor += ChangeColor;
            this.view.ChangeHotkey += ChangeHotkey;
            this.view.CloseWithOk += CloseWithOk;
            this.view.CloseWithCancel += CloseWithCancel;
        }

        public TagValueEditorPresenter(ITagValueEditorView tagValueEditorView, Tag tag, ITagDao tagDao, ISettingDao settingDao, TagValue tagValue)
        {
            this.view = tagValueEditorView;
            this.tagDao = tagDao;
            this.settingDao = settingDao;
            this.currentTag = tag;
            this.newTagValue = tagValue;
            this.oldTagValue = null;

            this.tagValueName = newTagValue.Name;
            this.tagValueColor = newTagValue.Color;
            this.tagValueHotkey = newTagValue.Hotkey;
            ((TagValueEditorView)this.view).SetTagValueName(newTagValue.Name, true);
            ((TagValueEditorView)this.view).SetColor(this.tagValueColor);
            ((TagValueEditorView)this.view).SetHotkey(this.tagValueHotkey);

            this.view.ChangeName += ChangeName;
            this.view.ChangeColor += ChangeColor;
            this.view.ChangeHotkey += ChangeHotkey;
            this.view.CloseWithOk += CloseWithOk;
            this.view.CloseWithCancel += CloseWithCancel;
        }

        private void ChangeName(object sender, ListEventArgs e)
        {
            this.tagValueName = e.StringField1;
        }
        private void ChangeColor(object sender, EventArgs e)
        {
            ColorDialog clrDialog = new ColorDialog();
            clrDialog.Color = this.tagValueColor;
            if (clrDialog.ShowDialog() == DialogResult.OK)
            {
                ((TagValueEditorView)this.view).DialogResult = DialogResult.None;
                this.tagValueColor = clrDialog.Color;
                ((TagValueEditorView)this.view).SetColor(this.tagValueColor);
            }
        } 
        private void ChangeHotkey(object sender, ListEventArgs e)
        {
            this.tagValueHotkey = e.IntegerField1;
        }

        private void CloseWithOk(object sender, EventArgs e)
        {
            bool result = true;


            String oldTagValueName = String.Empty;
            List<TagValue> tagValueList = this.tagDao.GetTagValuesByTagId(this.currentTag.Id);
            if (tagValueList != null && tagValueList.Count > 0 && this.newTagValue != null)
            {
                tagValueList.RemoveAll(x => x.Name == this.newTagValue.Name);
            }

            if (result)
            {
                if (String.IsNullOrEmpty(this.tagValueName))
                {
                    result = false;
                    MessageBox.Show("TagValue name must be entered!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (result)
            {
                if(this.newTagValue == null)
                {
                    if (tagValueList != null && tagValueList.Count > 0 && tagValueList.Exists(x => x.Name == this.tagValueName))
                    {
                        result = false;
                        MessageBox.Show("TagValue name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            if (result)
            {
                if (this.tagValueHotkey != 0)
                {
                    if (tagValueList.Exists(x => x.Hotkey == this.tagValueHotkey))
                    {
                        DialogResult dr = MessageBox.Show("Hotkey is already used by another TagValue. Do you want to replace?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            TagValue tagValue = tagValueList.Find(x => x.Hotkey == this.tagValueHotkey);
                            if (tagValue != null)
                            {
                                tagValue.Hotkey = 0;
                                this.tagDao.UpdateTagValue(tagValue);
                                oldTagValue = tagValue;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            if (result)
            {
                if(this.newTagValue == null)
                {
                    TagValue tagValue = new TagValue();
                    tagValue.Id = this.tagDao.GetNextId(TableName.TagValue.ToString());
                    tagValue.Name = this.tagValueName;
                    tagValue.TagId = this.currentTag.Id;
                    tagValue.TagName = this.currentTag.Name;
                    tagValue.Color = this.tagValueColor;
                    tagValue.Hotkey = this.tagValueHotkey;
                    this.newTagValue = tagValue;
                    try{
                        this.tagDao.CreateTagValue(this.newTagValue);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
                else
                {
                    this.newTagValue.Name = this.tagValueName;
                    this.newTagValue.Color = this.tagValueColor;
                    this.newTagValue.Hotkey = this.tagValueHotkey;
                    try
                    {
                        this.tagDao.UpdateTagValue(this.newTagValue);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
            }
            
            if (result)
            {
                ((TagValueEditorView)this.view).DialogResult = DialogResult.OK;
                ((TagValueEditorView)this.view).Close();
            }
            else
            {
                ((TagValueEditorView)this.view).DialogResult = DialogResult.None;
            }
        }

        private void CloseWithCancel(object sender, EventArgs e)
        {
            ((TagValueEditorView)this.view).DialogResult = DialogResult.Cancel;
            ((TagValueEditorView)this.view).Close();
        }
    }
}
