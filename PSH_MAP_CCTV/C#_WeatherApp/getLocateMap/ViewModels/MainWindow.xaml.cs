using CefSharp;
using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Windows;
using LibVLCSharp.Shared;
using getLocateMap.ViewModels;
using System.Diagnostics;
using System.Net;
using static OpenTK.Graphics.OpenGL.GL;
using LibVLCSharp.WPF;


namespace getLocateMap
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string locate_Html = string.Empty;
        private int height_val = 400;
        string CCTV_Url = "http://61.43.246.226:1935/rtplive/cctv_193.stream/playlist.m3u8";

        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        public MainWindow()
        {
            InitializeComponent();

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            CCTV_View.MediaPlayer = _mediaPlayer;

            CCTV_View.MediaPlayer.Play(new Media(_libVLC, new Uri(CCTV_Url)));
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
            string replace_Locate_Html = locate_Html.Replace("193px", $"{height_val}px");
            locateMap.locateMap_browser.LoadHtml(replace_Locate_Html, "https://www.team-one.com/");
            locateMap.ShowDialog();
        }
    }
}

