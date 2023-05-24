using LibVLCSharp.Shared;
using System;
using System.Windows;
using System.Windows.Controls;
using LibVLCSharp.WPF;
namespace test2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private bool _isFullScreen = false;
        private double _windowWidth;
        private double _windowHeight;

        public MainWindow()
        {
            InitializeComponent();
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
            var media = new Media(_libVLC, new Uri("path_to_your_video_file"));
            _mediaPlayer.Play(media);
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFullScreen)
            {
                ExitFullScreen();
            }
            else
            {
                GoFullScreen();
            }
        }

        private void GoFullScreen()
        {
            _windowWidth = Width;
            _windowHeight = Height;

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;

            // 확대/축소 모드 설정
            _mediaPlayer.AspectRatio = _windowWidth / _windowHeight;

            _isFullScreen = true;
        }

        private void ExitFullScreen()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;

            // 확대/축소 모드 초기화
            _mediaPlayer.AspectRatio = 0;

            Width = _windowWidth;
            Height = _windowHeight;

            _isFullScreen = false;
        }
    }
}
