using MitoPlayer_2024.Helpers;
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
    public partial class TagValueView : Form, ITagValueView
    {
        private BindingSource tagListBindingSource { get; set; }
        private BindingSource tagValueListBindingSource { get; set; }

        public event EventHandler CreateTag;
        public event EventHandler<ListEventArgs> EditTag;
        public event EventHandler<ListEventArgs> DeleteTag;
        public event EventHandler CreateTagValue;
        public event EventHandler<ListEventArgs> EditTagValue;
        public event EventHandler<ListEventArgs> DeleteTagValue;
        public event EventHandler CloseWithOk;
        public event EventHandler CloseWithCancel;
        public event EventHandler<ListEventArgs> SetCurrentTagId;
        public event EventHandler<ListEventArgs> SetCurrentTagValueId;

        public TagValueView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public void SetTagListBindingSource(BindingSource tagList)
        {
            this.tagListBindingSource = new BindingSource();
            this.tagListBindingSource.DataSource = tagList;
            this.dgvTagList.DataSource = this.tagListBindingSource.DataSource;
            this.dgvTagList.Columns["Id"].Visible = false;
        }
        public void SetTagValueListBindingSource(BindingSource tagValueList)
        {
            this.tagValueListBindingSource = new BindingSource();
            this.tagValueListBindingSource.DataSource = tagValueList;
            this.dgvTagValueList.DataSource = this.tagValueListBindingSource.DataSource;
            this.dgvTagValueList.Columns["Id"].Visible = false;
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            this.CreateTag?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditTag_Click(object sender, EventArgs e)
        {
            if(this.dgvTagList.SelectedRows.Count > 0)
            {
                this.EditTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Cells["Id"]) });
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.DeleteTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Cells["Id"]) });
            }
        }

        private void btnAddTagValue_Click(object sender, EventArgs e)
        {
            this.CreateTagValue?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.EditTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Cells["Id"]) });
            }
        }

        private void btnDeleteTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.DeleteTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Cells["Id"]) });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWithCancel?.Invoke(this, EventArgs.Empty);
        }

        private void dgvTagList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagId?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Cells["Id"].Value) });
            }
        }

        private void dgvTagValueList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagValueId?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Cells["Id"].Value) });
            }
        }
    }
}
