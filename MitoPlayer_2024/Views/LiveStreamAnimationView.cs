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
        private Timer timer = new Timer();
        private Random rand = new Random();


        public LiveStreamAnimationView()
        {
            InitializeComponent();

            pcbImage.Image = Image.FromFile(@"C:\\Users\\Mitoklin\\Downloads\\AI\\Wood in autumn\\mitoklin_in_the_wood_between_mountains_fall_by_Moebius_and_Syd__48c29159-31b1-4df2-8d14-3c7512422242.png");
            pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;

            // Időzítő beállítása
            doubleClickTimer.Interval = SystemInformation.DoubleClickTime;
            doubleClickTimer.Tick += new EventHandler(doubleClickTimer_Tick);
            // Arány kiszámítása
            aspectRatio = (float)this.Width / this.Height;
            // Képek betöltése
            LoadImages(@"C:\\Users\\Mitoklin\\Downloads\\AI\\Wood in autumn\\");

            // Képváltó időzítő beállítása
            imageChangeTimer.Interval = 20000; // 20 másodperc
            imageChangeTimer.Tick += new EventHandler(ChangeImage);
            imageChangeTimer.Start();

            timer.Interval = 1000; // 1 másodperc
            timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }
        private void LoadImages(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                imagePaths = Directory.GetFiles(folderPath, "*.jpg").ToList();
                imagePaths.AddRange(Directory.GetFiles(folderPath, "*.png"));
                imagePaths.AddRange(Directory.GetFiles(folderPath, "*.bmp"));
            }
        }
        private void ChangeImage(object sender, EventArgs e)
        {
            if (imagePaths.Count > 0)
            {
                string newImagePath;
                do
                {
                    newImagePath = imagePaths[random.Next(imagePaths.Count)];
                } while (newImagePath == currentImagePath);

                currentImagePath = newImagePath;
                pcbImage.Image = Image.FromFile(currentImagePath);
            }
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private Size originalSize;
        private Point originalLocation;
        private bool isMaximized = false;
        private Timer doubleClickTimer = new Timer();
        private bool resizing = false;
        private Point resizeCursorPoint;
        private Size resizeOriginalSize;
        private float aspectRatio;

        private Timer imageChangeTimer = new Timer();
        private List<string> imagePaths = new List<string>();
        private Random random = new Random();
        private string currentImagePath;

        private void pcbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                doubleClickTimer.Start();
            }
            else if (e.Button == MouseButtons.Left && e.X >= this.ClientSize.Width - 10 && e.Y >= this.ClientSize.Height - 10)
            {
                resizing = true;
                resizeCursorPoint = Cursor.Position;
                resizeOriginalSize = this.Size;
            }
            else if (e.Button == MouseButtons.Left && e.X >= this.ClientSize.Width - 10 && e.Y <= 10)
            {
                this.Close(); // Bezárja a formot, ha a jobb felső sarokba kattintasz
            }
            else
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }
        private void pcbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
            else if (resizing)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(resizeCursorPoint));
                int newWidth = resizeOriginalSize.Width + dif.X;
                int newHeight = (int)(newWidth / aspectRatio);
                this.Size = new Size(newWidth, newHeight);
            }
            else
            {
                // Módosítsd az egérkurzort, ha a jobb alsó sarokban van
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
            dragging = false;
            resizing = false;
        }
        private void pcbImage_DoubleClick(object sender, EventArgs e)
        {
            if (isMaximized)
            {
                // Visszaállítás az eredeti méretre és helyzetre
                this.Size = originalSize;
                this.Location = originalLocation;
                isMaximized = false;
            }
            else
            {
                // Eredeti méret és helyzet mentése
                originalSize = this.Size;
                originalLocation = this.Location;

                // Maximalizálás
                this.Size = Screen.PrimaryScreen.Bounds.Size;
                this.Location = new Point(0, 0);
                isMaximized = true;
            }
        }
        private void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            doubleClickTimer.Stop();
            pcbImage_DoubleClick(sender, EventArgs.Empty);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            int x = rand.Next(-5, 6); // -5 és 5 közötti véletlenszerű elmozdulás
            int y = rand.Next(-5, 6);
            pcbImage.Location = new Point(pcbImage.Location.X + x, pcbImage.Location.Y + y);
        }
    }
}
