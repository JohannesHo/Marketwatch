using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Marketwatch
{
    static class Marketwatch
    {
        public static bool isDebug = false;

        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String keyName = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
            if (Registry.GetValue(keyName, "Marketwatch.exe", null) == null)
            {
                Registry.SetValue(keyName, "Marketwatch.exe", 0x2711, RegistryValueKind.DWord);
            }

            checkDebugging();
            if (isDebug)
            {
                AllocConsole();
                IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                Encoding encoding = Encoding.GetEncoding(MY_CODE_PAGE);
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);

                if (Registry.GetValue(keyName, "Marketwatch.vshost.exe", null) == null)
                {
                    Registry.SetValue(keyName, "Marketwatch.vshost.exe", 0x2711, RegistryValueKind.DWord);
                }
            }
            //Update Settings if needed
            if (Properties.Settings.Default.upgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.upgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.language != "")
            {
                string language_string = "";
                switch (Properties.Settings.Default.language)
                {
                    case "English":
                        language_string = "en";
                        break;
                    case "German":
                        language_string = "de";
                        break;
                    default:
                        language_string = "en";
                        break;
                }
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language_string);
            }
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMarketwatch());
        }

        [ConditionalAttribute("DEBUG")]
        public static void checkDebugging()
        {
            isDebug = true;
        }
    }
}
