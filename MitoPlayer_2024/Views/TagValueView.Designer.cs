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
            bool isDialogEventAttached = false;
            this.dgvTagList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteTag = new System.Windows.Forms.Button();
            this.btnEditTag = new System.Windows.Forms.Button();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDeleteTagValue = new System.Windows.Forms.Button();
            this.dgvTagValueList = new System.Windows.Forms.DataGridView();
            this.btnEditTagValue = new System.Windows.Forms.Button();
            this.btnAddTagValue = new System.Windows.Forms.Button();
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
            this.dgvTagList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTagList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagList.MultiSelect = false;
            this.dgvTagList.Name = "dgvTagList";
            this.dgvTagList.ReadOnly = true;
            this.dgvTagList.RowHeadersVisible = false;
            this.dgvTagList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagList.Size = new System.Drawing.Size(668, 163);
            this.dgvTagList.TabIndex = 1;
            this.dgvTagList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTagList_CellDoubleClick);
            this.dgvTagList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTagList_CellMouseClick);
            this.dgvTagList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTagList_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.btnDeleteTag);
            this.groupBox1.Controls.Add(this.dgvTagList);
            this.groupBox1.Controls.Add(this.btnEditTag);
            this.groupBox1.Controls.Add(this.btnAddTag);
            this.groupBox1.Location = new System.Drawing.Point(18, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(774, 190);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tags";
            // 
            // btnDeleteTag
            // 
            this.btnDeleteTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTag.Location = new System.Drawing.Point(680, 77);
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
            this.btnEditTag.Location = new System.Drawing.Point(680, 48);
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
            this.btnAddTag.Location = new System.Drawing.Point(680, 19);
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
            this.groupBox2.Location = new System.Drawing.Point(18, 237);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(774, 254);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tag Values";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(680, 159);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 23);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnDeleteTagValue
            // 
            this.btnDeleteTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTagValue.Location = new System.Drawing.Point(680, 77);
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
            this.dgvTagValueList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTagValueList.Location = new System.Drawing.Point(6, 19);
            this.dgvTagValueList.MultiSelect = false;
            this.dgvTagValueList.Name = "dgvTagValueList";
            this.dgvTagValueList.ReadOnly = true;
            this.dgvTagValueList.RowHeadersVisible = false;
            this.dgvTagValueList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTagValueList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagValueList.Size = new System.Drawing.Size(668, 220);
            this.dgvTagValueList.TabIndex = 0;
            this.dgvTagValueList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTagValueList_CellDoubleClick);
            this.dgvTagValueList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTagValueList_CellMouseClick);
            this.dgvTagValueList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTagValueList_KeyDown);
            // 
            // btnEditTagValue
            // 
            this.btnEditTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTagValue.Location = new System.Drawing.Point(680, 48);
            this.btnEditTagValue.Name = "btnEditTagValue";
            this.btnEditTagValue.Size = new System.Drawing.Size(88, 23);
            this.btnEditTagValue.TabIndex = 4;
            this.btnEditTagValue.Text = "Edit";
            this.btnEditTagValue.UseVisualStyleBackColor = true;


            if (!isDialogEventAttached)
            {
                this.btnEditTagValue.Click += new System.EventHandler(this.btnEditTagValue_Click);
                isDialogEventAttached = true;
            }
               
            // 
            // btnAddTagValue
            // 
            this.btnAddTagValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTagValue.Location = new System.Drawing.Point(680, 19);
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
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TagValueView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tag Values ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
        private System.Windows.Forms.Button btnDeleteTagValue;
        private System.Windows.Forms.Button btnEditTagValue;
        private System.Windows.Forms.Button btnAddTagValue;
        private System.Windows.Forms.Button btnImport;
    }
}