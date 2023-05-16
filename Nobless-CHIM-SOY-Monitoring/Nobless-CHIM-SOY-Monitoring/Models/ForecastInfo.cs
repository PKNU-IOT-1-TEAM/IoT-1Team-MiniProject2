using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Forecast_API.Models
{
    public class ForecastInfo
    {
        public DateTime BaseDate { get; set; }  // 생성 날짜
        public DateTime BaseTime { get; set; }  // 생성 시간
        public DateTime FcstDate { get; set; } // 예보 날짜
        public DateTime FcstTime { get; set; }    // 예보 시간
        public int Nx { get; set; }
        public int Ny { get; set; }
        public int T1H { get; set; }    // 기온
        public string RN1 { get; set;} // 1시간 강수량
        public int SKY { get; set;} // 하늘 상태
        public float UUU { get; set;} // 동서바람 성분
        public float VVV { get; set;} // 남북바람 성분
        public int REH { get; set;} //습도
        public int PTY { get; set;} // 강수형태
        public int LGT { get; set;} // 낙뢰

        public int VEC { get; set;} // 풍향
        public int WSD { get; set;} // 풍속

    }

}
