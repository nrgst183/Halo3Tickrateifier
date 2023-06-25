using System;
using System.Runtime.InteropServices;

namespace CheatEngine
{
    public static class Win32
    {
        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = 1,
            PROCESS_CREATE_THREAD = 2,
            PROCESS_SET_SESSIONID = 4,
            PROCESS_VM_OPERATION = 8,
            PROCESS_VM_READ = 16,
            PROCESS_VM_WRITE = 32,
            PROCESS_DUP_HANDLE = 64,
            PROCESS_CREATE_PROCESS = 128,
            PROCESS_SET_QUOTA = 256,
            PROCESS_SET_INFORMATION = 512,
            PROCESS_QUERY_INFORMATION = 1024
        }

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr objectHandle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess([MarshalAs(UnmanagedType.U4)] ProcessAccessType access,
            [MarshalAs(UnmanagedType.Bool)] bool inheritHandler, uint processId);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr process, IntPtr address, byte[] buffer, uint size,
            ref uint read);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr process, IntPtr address, byte[] buffer, uint size,
            ref uint written);
    }
}