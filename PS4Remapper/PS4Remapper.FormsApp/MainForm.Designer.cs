namespace PS4Remapper.FormsApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonInject = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.axisDisplay = new PS4Remapper.FormsApp.Controls.AxisDisplay();
            this.labelKey = new System.Windows.Forms.Label();
            this.labelMouse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(12, 12);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(265, 59);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "Inject";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // axisDisplay
            // 
            this.axisDisplay.InnerColor = System.Drawing.Color.GhostWhite;
            this.axisDisplay.InnerSize = 12;
            this.axisDisplay.Location = new System.Drawing.Point(312, 12);
            this.axisDisplay.Name = "axisDisplay";
            this.axisDisplay.OuterColor = System.Drawing.Color.DodgerBlue;
            this.axisDisplay.Size = new System.Drawing.Size(150, 150);
            this.axisDisplay.TabIndex = 1;
            this.axisDisplay.Value = ((System.Drawing.PointF)(resources.GetObject("axisDisplay.Value")));
            // 
            // labelKey
            // 
            this.labelKey.AutoSize = true;
            this.labelKey.Location = new System.Drawing.Point(12, 93);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(61, 25);
            this.labelKey.TabIndex = 2;
            this.labelKey.Text = "Key: ";
            // 
            // labelMouse
            // 
            this.labelMouse.AutoSize = true;
            this.labelMouse.Location = new System.Drawing.Point(12, 122);
            this.labelMouse.Name = "labelMouse";
            this.labelMouse.Size = new System.Drawing.Size(89, 25);
            this.labelMouse.TabIndex = 3;
            this.labelMouse.Text = "Mouse: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 176);
            this.Controls.Add(this.labelMouse);
            this.Controls.Add(this.labelKey);
            this.Controls.Add(this.axisDisplay);
            this.Controls.Add(this.buttonInject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "PS4 Remapper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonInject;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Controls.AxisDisplay axisDisplay;
        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.Label labelMouse;
    }
}