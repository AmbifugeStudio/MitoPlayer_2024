namespace MitoPlayer_2024.Views
{
    partial class TrainingDataGenerator
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
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTrackList
            // 
            this.dgvTrackList.AllowUserToAddRows = false;
            this.dgvTrackList.AllowUserToDeleteRows = false;
            this.dgvTrackList.AllowUserToResizeColumns = false;
            this.dgvTrackList.AllowUserToResizeRows = false;
            this.dgvTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrackList.Location = new System.Drawing.Point(15, 25);
            this.dgvTrackList.MultiSelect = false;
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(474, 306);
            this.dgvTrackList.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tracklist";
            // 
            // TrainingDataGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvTrackList);
            this.Controls.Add(this.label2);
            this.Name = "TrainingDataGenerator";
            this.Text = "TrainingDataGenerator";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.Label label2;
    }
}