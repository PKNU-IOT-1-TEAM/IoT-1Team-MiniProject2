using CefSharp;
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.IO;
using System.Windows;
using getLocateMap.ViewModels;

namespace getLocateMap
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string locate_Html = string.Empty;
        int height_val = 400;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var map_url = $@"../../api2.html";
            string strHtml = File.ReadAllText(map_url);
            locate_Html = strHtml;
            // Debug.WriteLine(strHtml);
            browser.LoadHtml(strHtml, "https://www.team-one.com/");
        }

        private void browser_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var locateMap = new locateMap();
            locateMap.Owner = Application.Current.MainWindow;
            locateMap.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            string replace_Locate_Html = locate_Html.Replace("193px", "400px");
            locateMap.locateMap_browser.LoadHtml(replace_Locate_Html, "https://www.team-one.com/");
            locateMap.ShowDialog();
        }
    }
}

