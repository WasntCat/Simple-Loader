using Simple_Loader.Addons;
using Simple_Loader.Core.Protection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Simple_Loader
{
    internal class Program
    {
        // Using Base64 To Make A simple Cover For keyauths Server Stuff
        // You can change this to be AES or both or whatever you do you 
        public static Authentication.Database AuthCheck = new Authentication.Database(
        name: Encoding.Unicode.GetString(Convert.FromBase64String("")),
        ownerid: Encoding.ASCII.GetString(Convert.FromBase64String("")),
        secret: Encoding.UTF32.GetString(Convert.FromBase64String("")),
        version: Encoding.UTF7.GetString(Convert.FromBase64String(""))
        );
        static void Main(string[] args)
        {
     

            Console.Title = "Loading. . .";

            #region Starting protection Stage One
            try { Intger.StartProtections(); utils.Logs("Starting Core Libs"); } catch { utils.Error("A Error Had Occurred, Please Contact Staff | Killing. . ."); Thread.Sleep(3000); Environment.Exit(0); }
            #endregion

            #region Keyauth to Menu
            Console.Title = "waiting For auth [][][][]  ";

            AuthCheck.init();

            utils.Logs("Checking For licenses please Wait. . .");
            if (File.Exists(@"C:\Wholesone.Darling"))
            {
                string file = File.ReadAllText(@"C:\Wholesone.Darling");
                AuthCheck.license(file);
                if (AuthCheck.response.success)
                {
                    utils.Logs("Loading Menu. . .");
                    Thread.Sleep(1500);
                    Menu();
                }
                else
                {
                    utils.Error("ERROR Invaild Key. . .");
                    File.Delete(@"C:\Wholesone.Darling");
                }
            }
            utils.Logs("Enter Key");
            var UserKey = Console.ReadLine();
            AuthCheck.license(UserKey);
            if (AuthCheck.response.success)
            {
                File.Create(@"C:\Wholesone.Darling").Dispose();
                File.WriteAllText(@"C:\Wholesone.Darling", UserKey);
                Menu();
            }
            else
            {
                utils.Error("ERROR Invaild Key. . .");
                Helper.SelfDelete();
            }

            // Should make a timer to check this every 2 mins or something around that time
            AuthCheck.check(); // Checking again just in case

            utils.Logs("Vaild Key, Saving for auto load. . .");
            utils.Logs("Creating Menu Please Wait. . .");
            Thread.Sleep(500);
            Menu();
            #endregion
        }

        public static void Menu()
        {
            Console.Title = "Darling <3";

            utils.Logs("Creating Menu Please Wait. . .");
            Thread.Sleep(250);
            Console.Clear();
            utils.Logs("                =====================                                      =====================");
            utils.Logs(@"                                      Add your Based Ascii Art Logo Here ");
            utils.Logs("                         ========================================================");
            utils.Logs("                         =  [1] Meow             [2] Meow            [3] Meowe  =");
            utils.Logs("                         =                    [4] Meow Emulator                 =");
            utils.Logs("                         ========================================================");
            utils.Logs("                         Selection | ");
            string Choices = Console.ReadLine();
            switch (Choices)
            {

                case "1":
                    Menu();
                    break;
                case "2":
                    Menu();
                    break;
                case "3":
                    Menu();
                    break;
                case "4":
                    BasicAutoInjMapping();
                    break;
            }
            Menu();
        }

        public static void BasicAutoInjMapping()
        {
            WebClient webClient = new WebClient();

            string Driver = utils.GenerateRandomString(20) + ".sys"; // Creating A Mixed Name For your Driver
            string Mapper = utils.GenerateRandomString(20) + ".exe"; // Creating A Mixed Name For your Mapper
            string Injector = utils.GenerateRandomString(20) + ".exe"; // Creating A Mixed Name For your Injector Or emulator

            // Downloading The Files // Should Probally Added A modded Encp Webclient stuff here to prevent Reverseing Or You could also use the keyauth way
            // Or you can Add a method To securelly Protect Emmbeded Files So they Can be loaded from inside the loader and not dumped just up to you
            webClient.DownloadFile("", @"C:\Windows\" + Driver);
            webClient.DownloadFile("", @"C:\Windows\" + Mapper);
            webClient.DownloadFile("", @"C:\Windows\" + Injector);

            utils.Start(@"C:\Windows\" + Mapper, @"C:\Windows\" + Driver); // Maping the driver

            //Logger.Text += "Starting Loader" + Environment.NewLine;
            //Logger.Text += "Changing to Loader Console" + Environment.NewLine;

            utils.Start($@"C:\Windows\" + Injector); // Starting Your Injector Or Emulator
        }
        // You could also add Junk Methods, string, ints ect... manually thro the entire project
        // Could also add dnspy or dotpeak crashers, basiclly just long vals



    }
}
