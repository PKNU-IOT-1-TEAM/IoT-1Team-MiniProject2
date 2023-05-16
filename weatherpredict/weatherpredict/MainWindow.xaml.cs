using Caliburn.Micro;
using MySqlConnector;
using System.Windows;

namespace weatherpredict
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (MySqlConnection conn = new MySqlConnection("Server=pknuiot1team.cghin4qcf4s7.ap-northeast-2.rds.amazonaws.com;Port=3306;Database=miniproject02;Uid=pknuiot1team;Pwd=2V3lhihd8gIQ3krjNMf2;"))
            {
                conn.Open();

                string selQeury = @"SELECT * FROM ultrasrtfcst";
                MySqlCommand selCommand = new MySqlCommand(selQeury, conn);
                MySqlDataReader rdr = selCommand.ExecuteReader();
                ListWeather = new BindableCollection<Weather>();

                while (rdr.Read())
                {
                    var emp = new Weather
                    {
                        FcstDate = rdr["FcstDate"].ToString(),
                        FcstTime = rdr["FcstTime"].ToString(),

                        T1H = rdr["T1H"].ToString(), // 기온 (℃)
                        RN1 = rdr["RN1"].ToString(), // 1시간 강수량 (mm)
                        SKY = rdr["SKY"].ToString(), // 하늘상태
                        UUU = rdr["UUU"].ToString(), // 동서바람성분 (m/s)
                        VVV = rdr["VVV"].ToString(), // 남북바람성분 (m/s)
                        REH = rdr["REH"].ToString(), // 습도 (%)
                        PTY = rdr["PTY"].ToString(), // 강수형태 
                        LGT = rdr["LGT"].ToString(), // 낙뢰
                        VEC = rdr["VEC"].ToString(), // 풍향
                        WSD = rdr["WSD"].ToString(), // 풍속
                    };
                    ListWeather.Add(emp);
                }

                // 리소스 해제
                rdr.Close();
                conn.Close();
            }
        }

        private Weather weather;

        public BindableCollection<Weather> ListWeather { get; set;  } 

        public string FcstDate
        {
            get => weather.FcstDate;
            set
            {
                weather.FcstDate = value;
            }
        }

        public string FcstTime
        {
            get => weather.FcstTime;
            set
            {
                weather.FcstTime = value;
            }
        }

        public string T1H
        {
            get => weather.T1H;
            set
            {
                weather.T1H = value;
            }
        }

        public string RN1
        {
            get => weather.RN1;
            set
            {
                weather.RN1 = value;
            }
        }

        public string SKY
        {
            get => weather.SKY;
            set
            {
                weather.SKY = value;
            }
        }

        public string UUU
        {
            get => weather.UUU;
            set
            {
                weather.UUU = value;    
            }
        }

        public string VVV
        {
            get => weather.VVV;
            set
            {
                weather.VVV = value;
            }
        }

        public string REH
        {
            get => weather.REH;
            set
            {
                weather.REH = value;
            }
        }

        public string PTY
        {
            get => weather.PTY;
            set
            {
                weather.PTY = value;
            }
        }

        public string LGT
        {
            get => weather.LGT;
            set
            {
                weather.LGT = value;
            }
        }

        public string VEC
        {
            get => weather.VEC;
            set
            {
                weather.VEC = value;
            }
        }

        public string WSD
        {
            get => weather.WSD;
            set
            {
                weather.WSD = value;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
