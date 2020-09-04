using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShowPing.src
{
    static class colors
    {
        public static string white = "#FFC9C9C9";
        public static string green = "#FF22A01C";
        public static string red = "#FFB91818";
    }

    class PingProgram
    {
        string hostname = "51.178.64.97";   // warmane Blackrock realm server
        //int port = 8095;  // server port - not needed
        const int sleeptime = 300;
        int timeout = 250;

        TextBlock tb_ping;
        int posY = 274;     // more goes down

        public PingProgram(ref TextBlock pingValue_textBlock)
        {
            tb_ping = pingValue_textBlock;
        }
        public PingProgram(ref TextBlock pingValue_textBlock, string[] args)
        {
            tb_ping = pingValue_textBlock;
            ParseArgs(args);
        }

        void ParseArgs(string[] args)
        {
            if(args.Length == 1)
            {
                bool arg2 = int.TryParse(args[0], out posY);
                if (arg2)
                {
                    if (posY < 0 || posY > 2000)
                    {
                        posY = 274;
                    }
                }
                else
                {
                    hostname = args[0];
                }
            }

            else if (args.Length == 2)
            {
                bool arg2 = int.TryParse(args[1], out posY);
                if (arg2)
                {
                    if (posY < 0 || posY > 2000)
                    {
                        posY = 274;
                    }

                    hostname = args[0];
                }
                else
                {
                    arg2 = int.TryParse(args[0], out posY);
                    if (arg2)
                    {
                        if (posY < 0 || posY > 2000)
                        {
                            posY = 274;
                        }

                        hostname = args[1];
                    }
                }
            }
            tb_ping.Margin = new Thickness(20, posY, 0, 0);
        }

        public void Run()
        {
            Console.WriteLine("Pinging WoW Warmane server on realm: Blackrock ip 51.178.64.97, port 8095");

            Ping ping = new Ping();

            while(true)
            {
                try
                {
                    PingReply pingreply = ping.Send(hostname, timeout);
                    if (pingreply.Status == IPStatus.Success)
                    {
                        //Console.WriteLine("Address: {0}", pingreply.Address);
                        //Console.WriteLine("status: {0}", pingreply.Status);
                        //Console.WriteLine("Round trip time: {0}", pingreply.RoundtripTime);

                        Color color;
                        if (pingreply.RoundtripTime > 100)
                        {
                            color = (Color)ColorConverter.ConvertFromString(colors.red);

                            Application.Current.Dispatcher.BeginInvoke((Action)(() => tb_ping.Foreground = new SolidColorBrush(color)));
                        }
                        else
                        {
                            color = (Color)ColorConverter.ConvertFromString(colors.white);

                            Application.Current.Dispatcher.BeginInvoke((Action)(() => tb_ping.Foreground = new SolidColorBrush(color)));
                        }

                        Application.Current.Dispatcher.BeginInvoke((Action)(() => tb_ping.Text = pingreply.RoundtripTime.ToString()));
                    }
                    else
                    {
                        Console.WriteLine("Bad ip or hostname given.");
                    }
                }
                catch (PingException ex)
                {
                    Console.WriteLine(ex);
                    Color color = (Color)ColorConverter.ConvertFromString(colors.red);

                    Application.Current.Dispatcher.BeginInvoke((Action)(() => tb_ping.Foreground = new SolidColorBrush(color)));
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => tb_ping.Text = "TimeOut"));
                }
                finally
                {
                    Thread.Sleep(sleeptime);
                }
            }

        }
    }
}
