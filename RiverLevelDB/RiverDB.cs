﻿using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RiverLevelDB.Commons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;

namespace RiverLevelDB
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            string url = "http://apis.data.go.kr/6260000/BusanRvrwtLevelInfoService/getRvrwtLevelInfo"; // URL
            url += "?ServiceKey=" + "wGokgRxD1t3z5G4u7MsWumpoCeiWO8JM6yZ87rX1ELTO9nMSUuMOQjHj70rAzuopgyB1iLdKX0S9WK0RLs88bQ=="; // Service Key
            url += "&pageNo=1";
            url += "&numOfRows=100";
            url += "&resultType=json";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = string.Empty;
            HttpWebResponse response;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
            }

            JObject jobject = JObject.Parse(results);
            var riveritem = jobject["getRvrwtLevelInfo"]["body"]["items"]["item"];

            //MySqlConnection conn = new MySqlConnection("Server=210.119.12.66;Port=3306;Database=miniproject01;Uid=root;Pwd=12345;");

            MySqlConnection conn = new MySqlConnection("Server=pknuiot1team.cghin4qcf4s7.ap-northeast-2.rds.amazonaws.com;Port=3306;Database=parkseonghyeon;Uid=pknuiot1team;Pwd=2V3lhihd8gIQ3krjNMf2;");

            foreach (var item in riveritem)
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                string insQuery = @"INSERT INTO riverlev
                                    (Idx,
                                    siteName,
                                    waterLevel,
                                    dayLevelMax,
                                    obsrTime,
                                    alertLevel1,
                                    alertLevel1Nm,
                                    alertLevel2,
                                    alertLevel2Nm,
                                    alertLevel3,
                                    alertLevel3Nm,
                                    alertLevel4,
                                    alertLevel4Nm,
                                    sttus,
                                    sttusNm)
                                    VALUES
                                    (@Idx,
                                    @siteName,
                                    @waterLevel,
                                    @dayLevelMax,
                                    @obsrTime,
                                    @alertLevel1,
                                    @alertLevel1Nm,
                                    @alertLevel2,
                                    @alertLevel2Nm,
                                    @alertLevel3,
                                    @alertLevel3Nm,
                                    @alertLevel4,
                                    @alertLevel4Nm,
                                    @sttus,
                                    @sttusNm) ";

                MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                cmd.Parameters.AddWithValue("@Idx", 0);
                cmd.Parameters.AddWithValue("@siteName", Convert.ToString(item["siteName"]));
                cmd.Parameters.AddWithValue("@waterLevel", Convert.ToString(item["waterLevel"]));
                cmd.Parameters.AddWithValue("@dayLevelMax", Convert.ToString(item["dayLevelMax"]));
                cmd.Parameters.AddWithValue("@obsrTime", Convert.ToString(item["obsrTime"]));
                cmd.Parameters.AddWithValue("@alertLevel1", Convert.ToString(item["alertLevel1"]));
                cmd.Parameters.AddWithValue("@alertLevel1Nm", Convert.ToString(item["alertLevel1Nm"]));
                cmd.Parameters.AddWithValue("@alertLevel2", Convert.ToString(item["alertLevel2"]));
                cmd.Parameters.AddWithValue("@alertLevel2Nm", Convert.ToString(item["alertLevel2Nm"]));
                cmd.Parameters.AddWithValue("@alertLevel3", Convert.ToString(item["alertLevel3"]));
                cmd.Parameters.AddWithValue("@alertLevel3Nm", Convert.ToString(item["alertLevel3Nm"]));
                cmd.Parameters.AddWithValue("@alertLevel4", Convert.ToString(item["alertLevel4"]));
                cmd.Parameters.AddWithValue("@alertLevel4Nm", Convert.ToString(item["alertLevel4Nm"]));
                cmd.Parameters.AddWithValue("@sttus", Convert.ToString(item["sttus"]));
                cmd.Parameters.AddWithValue("@sttusNm", Convert.ToString(item["sttusNm"]));

                cmd.ExecuteNonQuery();

                Debug.WriteLine("완료");
            }





            //foreach (var val in items)
            //{
            //    var RiverItem = new River()
            //    {
            //        Idx = 0,
            //        siteName = Convertt.items.siteName;
            //        waterLevel = Convert.ToString(val["siteName"]),
            //        dayLevelMax = Convert.ToString(val["siteName"]),
            //        obsrTime = Convert.ToString(val["siteName"]),
            //        alertLevel1 = Convert.ToString(val["siteName"]),
            //        alertLevel1Nm = Convert.ToString(val["siteName"]),
            //        alertLevel2 = Convert.ToString(val["siteName"]),
            //        alertLevel2Nm = Convert.ToString(val["siteName"]),
            //        alertLevel3 = Convert.ToString(val["siteName"]),
            //        alertLevel3Nm = Convert.ToString(val["siteName"]),
            //        alertLevel4 = Convert.ToString(val["siteName"]),
            //        alertLevel4Nm = Convert.ToString(val["siteName"]),
            //        sttus = Convert.ToString(val["siteName"]),
            //        sttusNm = Convert.ToString(val["siteName"])
            //    };
            //}


        }
    }
}