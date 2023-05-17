using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Forecast_API.Models
{
    // API 요청
    public class RequestForecastWebApi
    {
        string ServiceUrl { get; set; }
        string ServiceName { get; set; } // 단기예보, 초단기예보 등.. 
        string ServiceKey { get; set; }
        string BaseDate { get; set; }
        string BaseTime { get; set; }

        string RequestUrl { get; set; } // 요청 URL

        public RequestForecastWebApi()
        {
            ServiceUrl = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0"; //서비스 URL
            ServiceName = "getUltraSrtFcst";
            ServiceKey = "Hp7RL4tCw0cXBMTYsWCTrydbix%2Fqtqe4%2Bu5yRNze4LKbniVQhVKmNWMk8IxYObz6%2FEB41Vo47zCdEVUVRfAvsA%3D%3D";
            SetBaseDateTime();
            RequestUrl = ServiceUrl + "/" + ServiceName
                        + "?serviceKey=" + ServiceKey
                        + "&pageNo=1&numOfRows=1000&dataType=json"
                        + "&base_date=" + BaseDate
                        + "&base_time=" + BaseTime
                        + "&nx=98"
                        + "&ny=76";
        }
        
        // 생성 날짜, 시간 세팅
        public void SetBaseDateTime()
        {
            BaseDate = DateTime.Now.ToString("yyyyMMdd");   // 현재 날짜

            if (DateTime.Now.Minute >= 45)  // 매시각 현재 45분 이상이면
            {
                BaseTime = $"{DateTime.Now.Hour}30";    // 현재 시, 30분
            }       
            else  // 매시각 45분 이전이면
            {
                BaseTime = $"{DateTime.Now.Hour - 1}30"; // 1시간 전, 30분
                if (DateTime.Now.Hour == 0) // 오전 0시 30분 이전이면 날짜도 하루전으로 바꾸기
                {
                    BaseDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                }
            }
            Debug.WriteLine(BaseDate, BaseTime);
        }


        // API 데이터 받은 후 우리가 쓸(DB에 알맞는)형태로 데이터 얻기
        public List<ForecastInfo> GetForecastWebApi()
        {
            List<ForecastInfo> transformedData = null;

            //WebRequst, WebRespone 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;
            string result = string.Empty;   // API 응답 결과

            // API 요청, 응답
            try
            {
                req = WebRequest.Create(RequestUrl);
                using (res = req.GetResponse())
                {
                    using (reader = new StreamReader(res.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        Debug.Write(result);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API 조회 오류 : {ex.Message}");
            }
            
            ResponseForecastWebApi responseResult = JsonConvert.DeserializeObject<ResponseForecastWebApi>(result);

            try
            {
                if (responseResult.Response.Header.ResultCode == "00" && responseResult.Response.Header.ResultMsg == "NORMAL_SERVICE")
                {
                    transformedData = responseResult.Response.Body.Items.Item
                        .GroupBy(d => new { d.BaseDate, d.BaseTime, d.FcstDate, d.FcstTime, d.Nx, d.Ny })
                        .Select(group => new ForecastInfo
                        {
                            FcstDate = DateTime.ParseExact(group.Key.FcstDate, "yyyyMMdd", CultureInfo.InvariantCulture),
                            FcstTime = DateTime.ParseExact(group.Key.FcstTime, "HHmm", CultureInfo.InvariantCulture),
                            BaseDate = DateTime.ParseExact(group.Key.BaseDate, "yyyyMMdd", CultureInfo.InvariantCulture),
                            BaseTime = DateTime.ParseExact(group.Key.BaseTime, "HHmm", CultureInfo.InvariantCulture),
                            Nx = int.Parse(group.Key.Nx),
                            Ny = int.Parse(group.Key.Ny),
                            T1H = int.Parse(group.FirstOrDefault(item => item.Category == "T1H")?.FcstValue),
                            RN1 = Convert.ToString(group.FirstOrDefault(item => item.Category == "RN1")?.FcstValue),
                            SKY = int.Parse(group.FirstOrDefault(item => item.Category == "SKY")?.FcstValue),
                            UUU = ParseIntWithNegative(group.FirstOrDefault(item => item.Category == "UUU")?.FcstValue),
                            VVV = ParseIntWithNegative(group.FirstOrDefault(item => item.Category == "VVV")?.FcstValue),
                            REH = int.Parse(group.FirstOrDefault(item => item.Category == "REH")?.FcstValue),
                            PTY = int.Parse(group.FirstOrDefault(item => item.Category == "PTY")?.FcstValue),
                            LGT = int.Parse(group.FirstOrDefault(item => item.Category == "LGT")?.FcstValue),
                            VEC = int.Parse(group.FirstOrDefault(item => item.Category == "VEC")?.FcstValue),
                            WSD = int.Parse(group.FirstOrDefault(item => item.Category == "WSD")?.FcstValue)
                        })
                        .ToList();
                }
                else
                {
                    throw new Exception("오류가 발생하였습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류가 발생했습니다: " + ex.Message);
            }
            return transformedData;
        }

        // 문자열 정수 음수 파싱
        private float ParseIntWithNegative(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                bool isParseSuccessful = float.TryParse(value, out float parsedValue);
                if (isParseSuccessful)
                {
                    return parsedValue;
                }
                else
                {
                    // 음수 문자열 처리
                    bool isNegativeValue = value.StartsWith("-");   // 문자열이 "-"로 시작한다면 음수
                    if (isNegativeValue)
                    {
                        string absoluteValueString = value.Substring(1);
                        bool isAbsoluteValueParseSuccessful = float.TryParse(absoluteValueString, out float absoluteValue);
                        if (isAbsoluteValueParseSuccessful)
                        {
                            return -absoluteValue;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
