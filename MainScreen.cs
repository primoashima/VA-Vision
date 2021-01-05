using Newtonsoft.Json;
using ServiceManagerCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TimeZoneNames;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VAVision;

namespace VAVision
{


    public partial class Form1 : Form
    {

        private delegate IntPtr LowLevelKeyBoardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        public bool clicked;
        public bool logedout;
        private bool started;
        private string timerstartedfor;
        private static int KeyboardMovements = 0;
        private static int MouseMoves = 0;
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static int WH_MOUSE_LL = 14;
        private static int WM_LBUTTONDOWN = 0x0201;
        private static int WM_LBUTTONUP = 0x0202;
        private static int WM_MOUSEWHEEL = 0x020A;
        private static int WM_RBUTTONDOWN = 0x0204;
        private static int WM_RBUTTONUP = 0x0205;
        private static IntPtr hook = IntPtr.Zero;
        private static IntPtr hook1 = IntPtr.Zero;
        private static LowLevelKeyBoardProc llkProcedure = HookCallback;
        private static LowLevelMouseProc llkProcedure1 = HookCallback;
        // private int UTCOffset = -5;
        // private int minutes = 0;
        private string Timexone = System.Configuration.ConfigurationManager.AppSettings["Timezone"];
        private static int UTCOffset = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UTCOffset"]);
        private static int minutes = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["minutes"]);
        DateTime StartTime = DateTime.Now;
        DateTime CurrentTime = DateTime.UtcNow;
        private static string PreviousVersion = System.Configuration.ConfigurationManager.AppSettings["PreviousVersion"];
        private static string Version = System.Configuration.ConfigurationManager.AppSettings["CurrentVersion"];
        private static string AppName = System.Configuration.ConfigurationManager.AppSettings["APPName"];
        private static string FontType = System.Configuration.ConfigurationManager.AppSettings["Font"];
        private static float FontSize = float.Parse(System.Configuration.ConfigurationManager.AppSettings["Fontsize"]);
        string sent = "";
        // DateTime curr = DateTime.Now;
        DateTime PreviousTime = DateTime.Now;
        DateTime NextScreenshot = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
        DateTime currentscreenshot = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
        DateTime nextcreenshottime = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
        string nxtlabeltxt = "";        
        int nxttime = 0;
        int pretime = 0;
        private static string height;
        private static string width;
        private static string xcoordinate;
        private static string ycoordinate;
        private static string size;
        private static string ProjectName;
        private static int Projectid;
        private static int todoId;
        private static string FileSent = "";
       private static string  BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        public static string todoName { get; set; }
        private static User user;
        private static ProjectRecord ProjectTrack { get; set; }
        private static int Totalworked { get; set; }
        private BindingSource bindingSource2 = new BindingSource();



        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyBoardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        private System.Windows.Forms.Timer timer1;
        private int minute = 00;
        private int second = 00;
        private int hour = 00;
    // private string Timexone = "Eastern Standard Time";
     
        private int Totalminute = 00;
        private int Totalsecond = 00;
        private int Totalhour = 00;


        public Form1()
        {
            InitializeComponent();
        }

