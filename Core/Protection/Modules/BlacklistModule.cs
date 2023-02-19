using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Core.Protection.Modules
{
    internal class BlacklistModule
    {
        // Add More to the two list to be more effective
        private static string[] IllegalProcessName = { "Fiddler", "Wireshark", "dumpcap", "dnSpy", "dnSpy-x86", "cheatengine-x86_64", "HTTPDebuggerUI", "Procmon", "Procmon64", "Procmon64a", "ProcessHacker", "x32dbg", "x64dbg", "DotNetDataCollector32", "DotNetDataCollector64" };
        private static string[] IllegalWindowName = { "Progress Telerik Fiddler Web Debugger", "Wireshark" };

        public static bool ProccesBlacklist()
        {
            Process[] ProcessList = Process.GetProcesses();
            foreach (Process proc in ProcessList)
            {
                for (int i = 0; i < IllegalProcessName.Length; i++)
                {
                    if (proc.ProcessName == IllegalProcessName[i])
                    {
                        return true;
                    }
                }

                for (int i = 0; i < IllegalWindowName.Length; i++)
                {
                    if (proc.MainWindowTitle == IllegalWindowName[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
