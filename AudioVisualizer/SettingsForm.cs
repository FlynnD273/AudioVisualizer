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
    partial class SettingsForm : Form
    {
        public event EventHandler<RenderEventArgs> SettingsChanged;
        public event EventHandler UpdateGraphics;

        private BufferedWaveProvider waveProvider;
        private ISampleProvider provider;
        private IWaveIn input;

        private List<Settings> settingsOptions;
        private List<RenderBase> renderOptions;

        public List<float> Samples { get; private set; }
        private RenderBase Render;
        public Settings Settings { get; private set; }
        public bool ProgramShuttingDown { get; set; }

        private int[] customColors;

        public SettingsForm()
        {
            InitializeComponent();

            Samples = new List<float>();

            InitOptions();
            InitUI();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!ProgramShuttingDown)
            {
                e.Cancel = true;
                Visible = false;
            }

            base.OnFormClosing(e);
        }

        private void InitUI()
        {
            xScaleNumberBox.KeyDown += NumericUpDown_KeyDown;
            yScaleNumberBox.KeyDown += NumericUpDown_KeyDown;
            samplePowNumberBox.KeyDown += NumericUpDown_KeyDown;
            smoothingNumberBox.KeyDown += NumericUpDown_KeyDown;

            renderModeComboBox.SelectedIndexChanged += RaiseSettingsChanged;

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

            UpdateSettings();
        }

        private void RaiseSettingsChanged(object sender, EventArgs e)
        {
            SettingsChanged?.Invoke(sender, new RenderEventArgs(Render));
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
            renderOptions.Add(new RenderWaveform(settingsOptions[renderOptions.Count], "Waveform"));
            renderOptions.Add(new RenderBasicFreq(settingsOptions[renderOptions.Count], "Frequency"));
            renderOptions.Add(new RenderReflectedFreq(settingsOptions[renderOptions.Count], "Reflections"));
            renderOptions.Add(new RenderWaveFreq(settingsOptions[renderOptions.Count], "Frequency Wave"));
            renderOptions.Add(new RenderOutlineCircle(settingsOptions[renderOptions.Count], "Circle Outline"));
            renderOptions.Add(new RenderShadowCircle(settingsOptions[renderOptions.Count], "Shadow"));
            renderOptions.Add(new RenderRainbowCircle(settingsOptions[renderOptions.Count], "Color Wheel"));
            renderOptions.Add(new RenderReflectedCircle(settingsOptions[renderOptions.Count], "Mirrored Circle"));

            Settings = settingsOptions[0];
            Render = renderOptions[0];
        }

        private void UpdateSettings()
        {
            Settings = settingsOptions[renderModeComboBox.SelectedIndex];
            Render = renderOptions[renderModeComboBox.SelectedIndex];
            xScaleNumberBox.DataBindings.Clear();
            xScaleNumberBox.DataBindings.Add("Value", Settings, "XScale", false, DataSourceUpdateMode.OnPropertyChanged);
            yScaleNumberBox.DataBindings.Clear();
            yScaleNumberBox.DataBindings.Add("Value", Settings, "YScale", false, DataSourceUpdateMode.OnPropertyChanged);
            samplePowNumberBox.DataBindings.Clear();
            samplePowNumberBox.DataBindings.Add("Value", Settings, "SamplePow", false, DataSourceUpdateMode.OnPropertyChanged);
            smoothingNumberBox.DataBindings.Clear();
            smoothingNumberBox.DataBindings.Add("Value", Settings, "Smoothing", false, DataSourceUpdateMode.OnPropertyChanged);
            colorNamesListBox.DataSource = Settings.Colors;
            colorsListBox.DataSource = Settings.Colors;
        }

        private void InitAudio(bool isMicrophone)
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

        private void AddData(object s, WaveInEventArgs a)
        {
            waveProvider.ClearBuffer();
            waveProvider.AddSamples(a.Buffer, 0, a.BytesRecorded);

            float[] temp = new float[a.BytesRecorded / 4];

            provider.Read(temp, 0, temp.Length);

            for (int i = 0; i < temp.Length / 2; i++)
            {
                Samples.Add(temp[i * 2] + temp[i * 2 + 1]);
            }

            while (Samples.Count > Settings.SampleCount)
            {
                Samples.RemoveAt(0);
            }

            UpdateGraphics?.Invoke(this, EventArgs.Empty);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            StopReading();
        }

        private void StopReading()
        {
            input.StopRecording();

            Start.Enabled = true;
            Stop.Enabled = false;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            StartReading();
        }

        private void StartReading()
        {
            RaiseSettingsChanged(this, EventArgs.Empty);
            InitAudio(inputModeComboBox.SelectedIndex == 1);
            input.StartRecording();

            Start.Enabled = false;
            Stop.Enabled = true;
        }

        private void Wave_Paint(object sender, PaintEventArgs e)
        {
            if (input != null && Samples != null && Samples.Count > 100)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;
                e.Graphics.Clear(Color.Black);
                Render.Render(e.Graphics, Samples.ToArray());
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

        private void NumericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void colorNamesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColorDialog col = new ColorDialog();
            col.AnyColor = false;
            col.FullOpen = true;
            if (customColors != null) { col.CustomColors = customColors; }
            col.Color = Settings.Colors[colorNamesListBox.SelectedIndex].Color;

            if (col.ShowDialog() == DialogResult.OK)
            {
                Settings.Colors[colorNamesListBox.SelectedIndex].Color = col.Color;
                customColors = col.CustomColors;
            }
        }

        private void ColorsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //
            // Draw the background of the ListBox control for each item.
            // Create a new Brush and initialize to a Black colored brush
            // by default.
            //
            e.DrawBackground();
            if (e.Index > -1)
            {
                e.Graphics.FillRectangle(new SolidBrush(((NamedColor)((ListBox)sender).Items[e.Index]).Color), e.Bounds);
            }
            //
            // If the ListBox has focus, draw a focus rectangle 
            // around the selected item.
            //
            e.DrawFocusRectangle();
        }
    }
}
