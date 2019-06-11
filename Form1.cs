using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Winwink.DesktopWallPaper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitLocation();
            GetBingWallPaper();
        }

        private void InitLocation()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Bottom - 300;
            int x = screenWidth - this.Width;
            int y = screenHeight - this.Height;
            this.Location = new Point(x, y);
        }

        private void GetBingWallPaper()
        {
            try
            {
                BingPicture bing = new BingPicture();
                bing.GetPictureOfToday();

                Wallpaper.Set(new Uri(bing.PictureUrl), Wallpaper.Style.Fill, bing.PictureName);
                ShowMessage("Done", GetHowlongtoClose());
            }
            catch (Exception ex)
            {
                LblMessage.Text = "Error:\r\n" + ex.Message+"\r\n"+ex.StackTrace;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        System.Timers.Timer timer = new System.Timers.Timer();
        private int _timeout = 0;
        public string Message { get; set; }
        private void ShowMessage(string message, int timeout)
        {
            _timeout = timeout/1000;
            Message = message;
            LblMessage.Text = Message + string.Format(", will close in ({0})s",_timeout);

            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_timeout > 0 )
            {
                _timeout--;
                var message = Message + string.Format(", will close in ({0})s", _timeout);
                this.Invoke(new Action(() => LblMessage.Text = message ));
            }
            else
            {
                timer.Stop();
                this.Invoke(new Action(() => Environment.Exit(0)));
            }
        }

        private int GetHowlongtoClose()
        {
            var str = System.Configuration.ConfigurationManager.AppSettings["HowLongToClose"];
            if (!int.TryParse(str, out var value))
            {
                value = 2;
            }
            return value * 1000;
        }
    }
}
