using System.Windows.Forms;

namespace VAVision
{
    partial class Break
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Break));
            this.timerlbl = new System.Windows.Forms.Label();
            this.Breaktimelbl = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timerlbl
            // 
            this.timerlbl.AutoSize = true;
            this.timerlbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerlbl.Location = new System.Drawing.Point(166, 95);
            this.timerlbl.Name = "timerlbl";
            this.timerlbl.Size = new System.Drawing.Size(43, 15);
            this.timerlbl.TabIndex = 0;
            this.timerlbl.Text = "00:00";
            // 
            // Breaktimelbl
            // 
            this.Breaktimelbl.AutoSize = true;
            this.Breaktimelbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Breaktimelbl.Location = new System.Drawing.Point(48, 95);
            this.Breaktimelbl.Name = "Breaktimelbl";
            this.Breaktimelbl.Size = new System.Drawing.Size(116, 15);
            this.Breaktimelbl.TabIndex = 1;
            this.Breaktimelbl.Text = "Total break time:";
            // 
            // StopButton
            // 
            this.StopButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.Location = new System.Drawing.Point(243, 91);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(80, 23);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "End Break";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // Break
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 202);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.Breaktimelbl);
            this.Controls.Add(this.timerlbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Break";
            this.Text = "Break";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BreakForm_FormClosing);
            this.Load += new System.EventHandler(this.Break_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timerlbl;
        private System.Windows.Forms.Label Breaktimelbl;
        private System.Windows.Forms.Button StopButton;
    }
}