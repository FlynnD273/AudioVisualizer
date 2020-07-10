using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioVisualizer
{
    partial class RenderForm : Form
    {
        private RenderBase activeRender;

        private List<float> samples = new List<float>();

        private SettingsForm settingsForm;

        private readonly RenderFormState _state = new RenderFormState();

        private bool isResizing = false;

        private Region mouseRegion;

        public RenderForm()
        {
            InitializeComponent();

            mouseRegion = new Region(new Rectangle(0, 0, 500, 500));

            Bitmap scaledIcon = new Bitmap(Properties.Resources.settings, new Size((int)(settingsButton.Width * 0.8), (int)(settingsButton.Height * 0.8)));
            settingsButton.Image = scaledIcon;
            settingsPanel.BackColor = Color.Black;
            settingsPanel.Visible = false;

            DoubleBuffered = true;

            InitSettingsForm();
        }

        private async void InitSettingsForm()
        {
            settingsForm = new SettingsForm();
            settingsForm.UpdateGraphics += (s, e) => Invalidate();
            settingsForm.SettingsChanged += UpdateSettings;
            settingsForm.Show();
            await Task.Delay(100);
            settingsForm.Activate();
        }

        private void UpdateSettings(object sender, RenderEventArgs e)
        {
            activeRender = e.Render;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (settingsForm.Samples != null && settingsForm.Samples.Count > 0)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
                e.Graphics.Clear(Color.Black);
                activeRender.Render(e.Graphics, settingsForm.Samples.ToArray());
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
            settingsForm.Activate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            settingsForm.ProgramShuttingDown = true;
            settingsForm.Close();

            base.OnFormClosed(e);
        }

        private void RenderForm_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!isResizing)
            {
                isResizing = true;
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                }
                isResizing = false;
            }
        }

        private void RenderForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (settingsPanel.ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                settingsButton.Visible = true;
            }
            else
            {
                settingsButton.Visible = false;
            }
        }
    }
}
