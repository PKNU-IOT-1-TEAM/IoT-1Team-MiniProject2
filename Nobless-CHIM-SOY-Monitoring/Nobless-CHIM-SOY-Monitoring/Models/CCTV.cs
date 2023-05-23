using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nobless_CHIM_SOY_Monitoring.Models
{
    public class CCTV
    {
        string locate_Html = string.Empty; // html 파일 string 값 복사해서 모달 창에 쓰기위해 할당
        private int height_val = 400;   // CCTV크기 높이
        string CCTV_Url = "http://61.43.246.226:1935/rtplive/cctv_193.stream/playlist.m3u8";

        LibVLC _libVLC = null;
        MediaPlayer _mediaPlayer = null;
    }
}
