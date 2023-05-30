using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobless_CHIM_SOY_Monitoring.Models
{
    public class ForecastUI
    {
        public DateTime BaseDate { get; set; }  // 생성 날짜
        public TimeSpan BaseTime { get; set; }  // 생성 시간
        public DateTime FcstDate { get; set; } // 예보 날짜
        public TimeSpan FcstTime { get; set; }    // 예보 시간
        public int Nx { get; set; } // 위치
        public int Ny { get; set; }
        public int T1H { get; set; }    // 기온
        public string RN1 { get; set; } // 1시간 강수량
        public string SKYCondition { get; set; } // 하늘 상태
        public float UUU { get; set; } // 동서바람 성분
        public float VVV { get; set; } // 남북바람 성분
        public int REH { get; set; } //습도
        public string PTYCondition { get; set; } // 강수형태
        public int LGT { get; set; } // 낙뢰

        public string VEC { get; set; } // 풍향
        public int WSD { get; set; } // 풍속
    }
}
