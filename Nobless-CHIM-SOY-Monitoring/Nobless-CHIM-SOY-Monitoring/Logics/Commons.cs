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

        // HTML URI
        string HtmlUriString = $@"<!DOCTYPE html>
		 					      <html>
			     				  <head>
								      <title>SOP Javascript : Map create sample</title>
									  <meta charset=""utf-8"">
									  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
									  <script type=""text/javascript"" src=""https://sgisapi.kostat.go.kr/OpenAPI3/auth/javascriptAuth?consumer_key=84752d8a83f14b3f8a10""></script>
								  </head>
								  <body>
									  <div id=""map"" style=""width:100%-20px;height:480px""></div>
									  <script type=""text/javascript"">
										  var map = sop.map(""map"");
										  map.setView(sop.utmk(1145117, 1689004), 9);
									  </script>
								  </body>
								  </html>";


        // 메트로 다이얼로그창을 위한 정적 메서드
        public static async Task<MessageDialogResult> ShowMessageAsync(string title,
            string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }
    }
}

