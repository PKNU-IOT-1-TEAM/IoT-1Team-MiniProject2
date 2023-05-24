using CefSharp;
using Forecast_API.Logics;
using MahApps.Metro.Controls;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;
using Forecast_API.Models;
using Nobless_CHIM_SOY_Monitoring.Models;
using LibVLCSharp.Shared;
using System.Windows.Media.Imaging;
using LibVLCSharp.WPF;
using System.Windows.Controls;
using Nobless_CHIM_SOY_Monitoring.Views;

namespace Nobless_CHIM_SOY_Monitoring
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string strHtml; // html 파일 string 값 복사해서 모달 창에 쓰기위해 할당
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region <날씨>
            ForecastUI dataUi = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    // 기온, 바람, 파트 쿼리
                    var query = @"SELECT BaseDate,
	                                     BaseTime,
	                                     T1H,
                                         RN1,
                                         REH,
                                         CASE
		                                     WHEN VEC BETWEEN 22.5 AND 67.5 THEN '북동향'
		                                     WHEN VEC BETWEEN 67.6 AND 112.5 THEN '동향'
                                             WHEN VEC BETWEEN 112.6 AND 157.5 THEN '남동향'
                                             WHEN VEC BETWEEN 157.6 AND 202.5 THEN '남향'
                                             WHEN VEC BETWEEN 202.6 AND 247.5 THEN '남서향'
                                             WHEN VEC BETWEEN 247.6 AND 292.5 THEN '서향'
                                             WHEN VEC BETWEEN 292.6 AND 337.5 THEN '북서향'
                                             WHEN VEC > 337.5 OR VEC < 22.5 THEN '북향'
	                                     END AS VEC,             
                                         WSD,
	                                     PTYCondition,
	                                     SkyCondition
	                                     FROM ultrasrtfcst 
	                                     JOIN sky
	                                     ON ultrasrtfcst.SKY = sky.code
                                         JOIN pty
                                         ON ultrasrtfcst.PTY = pty.code
                                         WHERE ultrasrtfcst.Idx=1";


                    var cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        dataUi = new ForecastUI
                        {
                            BaseDate = reader.GetDateTime("BaseDate"),
                            BaseTime = reader.GetTimeSpan("BaseTime"),
                            T1H = reader.GetInt32("T1H"),
                            RN1 = reader.GetString("RN1"),
                            REH = reader.GetInt32("REH"),
                            VEC = reader.GetString("VEC"),
                            WSD = reader.GetInt32("WSD"),
                            SKYCondition = reader.GetString("SkyCondition"),
                            PTYCondition = reader.GetString("PTYCondition")
                        };
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB조회 오류 {ex.Message}");
            }

            
            LblTemp.Content = dataUi.T1H;   // 라벨 온도
            LblUpdateTime.Content = dataUi.BaseDate.ToString("yyyy-MM-dd") + " " + dataUi.BaseTime;    // 라벨 생성 시간
            LblSkyCon.Content = dataUi.PTYCondition == "없음"? dataUi.SKYCondition: dataUi.PTYCondition;
            string imagePath = WeaterImg(dataUi.PTYCondition, dataUi.SKYCondition, dataUi.BaseTime);   // 날씨 이미지
            ImgWeather.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            
            LblHumid.Content = $"{dataUi.REH}%"; //라벨 습도
            LblWind.Content = $"{dataUi.VEC} {dataUi.WSD}m/s";  // 라벨 바람
            LblPrecipitaion.Content = dataUi.RN1 == "강수없음" ? "-" : dataUi.RN1;   // 라벨 강수량

            #endregion

            #region <CCTV>

            // VLC NuGet 패키지를 받아서 비디오 플레이어 역할 할당.
            // cefSharp으로 안띄워지는 CCTV를 띄우기 위한 방법
            Core.Initialize();
            LibVLC _libVLC = new LibVLC();
            MediaPlayer _mediaPlayer = new MediaPlayer(_libVLC);
            CCTV_View.MediaPlayer = _mediaPlayer;
            _mediaPlayer.AspectRatio = "16:9";    // 화면 비율 설정
            CCTV_View.MediaPlayer.Play(new Media(_libVLC, new Uri(Commons.CCTV_Url)));
            ImgBtnStart.Source = new BitmapImage(new Uri("./images/stopbutton.png", UriKind.RelativeOrAbsolute));
            #endregion

            #region <지도
            var map_url = $@"../../api2.html";
            strHtml = File.ReadAllText(map_url);
            // Debug.WriteLine(strHtml);
            browser.LoadHtml(strHtml, "https://www.team-one.com/");
            #endregion
        }

        #region < 날씨 이미지 선택 함수>
        // 하늘상태, 강수량 상태에 따라 날씨 이미지 선택
        public string WeaterImg(string PTYCondition, string SKYCondition, TimeSpan BaseTime)
        {
            string imagePath = null;

            if (PTYCondition == "없음")
            {
                switch (SKYCondition)
                {
                    case "맑음":
                        imagePath = BaseTime < new TimeSpan(20, 0, 0) ? "./images/sunny.png" : "./images/sunny_night.png";
                        break;
                    case "구름많음":
                        imagePath = BaseTime < new TimeSpan(20, 0, 0) ? "./images/cloud.png" : "./images/cloud_night.png";
                        break;
                    case "흐림":
                        imagePath = "./images/cloudy.png";
                        break;
                }
            }
            else
            {
                switch (PTYCondition)
                {
                    case "비":
                        imagePath = "./images/rain.png";
                        break;
                    case "비/눈":
                    case "빗방울눈날림":
                        imagePath = "./images/snow_rain.png";
                        break;
                    case "눈":
                        imagePath = "./images/snow.png";
                        break;
                    case "빗방울":
                        imagePath = "./images/rain_drop.png";
                        break;
                    case "눈날림":
                        imagePath = "./images/snow_drop.png";
                        break;
                }
            }
            return imagePath;
        }
        #endregion



        // 새로고침 DB 업데이트
        public async void UpdateDB()
        {
            //if ((string)LblUpdateTime.Content == temp)
            //{
            //    //        await Commons.ShowMessageAsync("업데이트", "최근시간 입니다.");

            //    return;
            //}
            RequestForecastWebApi forecastApiInfo = new RequestForecastWebApi();
            PushDB pushDB = new PushDB();
            pushDB.InsertDB(forecastApiInfo.GetForecastWebApi());
            Debug.WriteLine("업데이트");
            MetroWindow_Loaded(this, new RoutedEventArgs());
            await Commons.ShowMessageAsync("업데이트", "최근시간으로 업데이트 완료.");
        }

        // cctv 재생 버튼
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(CCTV_View.MediaPlayer.IsPlaying)
            {
                CCTV_View.MediaPlayer.Pause();
                ImgBtnStart.Source = new BitmapImage(new Uri("./images/playbutton.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                CCTV_View.MediaPlayer.Play();
                ImgBtnStart.Source = new BitmapImage(new Uri("./images/stopbutton.png", UriKind.RelativeOrAbsolute));
            }
            
        }
        // 새창에 cctv
        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            if (CCTV_View.MediaPlayer.IsPlaying)
            {
                CCTV_View.MediaPlayer.Pause();
                ImgBtnStart.Source = new BitmapImage(new Uri("./images/playbutton.png", UriKind.RelativeOrAbsolute));
            }
            var cctvWindow = new CCTVWindow();
            cctvWindow.Owner = this;
            cctvWindow.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            cctvWindow.ShowDialog();
        }

        private void browser_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mapWindow = new MapWindow();
            mapWindow.Owner = Application.Current.MainWindow;
            mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // 여기에서 locate_Html 값 사용 193 픽셀 값 수정
            //int height_val = 400;
            //string replace_Locate_Html = locate_Html.Replace("193px", $"{height_val}px");
            //mapWindow.locateMap_browser.LoadHtml(replace_Locate_Html, "https://www.team-one.com/");
            //mapWindow.ShowDialog();
        }
    }
}
