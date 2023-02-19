using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Addons
{
    internal class utils
    {
        // Logs(""); // Error(""); // Warning("");
        #region Console Printer
        public static void Logs(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("<");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
        }
        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(">");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        #endregion

        #region Misc
        public static void Start(string path, [Optional] string optional)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = optional;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
            proc.WaitForExit();
        }
        #endregion

        #region String Builder
        public static string GenerateRandomString(int size)
        {
            byte[] array = new byte[4 * size];
            using (RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngcryptoServiceProvider.GetBytes(array);
            }
            StringBuilder stringBuilder = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                long num = (long)((ulong)BitConverter.ToUInt32(array, i * 4) % (ulong)((long)chars.Length));
                stringBuilder.Append(chars[(int)(checked((IntPtr)num))]);
            }
            return stringBuilder.ToString();
        }
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
        internal static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        #endregion

    }
}

