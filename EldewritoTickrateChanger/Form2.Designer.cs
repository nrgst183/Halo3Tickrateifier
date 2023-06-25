namespace EldewritoTickrateChanger
{
    partial class Form2
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
            this.mccProcessStatusLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.engineTimePerTickNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.engineTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.lockTimePerTickCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.engineTimePerTickNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.engineTimeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mccProcessStatusLabel
            // 
            this.mccProcessStatusLabel.AutoSize = true;
            this.mccProcessStatusLabel.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mccProcessStatusLabel.Location = new System.Drawing.Point(7, 379);
            this.mccProcessStatusLabel.Name = "mccProcessStatusLabel";
            this.mccProcessStatusLabel.Size = new System.Drawing.Size(275, 28);
            this.mccProcessStatusLabel.TabIndex = 4;
            this.mccProcessStatusLabel.Text = "Status: Not Connected to Game";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.engineTimePerTickNumericUpDown);
            this.groupBox1.Controls.Add(this.engineTimeNumericUpDown);
            this.groupBox1.Controls.Add(this.lockTimePerTickCheckbox);
            this.groupBox1.Enabled = false;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 364);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Halo 3 Tickrate Options";
            // 
            // engineTimePerTickNumericUpDown
            // 
            this.engineTimePerTickNumericUpDown.DecimalPlaces = 10;
            this.engineTimePerTickNumericUpDown.Font = new System.Drawing.Font("Segoe UI Light", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.engineTimePerTickNumericUpDown.Location = new System.Drawing.Point(29, 175);
            this.engineTimePerTickNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.engineTimePerTickNumericUpDown.Name = "engineTimePerTickNumericUpDown";
            this.engineTimePerTickNumericUpDown.ReadOnly = true;
            this.engineTimePerTickNumericUpDown.Size = new System.Drawing.Size(384, 77);
            this.engineTimePerTickNumericUpDown.TabIndex = 2;
            this.engineTimePerTickNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.engineTimePerTickNumericUpDown.Value = new decimal(new int[] {
            1666666666,
            0,
            0,
            720896});
            // 
            // engineTimeNumericUpDown
            // 
            this.engineTimeNumericUpDown.Font = new System.Drawing.Font("Segoe UI Light", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.engineTimeNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.engineTimeNumericUpDown.Location = new System.Drawing.Point(29, 48);
            this.engineTimeNumericUpDown.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.engineTimeNumericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.engineTimeNumericUpDown.Name = "engineTimeNumericUpDown";
            this.engineTimeNumericUpDown.Size = new System.Drawing.Size(384, 103);
            this.engineTimeNumericUpDown.TabIndex = 1;
            this.engineTimeNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.engineTimeNumericUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.engineTimeNumericUpDown.ValueChanged += new System.EventHandler(this.engineTimeNumericUpDown_ValueChanged);
            // 
            // lockTimePerTickCheckbox
            // 
            this.lockTimePerTickCheckbox.AutoSize = true;
            this.lockTimePerTickCheckbox.Checked = true;
            this.lockTimePerTickCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lockTimePerTickCheckbox.Location = new System.Drawing.Point(29, 301);
            this.lockTimePerTickCheckbox.Name = "lockTimePerTickCheckbox";
            this.lockTimePerTickCheckbox.Size = new System.Drawing.Size(320, 32);
            this.lockTimePerTickCheckbox.TabIndex = 0;
            this.lockTimePerTickCheckbox.Text = "Lock Time per Tick to Engine Time";
            this.lockTimePerTickCheckbox.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 433);
            this.Controls.Add(this.mccProcessStatusLabel);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.engineTimePerTickNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.engineTimeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label mccProcessStatusLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown engineTimeNumericUpDown;
        private System.Windows.Forms.NumericUpDown engineTimePerTickNumericUpDown;
        private System.Windows.Forms.CheckBox lockTimePerTickCheckbox;
    }
}