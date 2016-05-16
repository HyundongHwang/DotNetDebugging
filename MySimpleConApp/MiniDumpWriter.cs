using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleConApp
{
    public static class MiniDumpWriter
    {
        [Flags]
        public enum MINIDUMP_TYPE
        {
            MiniDumpNormal = 0x00000000,
            MiniDumpWithDataSegs = 0x00000001,
            MiniDumpWithFullMemory = 0x00000002,
            MiniDumpWithHandleData = 0x00000004,
            MiniDumpFilterMemory = 0x00000008,
            MiniDumpScanMemory = 0x00000010,
            MiniDumpWithUnloadedModules = 0x00000020,
            MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
            MiniDumpFilterModulePaths = 0x00000080,
            MiniDumpWithProcessThreadData = 0x00000100,
            MiniDumpWithPrivateReadWriteMemory = 0x00000200,
            MiniDumpWithoutOptionalData = 0x00000400,
            MiniDumpWithFullMemoryInfo = 0x00000800,
            MiniDumpWithThreadInfo = 0x00001000,
            MiniDumpWithCodeSegs = 0x00002000
        }

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

        public static bool Write()
        {
            var currentProcess = Process.GetCurrentProcess();
            var currentProcessHandle = currentProcess.Handle;
            var currentProcessId = (uint)currentProcess.Id;

            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"{DateTime.Now.ToString("yyMMdd-HHmmss")}.mini.dmp");
                var options = MINIDUMP_TYPE.MiniDumpNormal;

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                {
                    MiniDumpWriteDump(currentProcessHandle, currentProcessId, fs.SafeFileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
            }

            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"{DateTime.Now.ToString("yyMMdd-HHmmss")}.full.dmp");

                var options = 
                    MINIDUMP_TYPE.MiniDumpNormal |
                    MINIDUMP_TYPE.MiniDumpWithFullMemory |
                    MINIDUMP_TYPE.MiniDumpWithHandleData |
                    MINIDUMP_TYPE.MiniDumpWithProcessThreadData |
                    MINIDUMP_TYPE.MiniDumpWithThreadInfo; ;

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                {
                    MiniDumpWriteDump(currentProcessHandle, currentProcessId, fs.SafeFileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
            }

            return true;
        }
    }
}
