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
    public partial class ColumnVisibilityEditorView : Form, IColumnVisibilityEditorView
    {
        public event EventHandler<ListEventArgs> AddColumn;
        public event EventHandler<ListEventArgs> RemoveColumn;
        public event EventHandler CloseViewWithOk;
        public event EventHandler CloseViewWithCancel;

        public ColumnVisibilityEditorView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(this.listColumns.SelectedItem != null)
                this.AddColumn?.Invoke(this, new ListEventArgs() { StringField1 = this.listColumns.SelectedItem.ToString() });
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.listVisibleColumns.SelectedItem != null)
                this.RemoveColumn?.Invoke(this, new ListEventArgs() { StringField1 = this.listVisibleColumns.SelectedItem.ToString() });
        }

        public void SetAllColumnList(List<string> allColumnList)
        {
            this.listColumns.Items.Clear();
            foreach(String element in allColumnList)
            {
                this.listColumns.Items.Add(element);
            }
        }

        public void SetVisibleColumnList(List<string> visibleColumnList)
        {
            this.listVisibleColumns.Items.Clear();
            foreach (String element in visibleColumnList)
            {
                this.listVisibleColumns.Items.Add(element);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseViewWithOk?.Invoke(this, new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseViewWithCancel?.Invoke(this, new EventArgs());
        }

        private void ColumnVisibilityEditorView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CloseViewWithOk?.Invoke(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseViewWithCancel?.Invoke(this, new EventArgs());
            }
        }
    }
}
