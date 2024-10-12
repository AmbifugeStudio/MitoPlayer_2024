namespace MitoPlayer_2024.Views
{
    partial class TagValueView
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
            this.dgvTagList = new System.Windows.Forms.DataGridView();
            this.grbTags = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDeleteTag = new System.Windows.Forms.Button();
            this.btnEditTag = new System.Windows.Forms.Button();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.grbTagValues = new System.Windows.Forms.GroupBox();
            this.btnDeleteTagValue = new System.Windows.Forms.Button();
            this.dgvTagValueList = new System.Windows.Forms.DataGridView();
            this.btnEditTagValue = new System.Windows.Forms.Button();
            this.btnAddTagValue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).BeginInit();
            this.grbTags.SuspendLayout();
            this.grbTagValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagValueList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTagList
            // 
            this.dgvTagList.AllowUserToAddRows = false;
            this.dgvTagList.AllowUserToDeleteRows = false;
            this.dgvTagList.AllowUserToResizeColumns = false;
            this.dgvTagList.AllowUserToResizeRows = false;
            this.dgvTagList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTagList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTagList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTagList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTagList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagList.MultiSelect = false;
            this.dgvTagList.Name = "dgvTagList";
            this.dgvTagList.ReadOnly = true;
            this.dgvTagList.RowHeadersVisible = false;
            this.dgvTagList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagList.Size = new System.Drawing.Size(271, 458);
            this.dgvTagList.TabIndex = 1;
            this.dgvTagList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTagList_CellDoubleClick);
            this.dgvTagList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTagList_CellMouseClick);
            this.dgvTagList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTagList_DataBindingComplete);
            this.dgvTagList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTagList_KeyDown);
            // 
            // grbTags
            // 
            this.grbTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbTags.Controls.Add(this.btnImport);
            this.grbTags.Controls.Add(this.btnDeleteTag);
            this.grbTags.Controls.Add(this.dgvTagList);
            this.grbTags.Controls.Add(this.btnEditTag);
            this.grbTags.Controls.Add(this.btnAddTag);
            this.grbTags.Location = new System.Drawing.Point(12, 12);
            this.grbTags.Name = "grbTags";
            this.grbTags.Size = new System.Drawing.Size(377, 485);
            this.grbTags.TabIndex = 2;
            this.grbTags.TabStop = false;
            this.grbTags.Text = "Tags";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Location = new System.Drawing.Point(283, 135);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 23);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnDeleteTag
            // 
            this.btnDeleteTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTag.Location = new System.Drawing.Point(283, 77);
            this.btnDeleteTag.Name = "btnDeleteTag";
            this.btnDeleteTag.Size = new System.Drawing.Size(88, 23);
            this.btnDeleteTag.TabIndex = 2;
            this.btnDeleteTag.Text = "Remove";
            this.btnDeleteTag.UseVisualStyleBackColor = true;
            this.btnDeleteTag.Click += new System.EventHandler(this.btnDeleteTag_Click);
            // 
            // btnEditTag
            // 
            this.btnEditTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditTag.Location = new System.Drawing.Point(283, 48);
            this.btnEditTag.Name = "btnEditTag";
            this.btnEditTag.Size = new System.Drawing.Size(88, 23);
            this.btnEditTag.TabIndex = 2;
            this.btnEditTag.Text = "Edit";
            this.btnEditTag.UseVisualStyleBackColor = true;
            this.btnEditTag.Click += new System.EventHandler(this.btnEditTag_Click);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTag.Location = new System.Drawing.Point(283, 19);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(88, 23);
            this.btnAddTag.TabIndex = 2;
            this.btnAddTag.Text = "Add";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // grbTagValues
            // 
            this.grbTagValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbTagValues.Controls.Add(this.btnDeleteTagValue);
            this.grbTagValues.Controls.Add(this.dgvTagValueList);
            this.grbTagValues.Controls.Add(this.btnEditTagValue);
            this.grbTagValues.Controls.Add(this.btnAddTagValue);
            this.grbTagValues.Location = new System.Drawing.Point(395, 12);
            this.grbTagValues.Name = "grbTagValues";
            this.grbTagValues.Size = new System.Drawing.Size(403, 485);
            this.grbTagValues.TabIndex = 3;
            this.grbTagValues.TabStop = false;
            this.grbTagValues.Text = "Tag Values";
            // 
            // btnDeleteTagValue
            // 
            this.btnDeleteTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTagValue.Location = new System.Drawing.Point(309, 77);
            this.btnDeleteTagValue.Name = "btnDeleteTagValue";
            this.btnDeleteTagValue.Size = new System.Drawing.Size(88, 23);
            this.btnDeleteTagValue.TabIndex = 3;
            this.btnDeleteTagValue.Text = "Remove";
            this.btnDeleteTagValue.UseVisualStyleBackColor = true;
            this.btnDeleteTagValue.Click += new System.EventHandler(this.btnDeleteTagValue_Click);
            // 
            // dgvTagValueList
            // 
            this.dgvTagValueList.AllowUserToAddRows = false;
            this.dgvTagValueList.AllowUserToDeleteRows = false;
            this.dgvTagValueList.AllowUserToResizeColumns = false;
            this.dgvTagValueList.AllowUserToResizeRows = false;
            this.dgvTagValueList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTagValueList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTagValueList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTagValueList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTagValueList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagValueList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagValueList.MultiSelect = false;
            this.dgvTagValueList.Name = "dgvTagValueList";
            this.dgvTagValueList.ReadOnly = true;
            this.dgvTagValueList.RowHeadersVisible = false;
            this.dgvTagValueList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagValueList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagValueList.Size = new System.Drawing.Size(297, 451);
            this.dgvTagValueList.TabIndex = 0;
            this.dgvTagValueList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTagValueList_CellDoubleClick);
            this.dgvTagValueList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTagValueList_CellMouseClick);
            this.dgvTagValueList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTagValueList_DataBindingComplete);
            this.dgvTagValueList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTagValueList_KeyDown);
            // 
            // btnEditTagValue
            // 
            this.btnEditTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditTagValue.Location = new System.Drawing.Point(309, 48);
            this.btnEditTagValue.Name = "btnEditTagValue";
            this.btnEditTagValue.Size = new System.Drawing.Size(88, 23);
            this.btnEditTagValue.TabIndex = 4;
            this.btnEditTagValue.Text = "Edit";
            this.btnEditTagValue.UseVisualStyleBackColor = true;
            this.btnEditTagValue.Click += new System.EventHandler(this.btnEditTagValue_Click);
            // 
            // btnAddTagValue
            // 
            this.btnAddTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTagValue.Location = new System.Drawing.Point(309, 19);
            this.btnAddTagValue.Name = "btnAddTagValue";
            this.btnAddTagValue.Size = new System.Drawing.Size(88, 23);
            this.btnAddTagValue.TabIndex = 5;
            this.btnAddTagValue.Text = "Add";
            this.btnAddTagValue.UseVisualStyleBackColor = true;
            this.btnAddTagValue.Click += new System.EventHandler(this.btnAddTagValue_Click);
            // 
            // TagValueView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 509);
            this.Controls.Add(this.grbTagValues);
            this.Controls.Add(this.grbTags);
            this.Name = "TagValueView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Values ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).EndInit();
            this.grbTags.ResumeLayout(false);
            this.grbTagValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagValueList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTagList;
        private System.Windows.Forms.GroupBox grbTags;
        private System.Windows.Forms.Button btnDeleteTag;
        private System.Windows.Forms.Button btnEditTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.GroupBox grbTagValues;
        private System.Windows.Forms.DataGridView dgvTagValueList;
        private System.Windows.Forms.Button btnDeleteTagValue;
        private System.Windows.Forms.Button btnEditTagValue;
        private System.Windows.Forms.Button btnAddTagValue;
        private System.Windows.Forms.Button btnImport;
    }
}