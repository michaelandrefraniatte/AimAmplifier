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
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
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
        private const int GWL_STYLE = -16;
        private const uint WS_BORDER = 0x00800000;
        private const uint WS_CAPTION = 0x00C00000;
        private const uint WS_SYSMENU = 0x00080000;
        private const uint WS_MINIMIZEBOX = 0x00020000;
        private const uint WS_MAXIMIZEBOX = 0x00010000;
        private const uint WS_OVERLAPPED = 0x00000000;
        private const uint WS_POPUP = 0x80000000;
        private const uint WS_TABSTOP = 0x00010000;
        private const uint WS_VISIBLE = 0x10000000;
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
            Task.Run(() => StartWindowTitleRemover());
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
        private void StartWindowTitleRemover()
        {
            while (true)
            {
                valchanged(12, GetAsyncKeyState(Keys.PageDown));
                if (wu[12] == 1)
                {
                    int width = Screen.PrimaryScreen.Bounds.Width;
                    int height = Screen.PrimaryScreen.Bounds.Height;
                    IntPtr window = GetForegroundWindow();
                    SetWindowLong(window, GWL_STYLE, WS_SYSMENU);
                    SetWindowPos(window, -2, 0, 0, width, height, 0x0040);
                    DrawMenuBar(window);
                }
                valchanged(13, GetAsyncKeyState(Keys.PageUp));
                if (wu[13] == 1)
                {
                    IntPtr window = GetForegroundWindow();
                    SetWindowLong(window, GWL_STYLE, WS_CAPTION | WS_POPUP | WS_BORDER | WS_SYSMENU | WS_TABSTOP | WS_VISIBLE | WS_OVERLAPPED | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                    DrawMenuBar(window);
                }
                System.Threading.Thread.Sleep(100);
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
