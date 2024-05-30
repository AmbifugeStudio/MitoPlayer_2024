namespace MitoPlayer_2024.Views
{
    partial class ColumnVisibilityEditorView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.dgvColumnList = new System.Windows.Forms.DataGridView();
            this.btnChangeVisibility = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(158, 370);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(239, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(243, 175);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 2;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(243, 204);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 2;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // dgvColumnList
            // 
            this.dgvColumnList.AllowUserToAddRows = false;
            this.dgvColumnList.AllowUserToDeleteRows = false;
            this.dgvColumnList.AllowUserToResizeColumns = false;
            this.dgvColumnList.AllowUserToResizeRows = false;
            this.dgvColumnList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvColumnList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnList.Location = new System.Drawing.Point(12, 25);
            this.dgvColumnList.MultiSelect = false;
            this.dgvColumnList.Name = "dgvColumnList";
            this.dgvColumnList.ReadOnly = true;
            this.dgvColumnList.RowHeadersVisible = false;
            this.dgvColumnList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvColumnList.Size = new System.Drawing.Size(225, 332);
            this.dgvColumnList.TabIndex = 4;
            this.dgvColumnList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvColumnList_MouseDoubleClick);
            // 
            // btnChangeVisibility
            // 
            this.btnChangeVisibility.Location = new System.Drawing.Point(243, 146);
            this.btnChangeVisibility.Name = "btnChangeVisibility";
            this.btnChangeVisibility.Size = new System.Drawing.Size(75, 23);
            this.btnChangeVisibility.TabIndex = 5;
            this.btnChangeVisibility.Text = "On/Off";
            this.btnChangeVisibility.UseVisualStyleBackColor = true;
            this.btnChangeVisibility.Click += new System.EventHandler(this.btnChangeVisibility_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Columns";
            // 
            // ColumnVisibilityEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 405);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChangeVisibility);
            this.Controls.Add(this.dgvColumnList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Name = "ColumnVisibilityEditorView";
            this.Text = "Column Visibility";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColumnVisibilityEditorView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.DataGridView dgvColumnList;
        private System.Windows.Forms.Button btnChangeVisibility;
        private System.Windows.Forms.Label label1;
    }
}