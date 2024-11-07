using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MitoPlayer_2024.Helpers
{
    internal class CustomVScrollBar : VScrollBar
    {
        public Color TrackColor { get; set; } = Color.Black;
        public Color ThumbColor { get; set; } = Color.Gray;
        public Color ThumbHoverColor { get; set; } = Color.DarkGray;
        public Color ThumbPressedColor { get; set; } = Color.LightGray;

        private ScrollBarState thumbState = ScrollBarState.Normal;

        public CustomVScrollBar()
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
            int thumbHeight = Math.Max(this.Height / 10, 10); // Minimum thumb height
            int thumbTop = (int)((float)this.Value / this.Maximum * (this.Height - thumbHeight));
            return new Rectangle(0, thumbTop, this.Width, thumbHeight);
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
                int thumbHeight = Math.Max(this.Height / 10, 10);
                int thumbTop = e.Y - (thumbHeight / 2);
                thumbTop = Math.Max(0, Math.Min(thumbTop, this.Height - thumbHeight));
                this.Value = (int)((float)thumbTop / (this.Height - thumbHeight) * this.Maximum);
                this.OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, this.Value));
                Invalidate();
            }
        }
    }
    }

