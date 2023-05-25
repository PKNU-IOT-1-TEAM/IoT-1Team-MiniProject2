using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Nobless_CHIM_SOY_Monitoring.Views
{
    /// <summary>
    /// MapWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MapWindow : Window
    {
        private string strHtml;
        public MapWindow()
        {
            InitializeComponent();
        }

        public MapWindow(string strHtml): this()    // this() 하면 생성자 MapWindow() 실행됨.
        {
            this.strHtml = strHtml;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string replace_Html = strHtml.Replace("365", $"{GridMap.ActualHeight-20}");
            Debug.WriteLine(replace_Html);
            locateMap_browser.LoadHtml(replace_Html, "https://www.team-one.com/");
           
        }
    }
}