        bool isRunning = true;
        private Mutex checking = new Mutex(false);
        private AutoResetEvent are = new AutoResetEvent(false);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode != 0)
                {
                    KeyboardMovements++;
                }
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode != 0)
                {
                    MouseMoves++;

                }
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_RBUTTONDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode != 0)
                {
                    MouseMoves++;

                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyBoardProc proc)
        {
            Process currentprocess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentprocess.MainModule;
            string moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        private static IntPtr SetHook1(LowLevelMouseProc proc)
        {
            Process currentprocess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentprocess.MainModule;
            string moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_MOUSE_LL, llkProcedure1, moduleHandle, 0);
        }

        void gmh_TheMouseMoved()
        {
            Point cur_pos = System.Windows.Forms.Cursor.Position;
            // MouseMoves++;
        }

        private bool MouseIsOverButton(Button btn) =>
        btn.ClientRectangle.Contains(btn.PointToClient(System.Windows.Forms.Cursor.Position));

        private bool MouseIsOverLabel(Label lbl) =>
       lbl.ClientRectangle.Contains(lbl.PointToClient(System.Windows.Forms.Cursor.Position));    

      
       /// <summary>
       /// Method called on loading the form
       /// </summary> 

        [STAThread]
        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Text = AppName+" - " +Version;
            try
            {
                lblCountDown.Font = new Font(FontType, lblCountDown.Font.Size);               
                label2.Font = new Font(FontType, FontSize);
                label3.Font = new Font(FontType, FontSize);
                button1.Font = new Font(FontType, FontSize);
                var UserData = Global.GlobalVar;
                try
                {
                    user = JsonConvert.DeserializeObject<User>(UserData);
                    Global.UserId = user.data.user_id;
                }

                catch (Exception ex)
                {
                    var lineno = ex.LineNumber();
                    if (ex.InnerException.Message != null)
                    {
                        WriteLog("log.txt", DateTime.Now + " UserData: " + UserData + " Line: " + lineno + ex.InnerException.Message);
                    }
                    else
                    {
                        WriteLog("log.txt", DateTime.Now + " UserData: " + UserData + " Line: " + lineno + ex.Message);
                    }                   
                    MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                    return;
                }
                var os = Environment.OSVersion;               
                if (user.data.project_list == null || user.data.project_list.Count() == 0)
                {
                    WriteLog("log.txt", DateTime.Now + " No projects added. UserData: " + UserData);
                    MessageBox.Show("No projects added.",AppName+" - "+Version);
                    TopStartButton.Enabled = false;
                    return;
                }
                TopStartButton.Enabled = true;
                TopStopButton.Visible = false;
                label1.Text = user.data.project_list[0].project_name;
                label1.Font = new Font(FontType, label1.Font.Size);
                if (user.data != null)
                {
                    WriteLog("Log.txt", DateTime.Now + " " + "Logged in as: " + user.data.name + ", User id: " + user.data.user_id);
                    WriteLog("log.txt", DateTime.Now + " Current OS Information- Platform:" + os.Platform + ", Version String:" + os.VersionString + ", Version Information- Major:" + os.Version.Major + " Minor:" + os.Version.Minor + " Service Pack:" + os.ServicePack);
                }
                using (var client = new HttpClient())
                {
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
                            MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                            showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                        }
                        client.BaseAddress = new Uri(BaseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        multipartContent.Add(new StringContent(user.data.user_id.ToString()), "userId");
                        var response = await client.PostAsync(BaseURL+"get_project_duration", multipartContent);
                        var result = await response.Content.ReadAsStringAsync();
                        WriteLog("log.txt", DateTime.Now + " " + "params : userId: " + user.data.user_id);
                        WriteLog("log.txt", "Projectduration Response in formload: " + result);
                        try
                        {
                            ProjectTrack = JsonConvert.DeserializeObject<ProjectRecord>(result);
                        }
                        catch (Exception ex)
                        {
                            var lineno = ex.LineNumber();
                            WriteLog("log.txt","result: "+ result+" Line: "+lineno);
                            MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                            return;
                        }
                       
                    }
                }
                Totalworked = ProjectTrack.data.Sum(x => Convert.ToInt32(x.time_duration_sec))+Convert.ToInt32(ProjectTrack.break_duration_sec);

                var time = CalculateTime(Totalworked.ToString());
                Totalhour = time.hours; Totalminute = time.minutes; Totalsecond = time.seconds;
                var totlTme = (Totalhour < 10 ? "0" + Totalhour : Totalhour.ToString()) + ":" + (Totalminute < 10 ? "0" + Totalminute.ToString() : Totalminute.ToString()) + ":" + (Totalsecond < 10 ? "0" + Totalsecond.ToString() : Totalsecond.ToString());
                label3.Text = totlTme;
                //if (Convert.ToInt32(ProjectTrack.break_duration_sec) < 60)
                //{
                //    if (ProjectTrack.break_duration_sec == "0")
                //    {
                //        brkavailed.Text = "00:00";
                //    }
                //    else
                //    {
                //        brkavailed.Text = "00:" + ProjectTrack.break_duration_sec;
                //    }                   
                //}
                //else
                //{
                    var breaktime = CalculateTime(ProjectTrack.break_duration_sec);
                    brkavailed.Text = (breaktime.hours < 10 ? "0" + breaktime.hours.ToString() : breaktime.hours.ToString()) + ":"+(breaktime.minutes < 10 ? "0" + breaktime.minutes.ToString() : breaktime.minutes.ToString())+":" + (breaktime.seconds < 10 ? "0" + breaktime.seconds.ToString() : breaktime.seconds.ToString());
                    if (breaktime.minutes > 20)
                    {
                        brkavailed.BackColor = Color.Red;
                    }                   
                //}
                // Creating label using Label class
                Label mylab = new Label();
                mylab.Text = user.data.organization_name;
                mylab.Name = "Projectlbl";
                // Set the location of the Label
                mylab.Location = new Point(1, 220);

                // Set the AutoSize property of the Label control
                mylab.AutoSize = false;

                // Set the font of the content present in the Label Control
                mylab.Font = new Font(FontType, FontSize, FontStyle.Bold);

                // Set the foreground color of the Label control
                //  mylab.ForeColor = Color.Green;
                mylab.BackColor = Color.FromArgb(229, 230, 233);
                // Set the padding in the Label control
                mylab.Padding = new Padding(15, 16, 0, 0);
                mylab.Size = new Size(383, 45);
                this.Controls.Add(mylab);

               
                Panel perentpanel = new Panel();
                perentpanel.Location = new System.Drawing.Point(1, 260);
                perentpanel.Size = new Size(383, 350);
                perentpanel.BackColor = Color.LightGray;
                perentpanel.Name = "MainPanel";
                perentpanel.AutoScroll = true;
                this.Controls.Add(perentpanel);

                var i = 0;
                var j = 0;
                var totalheight = 0;
                foreach (var item in user.data.project_list)
                {
                    Panel dynamicPanel = new Panel();
                    dynamicPanel.Location = new System.Drawing.Point(0, 1 + i + j);
                    dynamicPanel.Name = item.project_name + "_" + item.project_id + "_Panel";
                    dynamicPanel.Size = new System.Drawing.Size(383, 48);
                    dynamicPanel.BackColor = Color.FromArgb(255, 255, 255);
                    dynamicPanel.Click += new EventHandler(myPanel_Click);
                    dynamicPanel.MouseHover += new System.EventHandler(panel1_MouseHover);
                    dynamicPanel.MouseLeave += new System.EventHandler(panel1_MouseLeave);
                    dynamicPanel.TabIndex = 0;
                    dynamicPanel.Parent = perentpanel;
                    //this.Controls.Add(dynamicPanel);

                    CustomizedButton csbutton = new CustomizedButton();
                    csbutton.Location = new System.Drawing.Point(16, 10);
                    csbutton.Name = item.project_name + "_" + item.project_id + "_StopButton";
                    csbutton.Visible = false;
                    csbutton.Size = new System.Drawing.Size(23, 26);
                    csbutton.FlatStyle = FlatStyle.Flat;
                    csbutton.FlatAppearance.BorderSize = 0;
                    csbutton.Image = Image.FromFile(Application.StartupPath + @"\Stop.PNG");
                    csbutton.Click += new EventHandler(ProjectTodoStopButton_Click);
                    csbutton.TabIndex = 0;
                    csbutton.Parent = dynamicPanel;

                    CustomizedButton csbutton1 = new CustomizedButton();
                    csbutton1.Location = new System.Drawing.Point(16, 10);
                    csbutton1.Name = item.project_name + "_" + item.project_id + "_StartButton";
                    csbutton1.Visible = false;
                    csbutton1.Size = new System.Drawing.Size(23, 26);
                    csbutton1.FlatStyle = FlatStyle.Flat;
                    csbutton1.FlatAppearance.BorderSize = 0;
                    csbutton1.Image = Image.FromFile(Application.StartupPath + @"\Start.PNG");
                    csbutton1.Click += new EventHandler(ProjectTodoStartButton_Click);
                    csbutton1.MouseHover += new System.EventHandler(button_MouseHover);
                    csbutton1.MouseLeave += new System.EventHandler(button_MouseLeave);
                    csbutton1.TabIndex = 1;
                    csbutton1.Parent = dynamicPanel;

                    Label projectname = new Label();
                    projectname.Location = new System.Drawing.Point(40, 15);
                    projectname.AutoSize = true;
                    projectname.Name = item.project_name + "_" + item.project_id + "_ProjectName";
                    projectname.Click += new EventHandler(Project_Click);
                    projectname.MouseHover += new System.EventHandler(label1_MouseHover);
                    projectname.MouseLeave += new System.EventHandler(label1_MouseLeave);
                    projectname.Text = item.project_name.Length > 30? item.project_name.Substring(0,30) : item.project_name.Substring(0, item.project_name.Length);
                    projectname.Font = new Font(FontType, FontSize);
                    projectname.Parent = dynamicPanel;

                    Label timer = new Label();
                    timer.Location = new System.Drawing.Point(300, 15);
                    timer.AutoSize = true;
                    timer.Name = item.project_name + "_" + item.project_id + "_Timer";
                    var seconds = ProjectTrack.data.FirstOrDefault(x => x.project_id == item.project_id).time_duration_sec;

                    var totalmin = 0;
                    var huors = 0;
                    var secnds = 0;
                    if (Convert.ToInt32(seconds) >= 60)
                    {
                        totalmin = Convert.ToInt32(seconds) / 60;
                        secnds = Convert.ToInt32(seconds) - (60 * totalmin);
                        if (totalmin > 60)
                        {
                            huors = Convert.ToInt32(totalmin) / 60;
                            totalmin = (totalmin - (60 * huors));
                        }
                    }
                    else
                    {
                        secnds = Convert.ToInt32(seconds);
                    }
                    var totaltime = (huors < 10 ? "0" + huors : huors.ToString()) + ":" + (totalmin < 10 ? "0" + totalmin.ToString() : totalmin.ToString()) + ":" + (secnds < 10 ? "0" + secnds.ToString() : secnds.ToString());
                    timer.Text = totaltime;
                    timer.Font = new Font(FontType, FontSize);
                    timer.Parent = dynamicPanel;


                    var k = 0;
                    foreach (var todo in item.todo)
                    {
                        Panel dynamicPanel1 = new Panel();
                        dynamicPanel1.Location = new System.Drawing.Point(-1, i + 50 + j + k);
                        dynamicPanel1.Name = item.project_name + "_" + item.project_id + "_" + todo.todo_title + "_" + todo.todo_id + "_Panel";
                        dynamicPanel1.Size = new System.Drawing.Size(383, 28);
                        dynamicPanel1.BackColor = Color.FromArgb(255, 255, 255);
                        dynamicPanel1.Click += new EventHandler(Task_Click);
                        dynamicPanel1.MouseHover += new System.EventHandler(panel1_MouseHover);
                        dynamicPanel1.MouseLeave += new System.EventHandler(panel1_MouseLeave);
                        dynamicPanel1.TabIndex = 0;
                        dynamicPanel1.Parent = perentpanel;
                        //this.Controls.Add(dynamicPanel);

                        CustomizedButton csbutton2 = new CustomizedButton();
                        csbutton2.Location = new System.Drawing.Point(40, 2);
                        csbutton2.Name = item.project_name + "_" + item.project_id + "_" + todo.todo_title + "_" + todo.todo_id + "_StopButton";
                        csbutton2.Visible = false;
                        csbutton2.Size = new System.Drawing.Size(23, 26);
                        csbutton2.FlatStyle = FlatStyle.Flat;
                        csbutton2.FlatAppearance.BorderSize = 0;
                        csbutton2.Image = Image.FromFile(Application.StartupPath + @"\Stop.PNG");
                        csbutton2.Click += new EventHandler(ProjectTodoStopButton_Click);
                        csbutton2.TabIndex = 0;
                        csbutton2.Parent = dynamicPanel1;

                        CustomizedButton csbutton3 = new CustomizedButton();
                        csbutton3.Location = new System.Drawing.Point(40, 2);
                        csbutton3.Name = item.project_name + "_" + item.project_id + "_" + todo.todo_title + "_" + todo.todo_id + "_StartButton";
                        csbutton3.Visible = false;
                        csbutton3.Size = new System.Drawing.Size(23, 26);
                        csbutton3.FlatStyle = FlatStyle.Flat;
                        csbutton3.FlatAppearance.BorderSize = 0;
                        csbutton3.Image = Image.FromFile(Application.StartupPath + @"\Start.PNG");
                        csbutton3.Click += new EventHandler(ProjectTodoStartButton_Click);
                        csbutton3.MouseHover += new System.EventHandler(button_MouseHover);
                        csbutton3.MouseLeave += new System.EventHandler(button_MouseLeave);
                        csbutton3.TabIndex = 1;
                        csbutton3.Parent = dynamicPanel1;

                        Label mytodo1 = new Label();
                        mytodo1.Location = new System.Drawing.Point(64, 8);
                        mytodo1.AutoSize = true;
                        mytodo1.Name = item.project_name + "_" + item.project_id + "_" + todo.todo_title + "_" + todo.todo_id + "_TodoName";
                        mytodo1.Click += new EventHandler(Todo_Click);
                        mytodo1.MouseHover += new System.EventHandler(label1_MouseHover);
                        mytodo1.MouseLeave += new System.EventHandler(label1_MouseLeave);
                        mytodo1.Text = todo.todo_title.Length >30? todo.todo_title.Substring(0, 30): todo.todo_title.Substring(0,todo.todo_title.Length);
                        mytodo1.Font = new Font(FontType, FontSize);
                        mytodo1.Parent = dynamicPanel1;

                        k += 29;
                    }


                    Label seperator = new Label();
                    seperator.Location = new System.Drawing.Point(1, i + 50 + j + k);
                    totalheight = i + 50 + j + k;
                    i += 50;
                    j += k - 1;
                    seperator.Name = item.project_name + "_seperator";
                    seperator.AutoSize = false;
                    seperator.Size = new Size(383, 1);
                    seperator.BackColor = Color.FromArgb(198, 223, 249);
                    // this.Controls.Add(seperator);
                    seperator.Parent = perentpanel;
                }
                if (totalheight < 415)
                {
                    perentpanel.Height = totalheight;
                    perentpanel.AutoScroll = false;
                }
                ProjectName = user.data.project_list[0].project_name;
                Projectid = user.data.project_list[0].project_id;
                var currentprojectsecond = ProjectTrack.data.FirstOrDefault(x => x.project_id == Projectid).time_duration_sec;
                var currentprojectmin = 0;
                var currentprojecthour = 0;
                var currentprojectsecnd = 0;
                if (Convert.ToInt32(currentprojectsecond) >= 60)
                {
                    currentprojectmin = Convert.ToInt32(currentprojectsecond) / 60;
                    currentprojectsecnd = Convert.ToInt32(currentprojectsecond) - (60 * currentprojectmin);
                    if (currentprojectmin > 59)
                    {
                        currentprojecthour = Convert.ToInt32(currentprojectmin) / 60;
                        currentprojectmin = currentprojectmin - (60 * currentprojecthour);
                    }
                }
                else
                {
                    currentprojectsecnd = Convert.ToInt32(currentprojectsecond);
                }
                var currentProjectTime = (currentprojecthour < 10 ? "0" + currentprojecthour : currentprojecthour.ToString()) + ":" + (currentprojectmin < 10 ? "0" + currentprojectmin.ToString() : currentprojectmin.ToString()) + ":" + (currentprojectsecnd < 10 ? "0" + currentprojectsecnd.ToString() : currentprojectsecnd.ToString());
                lblCountDown.Text = currentProjectTime;

                var pnl = this.Controls.Find(ProjectName + "_" + Projectid + "_Panel", true)[0];
                pnl.BackColor = Color.FromArgb(198, 223, 249);
                var tt = ProjectName + "_" + Projectid + "_StartButton";
                foreach (Control item in pnl.Controls.OfType<Control>())
                {
                    if (item.Name == tt)
                        item.Visible = true;
                }
                if (Global.SendNotification)
                {
                    showBalloon("Sign in", "Signed in as " + user.data.name);
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
          }
   


        public class Screenshot
        {
            public Bitmap image { get; set; }
            public string FileName { get; set; }
        }

        /// <summary>
        /// Reducing the image size
        /// </summary> 
        public Image ReduceImage(Bitmap bmpScreenshot)
        {
            MemoryStream ms = new MemoryStream();
            bmpScreenshot.Save(ms, ImageFormat.Jpeg);


            var jpegQuality = 50;
            Image image;
            using (var inputStream = new MemoryStream(ms.ToArray()))
            {
                image = Image.FromStream(inputStream);
                var jpegEncoder = ImageCodecInfo.GetImageDecoders()
                  .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
                Byte[] outputBytes;
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, jpegEncoder, encoderParameters);
                    outputBytes = outputStream.ToArray();
                    MemoryStream strm = new MemoryStream(outputBytes);
                    image = Image.FromStream(strm);
                }
            }

            return image;
        }


        /// <summary>
        /// Capture the screenshot
        /// </summary> 
        public async Task<Screenshot> CaptureScreen(string Type)
        {
            try
            {
               Bitmap bmpScreenshot = null;
                if (user.data.is_screenshot_capture == "1")
                {
                    height = Screen.PrimaryScreen.Bounds.Height.ToString();
                    width = Screen.PrimaryScreen.Bounds.Width.ToString();
                    xcoordinate = Screen.PrimaryScreen.Bounds.X.ToString();
                    ycoordinate = Screen.PrimaryScreen.Bounds.Y.ToString();
                    size = Screen.PrimaryScreen.Bounds.Size.ToString();
                     bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                                  Screen.PrimaryScreen.Bounds.Height,
                                                                  PixelFormat.Format32bppArgb);

                    // Create a graphics object from the bitmap.

                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                    // Take the screenshot from the upper left corner to the right bottom corner.
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                Screen.PrimaryScreen.Bounds.Y,
                                                0,
                                                0,
                                                Screen.PrimaryScreen.Bounds.Size,
                                                CopyPixelOperation.SourceCopy);
                }

                var guid = NextScreenshot.Day.ToString() + NextScreenshot.Month.ToString() + NextScreenshot.Year.ToString() + NextScreenshot.TimeOfDay.ToString("hh") + NextScreenshot.ToString("mm");


                var root = "";
                Screenshot saved = new Screenshot();
                if (bmpScreenshot != null)
                {
                   saved.image = bmpScreenshot;
                     
                   var image = ReduceImage(bmpScreenshot);
                   
                    saved.FileName = guid + ".JPEG";
                    Random r = new Random();
                    int rInt = r.Next(5,12); //for ints
                                             // int range = 15;
                                             // double rDouble = Math.Round(r.NextDouble() * range);
                                             // CurrentTime = DateTime.Now;
                                             // CurrentTime = DateTime.UtcNow.AddHours(UTCOffset);
                    CurrentTime = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
                    // var timing = lblCountDown.Text.Split(':');
                    // var totSecond = GetTotal(timing);

                    //  var elapse = Type == "START" ? 0 : (totSecond - PreviousTime);
                    currentscreenshot = Type=="START" ? StartTime : nextcreenshottime;
                    if (Type== "STOP")
                    {
                        var labelseconds = GetTotal(lblCountDown.Text.Split(':'));
                        nxttime = labelseconds - pretime < 0 ? 0: labelseconds - pretime;
                        //  nxttime = Convert.ToInt32((CurrentTime - PreviousTime).TotalSeconds);
                        currentscreenshot = PreviousTime.AddSeconds(nxttime) ;
                    }
                    if (Type == "START")
                    {
                        nxttime = 0;
                    }
                    var elapse = Type == "START" ? 0 : (CurrentTime - PreviousTime).TotalSeconds;
                    var Project = timerstartedfor;
                    var project_id = Project != null ? Convert.ToInt32(Project.Split('_')[1]) : 0;
                    var todoid_id = Project != null ? Convert.ToInt32(Project.Split('_')[3]) : 0;
                    var Path = ""; 
                  
                    var tim = lblCountDown.Text.Split(':');
                   
                    if (CheckForInternetConnection())
                    {
                        root = @"C:\Users\Public\Documents\Screenshots\";
                        if (Directory.Exists(root))
                        {
                            Path = root + guid + ".JPEG";
                            image.Save(Path, ImageFormat.Jpeg);
                        }
                        else
                        {
                            Directory.CreateDirectory(root);
                            Path = root + guid + ".JPEG";
                            image.Save(Path, ImageFormat.Jpeg);
                        }
                        var response = await CallSaveScreenshotAPI(image, saved.FileName, project_id, Convert.ToInt32(nxttime), StartTime, currentscreenshot, MouseMoves, KeyboardMovements, todoid_id, Type, user.data.user_id);
                        if (response.ToString().Contains("\"status\":1"))
                        {
                            FileSent = saved.FileName;
                            File.Delete(Path);
                            if (Global.SendNotification)
                            {
                                showBalloon("Screenshot", "Screenshot has been captured!!!");
                                showBalloon("TotalTime", "Total keypress count: " + KeyboardMovements + ", Mouse click count: " + MouseMoves + " and time elapse in sec: " + elapse);
                            }
                        }
                        else
                        {
                            WriteLog("log.txt", DateTime.Now+" Callsavescreenshotapi response in capturescreen: " + response+"");
                        }
                    }
                    else
                    {
                        //root = @"C:\Users\Public\Documents\Local\";
                        //var strt = StartTime.ToString().Replace("/", "#").Replace(":", "@");
                        //var crnt = CurrentTime.ToString().Replace("/", "#").Replace(":", "@");
                        //Path = root + guid + "_" + project_id + "_" + Convert.ToInt32(elapse) + "_" + strt + "_" + crnt + "_" + MouseMoves + "_" + KeyboardMovements + "_" + todoid_id + "_" + Type + "_" + user.data.user_id + ".JPEG";
                        //if (Directory.Exists(root))
                        //{
                        //    image.Save(Path, ImageFormat.Jpeg);
                        //}
                        //else
                        //{
                        //    Directory.CreateDirectory(root);
                        //    image.Save(Path, ImageFormat.Jpeg);
                        //}
                    }                  
                     var totlSecond = GetTotal(tim);
                    // PreviousTime = totlSecond;
                     pretime = totlSecond;
                     var next = totlSecond + (60 * rInt);                    
                     nxttime = next - totlSecond;
                     nextcreenshottime = currentscreenshot.AddSeconds(nxttime);
                     var nxtlabeltime = CalculateTime(next.ToString());
                     nxtlabeltxt = (nxtlabeltime.hours < 10 ? "0" + nxtlabeltime.hours : nxtlabeltime.hours.ToString()) + ":" + (nxtlabeltime.minutes < 10 ? "0" + nxtlabeltime.minutes.ToString() : nxtlabeltime.minutes.ToString()) + ":" + (nxtlabeltime.seconds < 10 ? "0" + nxtlabeltime.seconds.ToString() : nxtlabeltime.seconds.ToString());
                    // PreviousTime = CurrentTime;
                    PreviousTime = currentscreenshot;
                    NextScreenshot = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes).AddMinutes(rInt);
                    KeyboardMovements = 0;
                    MouseMoves = 0;
                }

                else
                {
                   var img= Image.FromFile(Application.StartupPath+ "/image-not-available.jpg");

                   // string[] filePaths = Directory.GetFiles(Application.StartupPath + @"\image-not-available.jpg");


                    Bitmap bm = new Bitmap(img);
                    var image = ReduceImage(bm);

                    saved.FileName = guid + ".JPG";
                    Random r = new Random();
                    int rInt = r.Next(5, 12);
                    CurrentTime = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
                    // var timing = lblCountDown.Text.Split(':');
                    // var totSecond = GetTotal(timing);

                    //  var elapse = Type == "START" ? 0 : (totSecond - PreviousTime);
                    currentscreenshot = Type == "START"? StartTime : nextcreenshottime;
                    if (Type == "STOP")
                    {
                        nxttime = GetTotal(lblCountDown.Text.Split(':')) - pretime;
                        currentscreenshot = PreviousTime.AddSeconds(nxttime);
                    }
                    if (Type == "START")
                    {
                        nxttime = 0;
                    }
                    var elapse = Type == "START" ? 0 : (CurrentTime - PreviousTime).TotalSeconds;
                    var Project = timerstartedfor;
                    var project_id = Project != null ? Convert.ToInt32(Project.Split('_')[1]) : 0;
                    var todoid_id = Project != null ? Convert.ToInt32(Project.Split('_')[3]) : 0;
                    var Path = "";
                    var tim = lblCountDown.Text.Split(':');
                    if (CheckForInternetConnection())
                    {                       
                        var response = await CallSaveScreenshotAPI(image, saved.FileName, project_id, Convert.ToInt32(nxttime), StartTime, currentscreenshot, MouseMoves, KeyboardMovements, todoid_id, Type, user.data.user_id);
                        if (response.ToString().Contains("\"status\":1"))
                        {
                            FileSent = saved.FileName;
                            if (Global.SendNotification)
                            {
                                showBalloon("TotalTime", "Total keypress count: " + KeyboardMovements + ", Mouse click count: " + MouseMoves + " and time elapse in sec: " + elapse);
                            }
                        }
                        else
                        {
                            WriteLog("log.txt", DateTime.Now+" Callsavescreenshotapi response in capturescreen: " + response + "");
                        }
                    }
                    else
                    {                    
                    }

                    var totlSecond = GetTotal(tim);
                    pretime = totlSecond;
                    var next = totlSecond + (60 * rInt);
                    nxttime = next - totlSecond;
                    nextcreenshottime = currentscreenshot.AddSeconds(nxttime);
                    var nxtlabeltime = CalculateTime(next.ToString());
                    nxtlabeltxt = (nxtlabeltime.hours < 10 ? "0" + nxtlabeltime.hours : nxtlabeltime.hours.ToString()) + ":" + (nxtlabeltime.minutes < 10 ? "0" + nxtlabeltime.minutes.ToString() : nxtlabeltime.minutes.ToString()) + ":" + (nxtlabeltime.seconds < 10 ? "0" + nxtlabeltime.seconds.ToString() : nxtlabeltime.seconds.ToString());
                    PreviousTime = currentscreenshot;
                    NextScreenshot = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes).AddMinutes(rInt);
                    KeyboardMovements = 0;
                    MouseMoves = 0;

                    //var tim = lblCountDown.Text.Split(':');
                    //PreviousTime = CurrentTime;
                    //NextScreenshot = DateTime.Now.AddMinutes(rInt);
                    //KeyboardMovements = 0;
                    //MouseMoves = 0;
                }

                return saved;
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                if (ex.InnerException.Message != null)
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace+" Innerexception Message: "+ex.InnerException.Message);
                }
                else
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace + " Innerexception: " + ex.InnerException);
                }
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please restart the app to continue.",AppName+" - "+Version);
                }               
                return null;
            }
        }

        /// <summary>
        /// Maintain a log file.
        /// </summary> 
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\Documents\Temp\log.txt");
                //if (lines.Any(x => x.Contains(PreviousVersion)))
                //{
                //    File.Delete(@"C:\Users\Public\Documents\Temp\log.txt");
                //}
                var root = @"C:\Users\Public\Documents\Temp\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", root, strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(Version+" "+strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);                   
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return false;
            }
        }

        private void program2_Click(object sender, EventArgs e)
        {

        }


        private void keyPressEvent(object sender, KeyPressEventArgs e)
        {

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

                if (Convert.ToInt32(currentprojectsecond) >= 60)
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
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return null;
            }
        }

        /// <summary>
        /// Start the timer
        /// </summary> 
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!CheckForInternetConnection())
                {
                    TopStopButton_Click(TopStopButton,null);
                    MessageBox.Show("We are having some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                    return;
                }
                second++; Totalsecond++;
                if (second > 59)
                {
                    second = 0;
                    minute++;
                }
                if (Totalsecond > 59)
                {
                    Totalsecond = 0;
                    Totalminute++;
                }
                if (minute > 59)
                {
                    minute = 0;
                    hour++;
                }
                if (Totalminute > 59)
                {
                    Totalminute = 0;
                    Totalhour++;
                }
                if (minute >= 1)
                {
                    label3.Text = (Totalhour < 10 ? "0" + Totalhour : Totalhour.ToString()) + ":" + (Totalminute < 10 ? "0" + Totalminute : Totalminute.ToString())+ ":" + (Totalsecond < 10 ? "0" + Totalsecond : Totalsecond.ToString());
                }
                var projectcountdown = this.Controls.Find(timerstartedfor.Split('_')[0] + "_" + timerstartedfor.Split('_')[1] + "_Timer", true)[0];
                projectcountdown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                var guid = NextScreenshot.Day.ToString() + NextScreenshot.Month.ToString() + NextScreenshot.Year.ToString() + NextScreenshot.TimeOfDay.ToString("hh") + NextScreenshot.ToString("mm");
                if (lblCountDown.Text == nxtlabeltxt)
                //if (DateTime.Now.ToString("dd/MM/yyyy hh:mm") == NextScreenshot.ToString("dd/MM/yyyy hh:mm") && sent != NextScreenshot.ToString("dd/MM/yyyy hh:mm"))
                {
                    if (FileSent != guid+"JPEG")
                    {
                        sent = NextScreenshot.ToString("dd/MM/yyyy hh:mm");
                        var elapse = new double();
                         CaptureScreen("CURRENT");
                    }                   
                }
                lblCountDown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                if (ex.InnerException.Message != null)
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace+" InnerException message: "+ex.InnerException.Message);
                }
                else
                {
                    WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace + " InnerException: " + ex.InnerException);
                }
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }


      

        /// <summary>
        /// Show balloon notification on system tray
        /// </summary> 
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
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }


        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }

        /// <summary>
        /// Call project duration Api
        /// </summary> 
        private async void project_durationAPI(int userId = 0)
        {

            try
            {
                using (var client = new HttpClient())
                {
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
                            MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                            showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                        }
                        client.BaseAddress = new Uri(BaseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        multipartContent.Add(new StringContent(userId.ToString()), "userId");
                        var response = await client.PostAsync(BaseURL+"get_project_duration", multipartContent);
                        var result = await response.Content.ReadAsStringAsync();
                        WriteLog("log.txt", DateTime.Now + " " + "params : userId: " + userId);
                        WriteLog("log.txt", "Projectduration Response in projecturation: " + result);
                        try
                        {
                            ProjectTrack = JsonConvert.DeserializeObject<ProjectRecord>(result);
                        }
                        catch (Exception ex)
                        {
                            var lineno = ex.LineNumber();
                            WriteLog("log.txt","Result: "+ result+" Line: "+lineno);
                            MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                WriteLog("Log.txt", DateTime.Now + " " + ex.Message + ex.StackTrace);
                return;
            }
        }


        /// <summary>
        /// SAve screenshot API
        /// </summary> 
        private async Task<string>  CallSaveScreenshotAPI(Image image, string Filename, int project_id, int time_elapse_in_seconds, DateTime started_time, DateTime screenshot_current, int mouse_click_count, int keyboard_movement_count, int todo_id, string screenshot_type, int userid = 0)
        {
            var result = "";
            try
            {
                 HttpClient  httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseURL);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                result = await SaveScreenshot(httpClient,image, Filename, project_id, time_elapse_in_seconds, started_time, screenshot_current, mouse_click_count, keyboard_movement_count, todo_id, screenshot_type, userid);
                return result;
            }
            catch (Exception ex)
            {            
                WriteLog("log.txt",DateTime.Now+" SavescreenshotAPI response: "+result);
                return result;
            }
        }

        /// <summary>
        /// Save the screenshot
        /// </summary> 
        private async Task<string> SaveScreenshot(HttpClient client, Image image, string Filename, int project_id, int time_elapse_in_seconds, DateTime started_time, DateTime screenshot_current, int mouse_click_count, int keyboard_movement_count, int todo_id, string screenshot_type, int userid = 0)
        {
            var result = "";
            try
            {
              //  var ss = new List<project>();
              //var ghr=  ss.FirstOrDefault().time_duration_sec;
                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
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
                var project_started_time = started_time.Year.ToString() + "-" + started_time.Month.ToString() + "-" + started_time.Day.ToString() + " " + started_time.TimeOfDay.ToString("hh") + ":" + started_time.ToString("mm") + ":" + started_time.TimeOfDay.ToString("ss");
                var screenshot_current_time = screenshot_current.Year.ToString() + "-" + screenshot_current.Month.ToString() + "-" + screenshot_current.Day.ToString() + " " + screenshot_current.TimeOfDay.ToString("hh") + ":" + screenshot_current.ToString("mm") + ":" + screenshot_current.TimeOfDay.ToString("ss");

               

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
                         MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                        showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                    }
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        await stream.CopyToAsync(mem);
                        var byteContent = new ByteArrayContent(mem.ToArray());
                        multipartContent.Add(byteContent, "img", Filename + ".JPEG");
                        multipartContent.Add(new StringContent(userid.ToString()), "user_id");
                        multipartContent.Add(new StringContent(project_id.ToString()), "project_id");
                        multipartContent.Add(new StringContent(screenshot_type.ToString()), "screenshot_type");
                        multipartContent.Add(new StringContent(TimeZoneName.ToString()), "timezone");
                        multipartContent.Add(new StringContent(todo_id.ToString()), "todo_id");
                        multipartContent.Add(new StringContent(time_elapse_in_seconds.ToString()), "time_elapse_in_seconds");
                        multipartContent.Add(new StringContent(project_started_time), "project_started_time");
                        multipartContent.Add(new StringContent(screenshot_current_time), "screenshot_current_time");
                        multipartContent.Add(new StringContent(mouse_click_count.ToString()), "mouse_click_count");
                        multipartContent.Add(new StringContent(keyboard_movement_count.ToString()), "keyboard_movement_count");
                         var response = await client.PostAsync(BaseURL+"capture_screen", multipartContent);
                        result = await response.Content.ReadAsStringAsync();
                       WriteLog("log.txt", DateTime.Now + " SH ="+ height+", SW ="+ width+", SX ="+ xcoordinate+", SY ="+ ycoordinate+", SZ ="+ size);
                        WriteLog("log.txt", DateTime.Now + " " + "params sent : File name- " + Filename + " user id- " + userid +" Type: "+ screenshot_type + ", project id- " + project_id.ToString() + ", time lapse- " + time_elapse_in_seconds.ToString() + ", Project started-" + project_started_time + ", Current screenshot time-" + screenshot_current_time+", timer:"+lblCountDown.Text+" , IsScreenshotCapture-"+user.data.is_screenshot_capture+", Timezone-"+TimeZoneName + ", Mouse click-" + mouse_click_count.ToString() + ", Key press-" + keyboard_movement_count.ToString());
                        WriteLog("log.txt", DateTime.Now+" Savescreenshot Response: " + result);
                    }
                return result;
               }

            catch (Exception ex)
            {
                // var lineno = ex.LineNumber();
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                if (ex.InnerException.Message == null)
                {
                    WriteLog("Log.txt", DateTime.Now + " " + ex.Message + ex.StackTrace + "Response: " + result + "  Inner Exception: " + ex.InnerException);
                }
                else
                {
                    WriteLog("Log.txt", DateTime.Now + "Response: " + result + ex.Message + ex.StackTrace  + "  Inner Exception message: " + ex.InnerException.Message);
                }
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                // Get stack trace for the exception with source file information
             
                return result;              
            }
        }
        private async void label1_Click(object sender, EventArgs e)
        {

        }

        public class Screenshotsave
        {
            public int user_id { get; set; }
            public int project_id { get; set; }
            public int time_elapse_in_seconds { get; set; }
            public string project_started_time { get; set; }
            public string screenshot_current_time { get; set; }
            public int mouse_click_count { get; set; }
            public int keyboard_movement_count { get; set; }

        }
        public class User
        {
            public string status { get; set; }
            public result data { get; set; }           
        }

        public class ProjectRecord
        {
            public string status { get; set; }
            public string break_duration_sec { get; set; }
            public List<project> data { get; set; }
        }

        public class result
        {
            public int user_id { get; set; }
            public int role { get; set; }
            public string roleText { get; set; }
            public string name { get; set; }
            public DateTime lastLogin { get; set; }
            public bool isLoggedIn { get; set; }
            public int organization_id { get; set; }
            public string organization_name { get; set; }
            public string is_screenshot_capture { get; set; }
            public string organization_address { get; set; }
            public string organization_number { get; set; }
            public string Myorganization_currencyProperty { get; set; }
            public string organization_logo { get; set; }
            public string organization_timzone { get; set; }
            public int MyProperty { get; set; }
            public List<project> project_list { get; set; }
        }

        public class project
        {
            public int project_user_id { get; set; }
            public int user_id { get; set; }
            public int project_id { get; set; }
            public string is_deleted { get; set; }
            public int organization_id { get; set; }
            public string project_name { get; set; }
            public string is_billable { get; set; }
            public string project_created_date { get; set; }
            public string is_archived { get; set; }
            public List<Todo> todo { get; set; }
            public string time_duration_sec { get; set; }
            public string break_duration_sec { get; set; }
        }

        public class Todo
        {
            public int todo_id { get; set; }
            public string todo_title { get; set; }
            public int project_id { get; set; }
            public int assign_by { get; set; }
            public int assign_to { get; set; }
            public int todo_status { get; set; }
            public DateTime date_created { get; set; }
            public DateTime date_updated { get; set; }
        }
        public class todoDisplay
        {
            public string Todos { get; set; }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }


        /// <summary>
       /// Mouse hover on Project panel
       /// </summary> 
        private void panel1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var Panel = sender as Panel;
                var panel = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Panel>();
                var button = Panel.Name.Replace("Panel", "StopButton");
                var btn = Panel.Controls.OfType<Button>().FirstOrDefault(x => x.Name == button);

                foreach (var pnl in panel)
                {
                    if (Panel.Name != pnl.Name)
                    {
                        if (pnl.BackColor == Color.FromArgb(240, 244, 247))
                        {
                            pnl.BackColor = Color.FromArgb(255, 255, 255);
                            var Start = pnl.Name.Replace("_Panel", "_StartButton");
                            pnl.Controls.Find(Start, true)[0].Visible = false;
                        }
                    }
                }

                if (Panel.BackColor != Color.FromArgb(240, 244, 247) && Panel.BackColor != Color.FromArgb(198, 223, 249) && btn.Visible == false)
                {
                    Panel.BackColor = Color.FromArgb(240, 244, 247);
                    var split = Panel.Name.Split('_');
                    var Start = Panel.Name.Replace("_Panel", "_StartButton");
                    Panel.Controls.Find(Start, true)[0].Visible = true;
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
               
                return;
            }

        }

        /// <summary>
        /// Project/todo start stop button mouse hover
        /// </summary> 
        private void button_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var Button = sender as Button;

                var Name = Button.Name;
                var Panel = Name.Replace("StartButton", "Panel");
                var Pnl = this.Controls.Find(Panel, true)[0];
                panel1_MouseHover(Pnl, null);
            }
            catch (Exception ex)
            {
                 var lineno = ex.LineNumber();
                 WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }
        }

        /// <summary>
        /// Todo label mouse hover
        /// </summary> 
        private void label1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                var label = sender as Label;
                var Panel = label.Name.Split('_').Count() > 3 ? label.Name.Replace("TodoName", "Panel") : label.Name.Replace("ProjectName", "Panel");
                var pnl = this.Controls.Find(Panel, true)[0];
                panel1_MouseHover(pnl, null);
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }

        }


        /// <summary>
        /// Project panel mouse leave event
        /// </summary> 
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var Panel = sender as Panel;
                var Name = Panel.Name;
                var button = Name.Replace("Panel", "StartButton");
                var mainpanel = this.Controls.Find("MainPanel", true)[0];
                var Projectlabl = Name.Split('_').Count() > 3 ? Name.Replace("Panel", "TodoName") : Name.Replace("Panel", "ProjectName");
                var btn = Panel.Controls.OfType<Button>().FirstOrDefault(x => x.Name == button);
                var lbl = Panel.Controls.OfType<Label>().FirstOrDefault(x => x.Name == Projectlabl);

                if (Panel.BackColor != Color.FromArgb(198, 223, 249) && ((Panel.BackColor == Color.FromArgb(240, 244, 247) && btn.Visible == false) || !(Panel.ClientRectangle.Contains(Panel.PointToClient(System.Windows.Forms.Cursor.Position)))))
                {
                    Panel.BackColor = Color.FromArgb(255, 255, 255);
                    var split = Panel.Name.Split('_');
                    var Start = Panel.Name.Replace("_Panel", "_StartButton");
                    Panel.Controls.Find(Start, true)[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }              
                return;
            }
        }


        /// <summary>
        /// Todo label mouse leave event
        /// </summary> 
        private void label1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var label = sender as Label;
                var Panel = label.Name.Split('_').Count() > 3 ? label.Name.Replace("TodoName", "Panel") : label.Name.Replace("ProjectName", "Panel");
                var pnl = this.Controls.Find(Panel, true)[0];
                panel1_MouseLeave(pnl, null);
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }

        }

        /// <summary>
        /// Project/todo mouse leave event
        /// </summary> 
        private void button_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var Panel = btn.Name.Replace("StartButton", "Panel");
                var pnl = this.Controls.Find(Panel, true)[0];
                panel1_MouseLeave(pnl, null);
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }

        }

        /// <summary>
        /// Project panel click
        /// </summary> 
        private async void myPanel_Click(object sender, EventArgs e)
        {
            try
            {
                var panel = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Panel>();
                var label = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Label>();
                Panel pnl = sender as Panel;
                ProjectName = pnl.Name.ToString().Split('_')[0];
                label1.Text = started ? timerstartedfor.Split('_')[0] : ProjectName;
                Projectid = Convert.ToInt32(pnl.Name.ToString().Split('_')[1]);

                foreach (Panel P in panel)
                {
                    var panelsplit = P.Name.Split('_');
                    var strtbutton = panelsplit.Count() > 3 ? panelsplit[0] + "_" + panelsplit[1] + "_" + panelsplit[2] + "_" + panelsplit[3] + "_StartButton" : panelsplit[0] + "_" + panelsplit[1] + "_StartButton";
                    P.BackColor = Color.FromArgb(255, 255, 255);
                    var button = P.Controls.Find(strtbutton, true)[0];
                    button.Visible = false;
                }
               
                pnl.BackColor = Color.FromArgb(198, 223, 249);
                var panlsplit = pnl.Name.Split('_');
                var ButtonName = panlsplit.Count() > 3 ? panlsplit[0] + "_" + panlsplit[1] + "_" + panlsplit[2] + "_" + panlsplit[3] : panlsplit[0] + "_" + panlsplit[1];

                if (pnl.Controls.Find(ButtonName + "_StopButton", true)[0].Visible != true)
                {
                    var btn = pnl.Controls.Find(ButtonName + "_StartButton", true)[0];
                    btn.Visible = true;
                }
                todoId = 0;
                todoName = "";
                if (!started)
                {
                    using (var client = new HttpClient())
                    {
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
                                MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                                showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                            }
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            multipartContent.Add(new StringContent(user.data.user_id.ToString()), "userId");
                            var response = await client.PostAsync(BaseURL+"get_project_duration", multipartContent);
                            var result = await response.Content.ReadAsStringAsync();
                            WriteLog("log.txt", DateTime.Now + " " + "params : userId: " + user.data.user_id);
                            WriteLog("log.txt", "Projectduration Response mypanel1click: " + result);
                            try
                            {
                                ProjectTrack = JsonConvert.DeserializeObject<ProjectRecord>(result);
                            }
                            catch (Exception ex)
                            {
                                var lineno = ex.LineNumber();
                                WriteLog("log.txt","Result: "+ result+" Line:"+lineno);
                                MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                                return;
                            }
                        }
                    }
                    var TotalSeconds = ProjectTrack.data.FirstOrDefault(x => x.project_id == Projectid).time_duration_sec;
                    var Time = CalculateTime(TotalSeconds);
                    second = Time.seconds;
                    minute = Time.minutes;
                    hour = Time.hours;
                    lblCountDown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }          
                 return;
            }
        }


        /// <summary>
        /// Todo name click function
        /// </summary> 
        private async void Task_Click(object sender, EventArgs e)
        {
            try
            {
                var label = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Label>();

                var panel = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Panel>().Where(x => x.Name.EndsWith("_Panel"));
                Panel pnl = sender as Panel;

                foreach (Panel P in panel)
                {
                    var panelsplit = P.Name.Split('_');
                    var strtbutton = panelsplit.Count() > 3 ? panelsplit[0] + "_" + panelsplit[1] + "_" + panelsplit[2] + "_" + panelsplit[3] + "_StartButton" : panelsplit[0] + "_" + panelsplit[1] + "_StartButton";
                    P.BackColor = Color.FromArgb(255, 255, 255);
                    var button = P.Controls.Find(strtbutton, true)[0];
                    button.Visible = false;
                }

                pnl.BackColor = Color.FromArgb(198, 223, 249);
                var panlsplit = pnl.Name.Split('_');
                var ButtonName = panlsplit.Count() > 3 ? panlsplit[0] + "_" + panlsplit[1] + "_" + panlsplit[2] + "_" + panlsplit[3] : panlsplit[0] + "_" + panlsplit[1];

                if (pnl.Controls.Find(ButtonName + "_StopButton", true)[0].Visible != true)
                {
                    var btn = pnl.Controls.Find(ButtonName + "_StartButton", true)[0];
                    btn.Visible = true;
                }
                pnl.BackColor = Color.FromArgb(198, 223, 249);
                ProjectName = pnl.Name.ToString().Split('_')[0];
                Projectid = Convert.ToInt32(pnl.Name.ToString().Split('_')[1]);
                todoName = pnl.Name.Split('_')[2];
                label5.Text = started ? timerstartedfor.Split('_')[2] : todoName;
                label1.Text = started ? timerstartedfor.Split('_')[0] : ProjectName;
                todoId = Convert.ToInt32(pnl.Name.Split('_')[3]);
                if (!started)
                {
                    using (var client = new HttpClient())
                    {
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
                                MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                                showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                            }
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            multipartContent.Add(new StringContent(user.data.user_id.ToString()), "userId");
                            var response = await client.PostAsync(BaseURL+"get_project_duration", multipartContent);
                            var result = await response.Content.ReadAsStringAsync();
                            WriteLog("log.txt", DateTime.Now + " " + "params : userId: " + user.data.user_id);
                            WriteLog("log.txt", "Response: " + result);
                            try
                            {
                                ProjectTrack = JsonConvert.DeserializeObject<ProjectRecord>(result);
                            }
                            catch (Exception ex)
                            {
                                var lineno = ex.LineNumber();
                                WriteLog("log.txt","result: " +result+" Line: "+lineno);
                                MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                                return;
                            }
                        }
                    }
                    var TotalSeconds = ProjectTrack.data.FirstOrDefault(x => x.project_id == Projectid).time_duration_sec;
                    var Time = CalculateTime(TotalSeconds);
                    second = Time.seconds;
                    minute = Time.minutes;
                    hour = Time.hours;
                    lblCountDown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                }
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }

        /// <summary>
        /// Project name click function
        /// </summary> 
        private void Project_Click(object sender, EventArgs e)
        {
            try
            {
                Label lbl = sender as Label;
                ProjectName = lbl.Name.ToString().Split('_')[0];
                Projectid = Convert.ToInt32(lbl.Name.ToString().Split('_')[1]);
                var panel = ProjectName + "_" + Projectid + "_Panel";
                Control cc = this.Controls.Find(panel, true)[0];
                myPanel_Click(cc, e);
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }
        }

        /// <summary>
        /// Todo click function
        /// </summary> 
        private void Todo_Click(object sender, EventArgs e)
        {
            try
            {
                Label lbl = sender as Label;
                ProjectName = lbl.Name.ToString().Split('_')[0];
                Projectid = Convert.ToInt32(lbl.Name.ToString().Split('_')[1]);
                todoName = lbl.Name.ToString().Split('_')[2];
                label5.Text = todoName;
                label1.Text = ProjectName;
                todoId = Convert.ToInt32(lbl.Name.ToString().Split('_')[3]);
                var panel = ProjectName + "_" + Projectid + "_" + todoName + "_" + todoId + "_Panel";
                Control cc = this.Controls.Find(panel, true)[0];
                Task_Click(cc, e);
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: "+lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }               
                return;
            }
        }

        /// <summary>
        /// Top start button click
        /// </summary> 
        private async void TopStartButton_Click(object sender, EventArgs e)
        {
            try
            {
               
                started = true;
                hook = SetHook(llkProcedure);
                hook1 = SetHook1(llkProcedure1);

                button1.Enabled = false;

                TopStopButton.Visible = true;
                TopStartButton.Visible = false;
                Random r = new Random();
                int rInt = r.Next(5,12);              
                lblCountDown.BackColor = Color.FromArgb(27, 129, 233);
                timerstartedfor = ProjectName + "_" + Projectid + "_" + todoName + "_" + todoId;
                label1.Text = ProjectName;
                label5.Text = todoName;
                var splitTimer = timerstartedfor.Split('_');
                var ButtonName = Convert.ToInt32(splitTimer[3]) != 0 ? splitTimer[0] + "_" + splitTimer[1] + "_" + splitTimer[2] + "_" + splitTimer[3] : splitTimer[0] + "_" + splitTimer[1];

                var crntstrtbtn = this.Controls.Find(ButtonName + "_StartButton", true)[0];
                var crntstopbtn = this.Controls.Find(ButtonName + "_StopButton", true)[0];
                crntstrtbtn.Visible = false;
                crntstopbtn.Visible = true;

                //if (Directory.Exists(@"C:\Users\Public\Documents\Local\"))
                //{
                //    string[] filePaths = Directory.GetFiles(@"C:\Users\Public\Documents\Local\");
                //    if (filePaths.Count() > 0)

                //    {
                //        Image img;
                //        foreach (var item in filePaths)
                //        {
                //            var response = "";
                //            var name = "";
                //            var strt = new DateTime();
                //            var curr = new DateTime();
                //            var stream = new System.IO.MemoryStream();
                //            int Projectid;
                //            int elapse;
                //            int mouseclick;
                //            int keyboardcount;
                //            int todoid;
                //            int userid;
                //            using (Bitmap bm = new Bitmap(item))
                //            {
                //                name = Path.GetFileName(item);
                //                strt = Convert.ToDateTime(name.Split('_')[3].Replace("#", "/").Replace("@", ":"));
                //                curr = Convert.ToDateTime(name.Split('_')[4].Replace("#", "/").Replace("@", ":"));
                //                img = ReduceImage(bm);
                //                // bm.Save(stream, ImageFormat.Jpeg);
                //                Projectid = Convert.ToInt32(name.Split('_')[1]);
                //                elapse = Convert.ToInt32(name.Split('_')[2]);
                //                mouseclick = Convert.ToInt32(name.Split('_')[5]);
                //                keyboardcount = Convert.ToInt32(name.Split('_')[6]);
                //                todoid = Convert.ToInt32(name.Split('_')[7]);
                //                userid = Convert.ToInt32(name.Split('_')[9].Split('.')[0]);
                //                WriteLog("log.txt", "Local files: " + name);
                //            }

                //            response = await CallSaveScreenshotAPI(img, name.Split('_')[0], Projectid, elapse, strt, curr, mouseclick, keyboardcount, todoid, "CURRENT", userid);
                //            if (response.ToString().Contains("\"status\":1"))
                //            {
                //                File.Delete(item);
                //                WriteLog("log.txt", "File deleted: " + name);
                //            }
                //            else
                //            {
                //                WriteLog("log.txt", "CallSavescreenshotAPI response in topstartbuttonclick: " + response);
                //            }
                //        }

                //    }
                //}

                var str = "";
                if (Convert.ToInt32(splitTimer[3]) != 0)
                {
                    str = "Started timer for todo " + splitTimer[2] + " in project " + splitTimer[0];
                }
                else
                {
                    str = "Started timer for project " + splitTimer[0] + ".";
                }

                showBalloon("VA Vision", str);               
                var response = await GetProjectDetails();
                var TotalSeconds = response.data.FirstOrDefault(x => x.project_id == Convert.ToInt32(timerstartedfor.Split('_')[1])).time_duration_sec;
                Totalworked = response.data.Sum(x => Convert.ToInt32(x.time_duration_sec)) + Convert.ToInt32(response.break_duration_sec);
                var time = CalculateTime(Totalworked.ToString());
                Totalhour = time.hours; Totalminute = time.minutes; Totalsecond = time.seconds;
                var totlTme = (Totalhour < 10 ? "0" + Totalhour : Totalhour.ToString()) + ":" + (Totalminute < 10 ? "0" + Totalminute.ToString() : Totalminute.ToString()) + ":" + (Totalsecond < 10 ? "0" + Totalsecond.ToString() : Totalsecond.ToString());
                label3.Text = totlTme;
                // PreviousTime = Convert.ToInt32(TotalSeconds);
                StartTime = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
                var Time = CalculateTime(TotalSeconds);
                second = Time.seconds;
                minute = Time.minutes;
                hour = Time.hours;
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000; // 1 second
                timer1.Start();              
                lblCountDown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                var image = CaptureScreen("START");
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
               return;
            }
        }

        /// <summary>
        /// Top stop button click
        /// </summary> 
        private void TopStopButton_Click(object sender, EventArgs e)
        {
            try
            {
                started = false;
                button1.Enabled = true;
                TopStopButton.Visible = false;
                TopStartButton.Visible = true;
                var splitTimer = timerstartedfor.Split('_');
                var ButtonName = Convert.ToInt32(splitTimer[3]) != 0 ? splitTimer[0] + "_" + splitTimer[1] + "_" + splitTimer[2] + "_" + splitTimer[3] + "_StartButton" : splitTimer[0] + "_" + splitTimer[1] + "_StartButton";
                Control cc = this.Controls.Find(ButtonName, true)[0];
                cc.Visible = true;
                var StopBtn = Convert.ToInt32(splitTimer[3]) != 0 ? splitTimer[0] + "_" + splitTimer[1] + "_" + splitTimer[2] + "_" + splitTimer[3] + "_StopButton" : splitTimer[0] + "_" + splitTimer[1] + "_StopButton"; ;
                var crntstopbtn = this.Controls.Find(StopBtn, true)[0];
                crntstopbtn.Visible = false;
                timer1.Stop();
                lblCountDown.BackColor = Color.FromArgb(45, 49, 55);
                var str = "";
                if (Convert.ToInt32(splitTimer[3]) != 0)
                {
                    str = "Stopped timer for todo " + splitTimer[2] + " in project " + splitTimer[0] + ".";
                }
                else
                {
                    str = "Stopped timer for project " + splitTimer[0] + ".";
                }
                showBalloon("VA Vision", str);
                var image = CaptureScreen("STOP");
                UnhookWindowsHookEx(hook);
                UnhookWindowsHookEx(hook1);
                KeyboardMovements = 0;
                MouseMoves = 0;
            }
            catch (Exception ex)
            {
                started = false;
                button1.Enabled = true;
                TopStopButton.Visible = false;
                TopStartButton.Visible = true;
                timer1.Stop();
                lblCountDown.BackColor = Color.FromArgb(45, 49, 55);
                var str = "";
               
                    str = "Stopped timer for project." ;
               
                showBalloon("VA Vision", str);
                var image = CaptureScreen("STOP");
                UnhookWindowsHookEx(hook);
                UnhookWindowsHookEx(hook1);
                KeyboardMovements = 0;
                MouseMoves = 0;

                var lineno = ex.LineNumber();
                              
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno+ timerstartedfor+ ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }

        }   

        /// <summary>
        /// Get total seconds
        /// </summary> 
        public int GetTotal(string[] timing)
        {
            var totSecond = 0;
            if (timing[0] != "00")
            {
                var hrs = Convert.ToInt32(timing[0]);
                totSecond = hrs * 3600;
            }
            if (timing[1] != "00")
            {
                var min = Convert.ToInt32(timing[1]);
                totSecond += min * 60;
            }
            if (timing[2] != "00")
            {
                totSecond += Convert.ToInt32(timing[2]);
            }
            return totSecond;
        }


        /// <summary>
        /// Get timezone abbreviation
        /// </summary> 
        public string GetTimezone()
        {
            TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            string tzid = curTimeZone.StandardName;
            string lang = CultureInfo.CurrentCulture.Name;   // example: "en-US"
            var abbreviations = TZNames.GetAbbreviationsForTimeZone(tzid, lang);
            var TimeZoneName = abbreviations.Standard;
            return TimeZoneName;
        }

        /// <summary>
        /// Capture screen
        /// </summary> 
        public Screenshot Capture()
        {
            var bmpScreenshot1 = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                              Screen.PrimaryScreen.Bounds.Height,
                                              PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot1 = Graphics.FromImage(bmpScreenshot1);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot1.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            var guid1 = NextScreenshot.Day.ToString() + NextScreenshot.Month.ToString() + NextScreenshot.Year.ToString() + NextScreenshot.TimeOfDay.ToString("hh") + NextScreenshot.ToString("mm")+".JPEG";

            Screenshot sc = new Screenshot();
            sc.FileName = guid1;
            sc.image = bmpScreenshot1;
            return sc;
        }


        /// <summary>
        /// Save file
        /// </summary> 
        public string SaveFile(string root1, Screenshot saved1)
        {
            var Path = "";
            if (Directory.Exists(root1))
            {
                Path = root1 + saved1.FileName + ".JPEG";
                saved1.image.Save(Path, ImageFormat.Jpeg);
            }
            else
            {
                Directory.CreateDirectory(root1);
                Path = root1 + saved1.FileName + ".JPEG";
                saved1.image.Save(Path, ImageFormat.Jpeg);
            }
            return Path;
        }


        /// <summary>
        /// SAve image when net is off
        /// </summary> 
        public string SaveFile1(string root1,int project_id,double elapse,int todoid_id, Screenshot saved1)
        {
            var Path = "";
            var strt = StartTime.ToString().Replace("/", "#").Replace(":", "@");
            var crnt = CurrentTime.ToString().Replace("/", "#").Replace(":", "@");
            Path = root1 + saved1.FileName + "_" + project_id + "_" + Convert.ToInt32(elapse) + "_" + strt + "_" + crnt + "_" + MouseMoves + "_" + KeyboardMovements + "_" + todoid_id + "_" + "STOP" + "_" + user.data.user_id + ".JPEG";
            if (Directory.Exists(root1))
            {
                saved1.image.Save(Path, ImageFormat.Jpeg);
            }
            else
            {
                Directory.CreateDirectory(root1);
                saved1.image.Save(Path, ImageFormat.Jpeg);
            }
            return Path;
        }


        /// <summary>
        /// Project/todo start button click
        /// </summary> 
        private async void ProjectTodoStartButton_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                btn.Visible = false;
                button1.Enabled = false;
                var str = "";
                var buttonname = btn.Name.ToString();
                var SplitBtnName = buttonname.Split('_');
                var pnl = this.Controls.Find(buttonname.Replace("StartButton", "Panel"), true)[0];


                var panel = this.Controls.Find("MainPanel", true)[0].Controls.OfType<Panel>().Where(x => x.Name.EndsWith("_Panel"));

                foreach (Panel P in panel)
                {
                    var strtbutton = P.Name.Replace("Panel", "StartButton");
                    P.BackColor = Color.FromArgb(255, 255, 255);
                    var button = P.Controls.Find(strtbutton, true)[0];
                    button.Visible = false;
                }
                            
                pnl.BackColor = Color.FromArgb(198, 223, 249);
                ProjectName = pnl.Name.ToString().Split('_')[0];
                Projectid = Convert.ToInt32(pnl.Name.ToString().Split('_')[1]);
                todoName = pnl.Name.Split('_').Count() > 3 ? pnl.Name.Split('_')[2] : "";
                label5.Text = todoName;
                todoId = pnl.Name.Split('_').Count() > 3 ? Convert.ToInt32(pnl.Name.Split('_')[3]) : 0;
                label1.Text = ProjectName;


                if (timerstartedfor != null)
                {
                    var hello = SplitBtnName[1];
                    if (timerstartedfor.Split('_')[1] != hello)
                    {
                        minute = 0;
                        hour = 0;
                        second = 0;
                    }
                    if (started)
                    {

                        var timerstartedsplit = timerstartedfor.Split('_');
                        var ButtonName = Convert.ToInt32(timerstartedsplit[3]) != 0 ? timerstartedsplit[0] + "_" + timerstartedsplit[1] + "_" + timerstartedsplit[2] + "_" + timerstartedsplit[3] + "_StopButton" : timerstartedsplit[0] + "_" + timerstartedsplit[1] + "_StopButton";
                        var stopbutton = this.Controls.Find(ButtonName, true)[0];
                        stopbutton.Visible = false;

                        timer1.Stop();
                        KeyboardMovements = 0;
                        MouseMoves = 0;
                        if (Convert.ToInt32(timerstartedsplit[3]) != 0)
                        {
                            str = "Stopped timer for todo " + timerstartedsplit[2] + " in project " + timerstartedsplit[0] + ".";
                        }
                        else
                        {
                            str = "Stopped timer for project " + timerstartedsplit[0] + ".";
                        }
                        showBalloon("Va Vision", str);

                        #region CaptureScreen for 

                        await CaptureScreen("STOP");
                      
                        #endregion
                     }
                }

                started = true;

                timerstartedfor = SplitBtnName.Count() > 3 ? SplitBtnName[0] + "_" + SplitBtnName[1] + "_" + SplitBtnName[2] + "_" + SplitBtnName[3] : SplitBtnName[0] + "_" + SplitBtnName[1] + "_" + " " + "_" + 0;
                Control cc = this.Controls.Find(buttonname.Replace("StartButton", "StopButton"), true)[0];
                cc.Visible = true;
                started = true;
                hook = SetHook(llkProcedure);
                hook1 = SetHook1(llkProcedure1);
                TopStopButton.Visible = true;
                TopStartButton.Visible = false;
                Random r = new Random();
                int rInt = r.Next(5,12);              
                lblCountDown.BackColor = Color.FromArgb(27, 129, 233);
                var str1 = "";
                var timersplit = timerstartedfor.Split('_');
                if (Convert.ToInt32(timerstartedfor.Split('_')[3]) != 0)
                {
                    str1 = "Started timer for todo " + timersplit[2] + " in project " + timersplit[0];
                }
                else
                {
                    str1 = "Started timer for project " + timersplit[0] + ".";
                }
                showBalloon("VA Vision", str1);


                #region Capturescreen for start                 

                 #endregion
                var response = await GetProjectDetails(); 
                var TotalSeconds = response.data.FirstOrDefault(x => x.project_id == Convert.ToInt32(timerstartedfor.Split('_')[1])).time_duration_sec;
                Totalworked = response.data.Sum(x => Convert.ToInt32(x.time_duration_sec)) + Convert.ToInt32(response.break_duration_sec);

                var time = CalculateTime(Totalworked.ToString());
                Totalhour = time.hours; Totalminute = time.minutes; Totalsecond = time.seconds;
                var totlTme = (Totalhour < 10 ? "0" + Totalhour : Totalhour.ToString()) + ":" + (Totalminute < 10 ? "0" + Totalminute.ToString() : Totalminute.ToString()) + ":" + (Totalsecond < 10 ? "0" + Totalsecond.ToString() : Totalsecond.ToString());
                label3.Text = totlTme;
                // PreviousTime =Convert.ToInt32(TotalSeconds);
                StartTime = DateTime.UtcNow.AddHours(UTCOffset).AddMinutes(minutes);
                var Time = CalculateTime(TotalSeconds);
                second = Time.seconds;
                minute = Time.minutes;
                hour = Time.hours;
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000; // 1 second
                timer1.Start();               
                lblCountDown.Text = (hour < 10 ? "0" + hour : hour.ToString()) + ":" + (minute < 10 ? "0" + minute : minute.ToString()) + ":" + (second < 10 ? "0" + second : second.ToString());
                await CaptureScreen("START");
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno+ timerstartedfor + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }


        /// <summary>
        /// Get project details
        /// </summary> 
        public async Task<ProjectRecord> GetProjectDetails()
        {
            using (var client = new HttpClient())
            {
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
                        MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                        showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                    }
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    multipartContent.Add(new StringContent(user.data.user_id.ToString()), "userId");
                    var response = await client.PostAsync(BaseURL + "get_project_duration", multipartContent);
                    var result = await response.Content.ReadAsStringAsync();
                    WriteLog("log.txt", DateTime.Now + " " + "params : userId: " + user.data.user_id);
                    WriteLog("log.txt", "Projectduration Response in projectdetails: " + result);
                    try
                    {
                        ProjectTrack = JsonConvert.DeserializeObject<ProjectRecord>(result);
                        return ProjectTrack;
                    }
                    catch (Exception ex)
                    {
                        var lineno = ex.LineNumber();
                        WriteLog("log.txt","Result: "+ result+" Line: "+lineno);
                        MessageBox.Show("Something went wrong. Restart the app to continue.",AppName+" - "+Version);
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// Project/todo stop button click
        /// </summary> 
        private void ProjectTodoStopButton_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = true;
                started = false;
                var btn = sender as Button;
                btn.Visible = false;
                TopStopButton.PerformClick();
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }


        /// <summary>
        /// On closing the main screen
        /// </summary> 
        private void closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!logedout)
                {
                    MessageBox.Show("You need to logout first.", AppName + " - " + Version);
                e.Cancel = true;
                this.Activate();
             }
                //if (!logedout && Global.SendNotification)
                //{
                //    showBalloon("Closed", "Window was closed.");
                //}
                //CurrentTime = DateTime.UtcNow.AddHours(UTCOffset);
                //if (!logedout)
                //{
                //    var image = CaptureScreen("STOP");
                //}
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
               return;
            }
        }

        /// <summary>
        /// Logout button click
        /// </summary> 
        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {
                logedout = true;
                if (Global.SendNotification)
                {
                    showBalloon("Logout", "Logged out");
                }
               // var image = CaptureScreen("STOP");
                CurrentTime = DateTime.UtcNow.AddHours(UTCOffset);               
                this.Close();
                LoginScreen lg = new LoginScreen();
                lg.ShowDialog();
            }
            catch (Exception ex)
            {
                var lineno = ex.LineNumber();
                WriteLog("Log.txt", DateTime.Now + " Line: " + lineno + ex.Message + ex.StackTrace);
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("We are facing some internet connectivity issue.",AppName+" - "+Version);
                    showBalloon("VA Vision", "ERROR: Please check your internet connection.");
                }
                else
                {
                    MessageBox.Show("Something went wrong, please restart the app to continue.",AppName+" - "+Version);
                }
                return;
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label5_Click_2(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        private async void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {          
            var projectdetails = await GetProjectDetails();
            var Totalworked = projectdetails.data.Sum(x => Convert.ToInt32(x.time_duration_sec)) + Convert.ToInt32(ProjectTrack.break_duration_sec);
            var time = CalculateTime(Totalworked.ToString());
            Totalhour = time.hours; Totalminute = time.minutes; Totalsecond = time.seconds;
            var totlTme = (Totalhour < 10 ? "0" + Totalhour : Totalhour.ToString()) + ":" + (Totalminute < 10 ? "0" + Totalminute.ToString() : Totalminute.ToString()) + ":" + (Totalsecond < 10 ? "0" + Totalsecond.ToString() : Totalsecond.ToString());
            label3.Text = totlTme;
            var brektime = CalculateTime(ProjectTrack.break_duration_sec);           
            brkavailed.Text = (brektime.hours < 10 ? "0" + brektime.hours.ToString() : brektime.hours.ToString()) + ":" + (brektime.minutes < 10 ? "0" + brektime.minutes.ToString() : brektime.minutes.ToString())+":"+ (brektime.seconds < 10 ? "0" + brektime.seconds.ToString() : brektime.seconds.ToString());
            if (brektime.minutes > 20)
            {
                brkavailed.BackColor = Color.Red;
            }
            StartBreak.Enabled = true;
        }

        private async void StartBreak_Click(object sender, EventArgs e)
        {
            if (TopStopButton.Visible == true)
            {
                TopStopButton_Click(TopStopButton, null);
            }
            StartBreak.Enabled = false;
           
            var split = timerstartedfor;
            var response = await GetProjectDetails();
            var breakseconds =  Convert.ToInt32(response.break_duration_sec);

            Global.BreakSeconds = breakseconds.ToString();           
              var breaktime =  CalculateTime(breakseconds.ToString());
              Global.BreakTime = (breaktime.hours < 10 ? "0" + breaktime.hours.ToString() : breaktime.hours.ToString()) + ":" + (breaktime.minutes < 10 ? "0" + breaktime.minutes.ToString() : breaktime.minutes.ToString())+":"+ (breaktime.seconds < 10 ? "0" + breaktime.seconds.ToString() : breaktime.seconds.ToString());
           
         
            //Global.ProjectId = Convert.ToInt32(timerstartedfor.Split('_')[1]);
            //Global.TodoId = Convert.ToInt32(timerstartedfor.Split('_')[3]);
            Global.StartTime = StartTime;
            Break br = new Break();
            br.FormClosing += new FormClosingEventHandler(this.Form2_FormClosing);
            br.ShowDialog();          
           
        }
    }

    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {

            int linenum = 0;
            try
            {
                //linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

                //For Localized Visual Studio ... In other languages stack trace  doesn't end with ":Line 12"
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));

            }


            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
    }

}
