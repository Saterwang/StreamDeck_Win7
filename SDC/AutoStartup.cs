using System;
using System.Windows.Forms;
using Microsoft.Win32;


namespace SDC
{
    class AutoStartup
    {
        static string Key = "sdc_" + Application.StartupPath.GetHashCode();

        public static bool Set(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                string path = Application.ExecutablePath;
                runKey = OpenUserRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if ( runKey == null ) {
                    return false;
                }
                if (enabled)
                {
                    runKey.SetValue(Key, path);
                }
                else
                {
                    runKey.DeleteValue(Key);
                }
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try { runKey.Close(); }
                    catch 
                    {  }
                }
            }
        }

        public static bool Check()
        {
            RegistryKey runKey = null;
            try
            {
                string path = Application.ExecutablePath;
                runKey = OpenUserRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey == null) {
                    return false;
                }
                string[] runList = runKey.GetValueNames();
                foreach (string item in runList)
                {
                    if (item.Equals(Key, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try { runKey.Close(); }
                    catch
                    {  }
                }
            }
        }

        public static RegistryKey OpenUserRegKey(string name, bool writable)
        {
            // we are building x86 binary for both x86 and x64, which will
            // cause problem when opening registry key
            // detect operating system instead of CPU
            RegistryKey userKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
            userKey = userKey.OpenSubKey(name, writable);
            return userKey;
        }
    }
}
