using CefSharp;
using Forecast_API.Logics;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Forecast_API.Models;
using Nobless_CHIM_SOY_Monitoring.Models;
using System.Runtime.InteropServices.ComTypes;

namespace Nobless_CHIM_SOY_Monitoring
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

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ForecastUI dataUi = new ForecastUI();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    // 기온 파트 쿼리
                    var query = @"SELECT BaseDate,
	                                     BaseTime,
	                                     T1H,
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

                        dataUi.BaseDate = reader.GetDateTime("BaseDate");
                        dataUi.BaseTime = reader.GetTimeSpan("BaseTime");
                        dataUi.T1H = reader.GetInt32("T1H");
                        dataUi.SKYCondition = reader.GetString("SkyCondition");
                        dataUi.PTYCondition = reader.GetString("PTYCondition");
                    }
                    reader.Close();
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
        }

        // 하늘상태 강수량 상태에 따라 날씨 이미지 선택
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
                        imagePath = "./cloudy.png";
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
                        imagePath = "./images/raindrop.png";
                        break;
                    case "눈날림":
                        imagePath = "./images/snow_drift";
                        break;
                }
            }
            return imagePath;
        }

            // 지도
        private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            var map_url = $@"../../api2.html";
            string strHtml = File.ReadAllText(map_url);
            Debug.WriteLine(strHtml);
            browser.LoadHtml(strHtml, "https://www.team-one.com/");
        }
        
    }
}
