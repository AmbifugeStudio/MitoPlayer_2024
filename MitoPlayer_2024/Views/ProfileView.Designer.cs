namespace MitoPlayer_2024.Views
{
    partial class ProfileView
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
            this.dgvProfileList = new System.Windows.Forms.DataGridView();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnSetAsActive = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProfileList
            // 
            this.dgvProfileList.AllowUserToAddRows = false;
            this.dgvProfileList.AllowUserToDeleteRows = false;
            this.dgvProfileList.AllowUserToResizeColumns = false;
            this.dgvProfileList.AllowUserToResizeRows = false;
            this.dgvProfileList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileList.Location = new System.Drawing.Point(12, 12);
            this.dgvProfileList.MultiSelect = false;
            this.dgvProfileList.Name = "dgvProfileList";
            this.dgvProfileList.ReadOnly = true;
            this.dgvProfileList.RowHeadersVisible = false;
            this.dgvProfileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProfileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileList.Size = new System.Drawing.Size(240, 150);
            this.dgvProfileList.TabIndex = 0;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCreate.Location = new System.Drawing.Point(258, 12);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "New";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnSetAsActive
            // 
            this.btnSetAsActive.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSetAsActive.Location = new System.Drawing.Point(258, 41);
            this.btnSetAsActive.Name = "btnSetAsActive";
            this.btnSetAsActive.Size = new System.Drawing.Size(100, 23);
            this.btnSetAsActive.TabIndex = 1;
            this.btnSetAsActive.Text = "Set as active";
            this.btnSetAsActive.UseVisualStyleBackColor = true;
            this.btnSetAsActive.Click += new System.EventHandler(this.btnSetAsActive_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnDelete.Location = new System.Drawing.Point(258, 99);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRename.Location = new System.Drawing.Point(258, 70);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(100, 23);
            this.btnRename.TabIndex = 1;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // ProfileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 172);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSetAsActive);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.dgvProfileList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileView";
            this.Text = "Profiles";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProfileView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProfileList;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnSetAsActive;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRename;
    }
}