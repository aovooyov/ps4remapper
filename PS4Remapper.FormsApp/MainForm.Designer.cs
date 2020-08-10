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
            this.labelKey = new System.Windows.Forms.Label();
            this.labelMouse = new System.Windows.Forms.Label();
            this.buttonDebugMouse = new System.Windows.Forms.Button();
            this.sensitivity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.deadZone = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tickRate = new System.Windows.Forms.NumericUpDown();
            this.axisDisplay = new PS4Remapper.FormsApp.Controls.AxisDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickRate)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonInject
            // 
            this.buttonInject.Location = new System.Drawing.Point(11, 8);
            this.buttonInject.Margin = new System.Windows.Forms.Padding(2);
            this.buttonInject.Name = "buttonInject";
            this.buttonInject.Size = new System.Drawing.Size(129, 38);
            this.buttonInject.TabIndex = 0;
            this.buttonInject.Text = "Inject";
            this.buttonInject.UseVisualStyleBackColor = true;
            this.buttonInject.Click += new System.EventHandler(this.buttonInject_Click);
            // 
            // labelKey
            // 
            this.labelKey.AutoSize = true;
            this.labelKey.Location = new System.Drawing.Point(278, 157);
            this.labelKey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(40, 17);
            this.labelKey.TabIndex = 2;
            this.labelKey.Text = "Key: ";
            // 
            // labelMouse
            // 
            this.labelMouse.AutoSize = true;
            this.labelMouse.Location = new System.Drawing.Point(278, 122);
            this.labelMouse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMouse.Name = "labelMouse";
            this.labelMouse.Size = new System.Drawing.Size(58, 17);
            this.labelMouse.TabIndex = 3;
            this.labelMouse.Text = "Mouse: ";
            // 
            // buttonDebugMouse
            // 
            this.buttonDebugMouse.Location = new System.Drawing.Point(11, 50);
            this.buttonDebugMouse.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDebugMouse.Name = "buttonDebugMouse";
            this.buttonDebugMouse.Size = new System.Drawing.Size(129, 38);
            this.buttonDebugMouse.TabIndex = 0;
            this.buttonDebugMouse.Text = "Debug Mouse";
            this.buttonDebugMouse.UseVisualStyleBackColor = true;
            this.buttonDebugMouse.Click += new System.EventHandler(this.buttonDebugMouse_Click);
            // 
            // sensitivity
            // 
            this.sensitivity.DecimalPlaces = 2;
            this.sensitivity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sensitivity.Location = new System.Drawing.Point(361, 9);
            this.sensitivity.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.sensitivity.Name = "sensitivity";
            this.sensitivity.Size = new System.Drawing.Size(82, 22);
            this.sensitivity.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(278, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sensitivity";
            // 
            // deadZone
            // 
            this.deadZone.DecimalPlaces = 2;
            this.deadZone.Location = new System.Drawing.Point(361, 40);
            this.deadZone.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.deadZone.Name = "deadZone";
            this.deadZone.Size = new System.Drawing.Size(82, 22);
            this.deadZone.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Dead zone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(278, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Tick rate";
            // 
            // tickRate
            // 
            this.tickRate.DecimalPlaces = 2;
            this.tickRate.Location = new System.Drawing.Point(361, 71);
            this.tickRate.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.tickRate.Name = "tickRate";
            this.tickRate.Size = new System.Drawing.Size(82, 22);
            this.tickRate.TabIndex = 6;
            // 
            // axisDisplay
            // 
            this.axisDisplay.Location = new System.Drawing.Point(161, 7);
            this.axisDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.axisDisplay.Name = "axisDisplay";
            this.axisDisplay.OuterColor = System.Drawing.Color.DodgerBlue;
            this.axisDisplay.Size = new System.Drawing.Size(100, 100);
            this.axisDisplay.TabIndex = 1;
            this.axisDisplay.Value = ((System.Drawing.PointF)(resources.GetObject("axisDisplay.Value")));
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 183);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tickRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deadZone);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sensitivity);
            this.Controls.Add(this.labelMouse);
            this.Controls.Add(this.labelKey);
            this.Controls.Add(this.axisDisplay);
            this.Controls.Add(this.buttonDebugMouse);
            this.Controls.Add(this.buttonInject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "PS4 Remapper";
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deadZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonInject;
        private Controls.AxisDisplay axisDisplay;
        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.Label labelMouse;
        private System.Windows.Forms.Button buttonDebugMouse;
        private System.Windows.Forms.NumericUpDown sensitivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown deadZone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown tickRate;
    }
}