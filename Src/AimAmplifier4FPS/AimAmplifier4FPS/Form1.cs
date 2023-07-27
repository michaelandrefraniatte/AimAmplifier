using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace AimAmplifier4FPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        public static uint CurrentResolution = 0;
        private static bool closed = false;
        private static int height = 300, width;
        private static bool getstate = false;
        private static double ratio;
        private static Bitmap bmp;
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public void AppDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            FormClose();
        }
        public void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            FormClose();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClose();
        }
        private void FormClose()
        {
            closed = true;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(AppDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            this.TopMost = true;
            ratio = Convert.ToDouble(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) / Convert.ToDouble(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            height = Convert.ToInt32(300f * (double)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 768f);
            width = (int)(height * ratio);
            this.Size = new Size(width, height);
            this.ClientSize = new Size(width, height);
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - width - 10, 10);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Size = new Size(width, height);
            this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
            Task.Run(() => Start());
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow)
                return;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void Start()
        {
            while (!closed)
            {
                this.TopMost = true;
                try
                {
                    bmp = new Bitmap((int)((double)Screen.PrimaryScreen.Bounds.Width / 5f), (int)((double)Screen.PrimaryScreen.Bounds.Height / 5f));
                    Graphics graphics = Graphics.FromImage(bmp as Image);
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.Clear(Color.Transparent);
                    graphics.CopyFromScreen((int)((double)Screen.PrimaryScreen.Bounds.Width * 4f / 10f), (int)((double)Screen.PrimaryScreen.Bounds.Height * 4f / 10f), 0, 0, bmp.Size);
                    this.pictureBox1.Image = bmp;
                    graphics.Dispose();
                }
                catch { }
                try
                {
                    valchanged(1, GetAsyncKeyState(Keys.NumPad9));
                    if (wd[1] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - width - 10, 10);
                    }
                    valchanged(2, GetAsyncKeyState(Keys.NumPad8));
                    if (wd[2] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - width / 2, 10);
                    }
                    valchanged(3, GetAsyncKeyState(Keys.NumPad7));
                    if (wd[3] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(10, 10);
                    }
                    valchanged(4, GetAsyncKeyState(Keys.NumPad4));
                    if (wd[4] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height / 2 - height / 2);
                    }
                    valchanged(5, GetAsyncKeyState(Keys.NumPad1));
                    if (wd[5] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(10, Screen.PrimaryScreen.Bounds.Height - height - 10);
                    }
                    valchanged(6, GetAsyncKeyState(Keys.NumPad2));
                    if (wd[6] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - width / 2, Screen.PrimaryScreen.Bounds.Height - height - 10);
                    }
                    valchanged(7, GetAsyncKeyState(Keys.NumPad3));
                    if (wd[7] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - width - 10, Screen.PrimaryScreen.Bounds.Height - height - 10);
                    }
                    valchanged(8, GetAsyncKeyState(Keys.NumPad6));
                    if (wd[8] == 1)
                    {
                        this.pictureBox1.Size = new Size(width, height);
                        this.pictureBox1.Location = new Point((width / 2) - (this.pictureBox1.Width / 2), (height / 2) - (this.pictureBox1.Height / 2));
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - width - 10, (int)(double)Screen.PrimaryScreen.Bounds.Height / 2 - height / 2);
                    }
                    valchanged(9, GetAsyncKeyState(Keys.NumPad5));
                    if (wd[9] == 1)
                    {
                        this.pictureBox1.Size = new Size((int)(Screen.PrimaryScreen.Bounds.Height * ratio), Screen.PrimaryScreen.Bounds.Height);
                        this.pictureBox1.Location = new Point(0, 0);
                        this.Size = new Size((int)(Screen.PrimaryScreen.Bounds.Height * ratio), Screen.PrimaryScreen.Bounds.Height);
                        this.ClientSize = new Size((int)(Screen.PrimaryScreen.Bounds.Height * ratio), Screen.PrimaryScreen.Bounds.Height);
                        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - (int)(Screen.PrimaryScreen.Bounds.Height * ratio / 2), 0);
                    }
                    valchanged(10, GetAsyncKeyState(Keys.Multiply));
                    if (wd[10] == 1)
                    {
                        this.Size = new Size(0, 0);
                        this.ClientSize = new Size(0, 0);
                        this.Location = new Point(0, 0);
                    }
                    valchanged(11, GetAsyncKeyState(Keys.Subtract));
                    if (wd[11] == 1)
                    {
                        if (!getstate)
                        {
                            getstate = true;
                            this.Opacity = 1;
                        }
                        else
                        {
                            getstate = false;
                            this.Opacity = 0.5D;
                        }
                    }
                }
                catch { }
                System.Threading.Thread.Sleep(40);
            }
        }
    }
}
