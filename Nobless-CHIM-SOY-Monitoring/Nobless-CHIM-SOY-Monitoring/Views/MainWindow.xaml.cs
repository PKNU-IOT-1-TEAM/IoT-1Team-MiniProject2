using CefSharp;
using MahApps.Metro.Controls;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;
using Nobless_CHIM_SOY_Monitoring.Models;
using LibVLCSharp.Shared;
using System.Windows.Media.Imaging;
using LibVLCSharp.WPF;
using System.Windows.Controls;
using Nobless_CHIM_SOY_Monitoring.Views;
using Nobless_CHIM_SOY_Monitoring.Logics;
using System.Collections.Generic;

namespace Nobless_CHIM_SOY_Monitoring
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string strHtml=null; // html 파일 string 값 복사해서 모달 창에 쓰기위해 할당
        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region <날씨>
            List<ForecastData> forecastItems = new List<ForecastData>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    // 기온, 바람, 파트 쿼리
                    var query = @"SELECT BaseDate,
	                                     BaseTime,
                                         FcstTime,
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
                                         ORDER BY FcstTime";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "forcest");


                    foreach (DataRow row in ds.Tables["forcest"].Rows)
                    {
                        forecastItems.Add(new ForecastData
                        {
                            BaseDate = (DateTime)row["BaseDate"],   // DB에서 DATE 형식
                            BaseTime = (TimeSpan)row["BaseTime"],   // DB에서 TIME 형식
                            FcstTime = (TimeSpan)(row["FcstTime"]), // DB에서 TIME 형식
                            T1H = Convert.ToInt32(row["T1H"]),
                            RN1 = Convert.ToString(row["RN1"]),
                            REH = Convert.ToInt32(row["REH"]),
                            VEC = Convert.ToString(row["VEC"]),
                            WSD = Convert.ToInt32(row["WSD"]),
                            SKYCondition = Convert.ToString(row["SkyCondition"]),
                            PTYCondition = Convert.ToString(row["PTYCondition"]),
                            ImgPath = WeaterImg(row["PTYCondition"].ToString(), row["SkyCondition"].ToString(), (TimeSpan)row["FcstTime"])
                        });
                    }
                    dataGrid.ItemsSource = forecastItems;   // 데이터그리드 바인딩
                    
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB조회 오류 {ex.Message}");
            }

            
            LblTemp.Content = forecastItems[0].T1H;   // 라벨 온도
            LblUpdateTime.Content = forecastItems[0].BaseDate.ToString("yyyy-MM-dd") + " " + forecastItems[0].BaseTime;    // 라벨 생성 시간
            LblSkyCon.Content = forecastItems[0].PTYCondition == "없음"? forecastItems[0].SKYCondition: forecastItems[0].PTYCondition;
            ImgWeather.Source = new BitmapImage(new Uri(forecastItems[0].ImgPath, UriKind.RelativeOrAbsolute));            
            LblHumid.Content = $"{forecastItems[0].REH}%"; //라벨 습도
            LblWind.Content = $"{forecastItems[0].VEC} {forecastItems[0].WSD}m/s";  // 라벨 바람
            LblPrecipitaion.Content = forecastItems[0].RN1 == "강수없음" ? "-" : forecastItems[0].RN1;   // 라벨 강수량

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
            ImgBtnStart.Source = new BitmapImage(new Uri("../images/stopbutton.png", UriKind.RelativeOrAbsolute));
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
                        imagePath = BaseTime < new TimeSpan(20, 0, 0) ? "../images/sunny.png" : "../images/sunny_night.png";
                        break;
                    case "구름많음":
                        imagePath = BaseTime < new TimeSpan(20, 0, 0) ? "../images/cloud.png" : "../images/cloud_night.png";
                        break;
                    case "흐림":
                        imagePath = "../images/cloudy.png";
                        break;
                }
            }
            else
            {
                switch (PTYCondition)
                {
                    case "비":
                        imagePath = "../images/rain.png";
                        break;
                    case "비/눈":
                    case "빗방울눈날림":
                        imagePath = "../images/snow_rain.png";
                        break;
                    case "눈":
                        imagePath = "../images/snow.png";
                        break;
                    case "빗방울":
                        imagePath = "../images/rain_drop.png";
                        break;
                    case "눈날림":
                        imagePath = "../images/snow_drop.png";
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
                ImgBtnStart.Source = new BitmapImage(new Uri("../images/playbutton.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                CCTV_View.MediaPlayer.Play();
                ImgBtnStart.Source = new BitmapImage(new Uri("../images/stopbutton.png", UriKind.RelativeOrAbsolute));
            }
            
        }
        // 버튼 클릭시 cctv 모달창
        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            if (CCTV_View.MediaPlayer.IsPlaying)
            {
                CCTV_View.MediaPlayer.Pause();
                ImgBtnStart.Source = new BitmapImage(new Uri("../images/playbutton.png", UriKind.RelativeOrAbsolute));
            }
            var cctvWindow = new CCTVWindow();
            cctvWindow.Owner = this;
            cctvWindow.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            cctvWindow.ShowDialog();
        }

        // 지도 더블클릭 시 지도 모달창
        private void browser_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mapWindow = new MapWindow(strHtml);
            mapWindow.Owner = Application.Current.MainWindow;
            mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // 여기에서 locate_Html 값 사용 365 픽셀 값 수정
            mapWindow.ShowDialog();
        }
    }
}
