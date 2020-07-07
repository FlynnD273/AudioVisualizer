namespace AudioVisualizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            this.wavePictureBox = new System.Windows.Forms.PictureBox();
            this.inputModeComboBox = new System.Windows.Forms.ComboBox();
            this.renderModeComboBox = new System.Windows.Forms.ComboBox();
            this.xScaleNumberBox = new System.Windows.Forms.NumericUpDown();
            this.yScaleNumberBox = new System.Windows.Forms.NumericUpDown();
            this.sampleCountNumberBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xScaleNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yScaleNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleCountNumberBox)).BeginInit();
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
            // wavePictureBox
            // 
            this.wavePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wavePictureBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.wavePictureBox.Location = new System.Drawing.Point(0, 70);
            this.wavePictureBox.Name = "wavePictureBox";
            this.wavePictureBox.Size = new System.Drawing.Size(1190, 380);
            this.wavePictureBox.TabIndex = 2;
            this.wavePictureBox.TabStop = false;
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
            this.xScaleNumberBox.Location = new System.Drawing.Point(578, 21);
            this.xScaleNumberBox.Name = "xScaleNumberBox";
            this.xScaleNumberBox.Size = new System.Drawing.Size(180, 31);
            this.xScaleNumberBox.TabIndex = 6;
            this.xScaleNumberBox.ValueChanged += new System.EventHandler(this.xScaleNumberBox_ValueChanged);
            // 
            // yScaleNumberBox
            // 
            this.yScaleNumberBox.DecimalPlaces = 2;
            this.yScaleNumberBox.Location = new System.Drawing.Point(764, 21);
            this.yScaleNumberBox.Name = "yScaleNumberBox";
            this.yScaleNumberBox.Size = new System.Drawing.Size(180, 31);
            this.yScaleNumberBox.TabIndex = 6;
            this.yScaleNumberBox.ValueChanged += new System.EventHandler(this.yScaleNumberBox_ValueChanged);
            // 
            // sampleCountNumberBox
            // 
            this.sampleCountNumberBox.Location = new System.Drawing.Point(950, 21);
            this.sampleCountNumberBox.Name = "sampleCountNumberBox";
            this.sampleCountNumberBox.Size = new System.Drawing.Size(180, 31);
            this.sampleCountNumberBox.TabIndex = 7;
            this.sampleCountNumberBox.ValueChanged += new System.EventHandler(this.sampleCountNumberBox_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 450);
            this.Controls.Add(this.sampleCountNumberBox);
            this.Controls.Add(this.yScaleNumberBox);
            this.Controls.Add(this.xScaleNumberBox);
            this.Controls.Add(this.renderModeComboBox);
            this.Controls.Add(this.inputModeComboBox);
            this.Controls.Add(this.wavePictureBox);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Start);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.wavePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xScaleNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yScaleNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleCountNumberBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.PictureBox wavePictureBox;
        private System.Windows.Forms.ComboBox inputModeComboBox;
        private System.Windows.Forms.ComboBox renderModeComboBox;
        private System.Windows.Forms.NumericUpDown xScaleNumberBox;
        private System.Windows.Forms.NumericUpDown yScaleNumberBox;
        private System.Windows.Forms.NumericUpDown sampleCountNumberBox;
    }
}

