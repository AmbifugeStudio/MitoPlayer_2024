using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    public class CustomProgressBar : ProgressBar
    {
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ActiveButtonColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");
        public Color ProgressBarColor { get; set; }
        public Color ProgressBarBackgroundColor { get; set; }

        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.ProgressBarColor = this.ActiveButtonColor;
            this.ButtonColor = this.ProgressBarBackgroundColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            // Draw the background
            e.Graphics.FillRectangle(new SolidBrush(ProgressBarBackgroundColor), rec);

            // Calculate the width of the progress bar
            rec.Width = (int)(rec.Width * ((double)this.Value / this.Maximum)) - 4;
            rec.Height = rec.Height - 4;

            // Draw the progress bar
            e.Graphics.FillRectangle(new SolidBrush(ProgressBarColor), 2, 2, rec.Width, rec.Height);
        }
    }
    
}
