namespace MitoPlayer_2024.Views
{
    partial class RuleEditorView
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
            this.cbbTag1 = new System.Windows.Forms.ComboBox();
            this.lblTag1 = new System.Windows.Forms.Label();
            this.lblTag2 = new System.Windows.Forms.Label();
            this.nmpdPercent = new System.Windows.Forms.NumericUpDown();
            this.lblPercent = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbbTag2 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmpdPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // cbbTag1
            // 
            this.cbbTag1.FormattingEnabled = true;
            this.cbbTag1.Location = new System.Drawing.Point(62, 9);
            this.cbbTag1.Name = "cbbTag1";
            this.cbbTag1.Size = new System.Drawing.Size(112, 21);
            this.cbbTag1.TabIndex = 0;
            // 
            // lblTag1
            // 
            this.lblTag1.AutoSize = true;
            this.lblTag1.Location = new System.Drawing.Point(12, 12);
            this.lblTag1.Name = "lblTag1";
            this.lblTag1.Size = new System.Drawing.Size(35, 13);
            this.lblTag1.TabIndex = 1;
            this.lblTag1.Text = "Tag 1";
            // 
            // lblTag2
            // 
            this.lblTag2.AutoSize = true;
            this.lblTag2.Location = new System.Drawing.Point(180, 12);
            this.lblTag2.Name = "lblTag2";
            this.lblTag2.Size = new System.Drawing.Size(35, 13);
            this.lblTag2.TabIndex = 2;
            this.lblTag2.Text = "Tag 2";
            // 
            // nmpdPercent
            // 
            this.nmpdPercent.Location = new System.Drawing.Point(62, 36);
            this.nmpdPercent.Name = "nmpdPercent";
            this.nmpdPercent.Size = new System.Drawing.Size(112, 20);
            this.nmpdPercent.TabIndex = 4;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(12, 38);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(44, 13);
            this.lblPercent.TabIndex = 5;
            this.lblPercent.Text = "Percent";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(254, 65);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(173, 65);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbbTag2
            // 
            this.cbbTag2.FormattingEnabled = true;
            this.cbbTag2.Location = new System.Drawing.Point(221, 9);
            this.cbbTag2.Name = "cbbTag2";
            this.cbbTag2.Size = new System.Drawing.Size(112, 21);
            this.cbbTag2.TabIndex = 11;
            // 
            // RuleEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 100);
            this.Controls.Add(this.cbbTag2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.nmpdPercent);
            this.Controls.Add(this.lblTag2);
            this.Controls.Add(this.lblTag1);
            this.Controls.Add(this.cbbTag1);
            this.Name = "RuleEditorView";
            this.Text = "Rules";
            ((System.ComponentModel.ISupportInitialize)(this.nmpdPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbTag1;
        private System.Windows.Forms.Label lblTag1;
        private System.Windows.Forms.Label lblTag2;
        private System.Windows.Forms.NumericUpDown nmpdPercent;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cbbTag2;
    }
}