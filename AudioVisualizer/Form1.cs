using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
    public partial class Form1 : Form
    {
        BufferedWaveProvider waveProvider;
        ISampleProvider provider;
        IWaveIn input;

        Settings[] settingsOptions;
        Settings activeSettings;

        RenderBase[] renderOptions;
        RenderBase activeRender;

        List<float> samples = new List<float>();

        public Form1()
        {
            InitializeComponent();

            xScaleNumberBox.KeyDown += numericUpDown_KeyDown;
            yScaleNumberBox.KeyDown += numericUpDown_KeyDown;
            sampleCountNumberBox.KeyDown += numericUpDown_KeyDown;
            
            wavePictureBox.Paint += Wave_Paint;

            InitOptions();
            InitUI();
        }

        private void InitUI()
        {
            renderModeComboBox.DataSource = renderOptions;
            renderModeComboBox.DisplayMember = "Name";
            renderModeComboBox.SelectedIndex = 0;

            inputModeComboBox.SelectedIndex = 0;

            xScaleNumberBox.Minimum = 1M;
            yScaleNumberBox.Increment = 10M;
            yScaleNumberBox.Maximum = 500M;
            sampleCountNumberBox.Maximum = 16;

            UpdateSettings();
        }

        private void InitOptions()
        {
            settingsOptions = new Settings[4];
            settingsOptions[0] = new Settings(1.0f, 100f, 8192);
            settingsOptions[1] = new Settings(5.0f, 100f, 8192);
            settingsOptions[2] = new Settings(5.0f, 100f, 8192);
            settingsOptions[3] = new Settings(1.0f, 100f, 8192);

            renderOptions = new RenderBase[4];
            renderOptions[0] = new RenderWaveform(settingsOptions[0], wavePictureBox, "Waveform");
            renderOptions[1] = new RenderBasicFreq(settingsOptions[1], wavePictureBox, "Frequency");
            renderOptions[2] = new RenderReflectedFreq(settingsOptions[2], wavePictureBox, "Frequency Reflected");
            renderOptions[3] = new RenderBasicCircle(settingsOptions[3], wavePictureBox, "Circle");
        }

        private void UpdateSettings()
        {
            activeSettings = settingsOptions[renderModeComboBox.SelectedIndex];
            xScaleNumberBox.Value = (decimal)activeSettings.XScale;
            yScaleNumberBox.Value = (decimal)activeSettings.YScale;
            sampleCountNumberBox.Value = (decimal)Math.Log2(activeSettings.SampleCount);
        }

        private void InitAudio (bool isMicrophone)
        {
            if (isMicrophone)
            {
                input = new WaveIn();
                input.WaveFormat = new WaveFormat(44100, 2);
            }
            else
            {
                input = new WasapiLoopbackCapture();
            }

            waveProvider = new BufferedWaveProvider(input.WaveFormat);

            //waveProvider.DiscardOnBufferOverflow = true;
            provider = waveProvider.ToSampleProvider();

            input.DataAvailable += AddData;

            // When the Capturer Stops
            input.RecordingStopped += (s, a) =>
            {
                input.Dispose();
            };
        }

        private void AddData (object s, WaveInEventArgs a)
        {
            waveProvider.ClearBuffer();
            waveProvider.AddSamples(a.Buffer, 0, a.BytesRecorded);

            float[] temp = new float[a.BytesRecorded / 4];

            provider.Read(temp, 0, temp.Length);

            //samples = new float[settings.SampleCount];

            for (int i = 0; i < temp.Length / 2; i++)
            {
                samples.Add(temp[i * 2] + temp[i * 2 + 1]);
            }
            
            while (samples.Count > activeSettings.SampleCount)
            {
                samples.RemoveAt(0);
            }

            wavePictureBox.Invalidate();
        }

        private void Stop_Click (object sender, EventArgs e)
        {
            StopReading();
        }

        private void StopReading()
        {
            input.StopRecording();

            Start.Enabled = true;
            Stop.Enabled = false;
        }

        private void Start_Click (object sender, EventArgs e)
        {
            StartReading();
        }

        private void StartReading()
        {
            InitAudio(inputModeComboBox.SelectedIndex == 1);
            input.StartRecording();

            Start.Enabled = false;
            Stop.Enabled = true;
        }

        private void Wave_Paint(object sender, PaintEventArgs e)
        {
            if (input != null && samples != null && samples.Count > 100)
            {
                activeRender.Render(e.Graphics, samples.ToArray());
            }
        }

        private void inputModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (input != null)
            {
                StopReading();
            }
        }

        private void renderModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeRender = renderOptions[renderModeComboBox.SelectedIndex];
            UpdateSettings();
        }

        private void xScaleNumberBox_ValueChanged(object sender, EventArgs e)
        {
            activeSettings.XScale = (float)xScaleNumberBox.Value;
        }

        private void yScaleNumberBox_ValueChanged(object sender, EventArgs e)
        {
            activeSettings.YScale = (float)yScaleNumberBox.Value;
        }

        private void sampleCountNumberBox_ValueChanged(object sender, EventArgs e)
        {
            activeSettings.SampleCount = (int)Math.Pow(2, (int)sampleCountNumberBox.Value);
        }

        private void numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
