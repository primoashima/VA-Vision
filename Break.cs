using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeZoneNames;

namespace VAVision
{
    public partial class Break : Form
    {
        public Break()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.Timer timer;
        private string Timexone = System.Configuration.ConfigurationManager.AppSettings["Timezone"];
        private static string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        private static string Version = System.Configuration.ConfigurationManager.AppSettings["CurrentVersion"];
        private static string AppName = System.Configuration.ConfigurationManager.AppSettings["APPName"];
        private int UTCOffset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UTCOffset"]);
        private int minutes = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["minutes"]);
        private int minute = 00;
        private int second = 00;
        private int hour = 00;
        private bool IsClose;
        private DateTime Breakstart = DateTime.Now;
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {               
                second++; 
                if (second > 59)
                {
                    second = 0;
                    minute++;
                }
                if (minute > 59)
                {
                    minute = 0;
                    hour++;
                }
                if (minute % 10 == 0 && second == 0)
                {
                    showBalloon("VA Vision", "ALERT: Your total availed break time is <"+minute+"> minutes");
                }
                timerlbl.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                if (minute > 20)
                {
                    timerlbl.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                if (ex.InnerException.Message != null)
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace + " InnerException message: " + ex.InnerException.Message);
                }
                else
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace + " InnerException: " + ex.InnerException);
                }
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.", AppName + " - " + Version);
                }
                return;
            }
        }

        /// <summary>
        /// Maintain a log file.
        /// </summary> 
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                 var root = @"C:\Users\Public\Documents\Temp\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", root, strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(Version + " " + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.", AppName + " - " + Version);
                }
                return false;
            }
        }

        /// <summary>
        /// Check if internet is working
        /// </summary> 
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public class Timemodel
        {
            public int hours { get; set; }
            public int minutes { get; set; }
            public int seconds { get; set; }

        }

        /// <summary>
        /// Calculate total hour, minutes and seconds
        /// </summary> 
        public Timemodel CalculateTime(string TotalSecond)
        {
            try
            {
                Timemodel obj = new Timemodel();
                var currentprojectsecond = Convert.ToInt32(TotalSecond);

                if (Convert.ToInt32(currentprojectsecond) > 60)
                {
                    obj.minutes = Convert.ToInt32(currentprojectsecond) / 60;
                    obj.seconds = Convert.ToInt32(currentprojectsecond) - (60 * obj.minutes);
                    if (obj.minutes >= 59)
                    {
                        obj.hours = Convert.ToInt32(obj.minutes) / 60;
                        obj.minutes = (obj.minutes - (60 * obj.hours));
                    }
                }
                else
                {
                    obj.seconds = currentprojectsecond;
                }
                return obj;
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.", AppName + " - " + Version);
                }
                return null;
            }
        }

        private void showBalloon(string title, string body)
        {
            try
            {
                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Visible = true;
                notifyIcon.Icon = new System.Drawing.Icon(Application.StartupPath + @"\fav-icon.ico");
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                if (title != null)
                {
                    notifyIcon.BalloonTipTitle = title;
                }

                if (body != null)
                {
                    notifyIcon.BalloonTipText = body;
                }

                notifyIcon.ShowBalloonTip(30000);
                notifyIcon.Dispose();
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.", AppName + " - " + Version);
                }
                return;
            }
        }

        private void Break_Load(object sender, EventArgs e)
        {
            this.Text = AppName + " - " + Version+" - "+"Break Timer";
            timerlbl.Text = Global.BreakTime;
            var breaktime = CalculateTime(Global.BreakSeconds);
            hour = breaktime.hours; minute = breaktime.minutes; second = breaktime.seconds;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000; // 1 second
            timer.Start();
        }

        private async void StopButton_Click(object sender, EventArgs e)
        {            
            if (MessageBox.Show("Are you sure you want to stop the timer?", AppName + " - " + Version, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IsClose = true;
                timer.Stop();
                var res = await SaveScreenshot();
                this.Close();
            }           
        }

        private async void BreakForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsClose)
            {
                if (MessageBox.Show("Are you sure to want to exit? It will also stop the timer.", AppName + " - " + Version, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    timer.Stop();
                    var res = await SaveScreenshot();
                }
                else
                {
                    e.Cancel = true;
                    this.Activate();
                }
            }
        }

        /// <summary>
        /// Save the screenshot
        /// </summary> 
        private async Task<string> SaveScreenshot()
        {
            var result = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseURL);
                const string dataFmt = "{0,-30}{1}";
                const string timeFmt = "{0,-30}{1:MM-dd-yyyy HH:mm}";
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                TimeZone curTimeZone = TimeZone.CurrentTimeZone;
                string tzid = Timexone;                                                 // string tzid = "Mountain Standard Time";         
                string lang = CultureInfo.CurrentCulture.Name;   // example: "en-US"
                var abbreviations = TZNames.GetAbbreviationsForTimeZone(tzid, lang);
                var TimeZoneName = abbreviations.Standard;
                //  var TimeZoneName = "IST";

               var started_time = Global.StartTime;
                var screenshot_current = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
                var project_started_time = started_time.Year.ToString() + "-" + started_time.Month.ToString() + "-" + started_time.Day.ToString() + " " + started_time.TimeOfDay.ToString("hh") + ":" + started_time.ToString("mm") + ":" + started_time.TimeOfDay.ToString("ss");
                var screenshot_current_time = screenshot_current.Year.ToString() + "-" + screenshot_current.Month.ToString() + "-" + screenshot_current.Day.ToString() + " " + screenshot_current.TimeOfDay.ToString("hh") + ":" + screenshot_current.ToString("mm") + ":" + screenshot_current.TimeOfDay.ToString("ss");
                var time_elapse_in_seconds = Convert.ToInt32((DateTime.Now - Breakstart).TotalSeconds);


                var multipartContent = new MultipartFormDataContent();
                using (var mem = new MemoryStream())
                {
                    if (CheckForInternetConnection())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                    }
                    else
                    {
                        MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                    }
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   // multipartContent.Add(byteContent, "img", null);
                    multipartContent.Add(new StringContent(Global.UserId.ToString()), "user_id");
                  //  multipartContent.Add(new StringContent(Global.ProjectId.ToString()), "project_id");
                    multipartContent.Add(new StringContent("BREAK"), "screenshot_type");
                    multipartContent.Add(new StringContent(TimeZoneName.ToString()), "timezone");
                  //  multipartContent.Add(new StringContent(Global.TodoId!=0? Global.TodoId.ToString():"0"), "todo_id");
                    multipartContent.Add(new StringContent(time_elapse_in_seconds.ToString()), "time_elapse_in_seconds");
                    multipartContent.Add(new StringContent(project_started_time.ToString()), "project_started_time");
                    multipartContent.Add(new StringContent(screenshot_current_time), "screenshot_current_time");
                    multipartContent.Add(new StringContent("0"), "mouse_click_count");
                    multipartContent.Add(new StringContent("0"), "keyboard_movement_count");
                    var response = await client.PostAsync(BaseURL + "capture_screen", multipartContent);
                    result = await response.Content.ReadAsStringAsync();
                    WriteLog("log.txt", DateTime.Now + " " + "params sent : user id- " + Global.UserId + " Type: " + "BREAK" + ", project id- " + Global.ProjectId + ", time lapse- " + time_elapse_in_seconds.ToString() + ", Project started-" + project_started_time + ", Timezone-" + TimeZoneName + ", Mouse click- 0, Key press- 0");
                    WriteLog("log.txt", "Savescreenshot Response: " + result);
                }
                return result;
            }

            catch (Exception ex)
            {
                // var lineno = ex.LineNumber();
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                if (ex.InnerException.Message == null)
                {
                    WriteLog("Log.txt", DateTime.Now + " " + ex.Message + ex.StackTrace + "Line: " + trace.GetFrame(0).GetFileLineNumber() + "  Inner Exception: " + ex.InnerException);
                }
                else
                {
                    WriteLog("Log.txt", DateTime.Now + "Line: " + trace.GetFrame(0).GetFileLineNumber() + ex.Message + ex.StackTrace + "Line: " + trace.GetFrame(0).GetFileLineNumber() + "  Inner Exception message: " + ex.InnerException.Message);
                }
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.", AppName + " - " + Version);
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.", AppName + " - " + Version);
                }
                // Get stack trace for the exception with source file information

                return "Exception occured in save screenshot API.";
            }
        }

    }
}
