using LibVLCSharp.Shared;
using System;
using System.Windows;
using LibVLCSharp.WPF;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CCTV_Url = "http://61.43.246.226:1935/rtplive/cctv_193.stream/playlist.m3u8";
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        public MainWindow()
        {
            InitializeComponent();
            // CCTV용 : api받은 영상은 60초만 제공 => 해결방법으로 고유주소 찾아서 사용
            InitializeLibVLC();
            PlayVideo();

        }
        private void InitializeLibVLC()
        {
            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView.MediaPlayer = _mediaPlayer;
        }
        private void PlayVideo()
        {
            var media = new Media(_libVLC, new Uri(CCTV_Url));
            _mediaPlayer.Play(media);
        }
        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }
    }
}
