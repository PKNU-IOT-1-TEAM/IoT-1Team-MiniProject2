using System.Collections.Generic;

namespace Forecast_API.Models
{
    // API 응답
    public class ResponseForecastWebApi
    {
        public ResponseForecastSpaceData Response { get; set; }
    }

    public class ResponseForecastSpaceData
    {
        public ResponseHeader Header { get; set; }
        public ResponseBody Body { get; set; }
    }
    public class ResponseHeader
    {
        public string ResultCode { get; set; }
        public string ResultMsg { get; set; }
    }

    public class ResponseBody
    {
        public string DataType { get; set; }
        public ForecastItems Items { get; set; }
        public string NumofRows { get; set; }
        public string PageNo { get; set; }
        public string TotalCount { get; set; }
    }

    public class ForecastItems
    {
        public List<ForecastItem> Item { get; set; }
    }

    public class ForecastItem
    {
        public string BaseDate { get; set; } 
        public string BaseTime { get; set; }
        public string Category { get; set; }
        public string FcstDate { get; set; }
        public string FcstTime { get; set; }
        public string FcstValue { get; set; }
        public string Nx { get; set; }
        public string Ny { get; set; }
    }
}
