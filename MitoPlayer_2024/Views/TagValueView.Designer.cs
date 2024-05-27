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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteTag = new System.Windows.Forms.Button();
            this.btnEditTag = new System.Windows.Forms.Button();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteTagValue = new System.Windows.Forms.Button();
            this.dgvTagValueList = new System.Windows.Forms.DataGridView();
            this.btnEditTagValue = new System.Windows.Forms.Button();
            this.btnAddTagValue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.dgvTagList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagList.MultiSelect = false;
            this.dgvTagList.Name = "dgvTagList";
            this.dgvTagList.ReadOnly = true;
            this.dgvTagList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagList.Size = new System.Drawing.Size(670, 150);
            this.dgvTagList.TabIndex = 1;
            this.dgvTagList.SelectionChanged += new System.EventHandler(this.dgvTagList_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnDeleteTag);
            this.groupBox1.Controls.Add(this.dgvTagList);
            this.groupBox1.Controls.Add(this.btnEditTag);
            this.groupBox1.Controls.Add(this.btnAddTag);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 177);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tags";
            // 
            // btnDeleteTag
            // 
            this.btnDeleteTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTag.Location = new System.Drawing.Point(682, 77);
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
            this.btnEditTag.Location = new System.Drawing.Point(682, 48);
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
            this.btnAddTag.Location = new System.Drawing.Point(682, 19);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(88, 23);
            this.btnAddTag.TabIndex = 2;
            this.btnAddTag.Text = "Add";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnDeleteTagValue);
            this.groupBox2.Controls.Add(this.dgvTagValueList);
            this.groupBox2.Controls.Add(this.btnEditTagValue);
            this.groupBox2.Controls.Add(this.btnAddTagValue);
            this.groupBox2.Location = new System.Drawing.Point(12, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 176);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tag Value";
            // 
            // btnDeleteTagValue
            // 
            this.btnDeleteTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTagValue.Location = new System.Drawing.Point(682, 77);
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
            this.dgvTagValueList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagValueList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagValueList.MultiSelect = false;
            this.dgvTagValueList.Name = "dgvTagValueList";
            this.dgvTagValueList.ReadOnly = true;
            this.dgvTagValueList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagValueList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagValueList.Size = new System.Drawing.Size(670, 150);
            this.dgvTagValueList.TabIndex = 0;
            this.dgvTagValueList.SelectionChanged += new System.EventHandler(this.dgvTagValueList_SelectionChanged);
            // 
            // btnEditTagValue
            // 
            this.btnEditTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTagValue.Location = new System.Drawing.Point(682, 48);
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
            this.btnAddTagValue.Location = new System.Drawing.Point(682, 19);
            this.btnAddTagValue.Name = "btnAddTagValue";
            this.btnAddTagValue.Size = new System.Drawing.Size(88, 23);
            this.btnAddTagValue.TabIndex = 5;
            this.btnAddTagValue.Text = "Add";
            this.btnAddTagValue.UseVisualStyleBackColor = true;
            this.btnAddTagValue.Click += new System.EventHandler(this.btnAddTagValue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(713, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(632, 377);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // TagValueView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 413);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TagValueView";
            this.Text = "Tag Values ";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagValueList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTagList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDeleteTag;
        private System.Windows.Forms.Button btnEditTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvTagValueList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnDeleteTagValue;
        private System.Windows.Forms.Button btnEditTagValue;
        private System.Windows.Forms.Button btnAddTagValue;
    }
}