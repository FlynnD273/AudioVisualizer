namespace AudioVisualizer
{
    partial class RenderForm
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
            this.settingsButton = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // settingsButton
            // 
            this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.settingsButton.Location = new System.Drawing.Point(12, 12);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(50, 50);
            this.settingsButton.TabIndex = 0;
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.White;
            this.settingsPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(200, 200);
            this.settingsPanel.TabIndex = 1;
            // 
            // RenderForm
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(676, 455);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.settingsPanel);
            this.Name = "RenderForm";
            this.ClientSizeChanged += new System.EventHandler(this.RenderForm_ClientSizeChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderForm_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Panel wavePanel;
        private System.Windows.Forms.ComboBox inputModeComboBox;
        private System.Windows.Forms.ComboBox renderModeComboBox;
        private System.Windows.Forms.NumericUpDown xScaleNumberBox;
        private System.Windows.Forms.NumericUpDown yScaleNumberBox;
        private System.Windows.Forms.NumericUpDown samplePowNumberBox;
        private System.Windows.Forms.NumericUpDown smoothingNumberBox;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Panel settingsPanel;
    }
}

