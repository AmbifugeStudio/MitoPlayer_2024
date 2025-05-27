using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class RuleView : Form, IRuleView
    {
        private Form parentView { get; set; }

        //DATATABLES
        private BindingSource ruleListBindingSource { get; set; }

        //RULELIST
        public event EventHandler CreateRule;
        public event EventHandler<Messenger> EditRule;
        public event EventHandler<Messenger> DeleteRule;

        public event EventHandler<Messenger> SetTagEvent;
        public event EventHandler<Messenger> SetTagPercentEvent;

        public event EventHandler CloseWithOk;
        public event EventHandler CloseWithCancel;

        private bool isInitializing = true;

        public RuleView()
        {
            InitializeComponent();
            this.SetControlColors();

            this.ruleListBindingSource = new BindingSource();
            this.dgvRuleList.AllowDrop = false;
        }

        private void SetControlColors()
        {
            this.BackColor = CustomColor.BackColor;
            this.ForeColor = CustomColor.ForeColor;

            this.dgvRuleList.BackgroundColor = CustomColor.ButtonBackColor;
            this.dgvRuleList.ColumnHeadersDefaultCellStyle.BackColor = CustomColor.ButtonBackColor;
            this.dgvRuleList.ColumnHeadersDefaultCellStyle.ForeColor = CustomColor.ForeColor;
            this.dgvRuleList.EnableHeadersVisualStyles = false;
            this.dgvRuleList.ColumnHeadersDefaultCellStyle.SelectionBackColor = CustomColor.ButtonBackColor;
            this.dgvRuleList.DefaultCellStyle.SelectionBackColor = CustomColor.GridSelectionColor;

            this.grbTags.BackColor = CustomColor.BackColor;
            this.grbTags.ForeColor = CustomColor.ForeColor;
            this.grbRules.BackColor = CustomColor.BackColor;
            this.grbRules.ForeColor = CustomColor.ForeColor;

            this.pnlTagList.ForeColor = CustomColor.ForeColor;
            this.pnlTagList.BackColor = CustomColor.BackColor;
        }


        #region SINGLETON

        public static RuleView instance;
        public static RuleView GetInstance(Form parentView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new RuleView();
                instance.parentView = parentView;
                instance.MdiParent = parentView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
                instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        #region TABLE BINDINGS AND INIT

        //TAGS AND TAGVALUES
        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        public void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary)
        {
            this.tagList = tagList;
            this.tagValueDictionary = tagValueDictionary;
        }

        //RULELIST DATA BINDING
        private int currentRuleId { get; set; }
        public void InitializeRuleList(DataTableModel model)
        {
            if (model.BindingSource != null)
            {
                this.ruleListBindingSource.DataSource = model.BindingSource;
                this.dgvRuleList.DataSource = this.ruleListBindingSource.DataSource;
            }

            if (model.ColumnVisibilityArray != null && model.ColumnVisibilityArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvRuleList.Columns.Count - 1; i++)
                {
                    this.dgvRuleList.Columns[i].Visible = model.ColumnVisibilityArray[i];
                    this.dgvRuleList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            if (model.CurrentObjectId != -1)
            {
                this.currentRuleId = model.CurrentObjectId;
            }

            this.ruleListBindingSource.ResetBindings(false);

            this.UpdateRuleListColor(model.CurrentObjectId);
        }
        private void dgvRuleList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.UpdateRuleListColor(this.currentRuleId);
        }
        public void ReloadRuleList(DataTableModel model)
        {
            this.UpdateRuleListColor(model.CurrentObjectId);
        }
        public void UpdateRuleListColor(int currentObjectId = -1)
        {
            int ruleId = -1;
            if (this.dgvRuleList != null && this.dgvRuleList.Rows != null && this.dgvRuleList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvRuleList.Rows.Count; i++)
                {
                    ruleId = (int)this.dgvRuleList.Rows[i].Cells["Id"].Value;
                    if (currentObjectId != -1 && ruleId == currentObjectId)
                    {
                        this.dgvRuleList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridPlayingColor;
                    }
                    else
                    {
                        if (i == 0 || i % 2 == 0)
                        {
                            this.dgvRuleList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridLineColor1;
                        }
                        else
                        {
                            this.dgvRuleList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridLineColor2;
                        }
                    }
                }
            }
        }


        //TAGPANEL INIT
        public void InitializeTagPanel(List<Tag> tagList)
        {
            this.pnlTagList.Controls.Clear();

            for (int i = 0; i < tagList.Count; i++)
            {
                int buttonLengthX = 75;
                int buttonLengthY = 23;
                int buttonsIntervalX = 5;
                int buttonsIntervalY = 20;

                TagValueButton btn = new TagValueButton();
                btn.TagId = tagList[i].Id;
                btn.Name = "TagButton_" + i.ToString();
                btn.Text = tagList[i].Name;
                btn.Size = new Size(buttonLengthX, buttonLengthY);
                btn.MouseDown += new MouseEventHandler(
                      (sender, e) => this.btnSetTag_Click(sender, e));
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.UseVisualStyleBackColor = false;
                btn.BackColor = CustomColor.ButtonBackColor;
                btn.ForeColor = CustomColor.ForeColor;
                btn.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                btn.Location = new Point(buttonsIntervalX, i * (buttonsIntervalY + buttonLengthY));
                pnlTagList.Controls.Add(btn);

                NumericUpDown nupd = new NumericUpDown();
                nupd.Value = tagList[i].Percent;
                nupd.Size = new Size(buttonLengthX, buttonLengthY);
                nupd.ValueChanged += new System.EventHandler(
                    (sender,e) => this.numericUpDown_ValueChanged(sender, e, nupd, btn));
                nupd.BackColor = CustomColor.ButtonBackColor;
                nupd.ForeColor = CustomColor.ForeColor;
                nupd.Location = new Point(2 * buttonsIntervalX + buttonLengthX, i * (buttonsIntervalY + buttonLengthY));
                pnlTagList.Controls.Add(nupd);

                Label lbl = new Label();
                lbl.Text = "%";
                lbl.Size = new Size(buttonLengthX, buttonLengthY);
                lbl.BackColor = CustomColor.ButtonBackColor;
                lbl.ForeColor = CustomColor.ForeColor;
                lbl.Location = new Point(3 *buttonsIntervalX + 2* buttonLengthX, i * (buttonsIntervalY + buttonLengthY));
                pnlTagList.Controls.Add(lbl);
            }
        }
        private void btnSetTag_Click(object sender, EventArgs e)
        {
            this.SetTag(sender, e, null);
        }
        private void SetTag(object sender, EventArgs e, Button btn)
        {
            TagValueButton button = (sender as TagValueButton);

            if (btn != null)
                button = (TagValueButton)btn;

            ClearTagButtonColors();

            button.FlatAppearance.BorderColor = CustomColor.ActiveButtonColor;

            String tagName = button.TagName;

            this.SetTagEvent?.Invoke(this, new Messenger()
            {
                StringField1 = tagName
            });
            this.SetFocusToDataGridView();
        }
        private void ClearTagButtonColors()
        {
            List<TagValueButton> tvbList = new List<TagValueButton>();

            foreach( var control in this.pnlTagList.Controls)
            {
                if(control is TagValueButton)
                {
                    ((TagValueButton)control).FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                }
            }
        }
        internal void SetFocusToDataGridView()
        {
            this.dgvRuleList.Focus();
        }
        private void numericUpDown_ValueChanged(object sender, EventArgs e, NumericUpDown npd, TagValueButton button)
        {
            NumericUpDown nupd = (sender as NumericUpDown);
            if (nupd != null)
                npd = (NumericUpDown)nupd;
            this.SetTagPercentEvent?.Invoke(this, new Messenger { IntegerField1 = ((TagValueButton)button).TagId, DecimalField1 = npd.Value });
        }
        #endregion

        #region RULE LIST
        
        #endregion

        #region OK AND CANCEL
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWithCancel?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
