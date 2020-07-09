using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
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
    public partial class Form1 : Form
    {
        BufferedWaveProvider waveProvider;
        ISampleProvider provider;
        IWaveIn input;

        List<Settings> settingsOptions;
        Settings activeSettings;

        List<RenderBase> renderOptions;
        RenderBase activeRender;

        List<float> samples = new List<float>();

        bool init;
        public Form1()
        {
            init = true;
            InitializeComponent();

            DoubleBuffered = true;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, wavePanel, new object[] { true });

            xScaleNumberBox.KeyDown += numericUpDown_KeyDown;
            yScaleNumberBox.KeyDown += numericUpDown_KeyDown;
            samplePowNumberBox.KeyDown += numericUpDown_KeyDown;
            smoothingNumberBox.KeyDown += numericUpDown_KeyDown;

            wavePanel.Paint += Wave_Paint;

            InitOptions();
            InitUI();
            init = false;
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
            samplePowNumberBox.Maximum = 16;
            smoothingNumberBox.Minimum = 1;
            smoothingNumberBox.Maximum = 10000;

            init = false;
            UpdateSettings();
        }

        private void InitOptions()
        {
            settingsOptions = new List<Settings>();
            settingsOptions.Add(new Settings(1.0f, 100f, 13, 1));
            settingsOptions.Add(new Settings(5.0f, 150f, 13, 1));
            settingsOptions.Add(new Settings(5.0f, 100f, 13, 1));
            settingsOptions.Add(new Settings(5.0f, 100f, 13, 1));
            settingsOptions.Add(new Settings(8.0f, 70f, 13, 10));
            settingsOptions.Add(new Settings(8.0f, 70f, 13, 10));
            settingsOptions.Add(new Settings(8.0f, 70f, 13, 10));
            settingsOptions.Add(new Settings(4.0f, 40f, 13, 10));

            renderOptions = new List<RenderBase>();
            renderOptions.Add(new RenderWaveform(settingsOptions[renderOptions.Count], wavePanel, "Waveform"));
            renderOptions.Add(new RenderBasicFreq(settingsOptions[renderOptions.Count], wavePanel, "Frequency"));
            renderOptions.Add(new RenderReflectedFreq(settingsOptions[renderOptions.Count], wavePanel, "Reflections"));
            renderOptions.Add(new RenderWaveFreq(settingsOptions[renderOptions.Count], wavePanel, "Frequency Wave"));
            renderOptions.Add(new RenderOutlineCircle(settingsOptions[renderOptions.Count], wavePanel, "Circle Outline"));
            renderOptions.Add(new RenderBasicCircle(settingsOptions[renderOptions.Count], wavePanel, "Shadow"));
            renderOptions.Add(new RenderRainbowCircle(settingsOptions[renderOptions.Count], wavePanel, "Color Wheel"));
            renderOptions.Add(new RenderReflectedCircle(settingsOptions[renderOptions.Count], wavePanel, "Mirrored Circle"));

            activeSettings = settingsOptions[0];
            activeRender = renderOptions[0];
        }

        private void UpdateSettings()
        {
            if (!init)
            {
                activeSettings = settingsOptions[renderModeComboBox.SelectedIndex];
                activeRender = renderOptions[renderModeComboBox.SelectedIndex];
                xScaleNumberBox.DataBindings.Clear();
                xScaleNumberBox.DataBindings.Add("Value", activeSettings, "XScale", false, DataSourceUpdateMode.OnPropertyChanged);
                yScaleNumberBox.DataBindings.Clear();
                yScaleNumberBox.DataBindings.Add("Value", activeSettings, "YScale", false, DataSourceUpdateMode.OnPropertyChanged);
                samplePowNumberBox.DataBindings.Clear();
                samplePowNumberBox.DataBindings.Add("Value", activeSettings, "SamplePow", false, DataSourceUpdateMode.OnPropertyChanged);
                smoothingNumberBox.DataBindings.Clear();
                smoothingNumberBox.DataBindings.Add("Value", activeSettings, "Smoothing", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void InitAudio (bool isMicrophone)
        {
            if (isMicrophone)
            {
                input = new WaveIn();
                input.WaveFormat = new WaveFormat(44100, 32, 2);
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
            input.RecordingStopped += (s, a) => { input.Dispose(); };
        }

        private void AddData (object s, WaveInEventArgs a)
        {
            waveProvider.ClearBuffer();
            waveProvider.AddSamples(a.Buffer, 0, a.BytesRecorded);

            float[] temp = new float[a.BytesRecorded / 4];

            provider.Read(temp, 0, temp.Length);

            for (int i = 0; i < temp.Length / 2; i++)
            {
                samples.Add(temp[i * 2] + temp[i * 2 + 1]);
            }
            
            while (samples.Count > activeSettings.SampleCount)
            {
                samples.RemoveAt(0);
            }

            wavePanel.Invalidate();
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
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
                e.Graphics.Clear(Color.Black);
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
            UpdateSettings();
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
