namespace PS4Remapper.FormsApp
{
    partial class MouseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MouseForm));
            this.label5 = new System.Windows.Forms.Label();
            this.makeupSpeed = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.analogDeadzone = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.decayThreshold = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.decayRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.sensitivity = new System.Windows.Forms.NumericUpDown();
            this.labelMouse = new System.Windows.Forms.Label();
            this.axisDisplay = new PS4Remapper.FormsApp.Controls.AxisDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.makeupSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analogDeadzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 493);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Makeup Speed";
            // 
            // makeupSpeed
            // 
            this.makeupSpeed.DecimalPlaces = 2;
            this.makeupSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.makeupSpeed.Location = new System.Drawing.Point(688, 491);
            this.makeupSpeed.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.makeupSpeed.Name = "makeupSpeed";
            this.makeupSpeed.Size = new System.Drawing.Size(82, 22);
            this.makeupSpeed.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(566, 460);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Analog Deadzone";
            // 
            // analogDeadzone
            // 
            this.analogDeadzone.DecimalPlaces = 2;
            this.analogDeadzone.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.analogDeadzone.Location = new System.Drawing.Point(688, 458);
            this.analogDeadzone.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.analogDeadzone.Name = "analogDeadzone";
            this.analogDeadzone.Size = new System.Drawing.Size(82, 22);
            this.analogDeadzone.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(566, 428);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Decay Threshold";
            // 
            // decayThreshold
            // 
            this.decayThreshold.DecimalPlaces = 2;
            this.decayThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.decayThreshold.Location = new System.Drawing.Point(688, 426);
            this.decayThreshold.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.decayThreshold.Name = "decayThreshold";
            this.decayThreshold.Size = new System.Drawing.Size(82, 22);
            this.decayThreshold.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(566, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Decay Rate";
            // 
            // decayRate
            // 
            this.decayRate.DecimalPlaces = 2;
            this.decayRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.decayRate.Location = new System.Drawing.Point(688, 395);
            this.decayRate.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.decayRate.Name = "decayRate";
            this.decayRate.Size = new System.Drawing.Size(82, 22);
            this.decayRate.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(566, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Sensitivity";
            // 
            // sensitivity
            // 
            this.sensitivity.DecimalPlaces = 2;
            this.sensitivity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sensitivity.Location = new System.Drawing.Point(688, 364);
            this.sensitivity.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.sensitivity.Name = "sensitivity";
            this.sensitivity.Size = new System.Drawing.Size(82, 22);
            this.sensitivity.TabIndex = 10;
            // 
            // labelMouse
            // 
            this.labelMouse.AutoSize = true;
            this.labelMouse.Location = new System.Drawing.Point(12, 137);
            this.labelMouse.Name = "labelMouse";
            this.labelMouse.Size = new System.Drawing.Size(71, 17);
            this.labelMouse.TabIndex = 16;
            this.labelMouse.Text = "Mouse x;y";
            // 
            // axisDisplay
            // 
            this.axisDisplay.InnerColor = System.Drawing.Color.GhostWhite;
            this.axisDisplay.InnerSize = 12;
            this.axisDisplay.Location = new System.Drawing.Point(75, 12);
            this.axisDisplay.Name = "axisDisplay";
            this.axisDisplay.OuterColor = System.Drawing.Color.DodgerBlue;
            this.axisDisplay.Size = new System.Drawing.Size(100, 100);
            this.axisDisplay.TabIndex = 0;
            this.axisDisplay.Value = ((System.Drawing.PointF)(resources.GetObject("axisDisplay.Value")));
            this.axisDisplay.DoubleClick += new System.EventHandler(this.axisDisplay_DoubleClick);
            // 
            // MouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 184);
            this.Controls.Add(this.labelMouse);
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
            this.Controls.Add(this.axisDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MouseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mouse ";
            ((System.ComponentModel.ISupportInitialize)(this.makeupSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analogDeadzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decayRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AxisDisplay axisDisplay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown makeupSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown analogDeadzone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown decayThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown decayRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown sensitivity;
        private System.Windows.Forms.Label labelMouse;
    }
}