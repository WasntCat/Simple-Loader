using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Core.Protection.Modules
{
    internal class DumpModule
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        public static void PreventDumping()
        {
            IntPtr myMod = GetModuleHandle(null);
            Helper.VirtualProtect(myMod, 0x1000, 0x40, out _);
            Helper.WriteProcessMemory(Process.GetCurrentProcess().Handle, myMod, 0x00, 4, out _);
            Helper.WriteProcessMemory(Process.GetCurrentProcess().Handle, (myMod + 0x3C), 0x00, 4, out _);
        }
    }
}
