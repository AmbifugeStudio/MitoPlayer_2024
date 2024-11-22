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
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).BeginInit();
            this.pnlImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcbImage
            // 
            this.pcbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbImage.Location = new System.Drawing.Point(0, 0);
            this.pcbImage.Name = "pcbImage";
            this.pcbImage.Size = new System.Drawing.Size(800, 450);
            this.pcbImage.TabIndex = 0;
            this.pcbImage.TabStop = false;
            this.pcbImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseDown);
            this.pcbImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseMove);
            this.pcbImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbImage_MouseUp);
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.pcbImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(800, 450);
            this.pnlImage.TabIndex = 1;
            // 
            // LiveStreamAnimationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.pnlImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LiveStreamAnimationView";
            this.ShowIcon = false;
            this.Text = "LiveStreamAnimation";
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).EndInit();
            this.pnlImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcbImage;
        private System.Windows.Forms.Panel pnlImage;
    }
}