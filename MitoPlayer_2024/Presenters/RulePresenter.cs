using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class RulePresenter
    {
        private IRuleView ruleView { get; set; }
        private ITagDao tagDao { get; set; }
        private ITrackDao trackDao { get; set; }
        private ISettingDao settingDao { get; set; }

        private bool isRuleListInitializing = true;
        private BindingSource ruleListBindingSource { get; set; }
        private DataTable ruleListTable { get; set; }
        public int CloseWitOk { get; private set; }
        public int CloseWitCancel { get; }

        public RulePresenter(IRuleView ruleView, ITagDao tagDao, ITrackDao trackDao, ISettingDao settingDao)
        {
            this.ruleView = ruleView;
            this.tagDao = tagDao;
            this.trackDao = trackDao;
            this.settingDao = settingDao;

            this.ruleView.CreateRule += CreateRule;
            this.ruleView.EditRule += RuleView_EditRule; ;
            this.ruleView.DeleteRule += RuleView_DeleteRule; ;

            this.ruleView.SetTagEvent += RuleView_SetTagEvent; ;
            this.ruleView.SetTagPercentEvent += RuleView_SetTagPercentEvent; ;

            this.ruleView.CloseWithOk += CloseWithOk;
            this.ruleView.CloseWithCancel += CloseWithCancel;
        }

        public void Initialize()
        {

            isRuleListInitializing = true;

            try
            {
                this.InitializeTagsAndTagValues();

               /* this.InitializeRuleListColumns();
                this.InitializeRuleListRows();
                this.InitializeRuleList();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            isRuleListInitializing = false;
        }
        public void ReloadData()
        {

        }

        #region INITALIZE - TAGS AND TAGVALUES
        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        private void InitializeTagsAndTagValues()
        {
            this.tagValueDictionary = new Dictionary<String, Dictionary<String, Color>>();

            List<Tag> tagList = this.tagDao.GetAllTag().Value;
            if (tagList != null && tagList.Count > 0)
            {
                this.tagList = tagList;

                List<TagValue> tagValueList = new List<TagValue>();

                foreach (Tag tag in this.tagList)
                {
                    Dictionary<String, Color> tvDic = new Dictionary<String, Color>();

                    tagValueList = this.tagDao.GetTagValuesByTagId(tag.Id).Value;
                    if (tagValueList != null && tagValueList.Count > 0)
                    {
                        foreach (TagValue tv in tagValueList)
                        {
                            tvDic.Add(tv.Name, tv.Color);
                        }
                        this.tagValueDictionary.Add(tag.Name, tvDic);
                    }
                }
            }
            this.ruleView.InitializeTagsAndTagValues(this.tagList, this.tagValueDictionary);
            this.ruleView.InitializeTagPanel(this.tagList);
        }
        #endregion

        #region INITIALIZE - TAGLIST
      /*  private BindingSource tagListBindingSource { get; set; }
        private DataTable tagListTable { get; set; }
        private bool[] tagListColumnVisibilityArray { get; set; }
        private Tag currentTag { get; set; }
        public void InitializeTagListColumns()
        {
            this.tagListBindingSource = new BindingSource();
            this.tagListTable = new DataTable();
            this.tagListTable.Columns.Add("Id", typeof(Int32));
            this.tagListTable.Columns.Add("Name", typeof(String));
            this.tagListTable.Columns.Add("Weight", typeof(Int32));
            this.tagListTable.Columns.Add("OrderInList", typeof(Int32));
            this.tagListBindingSource.DataSource = tagListTable;
            this.rulesView.InitializeTagListBindingSource(this.tagListBindingSource);
        }
        private void InitializeTagListRows()
        {
            this.tagListTable.Clear();
            if(this.tagList != null && this.tagList.Count > 0)
            {
                this.tagList = this.tagList.OrderBy(x => x.OrderInList).ToList();
                foreach (Tag tag in this.tagList)
                {
                    DataRow dataRow = this.tagListTable.NewRow();
                    dataRow["Id"] = tag.Id;
                    dataRow["Name"] = tag.Name;
                    dataRow["Weight"] = tag.Weight;
                    dataRow["OrderInList"] = tag.OrderInList;
                    this.tagListTable.Rows.Add(dataRow);
                }
            }

            this.rulesView.ReloadTagListBindingSource(this.currentTag);
        }
        */
        #endregion


        private void CreateRule(object sender, EventArgs e)
        {

        }
        private void RuleView_EditRule(object sender, Helpers.Messenger e)
        {

        }
        private void RuleView_DeleteRule(object sender, Helpers.Messenger e)
        {

        }
        private void RuleView_SetTagPercentEvent(object sender, Helpers.Messenger e)
        {

        }

        private void RuleView_SetTagEvent(object sender, Helpers.Messenger e)
        {

        }

        private void CloseWithOk(object sender, EventArgs e)
        {
            ((RuleView)this.ruleView).DialogResult = DialogResult.OK;
            ((RuleView)this.ruleView).Close();
        }
        private void CloseWithCancel(object sender, EventArgs e)
        {
            ((RuleView)this.ruleView).DialogResult = DialogResult.Cancel;
            ((RuleView)this.ruleView).Close();
        }
    }
}
