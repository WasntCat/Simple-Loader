using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Core.Protection.Modules
{
    internal class VMModule
    {
        // Add More to the two list to be more effective
        private static string[] VmProcess = { "VBoxService", "VBoxTray" };
        private static string[] VmDriver = { "VBoxGuest.sys", "VBoxMouse.sys", "VBoxSF.sys", "VBoxWddm.sys" };

        public static bool PreventVM()
        {
            bool Vmdectecion = false;
            Process[] ProcessList = Process.GetProcesses();
            foreach (Process proc in ProcessList)
            {
                for (int i = 0; i < VmProcess.Length; i++)
                {
                    if (proc.ProcessName == VmProcess[i])
                    {
                        Vmdectecion = true;
                        return Vmdectecion;
                    }
                }
            }
            for (int i = 0; i < VmDriver.Length; i++)
            {
                if (Directory.Exists("C:\\Windows\\System32\\drivers\\" + VmDriver[i]))
                {
                    Vmdectecion = true;
                    return Vmdectecion;
                }
            }
            return Vmdectecion;
        }
    }
}
