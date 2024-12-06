

namespace MitoPlayer_2024.Views
{
    partial class LiveStreamAnimationView
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
            this.pcbImage = new System.Windows.Forms.PictureBox();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pnlLiveStreamBackgroundBorder = new System.Windows.Forms.Panel();
            this.pnlLiveStreamBackground = new System.Windows.Forms.Panel();
            this.pcbRedDot = new System.Windows.Forms.PictureBox();
            this.lblLiveStream = new System.Windows.Forms.Label();
            this.pcbCover = new System.Windows.Forms.PictureBox();
            this.pcbCoverBorder = new System.Windows.Forms.PictureBox();
            this.pnlTitleBackgroundBorder = new System.Windows.Forms.Panel();
            this.pnlTitleBackground = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).BeginInit();
            this.pnlImage.SuspendLayout();
            this.pnlLiveStreamBackgroundBorder.SuspendLayout();
            this.pnlLiveStreamBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRedDot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCoverBorder)).BeginInit();
            this.pnlTitleBackgroundBorder.SuspendLayout();
            this.pnlTitleBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcbImage
            // 
            this.pcbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbImage.Location = new System.Drawing.Point(0, 0);
            this.pcbImage.Name = "pcbImage";
            this.pcbImage.Size = new System.Drawing.Size(1024, 768);
            this.pcbImage.TabIndex = 0;
            this.pcbImage.TabStop = false;
            this.pcbImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseDown);
            this.pcbImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseMove);
            this.pcbImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseUp);
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.pnlLiveStreamBackgroundBorder);
            this.pnlImage.Controls.Add(this.pcbCover);
            this.pnlImage.Controls.Add(this.pcbCoverBorder);
            this.pnlImage.Controls.Add(this.pnlTitleBackgroundBorder);
            this.pnlImage.Controls.Add(this.pcbImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(1024, 768);
            this.pnlImage.TabIndex = 1;
            // 
            // pnlLiveStreamBackgroundBorder
            // 
            this.pnlLiveStreamBackgroundBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLiveStreamBackgroundBorder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlLiveStreamBackgroundBorder.Controls.Add(this.pnlLiveStreamBackground);
            this.pnlLiveStreamBackgroundBorder.Location = new System.Drawing.Point(12, 676);
            this.pnlLiveStreamBackgroundBorder.Name = "pnlLiveStreamBackgroundBorder";
            this.pnlLiveStreamBackgroundBorder.Size = new System.Drawing.Size(335, 80);
            this.pnlLiveStreamBackgroundBorder.TabIndex = 5;
            // 
            // pnlLiveStreamBackground
            // 
            this.pnlLiveStreamBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLiveStreamBackground.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlLiveStreamBackground.Controls.Add(this.pcbRedDot);
            this.pnlLiveStreamBackground.Controls.Add(this.lblLiveStream);
            this.pnlLiveStreamBackground.Location = new System.Drawing.Point(4, 4);
            this.pnlLiveStreamBackground.Name = "pnlLiveStreamBackground";
            this.pnlLiveStreamBackground.Size = new System.Drawing.Size(327, 72);
            this.pnlLiveStreamBackground.TabIndex = 4;
            // 
            // pcbRedDot
            // 
            this.pcbRedDot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pcbRedDot.BackColor = System.Drawing.Color.Transparent;
            this.pcbRedDot.Image = global::MitoPlayer_2024.Properties.Resources.DotRed_20_20;
            this.pcbRedDot.Location = new System.Drawing.Point(11, 28);
            this.pcbRedDot.Name = "pcbRedDot";
            this.pcbRedDot.Size = new System.Drawing.Size(15, 15);
            this.pcbRedDot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbRedDot.TabIndex = 2;
            this.pcbRedDot.TabStop = false;
            // 
            // lblLiveStream
            // 
            this.lblLiveStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLiveStream.AutoSize = true;
            this.lblLiveStream.BackColor = System.Drawing.SystemColors.Control;
            this.lblLiveStream.Font = new System.Drawing.Font("Conthrax Sb", 15F, System.Drawing.FontStyle.Bold);
            this.lblLiveStream.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLiveStream.Location = new System.Drawing.Point(7, 24);
            this.lblLiveStream.Name = "lblLiveStream";
            this.lblLiveStream.Size = new System.Drawing.Size(296, 24);
            this.lblLiveStream.TabIndex = 1;
            this.lblLiveStream.Text = "   Mitoklin LIVE Stream";
            // 
            // pcbCover
            // 
            this.pcbCover.Location = new System.Drawing.Point(38, 33);
            this.pcbCover.Name = "pcbCover";
            this.pcbCover.Size = new System.Drawing.Size(90, 90);
            this.pcbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbCover.TabIndex = 3;
            this.pcbCover.TabStop = false;
            // 
            // pcbCoverBorder
            // 
            this.pcbCoverBorder.Location = new System.Drawing.Point(34, 29);
            this.pcbCoverBorder.Name = "pcbCoverBorder";
            this.pcbCoverBorder.Size = new System.Drawing.Size(98, 98);
            this.pcbCoverBorder.TabIndex = 3;
            this.pcbCoverBorder.TabStop = false;
            // 
            // pnlTitleBackgroundBorder
            // 
            this.pnlTitleBackgroundBorder.AutoSize = true;
            this.pnlTitleBackgroundBorder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTitleBackgroundBorder.Controls.Add(this.pnlTitleBackground);
            this.pnlTitleBackgroundBorder.Location = new System.Drawing.Point(127, 29);
            this.pnlTitleBackgroundBorder.Name = "pnlTitleBackgroundBorder";
            this.pnlTitleBackgroundBorder.Size = new System.Drawing.Size(819, 98);
            this.pnlTitleBackgroundBorder.TabIndex = 4;
            // 
            // pnlTitleBackground
            // 
            this.pnlTitleBackground.AutoSize = true;
            this.pnlTitleBackground.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTitleBackground.Controls.Add(this.lblTitle);
            this.pnlTitleBackground.Location = new System.Drawing.Point(4, 4);
            this.pnlTitleBackground.Name = "pnlTitleBackground";
            this.pnlTitleBackground.Size = new System.Drawing.Size(811, 90);
            this.pnlTitleBackground.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblTitle.Font = new System.Drawing.Font("Helvetica CondensedBlack", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(3, 26);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 36);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Click += new System.EventHandler(this.lblArtist_Click);
            // 
            // LiveStreamAnimationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.pnlImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LiveStreamAnimationView";
            this.ShowIcon = false;
            this.Text = "LiveStreamAnimation";
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).EndInit();
            this.pnlImage.ResumeLayout(false);
            this.pnlImage.PerformLayout();
            this.pnlLiveStreamBackgroundBorder.ResumeLayout(false);
            this.pnlLiveStreamBackground.ResumeLayout(false);
            this.pnlLiveStreamBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRedDot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCoverBorder)).EndInit();
            this.pnlTitleBackgroundBorder.ResumeLayout(false);
            this.pnlTitleBackgroundBorder.PerformLayout();
            this.pnlTitleBackground.ResumeLayout(false);
            this.pnlTitleBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcbImage;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.PictureBox pcbRedDot;
        private System.Windows.Forms.Label lblLiveStream;
        private System.Windows.Forms.PictureBox pcbCover;
        private System.Windows.Forms.PictureBox pcbCoverBorder;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTitleBackground;
        private System.Windows.Forms.Panel pnlLiveStreamBackground;
        private System.Windows.Forms.Panel pnlTitleBackgroundBorder;
        private System.Windows.Forms.Panel pnlLiveStreamBackgroundBorder;
    }
}