using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SDC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string strEnableLog = ConfigurationSettings.AppSettings["EnableLog"].ToUpper();
            if (strEnableLog == "TRUE")
            {
                string s = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf('\\')) + "\\log4net.config";
                log4net.Config.XmlConfigurator.Configure(new FileStream(s, FileMode.Open));
                this.DispatcherUnhandledException += App_DispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is System.Exception)
            {
                LogHelper.ErrorLog(null, (System.Exception)e.ExceptionObject);
            }
        }

        public static void HandleException(Exception ex)
        {
            LogHelper.ErrorLog(null, ex);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogHelper.ErrorLog(null, e.Exception);
        }
    }
}
