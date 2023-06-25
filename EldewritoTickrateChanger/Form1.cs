using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheatEngine;

namespace EldewritoTickrateChanger
{
    public partial class Form1 : Form
    {
        private const uint OriginalTickRate = 60;
        private const float OriginalTickTime = 1 / 60.0f;
        private readonly CancellationTokenSource _tokens = new CancellationTokenSource();
        private IntPtr _engineTickTimeAddress = IntPtr.Zero;
        private IntPtr _timePerTickAddress = IntPtr.Zero;
        private Process _mccProcess;
        private bool _oldStatus;

        public Form1()
        {
            InitializeComponent();
        }

        public bool IsHalo3Hooked
        {
            get
            {
                if (_mccProcess == null) return false;

                try
                {
                    var tick = ReadEngineTickTimeValue();
                    var timePerTick = ReadTimePerTickValue();

                    return tick != 0 && timePerTick != 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public uint TickRate
        {
            get => IsHalo3Hooked ? ReadEngineTickTimeValue() : 0;
            set
            {
                if (IsHalo3Hooked) WriteEngineTickTimeValue(value);
            }
        }

        public float TimePerTick
        {
            get => IsHalo3Hooked ? ReadTimePerTickValue() : 0;
            set
            {
                if (IsHalo3Hooked) WriteTimePerTickValue(value);
            }
        }

        private uint ReadEngineTickTimeValue()
        {
            using (var memory = new Memory(_mccProcess))
            {
                if (_engineTickTimeAddress == IntPtr.Zero)
                {
                    _engineTickTimeAddress = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+4");
                }
                return memory.ReadUInt32(_engineTickTimeAddress);
            }
        }

        private void WriteEngineTickTimeValue(uint toWrite)
        {
            using (var memory = new Memory(_mccProcess))
            {
                try
                {
                    if (_engineTickTimeAddress == IntPtr.Zero)
                    {
                        _engineTickTimeAddress = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+4");
                    }
                    memory.WriteUInt32(_engineTickTimeAddress, toWrite);
                }
                catch (Exception)
                {
                    _engineTickTimeAddress = IntPtr.Zero; // Reset the cached address
                }
            }
        }

        private float ReadTimePerTickValue()
        {
            using (var memory = new Memory(_mccProcess))
            {
                if (_timePerTickAddress == IntPtr.Zero)
                {
                    _timePerTickAddress = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+8");
                }
                return memory.ReadFloat(_timePerTickAddress);
            }
        }

        private void WriteTimePerTickValue(float toWrite)
        {
            using (var memory = new Memory(_mccProcess))
            {
                try
                {
                    if (_timePerTickAddress == IntPtr.Zero)
                    {
                        _timePerTickAddress = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+8");
                    }
                    memory.WriteFloat(_timePerTickAddress, toWrite);
                }
                catch (Exception)
                {
                    _timePerTickAddress = IntPtr.Zero; // Reset the cached address
                }
            }
        }

        private async Task HandleMccProcessesAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(100, token);

                if (!IsHalo3Hooked)
                {
                    _mccProcess = GetMccProcess();

                    label1.Text = "Status: Not Connected to Game.";
                }
                else
                {
                    label1.Text = "Status: Connected to Game.";
                }

                if (!_oldStatus && IsHalo3Hooked) SetTickRate(checkBox1.Checked);

                _oldStatus = IsHalo3Hooked;
                groupBox1.Enabled = IsHalo3Hooked;
            }
        }

        private static Process GetMccProcess()
        {
            var processlist = Process.GetProcessesByName("MCC-Win64-Shipping");

            return processlist.Any() ? processlist.FirstOrDefault() : null;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await HandleMccProcessesAsync(_tokens.Token);
        }

        private void SetTickRate(bool is30)
        {
            uint expectedTickRate = is30 ? 30 : OriginalTickRate;
            float expectedTimePerTick = is30 ? 1 / 30.0f : OriginalTickTime;

            if (TickRate != expectedTickRate)
            {
                TickRate = expectedTickRate;
            }

            if (Math.Abs(TimePerTick - expectedTimePerTick) > 0.0001f)
            {
                TimePerTick = expectedTimePerTick;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetTickRate(checkBox1.Checked);
        }
    }
}