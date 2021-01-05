using System.Windows.Forms;

namespace VAVision
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblCountDown = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.breaklbl = new System.Windows.Forms.Label();
            this.brkavailed = new System.Windows.Forms.Label();
            this.StartBreak = new System.Windows.Forms.Button();
            this.TopStopButton = new VAVision.CustomizedButton();
            this.TopStartButton = new VAVision.CustomizedButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Screenshot";
            this.notifyIcon1.Visible = true;
            // 
            // lblCountDown
            // 
            this.lblCountDown.AutoSize = true;
            this.lblCountDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(49)))), ((int)(((byte)(55)))));
            this.lblCountDown.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountDown.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCountDown.Location = new System.Drawing.Point(130, 43);
            this.lblCountDown.Name = "lblCountDown";
            this.lblCountDown.Padding = new System.Windows.Forms.Padding(20, 5, 20, 5);
            this.lblCountDown.Size = new System.Drawing.Size(128, 32);
            this.lblCountDown.TabIndex = 2;
            this.lblCountDown.Text = "00:00:00";
            this.lblCountDown.Click += new System.EventHandler(this.label1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 22);
            this.label1.TabIndex = 6;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total worked today:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(226, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 14);
            this.label3.TabIndex = 8;
            this.label3.Text = "00:00:00";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(300, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 26);
            this.button1.TabIndex = 14;
            this.button1.Text = "Logout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(134, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(34, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(318, 22);
            this.label5.TabIndex = 16;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // breaklbl
            // 
            this.breaklbl.AutoSize = true;
            this.breaklbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.breaklbl.Location = new System.Drawing.Point(87, 173);
            this.breaklbl.Name = "breaklbl";
            this.breaklbl.Size = new System.Drawing.Size(134, 15);
            this.breaklbl.TabIndex = 17;
            this.breaklbl.Text = "Total break availed:";
            this.breaklbl.Click += new System.EventHandler(this.label6_Click);
            // 
            // brkavailed
            // 
            this.brkavailed.AutoSize = true;
            this.brkavailed.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brkavailed.Location = new System.Drawing.Point(227, 173);
            this.brkavailed.Name = "brkavailed";
            this.brkavailed.Size = new System.Drawing.Size(57, 14);
            this.brkavailed.TabIndex = 18;
            this.brkavailed.Text = "00:00:00";
            // 
            // StartBreak
            // 
            this.StartBreak.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartBreak.Location = new System.Drawing.Point(200, 12);
            this.StartBreak.Name = "StartBreak";
            this.StartBreak.Size = new System.Drawing.Size(95, 26);
            this.StartBreak.TabIndex = 19;
            this.StartBreak.Text = "Start Break";
            this.StartBreak.UseVisualStyleBackColor = true;
            this.StartBreak.Click += new System.EventHandler(this.StartBreak_Click);
            // 
            // TopStopButton
            // 
            this.TopStopButton.BackColor = System.Drawing.Color.Transparent;
            this.TopStopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TopStopButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.TopStopButton.FlatAppearance.BorderSize = 0;
            this.TopStopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.TopStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TopStopButton.Image = ((System.Drawing.Image)(resources.GetObject("TopStopButton.Image")));
            this.TopStopButton.Location = new System.Drawing.Point(23, 43);
            this.TopStopButton.Name = "TopStopButton";
            this.TopStopButton.Size = new System.Drawing.Size(36, 35);
            this.TopStopButton.TabIndex = 5;
            this.TopStopButton.UseVisualStyleBackColor = false;
            this.TopStopButton.Click += new System.EventHandler(this.TopStopButton_Click);
            // 
            // TopStartButton
            // 
            this.TopStartButton.BackColor = System.Drawing.SystemColors.Menu;
            this.TopStartButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TopStartButton.FlatAppearance.BorderSize = 0;
            this.TopStartButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.TopStartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TopStartButton.Image = ((System.Drawing.Image)(resources.GetObject("TopStartButton.Image")));
            this.TopStartButton.Location = new System.Drawing.Point(23, 43);
            this.TopStartButton.Name = "TopStartButton";
            this.TopStartButton.Size = new System.Drawing.Size(36, 35);
            this.TopStartButton.TabIndex = 4;
            this.TopStartButton.UseVisualStyleBackColor = false;
            this.TopStartButton.Click += new System.EventHandler(this.TopStartButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(384, 636);
            this.Controls.Add(this.StartBreak);
            this.Controls.Add(this.brkavailed);
            this.Controls.Add(this.breaklbl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TopStopButton);
            this.Controls.Add(this.TopStartButton);
            this.Controls.Add(this.lblCountDown);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label lblCountDown;
        private System.Windows.Forms.BindingSource bindingSource1;
        private CustomizedButton TopStartButton;
        private CustomizedButton TopStopButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private Label label4;
        private Label label5;
        private Label breaklbl;
        private Label brkavailed;
        private Button StartBreak;
    }
}

