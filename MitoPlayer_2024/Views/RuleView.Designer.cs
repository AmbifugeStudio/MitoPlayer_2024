namespace MitoPlayer_2024.Views
{
    partial class RuleView
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
            this.grbRules = new System.Windows.Forms.GroupBox();
            this.btnDeleteRule = new System.Windows.Forms.Button();
            this.btnEditRule = new System.Windows.Forms.Button();
            this.btnCreateRule = new System.Windows.Forms.Button();
            this.dgvRuleList = new System.Windows.Forms.DataGridView();
            this.grbTags = new System.Windows.Forms.GroupBox();
            this.pnlTagList = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.grbRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleList)).BeginInit();
            this.grbTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // grbRules
            // 
            this.grbRules.Controls.Add(this.numericUpDown1);
            this.grbRules.Controls.Add(this.btnDeleteRule);
            this.grbRules.Controls.Add(this.btnEditRule);
            this.grbRules.Controls.Add(this.btnCreateRule);
            this.grbRules.Controls.Add(this.dgvRuleList);
            this.grbRules.Location = new System.Drawing.Point(374, 12);
            this.grbRules.Name = "grbRules";
            this.grbRules.Size = new System.Drawing.Size(878, 657);
            this.grbRules.TabIndex = 0;
            this.grbRules.TabStop = false;
            this.grbRules.Text = "Rules";
            // 
            // btnDeleteRule
            // 
            this.btnDeleteRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteRule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteRule.Location = new System.Drawing.Point(784, 77);
            this.btnDeleteRule.Name = "btnDeleteRule";
            this.btnDeleteRule.Size = new System.Drawing.Size(88, 23);
            this.btnDeleteRule.TabIndex = 7;
            this.btnDeleteRule.Text = "Remove";
            this.btnDeleteRule.UseVisualStyleBackColor = true;
            // 
            // btnEditRule
            // 
            this.btnEditRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditRule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditRule.Location = new System.Drawing.Point(784, 48);
            this.btnEditRule.Name = "btnEditRule";
            this.btnEditRule.Size = new System.Drawing.Size(88, 23);
            this.btnEditRule.TabIndex = 7;
            this.btnEditRule.Text = "Edit";
            this.btnEditRule.UseVisualStyleBackColor = true;
            // 
            // btnCreateRule
            // 
            this.btnCreateRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateRule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateRule.Location = new System.Drawing.Point(784, 19);
            this.btnCreateRule.Name = "btnCreateRule";
            this.btnCreateRule.Size = new System.Drawing.Size(88, 23);
            this.btnCreateRule.TabIndex = 7;
            this.btnCreateRule.Text = "Add";
            this.btnCreateRule.UseVisualStyleBackColor = true;
            // 
            // dgvRuleList
            // 
            this.dgvRuleList.AllowDrop = true;
            this.dgvRuleList.AllowUserToAddRows = false;
            this.dgvRuleList.AllowUserToDeleteRows = false;
            this.dgvRuleList.AllowUserToResizeColumns = false;
            this.dgvRuleList.AllowUserToResizeRows = false;
            this.dgvRuleList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRuleList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRuleList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRuleList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRuleList.Location = new System.Drawing.Point(6, 19);
            this.dgvRuleList.MultiSelect = false;
            this.dgvRuleList.Name = "dgvRuleList";
            this.dgvRuleList.ReadOnly = true;
            this.dgvRuleList.RowHeadersVisible = false;
            this.dgvRuleList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRuleList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRuleList.Size = new System.Drawing.Size(772, 620);
            this.dgvRuleList.TabIndex = 4;
            // 
            // grbTags
            // 
            this.grbTags.Controls.Add(this.pnlTagList);
            this.grbTags.Location = new System.Drawing.Point(12, 12);
            this.grbTags.Name = "grbTags";
            this.grbTags.Size = new System.Drawing.Size(356, 657);
            this.grbTags.TabIndex = 1;
            this.grbTags.TabStop = false;
            this.grbTags.Text = "Tags";
            // 
            // pnlTagList
            // 
            this.pnlTagList.AutoScroll = true;
            this.pnlTagList.Location = new System.Drawing.Point(6, 19);
            this.pnlTagList.Name = "pnlTagList";
            this.pnlTagList.Size = new System.Drawing.Size(344, 632);
            this.pnlTagList.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(111, 541);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 8;
            // 
            // RuleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.grbTags);
            this.Controls.Add(this.grbRules);
            this.Name = "RuleView";
            this.Text = "RuleView";
            this.grbRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleList)).EndInit();
            this.grbTags.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbRules;
        private System.Windows.Forms.GroupBox grbTags;
        private System.Windows.Forms.Button btnDeleteRule;
        private System.Windows.Forms.Button btnEditRule;
        private System.Windows.Forms.Button btnCreateRule;
        private System.Windows.Forms.DataGridView dgvRuleList;
        private System.Windows.Forms.Panel pnlTagList;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}