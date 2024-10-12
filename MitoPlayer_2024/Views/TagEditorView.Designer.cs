namespace MitoPlayer_2024.Views
{
    partial class TagEditorView
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
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chbHasMultipleValues = new System.Windows.Forms.CheckBox();
            this.rdbtnText = new System.Windows.Forms.RadioButton();
            this.rdbtnField = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(160, 93);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtTagName
            // 
            this.txtTagName.Location = new System.Drawing.Point(115, 13);
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.Size = new System.Drawing.Size(202, 20);
            this.txtTagName.TabIndex = 4;
            this.txtTagName.Text = "New Tag";
            this.txtTagName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTagName_KeyDown);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(61, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Enter name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Tag Value Coloring";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Tag Value Type";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(241, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chbHasMultipleValues
            // 
            this.chbHasMultipleValues.AutoSize = true;
            this.chbHasMultipleValues.Location = new System.Drawing.Point(115, 69);
            this.chbHasMultipleValues.Name = "chbHasMultipleValues";
            this.chbHasMultipleValues.Size = new System.Drawing.Size(119, 17);
            this.chbHasMultipleValues.TabIndex = 12;
            this.chbHasMultipleValues.Text = "Has Multiple Values";
            this.chbHasMultipleValues.UseVisualStyleBackColor = true;
            this.chbHasMultipleValues.CheckedChanged += new System.EventHandler(this.chbHasMultipleValues_CheckedChanged);
            // 
            // rdbtnText
            // 
            this.rdbtnText.AutoSize = true;
            this.rdbtnText.Location = new System.Drawing.Point(115, 41);
            this.rdbtnText.Name = "rdbtnText";
            this.rdbtnText.Size = new System.Drawing.Size(46, 17);
            this.rdbtnText.TabIndex = 16;
            this.rdbtnText.TabStop = true;
            this.rdbtnText.Text = "Text";
            this.rdbtnText.UseVisualStyleBackColor = true;
            this.rdbtnText.CheckedChanged += new System.EventHandler(this.rdbtnText_CheckedChanged);
            // 
            // rdbtnField
            // 
            this.rdbtnField.AutoSize = true;
            this.rdbtnField.Location = new System.Drawing.Point(167, 41);
            this.rdbtnField.Name = "rdbtnField";
            this.rdbtnField.Size = new System.Drawing.Size(47, 17);
            this.rdbtnField.TabIndex = 16;
            this.rdbtnField.TabStop = true;
            this.rdbtnField.Text = "Field";
            this.rdbtnField.UseVisualStyleBackColor = true;
            this.rdbtnField.CheckedChanged += new System.EventHandler(this.rdbtnField_CheckedChanged);
            // 
            // TagEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 126);
            this.Controls.Add(this.rdbtnField);
            this.Controls.Add(this.rdbtnText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chbHasMultipleValues);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTagName);
            this.Controls.Add(this.lblName);
            this.Name = "TagEditorView";
            this.Text = "TagEditorView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtTagName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chbHasMultipleValues;
        private System.Windows.Forms.RadioButton rdbtnText;
        private System.Windows.Forms.RadioButton rdbtnField;
    }
}