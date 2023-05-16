using MahApps.Metro.Controls;
using MiniProject_weatherApp.Logics;
using MiniProject_weatherApp.Model;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MiniProject_weatherApp
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
            {
                conn.Open();
                var query = @"SELECT
                                   RN1,
                                   REH,
                                   VEC,
                                   WSD
                                FROM ultrasrtfcst
                                WHERE Idx=1";
                MySqlCommand cmd = new MySqlCommand(query, conn); // 쿼리문 날린거
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); // 데이터 가져오는거
                DataSet ds = new DataSet();
                adapter.Fill(ds, "ultrasrtfcst");
                Ultrasrtfcst fcst = new Ultrasrtfcst
                {
                    RN1 = Convert.ToString(ds.Tables["ultrasrtfcst"].Rows[0]["RN1"]),
                    REH = Convert.ToInt32(ds.Tables["ultrasrtfcst"].Rows[0]["REH"]),
                    VEC = Convert.ToInt32(ds.Tables["ultrasrtfcst"].Rows[0]["VEC"]),
                    WSD = Convert.ToInt32(ds.Tables["ultrasrtfcst"].Rows[0]["WSD"])
                };
                //List<Ultrasrtfcst> list = new List<Ultrasrtfcst>();

                //foreach (DataRow row in ds.Tables["ultrasrtfcst"].Rows)
                //{
                //    list.Add(new Ultrasrtfcst
                //    {
                //        RN1 = Convert.ToString(row["RN1"]),
                //        REH = Convert.ToInt32(row["REH"]),
                //        VEC = Convert.ToInt32(row["VEC"]),
                //        WSD = Convert.ToInt32(row["WSD"])
                //    });
                //}
                string vvec = null;
                if (22.5 < fcst.VEC && fcst.VEC < 67.5)
                {
                    vvec = "북동향";
                }
                else if (67.5 < fcst.VEC && fcst.VEC < 112.5)
                {
                    vvec = "동향";
                }
                else if (112.5 < fcst.VEC && fcst.VEC < 157.5)
                {
                    vvec = "남동향";
                }
                else if (157.5 < fcst.VEC && fcst.VEC < 202.5)
                {
                    vvec = "남향";
                }
                else if (202.5 < fcst.VEC && fcst.VEC < 247.5)
                {
                    vvec = "남서향";
                }
                else if (247.5 < fcst.VEC && fcst.VEC < 292.5)
                {
                    vvec = "서향";
                }
                else if (292.5 < fcst.VEC && fcst.VEC < 337.5)
                {
                    vvec = "북서향";
                }
                else if (337.5 < fcst.VEC || fcst.VEC < 22.5)
                {
                    vvec = "북향";
                }
                TxtVec.Content = $"{vvec} {fcst.WSD}m/s";
                TxtReh.Content = $"{fcst.REH}%";
                if (fcst.RN1 == "강수없음")
                {
                    fcst.RN1 = "-";
                }
                TxtRn1.Content = $"{fcst.RN1} mm";
            }
        }
        
    }
}
