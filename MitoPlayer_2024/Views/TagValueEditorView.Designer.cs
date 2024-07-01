namespace MitoPlayer_2024.Views
{
    partial class TagValueEditorView
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
            this.txtTagValueName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.lblColor = new System.Windows.Forms.Label();
            this.btnColorChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(323, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtTagValueName
            // 
            this.txtTagValueName.Location = new System.Drawing.Point(79, 12);
            this.txtTagValueName.Name = "txtTagValueName";
            this.txtTagValueName.Size = new System.Drawing.Size(238, 20);
            this.txtTagValueName.TabIndex = 7;
            this.txtTagValueName.Text = "New TagValue";
            this.txtTagValueName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTagValueName_KeyDown);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(61, 13);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Enter name";
            // 
            // pnlColor
            // 
            this.pnlColor.Location = new System.Drawing.Point(79, 42);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(157, 23);
            this.pnlColor.TabIndex = 9;
            this.pnlColor.Click += new System.EventHandler(this.pnlColor_Click);
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(12, 47);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(31, 13);
            this.lblColor.TabIndex = 10;
            this.lblColor.Text = "Color";
            // 
            // btnColorChange
            // 
            this.btnColorChange.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnColorChange.Location = new System.Drawing.Point(242, 42);
            this.btnColorChange.Name = "btnColorChange";
            this.btnColorChange.Size = new System.Drawing.Size(75, 23);
            this.btnColorChange.TabIndex = 8;
            this.btnColorChange.Text = "Change";
            this.btnColorChange.UseVisualStyleBackColor = true;
            this.btnColorChange.Click += new System.EventHandler(this.btnColorChange_Click);
            // 
            // TagValueEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 76);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.pnlColor);
            this.Controls.Add(this.btnColorChange);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTagValueName);
            this.Controls.Add(this.lblName);
            this.Name = "TagValueEditorView";
            this.Text = "TagValueEditorView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtTagValueName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnlColor;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Button btnColorChange;
    }
}