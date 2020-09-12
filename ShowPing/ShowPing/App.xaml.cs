using System;
using System.Threading;
using System.Windows;
using ShowPing.src;

namespace ShowPing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();
            
            wnd.Activate();
            Current.MainWindow.Activate();
            Current.MainWindow.Topmost = true;

            if (e.Args.Length == 0)
            {
                Console.WriteLine("0 arguments");
                PingProgram pingProgram = new PingProgram(ref wnd.textBlock_ping_value);
                Thread pingMe = new Thread(new ThreadStart(pingProgram.Run));
                pingMe.Start();
            }
            else if (e.Args.Length == 1 || e.Args.Length == 2)
            {
                Console.WriteLine("3 arguments");
                PingProgram pingProgram = new PingProgram(ref wnd.textBlock_ping_value, e.Args);
                Thread pingMe = new Thread(new ThreadStart(pingProgram.Run));
                pingMe.Start();
            }
            else
            {
                Environment.Exit(0);
            }
            
            wnd.Show();
        }
    }
}
