using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace MitoPlayer_2024.Views
{
    public partial class LiveStreamAnimationView : Form, ILiveStreamAnimationView
    {
        private Timer timer;

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color ActiveColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");
        Color StreamColor = System.Drawing.ColorTranslator.FromHtml("#FAE6B0");

        //INIT
        private Random random = new Random();
        private String imageDirectoryPath;
        private List<String> imagePathList;
        private string currentImagePath;

        //TIMERS
        private Timer imageChangeTimer;
        private Timer doubleClickTimer;
        private Timer trackChangeCheckTimer;

        //DRAG
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        //RESIZE
        private bool resizing = false;
        private Point resizeCursorPoint;
        private Size resizeOriginalSize;
        private float aspectRatio;

        //MAXIMIZE
        private Size originalSize;
        private Point originalLocation;
        private bool isMaximized = false;


        public event EventHandler TrackChangeCheckEvent;
        public LiveStreamAnimationView()
        {
            InitializeComponent();



            this.timer = new Timer();

            this.random = new Random();

            this.doubleClickTimer = new Timer();
            this.imageChangeTimer = new Timer();
            this.trackChangeCheckTimer = new Timer();

            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            

            
            

            this.lblTitle.BackColor = this.BackgroundColor;
            this.lblTitle.ForeColor = this.StreamColor;
            this.pnlTitleBackground.BackColor = this.BackColor;
            this.pnlTitleBackgroundBorder.BackColor = this.StreamColor;

            this.lblLiveStream.BackColor = this.BackgroundColor;
            this.lblLiveStream.ForeColor = this.StreamColor;
            this.pnlLiveStreamBackground.BackColor = this.BackColor;
            this.pnlLiveStreamBackgroundBorder.BackColor = this.StreamColor;

            this.pcbCoverBorder.BackColor = this.StreamColor;

            // this.pnlImage.BackColor = this.BackgroundColor;
            //this.pnlLiveStreamBackground.BackColor = this.BackgroundColor;




            //this.BackColor = Color.White;
            // this.TransparencyKey = Color.White;
            /*
            pcbImage.Image = Image.FromFile(@"C:\\Users\\Mitoklin\\Downloads\\AI\\Wood in autumn\\mitoklin_in_the_wood_between_mountains_fall_by_Moebius_and_Syd__48c29159-31b1-4df2-8d14-3c7512422242.png");
            pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;

            doubleClickTimer.Interval = SystemInformation.DoubleClickTime;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
            aspectRatio = (float)this.Width / this.Height;
            LoadImages(@"C:\\Users\\Mitoklin\\Downloads\\AI\\Wood in autumn\\");

            imageChangeTimer.Interval = 20000; // 20 másodperc
            imageChangeTimer.Tick += new EventHandler(ChangeImage);
            imageChangeTimer.Start();

            timer.Interval = 1000; // 1 másodperc
            timer.Tick += new EventHandler(timer_Tick);
            */
        }
        public void InitializeView(Messenger msg)
        {
            String trackInfo = String.Empty;
            if(msg!= null)
            {
                if(msg.Image != null)
                {
                    this.pcbCover.Image = msg.Image;
                }
                if (!String.IsNullOrEmpty(msg.StringField2) && !String.IsNullOrEmpty(msg.StringField3))
                {
                    trackInfo = msg.StringField2 + " - " + msg.StringField3;
                    
                }
                else if (!String.IsNullOrEmpty(msg.StringField2) && String.IsNullOrEmpty(msg.StringField3))
                {
                    trackInfo = msg.StringField2;
                }
                else if (String.IsNullOrEmpty(msg.StringField2) && !String.IsNullOrEmpty(msg.StringField3))
                {
                    trackInfo = msg.StringField3;
                }

                this.lblTitle.Text = trackInfo;
            }
        }
        public void FirstInitialization(Messenger messenger)
        {
            if (messenger.BooleanField1)
            {
                this.imageDirectoryPath = messenger.StringField1;
                this.currentImagePath = messenger.StringField2;

                //FIRST IMAGE
                if (!String.IsNullOrEmpty(this.currentImagePath))
                {
                    this.pcbImage.Image = Image.FromFile(this.currentImagePath);
                }
                else
                {
                    if (messenger.StringList != null && messenger.StringList.Count > 0)
                    {
                        this.imagePathList = messenger.StringList;

                        int randomIndex = random.Next(messenger.StringList.Count);
                        string firstImagePath = messenger.StringList[randomIndex];
                        this.currentImagePath = firstImagePath;
                        this.pcbImage.Image = Image.FromFile(this.currentImagePath);
                    }
                }

                this.pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                this.aspectRatio = (float)this.Width / this.Height;

                this.trackChangeCheckTimer.Interval = 1000;
                this.trackChangeCheckTimer.Tick += new EventHandler(this.trackChangeCheck_Tick);
                this.trackChangeCheckTimer.Start();

                this.imageChangeTimer.Interval = 20000; // 20 sec
                this.imageChangeTimer.Tick += new EventHandler(this.changeImage_Tick);
                this.imageChangeTimer.Start();

                this.doubleClickTimer.Interval = SystemInformation.DoubleClickTime;
                this.doubleClickTimer.Tick += new EventHandler(this.doubleClickTimer_Tick);

               /* this.timer.Interval = 1000; // 1 másodperc
                this.timer.Tick += new EventHandler(timer_Tick);
                this.timer.Start();*/

                messenger.BooleanField1 = false;

                this.Show();
            }
        }
        private void trackChangeCheck_Tick(object sender, EventArgs e)
        {
            this.TrackChangeCheckEvent?.Invoke(this, EventArgs.Empty);
        }
        private void changeImage_Tick(object sender, EventArgs e)
        {
            if (this.imagePathList != null && this.imagePathList.Count > 0)
            {
                string newImagePath;
                do
                {
                    newImagePath = this.imagePathList[random.Next(this.imagePathList.Count)];
                } while (newImagePath == this.currentImagePath);

                this.currentImagePath = newImagePath;
                this.pcbImage.Image = Image.FromFile(this.currentImagePath);
            }
        }
        private void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            this.doubleClickTimer.Stop();
            this.pcbImage_DoubleClick(sender, EventArgs.Empty);
        }
        private void pcbImage_DoubleClick(object sender, EventArgs e)
        {
            if (this.isMaximized)
            {
                this.Size = this.originalSize;
                this.Location = this.originalLocation;
                this.isMaximized = false;
            }
            else
            {
                this.originalSize = this.Size;
                this.originalLocation = this.Location;

                this.Size = Screen.PrimaryScreen.Bounds.Size;
                this.Location = new Point(0, 0);
                this.isMaximized = true;
            }
        }

        private void pcbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                this.doubleClickTimer.Start();
            }
            else if (e.Button == MouseButtons.Left && e.X >= this.ClientSize.Width - 10 && e.Y >= this.ClientSize.Height - 10)
            {
                this.resizing = true;
                this.resizeCursorPoint = Cursor.Position;
                this.resizeOriginalSize = this.Size;
            }
            else if (e.Button == MouseButtons.Left && e.X >= this.ClientSize.Width - 10 && e.Y <= 10)
            {
                this.Close();
            }
            else
            {
                this.dragging = true;
                this.dragCursorPoint = Cursor.Position;
                this.dragFormPoint = this.Location;
            }
        }
        private void pcbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.dragCursorPoint));
                this.Location = Point.Add(this.dragFormPoint, new Size(dif));
            }
            else if (this.resizing)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.resizeCursorPoint));
                int newWidth = this.resizeOriginalSize.Width + dif.X;
                int newHeight = (int)(newWidth / this.aspectRatio);
                this.Size = new Size(newWidth, newHeight);
            }
            else
            {
                if (e.X >= this.ClientSize.Width - 10 && e.Y >= this.ClientSize.Height - 10)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
        private void pcbImage_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
            this.resizing = false;
        }

       
        
        private void timer_Tick(object sender, EventArgs e)
        {
            int x = this.random.Next(-50, 60); // -5 és 5 közötti véletlenszerű elmozdulás
            int y = this.random.Next(-50, 60);
            pcbImage.Location = new Point(pcbImage.Location.X + 100, pcbImage.Location.Y +50);
        }

        private void lblArtist_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
