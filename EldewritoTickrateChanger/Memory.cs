﻿using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CheatEngine
{
    /// <summary>
    ///     Represents an access to a remote process memory
    /// </summary>
    public class Memory : IDisposable
    {
        public const string OffsetPattern = "(\\+|\\-){0,1}(0x){0,1}[a-fA-F0-9]{1,}";
        private bool isDisposed;
        private IntPtr processHandle;

        #region Properties

        /// <summary>
        ///     Gets the process to which this memory is attached to
        /// </summary>
        public Process Process { get; private set; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the Memory
        /// </summary>
        /// <param name="process">Remote process</param>
        public Memory(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            Process = process;
            processHandle = Win32.OpenProcess(
                Win32.ProcessAccessType.PROCESS_VM_READ | Win32.ProcessAccessType.PROCESS_VM_WRITE |
                Win32.ProcessAccessType.PROCESS_VM_OPERATION, true, (uint) process.Id);
            if (processHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not open the process");
            }
        }

        /// <summary>
        ///     Finds module with the given name
        /// </summary>
        /// <param name="name">Module name</param>
        /// <returns></returns>
        public ProcessModule FindModule(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            foreach (ProcessModule module in Process.Modules)
            {
                if (string.Equals(module.ModuleName, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return module;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets module based address
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <param name="baseAddress">Base address</param>
        /// <param name="offsets">Collection of offsets</param>
        /// <returns></returns>
        public IntPtr GetAddress(string moduleName, IntPtr baseAddress, int[] offsets)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentNullException(nameof(moduleName));
            }

            var module = FindModule(moduleName);
            if (module == null)
            {
                return IntPtr.Zero;
            }

            var address = module.BaseAddress.ToInt64() + baseAddress.ToInt64();
            return GetAddress((IntPtr) address, offsets);
        }

        /// <summary>
        ///     Gets address
        /// </summary>
        /// z
        /// <param name="baseAddress">Base address</param>
        /// <param name="offsets">Collection of offsets</param>
        /// <returns></returns>
        public IntPtr GetAddress(IntPtr baseAddress, int[] offsets)
        {
            if (baseAddress == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid base address");
            }

            var address = baseAddress.ToInt64();

            if (offsets != null && offsets.Length > 0)
            {
                foreach (var offset in offsets)
                {
                    address = ReadInt64((IntPtr) address) + offset;
                }
            }

            return (IntPtr) address;
        }

        /// <summary>
        ///     Gets address pointer
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns></returns>
        public IntPtr GetAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            string moduleName = null;
            var index = address.IndexOf('"');
            if (index != -1)
            {
                // Module name at the beginning
                var endIndex = address.IndexOf('"', index + 1);
                if (endIndex == -1)
                {
                    throw new ArgumentException("Invalid module name. Could not find matching \"");
                }

                moduleName = address.Substring(index + 1, endIndex - 1);
                address = address.Substring(endIndex + 1);
            }

            var offsets = GetAddressOffsets(address);
            int[] _offsets = null;
            var baseAddress = offsets != null && offsets.Length > 0 ? (IntPtr) offsets[0] : IntPtr.Zero;
            if (offsets != null && offsets.Length > 1)
            {
                _offsets = new int[offsets.Length - 1];
                for (var i = 0; i < offsets.Length - 1; i++)
                {
                    _offsets[i] = offsets[i + 1];
                }
            }

            if (moduleName != null)
            {
                return GetAddress(moduleName, baseAddress, _offsets);
            }

            return GetAddress(baseAddress, _offsets);
        }

        /// <summary>
        ///     Gets address offsets
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns></returns>
        protected static int[] GetAddressOffsets(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return new int[0];
            }

            var matches = Regex.Matches(address, OffsetPattern);
            var offsets = new int[matches.Count];
            for (var i = 0; i < matches.Count; i++)
            {
                var ch = matches[i].Value[0];
                string value;
                if (ch == '+' || ch == '-')
                {
                    value = matches[i].Value.Substring(1);
                }
                else
                {
                    value = matches[i].Value;
                }

                offsets[i] = Convert.ToInt32(value, 16);
                if (ch == '-')
                {
                    offsets[i] = -offsets[i];
                }
            }

            return offsets;
        }

        /// <summary>
        ///     Reads memory at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="size">Size in bytes</param>
        public void ReadMemory(IntPtr address, byte[] buffer, int size)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Memory");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than zero");
            }

            if (address == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid address");
            }

            uint read = 0;
            if (!Win32.ReadProcessMemory(processHandle, address, buffer, (uint) size, ref read) ||
                read != size)
            {
                throw new AccessViolationException();
            }
        }

        /// <summary>
        ///     Writes memory at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="buffer">Buffer</param>
        /// <param name="size">Size in bytes</param>
        public void WriteMemory(IntPtr address, byte[] buffer, int size)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Memory");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than zero");
            }

            if (address == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid address");
            }

            uint write = 0;
            if (!Win32.WriteProcessMemory(processHandle, address, buffer, (uint) size, ref write) ||
                write != size)
            {
                throw new AccessViolationException();
            }
        }

        /// <summary>
        ///     Reads 32 bit signed integer at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns></returns>
        public int ReadInt32(IntPtr address)
        {
            var buffer = new byte[4];
            ReadMemory(address, buffer, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        ///     Reads 32 bit signed integer at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns></returns>
        public long ReadInt64(IntPtr address)
        {
            var buffer = new byte[8];
            ReadMemory(address, buffer, 8);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        ///     Reads 32 bit unsigned integer at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns></returns>
        public uint ReadUInt32(IntPtr address)
        {
            var buffer = new byte[4];
            ReadMemory(address, buffer, 4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        ///     Reads single precision value at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns></returns>
        public float ReadFloat(IntPtr address)
        {
            var buffer = new byte[4];
            ReadMemory(address, buffer, 4);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        ///     Reads double precision value at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns></returns>
        public double ReadDouble(IntPtr address)
        {
            var buffer = new byte[8];
            ReadMemory(address, buffer, 8);
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        ///     Writes 32 bit unsigned integer at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public void WriteUInt32(IntPtr address, uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteMemory(address, buffer, 4);
        }

        /// <summary>
        ///     Writes 32 bit signed integer at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public void WriteInt32(IntPtr address, int value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteMemory(address, buffer, 4);
        }

        /// <summary>
        ///     Writes single precision value at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public void WriteFloat(IntPtr address, float value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteMemory(address, buffer, 4);
        }

        /// <summary>
        ///     Writes double precision value at the address
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public void WriteDouble(IntPtr address, double value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteMemory(address, buffer, 8);
        }

        #region IDisposable

        ~Memory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            Win32.CloseHandle(processHandle);
            Process = null;
            processHandle = IntPtr.Zero;
            isDisposed = true;
        }

        #endregion
    }
}