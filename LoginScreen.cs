using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VAVision;

namespace VAVision
{
    public partial class LoginScreen : Form
    {
        private static string BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        private static string PreviousVersion = System.Configuration.ConfigurationManager.AppSettings["PreviousVersion"];
        private static string Version = System.Configuration.ConfigurationManager.AppSettings["CurrentVersion"];
        private static string AppName = System.Configuration.ConfigurationManager.AppSettings["APPName"];
        private static string VersionNumber = System.Configuration.ConfigurationManager.AppSettings["VersionNumber"];
        private static string FontType = System.Configuration.ConfigurationManager.AppSettings["Font"];
        private static string LoginURL = System.Configuration.ConfigurationManager.AppSettings["LoginURL"];
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static HttpResponseMessage response;

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

        private void LoginScreen_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void MyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendKeys.Send("{TAB}");
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EmailText.Text))
                {
                    MessageBox.Show("Please enter email.",AppName +" - "+Version);
                    return;
                }
                else if (string.IsNullOrEmpty(PasswordText.Text))
                {
                    MessageBox.Show("Please enter password.",AppName + " - " + Version);
                    return;
                }
                button2.Enabled = false;
                using (var client = new HttpClient())
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
                    var multipartContent = new MultipartFormDataContent();
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //  await stream.CopyToAsync(mem);
                    //  var byteContent = new ByteArrayContent(mem.ToArray());
                    ////  multipartContent.Add(byteContent, "img", Filename + ".PNG");
                    var Email = EmailText.Text;
                    var Password = PasswordText.Text;
                    multipartContent.Add(new StringContent(Email), "email");
                    multipartContent.Add(new StringContent(Password), "password");
                    multipartContent.Add(new StringContent(VersionNumber), "build_version");
                    response = await client.PostAsync(BaseURL + "login", multipartContent);
                    var result = await response.Content.ReadAsStringAsync();
                    Global.GlobalVar = result;                   
                    var res = JsonConvert.DeserializeObject<ResponseModel>(result);
                    if (!result.Contains("status\":0"))
                    {
                        button2.Enabled = true;
                        Program.OpenDetailFormOnClose = true;
                        this.Close();
                    }
                    else
                    {
                        button2.Enabled = true;
                        if (res.error.Contains("update"))
                        {
                            var updatedmsg = res.error;
                            var final = res.build_date.build_link;
                            if (MessageBox.Show(
                         updatedmsg, AppName+" - "+Version, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk
                         ) == DialogResult.OK)
                            {
                                System.Diagnostics.Process.Start(final.ToString());
                            }
                        }
                       //else if (res.error.Contains("attendance"))
                       // {
                       //     var updatedmsg = res.error;
                       //     var logiURL = LoginURL;
                       //     if (MessageBox.Show(
                       //  updatedmsg, AppName + " - " + Version, MessageBoxButtons.OK, MessageBoxIcon.Asterisk
                       //  ) == DialogResult.OK)
                       //     {
                       //         System.Diagnostics.Process.Start(logiURL.ToString());
                       //     }
                       // }
                        else
                        {
                            MessageBox.Show(res.error, AppName + " - " + Version);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                button2.Enabled = true;
                MessageBox.Show("Invalid email or password.",AppName+" - "+Version);
                WriteLog("log.txt",ex.Message);
            }
         // Global.GlobalVar = result;
            //var data = JsonConvert.DeserializeObject(result);
        }

        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\Documents\Temp\log.txt");
                //if (lines.Any(x=> x.Contains(PreviousVersion)))
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

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(button2,null);
                e.SuppressKeyPress = true;
            }
        }

        public class ResponseModel
        {
        public string status { get; set; }
        public string error { get; set; }
            public string title { get; set; }
            public BuildDate build_date { get; set; }
        }

        public class BuildDate
        {
            public int build_id { get; set; }
            public string version_name { get; set; }
            public string publish_date { get; set; }
            public string build_link { get; set; }
            public string release_notes { get; set; }
            public int is_active { get; set; }
            public int is_deleted { get; set; }
            public string build_created_date { get; set; }
        }

        private async void CallLoginAPI()
        {

        }

        void f_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ForgotPassword fgpass = new ForgotPassword();
            fgpass.Show();
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {
            this.Text = AppName + " - " + Version;
            label1.Font = new Font(FontType,label1.Font.Size);
            label2.Font = new Font(FontType, label2.Font.Size);
            label3.Font = new Font(FontType, label3.Font.Size);
            button1.Font = new Font(FontType, button1.Font.Size);
            button2.Font = new Font(FontType, button2.Font.Size);
        }
    }
}
