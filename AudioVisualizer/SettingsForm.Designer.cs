namespace AudioVisualizer
{
    partial class SettingsForm
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Start = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.inputModeComboBox = new System.Windows.Forms.ComboBox();
            this.renderModeComboBox = new System.Windows.Forms.ComboBox();
            this.xScaleNumberBox = new System.Windows.Forms.NumericUpDown();
            this.yScaleNumberBox = new System.Windows.Forms.NumericUpDown();
            this.samplePowNumberBox = new System.Windows.Forms.NumericUpDown();
            this.smoothingNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.samplePowLabel = new System.Windows.Forms.Label();
            this.smoothingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xScaleNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yScaleNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplePowNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothingNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(12, 12);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(84, 40);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Stop
            // 
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(102, 12);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(94, 40);
            this.Stop.TabIndex = 1;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // inputModeComboBox
            // 
            this.inputModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputModeComboBox.FormattingEnabled = true;
            this.inputModeComboBox.Items.AddRange(new object[] {
            "Speaker Out",
            "Microphone In"});
            this.inputModeComboBox.Location = new System.Drawing.Point(202, 17);
            this.inputModeComboBox.Name = "inputModeComboBox";
            this.inputModeComboBox.Size = new System.Drawing.Size(182, 33);
            this.inputModeComboBox.TabIndex = 4;
            this.inputModeComboBox.SelectedIndexChanged += new System.EventHandler(this.inputModeComboBox_SelectedIndexChanged);
            // 
            // renderModeComboBox
            // 
            this.renderModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.renderModeComboBox.FormattingEnabled = true;
            this.renderModeComboBox.Location = new System.Drawing.Point(390, 19);
            this.renderModeComboBox.Name = "renderModeComboBox";
            this.renderModeComboBox.Size = new System.Drawing.Size(182, 33);
            this.renderModeComboBox.TabIndex = 5;
            this.renderModeComboBox.SelectedIndexChanged += new System.EventHandler(this.renderModeComboBox_SelectedIndexChanged);
            // 
            // xScaleNumberBox
            // 
            this.xScaleNumberBox.DecimalPlaces = 2;
            this.xScaleNumberBox.Location = new System.Drawing.Point(12, 83);
            this.xScaleNumberBox.Name = "xScaleNumberBox";
            this.xScaleNumberBox.Size = new System.Drawing.Size(180, 31);
            this.xScaleNumberBox.TabIndex = 6;
            // 
            // yScaleNumberBox
            // 
            this.yScaleNumberBox.DecimalPlaces = 2;
            this.yScaleNumberBox.Location = new System.Drawing.Point(12, 145);
            this.yScaleNumberBox.Name = "yScaleNumberBox";
            this.yScaleNumberBox.Size = new System.Drawing.Size(180, 31);
            this.yScaleNumberBox.TabIndex = 6;
            // 
            // samplePowNumberBox
            // 
            this.samplePowNumberBox.Location = new System.Drawing.Point(202, 83);
            this.samplePowNumberBox.Name = "samplePowNumberBox";
            this.samplePowNumberBox.Size = new System.Drawing.Size(180, 31);
            this.samplePowNumberBox.TabIndex = 7;
            // 
            // smoothingNumberBox
            // 
            this.smoothingNumberBox.Location = new System.Drawing.Point(204, 145);
            this.smoothingNumberBox.Name = "smoothingNumberBox";
            this.smoothingNumberBox.Size = new System.Drawing.Size(180, 31);
            this.smoothingNumberBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "X Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y Scale";
            // 
            // samplePowLabel
            // 
            this.samplePowLabel.AutoSize = true;
            this.samplePowLabel.Location = new System.Drawing.Point(202, 55);
            this.samplePowLabel.Name = "samplePowLabel";
            this.samplePowLabel.Size = new System.Drawing.Size(150, 25);
            this.samplePowLabel.TabIndex = 9;
            this.samplePowLabel.Text = "Sample Size (Exp)";
            // 
            // smoothingLabel
            // 
            this.smoothingLabel.AutoSize = true;
            this.smoothingLabel.Location = new System.Drawing.Point(202, 117);
            this.smoothingLabel.Name = "smoothingLabel";
            this.smoothingLabel.Size = new System.Drawing.Size(101, 25);
            this.smoothingLabel.TabIndex = 10;
            this.smoothingLabel.Text = "Smoothing";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 190);
            this.Controls.Add(this.smoothingLabel);
            this.Controls.Add(this.samplePowLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.smoothingNumberBox);
            this.Controls.Add(this.samplePowNumberBox);
            this.Controls.Add(this.yScaleNumberBox);
            this.Controls.Add(this.xScaleNumberBox);
            this.Controls.Add(this.renderModeComboBox);
            this.Controls.Add(this.inputModeComboBox);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "OptionsForm";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.xScaleNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yScaleNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplePowNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothingNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.ComboBox inputModeComboBox;
        private System.Windows.Forms.ComboBox renderModeComboBox;
        private System.Windows.Forms.NumericUpDown xScaleNumberBox;
        private System.Windows.Forms.NumericUpDown yScaleNumberBox;
        private System.Windows.Forms.NumericUpDown samplePowNumberBox;
        private System.Windows.Forms.NumericUpDown smoothingNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label samplePowLabel;
        private System.Windows.Forms.Label smoothingLabel;
    }
}