using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ExStrataUWP.ColourPattern;
using ExStrataUWP.APIs;
using ExStrataUWP.Communication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ExStrataUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Set log output to console
            Log.ConsoleOutputMessage = true;
            Log.ConsoleOutputAPI = true;
            Log.ConsoleOutputError = true;
            Log.ConsoleOutputRawData = false;

            Log.Message("Started program.");

            // Start the API Manager with a few APIs
            APIManager manager = new APIManager(
                new WatchTest(1000 * 60 * 2),
                new Watch9292(1000 * 60, "NHL Stenden Hogeschool"),
                new WatchWeather(1000 * 60, "NL", "Leeuwarden"));

            manager.Add(new WatchCBS(1000 * 60));

            // Later input accepteren van named pipe
            while (true) ;
        }
    }
}
