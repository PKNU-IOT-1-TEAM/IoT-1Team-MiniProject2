using Caliburn.Micro;
using MySqlConnector;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using wp10_employeesApp.Models;

namespace wp10_employeesApp.ViewModels
{
    public class MainViewModel : Screen
    {
        private Weather weather;

        public BindableCollection<Weather> ListWeather { get; set; }


        public string FcstTime
        {
            get => weather.FcstTime;
            set
            {
                weather.FcstTime = value;
                NotifyOfPropertyChange(nameof(FcstTime));
            }
        }

        public string T1H
        {
            get => weather.T1H;
            set
            {
                weather.T1H = value;
                NotifyOfPropertyChange(nameof(T1H));
            }
        }


        public string REH
        {
            get => weather.REH;
            set
            {
                weather.REH = value;
                NotifyOfPropertyChange(nameof(REH));
            }
        }


        public string WSD
        {
            get => weather.WSD;
            set
            {
                weather.WSD = value;
                NotifyOfPropertyChange(nameof(WSD));
            }
        }

        public string Image
        {
            get => weather.Image;
            set
            {
                weather.Image = value;
                NotifyOfPropertyChange(nameof(Image));
            }
        }

        public string WeaterImg(string PTYCondition, string SKYCondition, string BaseTime)
        {

            DateTime dateTime = Convert.ToDateTime(BaseTime);

            string imagePath = null;

            if (PTYCondition == "없음")
            {
                switch (SKYCondition)
                {
                    case "맑음":
                        imagePath = dateTime.TimeOfDay >= new TimeSpan(20, 0, 0) ? "./sunny.png" : "./sunny_night.png";
                        break;
                    case "구름많음":
                        imagePath = dateTime.TimeOfDay >= new TimeSpan(20, 0, 0) ? "./cloud.png" : "./cloud_night.png";
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
                        imagePath = "./rain.png";
                        break;
                    case "비/눈":
                    case "빗방울눈날림":
                        imagePath = "./snow_rain.png";
                        break;
                    case "눈":
                        imagePath = "./snow.png";
                        break;
                    case "빗방울":
                        imagePath = "./raindrop.png";
                        break;
                    case "눈날림":
                        imagePath = "./snow_drift";
                        break;
                }
            }
            return imagePath;
        }

        public MainViewModel()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=pknuiot1team.cghin4qcf4s7.ap-northeast-2.rds.amazonaws.com;Port=3306;Database=miniproject02;Uid=pknuiot1team;Pwd=2V3lhihd8gIQ3krjNMf2;"))
            {
                conn.Open();

                string selQeury = @"SELECT FcstTime,
                                        BaseTime,
                                        T1H,
                                        PTYCondition,
                                        SkyCondition,
                                        REH, WSD
                                        FROM ultrasrtfcst 
                                        JOIN sky
                                        ON ultrasrtfcst.SKY = sky.code
                                         JOIN pty
                                         ON ultrasrtfcst.PTY = pty.code;";
                MySqlCommand selCommand = new MySqlCommand(selQeury, conn);
                MySqlDataReader rdr = selCommand.ExecuteReader();
                ListWeather = new BindableCollection<Weather>();

                while (rdr.Read())
                {

                    var emp = new Weather
                    {
                        FcstTime = rdr["FcstTime"].ToString().Substring(0, 2) + "시",
                        T1H = rdr["T1H"].ToString() + "℃", // 기온 (℃)
                        Image = WeaterImg(rdr["PTYCondition"].ToString(), rdr["SkyCondition"].ToString(),rdr["BaseTime"].ToString()),
                        REH = rdr["REH"].ToString() + "%", // 습도 (%)
                        WSD = rdr["WSD"].ToString() + "m/s", // 풍속
                    };
                    ListWeather.Add(emp);
                }
            }
        }
    }
}
