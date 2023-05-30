using LibVLCSharp.Shared;
using Nobless_CHIM_SOY_Monitoring.Logics;
using System;
using System.Windows;

namespace Nobless_CHIM_SOY_Monitoring.Views
{
    /// <summary>
    /// CCTVWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CCTVWindow : Window
    {
        public CCTVWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // VLC NuGet 패키지를 받아서 비디오 플레이어 역할 할당.
            // cefSharp으로 안띄워지는 CCTV를 띄우기 위한 방법
            Core.Initialize();
            LibVLC _libVLC = new LibVLC();
            MediaPlayer _mediaPlayer = new MediaPlayer(_libVLC);
            CCTV_View.MediaPlayer = _mediaPlayer;
            _mediaPlayer.AspectRatio = "16:9";    // 화면 비율 설정
            CCTV_View.MediaPlayer.Play(new Media(_libVLC, new Uri(Commons.CCTV_Url)));
      
        }
    }
}
