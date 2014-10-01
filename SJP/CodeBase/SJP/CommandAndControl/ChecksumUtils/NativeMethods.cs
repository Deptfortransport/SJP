using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.ChecksumUtils
{
    internal static class NativeMethods
    {
        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);
    }
}
