using Simple_Loader.Addons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Core.Protection.Modules
{
    internal class DebuggerModule
    {
        #region Imports
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsDebuggerPresent();
        #endregion

        #region Call Funcs
        public static int PerformChecks()
        {
            if (CheckDebuggerManagedPresent() == 1)
            {
                utils.Error("Do not the cat");
                return 1;
            }

            if (CheckDebuggerUnmanagedPresent() == 1)
            {
                utils.Error("Do not the cat");
                return 1;
            }

            if (CheckRemoteDebugger() == 1)
            {
                utils.Error("Do not the cat");
                return 1;
            }

            return 0;
        }
        private static int CheckDebuggerManagedPresent()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return 1;
            }

            return 0;
        }
        private static int CheckDebuggerUnmanagedPresent()
        {
            if (IsDebuggerPresent())
            {
                return 1;
            }

            return 0;
        }
        private static int CheckRemoteDebugger()
        {
            var Cats = (bool)false;

            var Meows = CheckRemoteDebuggerPresent(System.Diagnostics.Process.GetCurrentProcess().Handle, ref Cats);

            if (Meows && Cats)
            {
                return 1;
            }

            return 0;
        }
        #endregion

    }
}
