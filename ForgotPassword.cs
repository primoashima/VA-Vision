using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAVision
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private static string  BaseURL = System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
        private static string Version = System.Configuration.ConfigurationManager.AppSettings["CurrentVersion"];
        private static string AppName = System.Configuration.ConfigurationManager.AppSettings["APPName"];
        private static string FontType = System.Configuration.ConfigurationManager.AppSettings["Font"];
        public class response
        {
            public string status { get; set; }
            public string msg { get; set; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Email field is required.",AppName+" - "+Version);
                return;
            }
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                    var multipartContent = new MultipartFormDataContent();
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //  await stream.CopyToAsync(mem);
                    //  var byteContent = new ByteArrayContent(mem.ToArray());
                    //  multipartContent.Add(byteContent, "img", Filename + ".PNG");
                    var Email = textBox1.Text;
                    multipartContent.Add(new StringContent(Email), "email");
                    var response = await client.PostAsync(BaseURL + "forgotPassword", multipartContent);
                    var result = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<response>(result);    

                    DialogResult res = MessageBox.Show(data.msg, AppName + " - " + Version, MessageBoxButtons.OK);    
                    if (res == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter a valid email",AppName+" - "+Version);                  
                }
            }

        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {
            this.Text = AppName + " - " + Version; 
            label1.Font = new Font(FontType, label1.Font.Size);
            button1.Font = new Font(FontType, button1.Font.Size);
        }
    }
}
