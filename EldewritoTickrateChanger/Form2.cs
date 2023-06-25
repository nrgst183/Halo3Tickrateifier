using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheatEngine;

namespace EldewritoTickrateChanger
{
    public partial class Form2 : Form
    {
        private const uint OriginalTickRate = 60;
        private const float OriginalTickTime = 1 / 60.0f;
        private readonly CancellationTokenSource _tokens = new CancellationTokenSource();

        private Process _mccProcess;

        private bool _oldStatus;

        public Form2()
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
                var ptr = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+4");
                return memory.ReadUInt32(ptr);
            }
        }

        private void WriteEngineTickTimeValue(uint toWrite)
        {
            using (var memory = new Memory(_mccProcess))
            {
                var ptr = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+4");
                memory.WriteUInt32(ptr, toWrite);
            }
        }

        private float ReadTimePerTickValue()
        {
            using (var memory = new Memory(_mccProcess))
            {
                var ptr = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+8");
                return memory.ReadFloat(ptr);
            }
        }

        private void WriteTimePerTickValue(float toWrite)
        {
            using (var memory = new Memory(_mccProcess))
            {
                var ptr = memory.GetAddress("\"halo3.dll\"+02012630+498+C8+8");
                memory.WriteFloat(ptr, toWrite);
            }
        }

        private static Process GetMccProcess()
        {
            var processlist = Process.GetProcessesByName("MCC-Win64-Shipping");

            return processlist.Any() ? processlist.FirstOrDefault() : null;
        }

        private async Task HandleMccProcessesAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(100, token);

                if (!IsHalo3Hooked)
                {
                    _mccProcess = GetMccProcess();

                    mccProcessStatusLabel.Text = "Status: Not Connected to Game.";
                }
                else
                {
                    mccProcessStatusLabel.Text = "Status: Connected to Game.";
                }

                if (!_oldStatus && IsHalo3Hooked)
                {
                    TickRate = (uint) engineTimeNumericUpDown.Value;
                    TimePerTick = (float) engineTimePerTickNumericUpDown.Value;
                }

                _oldStatus = IsHalo3Hooked;
                groupBox1.Enabled = IsHalo3Hooked;
            }
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            await HandleMccProcessesAsync(_tokens.Token);
        }

        private void engineTimeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            TickRate = (uint) engineTimeNumericUpDown.Value;

            TimePerTick = lockTimePerTickCheckbox.Enabled
                ? (float) (1 / engineTimeNumericUpDown.Value)
                : (float) engineTimePerTickNumericUpDown.Value;
        }
    }
}