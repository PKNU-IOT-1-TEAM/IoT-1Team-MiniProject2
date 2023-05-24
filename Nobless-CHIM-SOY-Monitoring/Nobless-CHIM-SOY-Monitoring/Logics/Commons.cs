using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forecast_API.Logics
{
    public class Commons
    {
        // MySQL용
        public static readonly string myConnString = "Server=pknuiot1team.cghin4qcf4s7.ap-northeast-2.rds.amazonaws.com;" +
                                                     "Port=3306;" +
                                                     "Database=miniproject02;" +
                                                     "Uid=pknuiot1team;" +
                                                     "Pwd=2V3lhihd8gIQ3krjNMf2;";

        // CCTV용 : api받은 영상은 60초만 제공 => 해결방법으로 고유주소 찾아서 사용
        public static readonly string CCTV_Url = "http://61.43.246.226:1935/rtplive/cctv_193.stream/playlist.m3u8";

        // 메트로 다이얼로그창을 위한 정적 메서드
        public static async Task<MessageDialogResult> ShowMessageAsync(string title,
            string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }
    }
}

