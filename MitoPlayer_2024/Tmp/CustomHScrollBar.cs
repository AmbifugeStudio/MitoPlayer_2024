using MathNet.Numerics;
using MitoPlayer_2024.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MitoPlayer_2024.Helpers
{
    public class CustomHScrollBar : HScrollBar
    {

        public Color TrackColor { get; set; } = Color.Black;
        public Color ThumbColor { get; set; } = Color.Gray;
        public Color ThumbHoverColor { get; set; } = Color.DarkGray;
        public Color ThumbPressedColor { get; set; } = Color.LightGray;

        private ScrollBarState thumbState = ScrollBarState.Normal;

        public CustomHScrollBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Draw the scrollbar track
            using (Brush brush = new SolidBrush(TrackColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Draw the thumb
            Color thumbColor = ThumbColor;
            if (thumbState == ScrollBarState.Hot)
            {
                thumbColor = ThumbHoverColor;
            }
            else if (thumbState == ScrollBarState.Pressed)
            {
                thumbColor = ThumbPressedColor;
            }

            using (Brush brush = new SolidBrush(thumbColor))
            {
                g.FillRectangle(brush, GetThumbRectangle());
            }
        }

        private Rectangle GetThumbRectangle()
        {
            int thumbWidth = Math.Max(this.Width / 10, 10); // Minimum thumb width
            int thumbLeft = (int)((float)this.Value / this.Maximum * (this.Width - thumbWidth));
            return new Rectangle(thumbLeft, 0, thumbWidth, this.Height);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            thumbState = ScrollBarState.Hot;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            thumbState = ScrollBarState.Normal;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                thumbState = ScrollBarState.Pressed;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            thumbState = ScrollBarState.Normal;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.Capture && e.Button == MouseButtons.Left)
            {
                int thumbWidth = Math.Max(this.Width / 10, 10);
                int thumbLeft = e.X - (thumbWidth / 2);
                thumbLeft = Math.Max(0, Math.Min(thumbLeft, this.Width - thumbWidth));
                this.Value = (int)((float)thumbLeft / (this.Width - thumbWidth) * this.Maximum);
                this.OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, this.Value));
                Invalidate();
            }
        }

    }

}