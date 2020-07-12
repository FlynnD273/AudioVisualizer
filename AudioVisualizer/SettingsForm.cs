using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using AudioVisualizer.Properties;
using MathNet.Numerics;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using static AudioVisualizer.NamedInputState;

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

        private WaveFileReader reader;
        private WaveOut output;

        private System.Timers.Timer progressTimer;

        private SongInfo songInfo;
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

            inputModeComboBox.DataSource = NamedInputState.FromInputStateEnum();
            inputModeComboBox.DisplayMember = "Name";
            inputModeComboBox.ValueMember = "State";
            inputModeComboBox.SelectedIndex = 0;

            xScaleNumberBox.Minimum = 1M;
            yScaleNumberBox.Increment = 10M;
            yScaleNumberBox.Maximum = 500M;
            samplePowNumberBox.Maximum = 16;
            smoothingNumberBox.Minimum = 1;
            smoothingNumberBox.Maximum = 10000;

            playButton.Image = Resources.Play;
            audioPlaybackPanel.Enabled = false;
            filePanel.Enabled = false;

            UpdateSettings();
        }

        private void RaiseSettingsChanged(object sender, EventArgs e)
        {
            SettingsChanged?.Invoke(sender, new RenderEventArgs(Render));
        }

        private void InitOptions()
        {
            settingsOptions = new List<Settings>();
            settingsOptions.Add(new Settings(1.0f, 200f, 13, 1)); //Waveform
            settingsOptions.Add(new Settings(5.0f, 300f, 13, 1)); //Frequency
            settingsOptions.Add(new Settings(5.0f, 200f, 13, 1)); //Reflections
            settingsOptions.Add(new Settings(5.0f, 300f, 13, 20)); //Frequency Wave
            settingsOptions.Add(new Settings(8.0f, 200f, 13, 10)); //Circle Outline
            settingsOptions.Add(new Settings(8.0f, 200f, 13, 10)); //Shadow
            settingsOptions.Add(new Settings(8.0f, 200f, 13, 10)); //Color Wheel
            settingsOptions.Add(new Settings(4.0f, 100f, 13, 10)); //Mirrored Circle

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

        private void InitAudio(InputState state)
        {
            filePanel.Enabled = false;

            switch (state)
            {
                case InputState.SpeakerOut:
                    input = new WasapiLoopbackCapture();

                    InitReader();
                    break;
                case InputState.MicrophoneIn:
                    input = new WaveIn();
                    input.WaveFormat = new WaveFormat(44100, 32, 2);

                    InitReader();
                    break;
                case InputState.FileIn:
                    input = new WasapiLoopbackCapture();

                    waveProvider = new BufferedWaveProvider(input.WaveFormat);

                    provider = waveProvider.ToSampleProvider();
                    input.DataAvailable += AddDataFromFile;
                    input.RecordingStopped += (s, a) => { input?.Dispose(); };
                    break;
                default:
                    break;
            }
        }

        private void AddDataFromFile(object s, WaveInEventArgs a)
        {
            Samples.Clear();
            int index = (int)(reader.CurrentTime.TotalMilliseconds / reader.TotalTime.TotalMilliseconds * reader.SampleCount);
            for (int i = Math.Max(index - Settings.SampleCount / 2, 0); i < Math.Min(reader.SampleCount, index + Settings.SampleCount / 2); i++)
            {
                Samples.Add(songInfo.Samples[i]);
            }

            UpdateGraphics?.Invoke(this, EventArgs.Empty);
        }

        private void InitReader ()
        {
            waveProvider = new BufferedWaveProvider(input.WaveFormat);

            provider = waveProvider.ToSampleProvider();
            input.DataAvailable += AddData;
            input.RecordingStopped += (s, a) => { input?.Dispose(); };
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
            input?.StopRecording();

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
            InitAudio(((NamedInputState)inputModeComboBox.SelectedItem).State);
            input?.StartRecording();

            Start.Enabled = false;
            Stop.Enabled = true;

            if (((NamedInputState)inputModeComboBox.SelectedItem).State == InputState.FileIn)
            {
                filePanel.Enabled = true;
            }
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

            if (((NamedInputState)inputModeComboBox.SelectedItem).State == InputState.FileIn)
            {
                filePanel.Enabled = true;
            }
            else
            {
                filePanel.Enabled = false;
                output?.Pause();
                playButton.Image = Resources.Play;
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
            ColorPicker col = new ColorPicker(((NamedColor)colorNamesListBox.SelectedItem).Color);

            if (col.ShowDialog() == DialogResult.OK)
            {
                Settings.Colors[colorNamesListBox.SelectedIndex].Color = col.Color;
                colorsListBox.Invalidate();
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

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Sound Files (*.mp3; *.wav)|*.mp3; *.wav|All Files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StopReading();
                output?.Dispose();
                reader?.Dispose();
                input?.Dispose();
                output = null;
                reader = null;
                input = null;

                songInfo = new SongInfo(dialog.FileName);
            }

            if (File.Exists(songInfo.FilePath))
            {
                fileNameLabel.Text = songInfo.SongName;

                audioPlaybackPanel.Enabled = true;

                reader = new WaveFileReader(songInfo.FilePath);

                output = new WaveOut();
                output.NumberOfBuffers = 8;
                output.PlaybackStopped += Output_PlaybackStopped;
                output.Init(reader);

                playButton.Image = Resources.Play;
            }
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            playButton.Image = Resources.Play;

            StopProgressTimer();
            songProgressBar.Value = 0;
            reader.Position = 0;
        }

        private void UpdateProgressBar(object obj)
        {
            Thread.Sleep(500);
            while (output.PlaybackState == PlaybackState.Playing)
            {
                songProgressBar.Value = (int)((float)reader.CurrentTime.TotalSeconds / reader.TotalTime.TotalSeconds * 100);
                Thread.Sleep(500);
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (output.PlaybackState == PlaybackState.Paused || output.PlaybackState == PlaybackState.Stopped)
            {
                output.Play();

                if (Start.Enabled)
                    StartReading();
                playButton.Image = Resources.Pause;

                StartProgressBar();
            }
            else
            {
                output.Pause();
                playButton.Image = Resources.Play;

                StopProgressTimer();
            }
        }

        private void StartProgressBar()
        {
            progressTimer = new System.Timers.Timer();

            songProgressBar.Maximum = (int)reader.TotalTime.TotalSeconds;
            songProgressBar.Value = (int)reader.CurrentTime.TotalSeconds;
            progressTimer.Interval = 1000;
            progressTimer.Elapsed += IncreaseProgressBar;
            progressTimer.Start();
        }
        private void IncreaseProgressBar(object sender, EventArgs e)
        {
            songProgressBar.BeginInvoke(new Action(() => songProgressBar.Increment(1)));
            if (songProgressBar.Value == songProgressBar.Maximum)
                StopProgressTimer();
        }

        private void StopProgressTimer()
        {
            progressTimer.Stop();
        }
    }
}
