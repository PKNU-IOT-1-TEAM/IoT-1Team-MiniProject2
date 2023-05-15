using CefSharp;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using getLocateMap.Logics;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using RestSharp;
using System.Net.Http;
using System.Threading.Tasks;

namespace getLocateMap
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var map_url = $@"../../api2.html";
            string strHtml = File.ReadAllText(map_url);
            Debug.WriteLine(strHtml);
            browser.LoadHtml(strHtml, "https://www.team-one.com/");
        }
    }
}

