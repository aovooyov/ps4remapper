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
            this.buttonDebugKeyboard = new System.Windows.Forms.Button();
            this.buttonDebugMouse = new System.Windows.Forms.Button();
            this.sensitivity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.decayRate = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.decayThreshold = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.analogDeadzone = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.makeupSpeed = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.axisDisplay = new PS4Remapper.FormsApp.Controls.AxisDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analogDeadzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.makeupSpeed)).BeginInit();
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
            this.labelKey.Location = new System.Drawing.Point(187, 147);
            this.labelKey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelKey.Name = "labelKey";
            this.labelKey.Size = new System.Drawing.Size(40, 17);
            this.labelKey.TabIndex = 2;
            this.labelKey.Text = "Key: ";
            // 
            // labelMouse
            // 
            this.labelMouse.AutoSize = true;
            this.labelMouse.Location = new System.Drawing.Point(187, 113);
            this.labelMouse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMouse.Name = "labelMouse";
            this.labelMouse.Size = new System.Drawing.Size(58, 17);
            this.labelMouse.TabIndex = 3;
            this.labelMouse.Text = "Mouse: ";
            // 
            // buttonDebugKeyboard
            // 
            this.buttonDebugKeyboard.Location = new System.Drawing.Point(11, 50);
            this.buttonDebugKeyboard.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDebugKeyboard.Name = "buttonDebugKeyboard";
            this.buttonDebugKeyboard.Size = new System.Drawing.Size(129, 38);
            this.buttonDebugKeyboard.TabIndex = 0;
            this.buttonDebugKeyboard.Text = "Debug Keyboard";
            this.buttonDebugKeyboard.UseVisualStyleBackColor = true;
            this.buttonDebugKeyboard.Click += new System.EventHandler(this.buttonDebugKeyboard_Click);
            // 
            // buttonDebugMouse
            // 
            this.buttonDebugMouse.Location = new System.Drawing.Point(11, 92);
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
            this.sensitivity.Location = new System.Drawing.Point(462, 7);
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
            this.label1.Location = new System.Drawing.Point(340, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sensitivity";
            // 
            // decayRate
            // 
            this.decayRate.DecimalPlaces = 2;
            this.decayRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.decayRate.Location = new System.Drawing.Point(462, 38);
            this.decayRate.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.decayRate.Name = "decayRate";
            this.decayRate.Size = new System.Drawing.Size(82, 22);
            this.decayRate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(340, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Decay Rate";
            // 
            // decayThreshold
            // 
            this.decayThreshold.DecimalPlaces = 2;
            this.decayThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.decayThreshold.Location = new System.Drawing.Point(462, 69);
            this.decayThreshold.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.decayThreshold.Name = "decayThreshold";
            this.decayThreshold.Size = new System.Drawing.Size(82, 22);
            this.decayThreshold.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Decay Threshold";
            // 
            // analogDeadzone
            // 
            this.analogDeadzone.DecimalPlaces = 2;
            this.analogDeadzone.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.analogDeadzone.Location = new System.Drawing.Point(462, 101);
            this.analogDeadzone.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.analogDeadzone.Name = "analogDeadzone";
            this.analogDeadzone.Size = new System.Drawing.Size(82, 22);
            this.analogDeadzone.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(340, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Analog Deadzone";
            // 
            // makeupSpeed
            // 
            this.makeupSpeed.DecimalPlaces = 2;
            this.makeupSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.makeupSpeed.Location = new System.Drawing.Point(462, 134);
            this.makeupSpeed.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.makeupSpeed.Name = "makeupSpeed";
            this.makeupSpeed.Size = new System.Drawing.Size(82, 22);
            this.makeupSpeed.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(340, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Makeup Speed";
            // 
            // axisDisplay
            // 
            this.axisDisplay.InnerColor = System.Drawing.Color.GhostWhite;
            this.axisDisplay.InnerSize = 12;
            this.axisDisplay.Location = new System.Drawing.Point(208, 8);
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
            this.ClientSize = new System.Drawing.Size(337, 173);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.makeupSpeed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.analogDeadzone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.decayThreshold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.decayRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sensitivity);
            this.Controls.Add(this.labelMouse);
            this.Controls.Add(this.labelKey);
            this.Controls.Add(this.axisDisplay);
            this.Controls.Add(this.buttonDebugMouse);
            this.Controls.Add(this.buttonDebugKeyboard);
            this.Controls.Add(this.buttonInject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "PS4 Remapper";
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analogDeadzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.makeupSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonInject;
        private Controls.AxisDisplay axisDisplay;
        private System.Windows.Forms.Label labelKey;
        private System.Windows.Forms.Label labelMouse;
        private System.Windows.Forms.Button buttonDebugKeyboard;
        private System.Windows.Forms.Button buttonDebugMouse;
        private System.Windows.Forms.NumericUpDown sensitivity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown decayRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown decayThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown analogDeadzone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown makeupSpeed;
        private System.Windows.Forms.Label label5;
    }
}