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
            this.buttonTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(11, 8);
            this.buttonInject.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(87, 38);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "Inject";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // axisDisplay
            // 
            this.axisDisplay.InnerColor = System.Drawing.Color.GhostWhite;
            this.axisDisplay.InnerSize = 12;
            this.axisDisplay.Location = new System.Drawing.Point(208, 8);
            this.axisDisplay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.axisDisplay.Name = "axisDisplay";
            this.axisDisplay.OuterColor = System.Drawing.Color.DodgerBlue;
            this.axisDisplay.Size = new System.Drawing.Size(100, 96);
            this.axisDisplay.TabIndex = 1;
            this.axisDisplay.Value = ((System.Drawing.PointF)(resources.GetObject("axisDisplay.Value")));
            // 
            // labelKey
            // 
            this.labelKey.AutoSize = true;
            this.labelKey.Location = new System.Drawing.Point(8, 60);
            this.labelKey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(40, 17);
            this.labelKey.TabIndex = 2;
            this.labelKey.Text = "Key: ";
            // 
            // labelMouse
            // 
            this.labelMouse.AutoSize = true;
            this.labelMouse.Location = new System.Drawing.Point(8, 78);
            this.labelMouse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMouse.Name = "labelMouse";
            this.labelMouse.Size = new System.Drawing.Size(58, 17);
            this.labelMouse.TabIndex = 3;
            this.labelMouse.Text = "Mouse: ";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(102, 8);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(87, 38);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 113);
            this.Controls.Add(this.labelMouse);
            this.Controls.Add(this.labelKey);
            this.Controls.Add(this.axisDisplay);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonInject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Button buttonTest;
    }
}