using MitoPlayer_2024.Helpers;
using System;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class ColumnVisibilityEditorView : Form, IColumnVisibilityEditorView
    {
        public event EventHandler<Messenger> ChangeVisibility;
        public event EventHandler<Messenger> MoveUp;
        public event EventHandler<Messenger> MoveDown;
        public event EventHandler CloseViewWithOk;
        public event EventHandler CloseViewWithCancel;
        private BindingSource columnListBindingSource { get; set; }
        public ColumnVisibilityEditorView()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public void SetColumnListBindingSource(BindingSource columnList, int selectedIndex = 0)
        {
            this.columnListBindingSource = new BindingSource();
            this.columnListBindingSource.DataSource = columnList;
            this.dgvColumnList.DataSource = this.columnListBindingSource.DataSource;
            this.dgvColumnList.Columns["Id"].Visible = false;

            if(selectedIndex > 0)
            {
                this.dgvColumnList.Rows[selectedIndex].Selected = true;
            }
        }

        private void dgvColumnList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.dgvColumnList.SelectedRows != null && this.dgvColumnList.SelectedRows.Count > 0)
                this.ChangeVisibility?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvColumnList.SelectedRows[0].Cells["Id"].Value) });
        }
        private void btnChangeVisibility_Click(object sender, EventArgs e)
        {
            if (this.dgvColumnList.SelectedRows != null && this.dgvColumnList.SelectedRows.Count > 0)
                this.ChangeVisibility?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvColumnList.SelectedRows[0].Cells["Id"].Value) });
        }
       

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (this.dgvColumnList.SelectedRows != null && this.dgvColumnList.SelectedRows.Count > 0)
                this.MoveUp?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvColumnList.SelectedRows[0].Cells["Id"].Value) });
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (this.dgvColumnList.SelectedRows != null && this.dgvColumnList.SelectedRows.Count > 0)
                this.MoveDown?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvColumnList.SelectedRows[0].Cells["Id"].Value) });
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
