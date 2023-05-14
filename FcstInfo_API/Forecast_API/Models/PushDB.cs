﻿using Forecast_API.Logics;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forecast_API.Models
{
    public class PushDB
    {
        public void insertDB(List<ForecastInfo> forecastInfos)
        {
            try 
            {
                using(MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var query = @"INSERT INTO ultrasrtfcst
                                        (FcstDate,
                                        FcstTime,
                                        BaseDate,
                                        BaseTime,
                                        Nx,
                                        Ny,
                                        T1H,
                                        RN1,
                                        SKY,
                                        UUU,
                                        VVV,
                                        REH,
                                        PTY,
                                        LGT,
                                        VEC,
                                        WSD)
                                VALUES
                                        (@FcstDate,
                                        @FcstTime,
                                        @BaseDate,
                                        @BaseTime,
                                        @Nx,
                                        @Ny,
                                        @T1H,
                                        @RN1,
                                        @SKY,
                                        @UUU,
                                        @VVV,
                                        @REH,
                                        @PTY,
                                        @LGT,
                                        @VEC,
                                        @WSD)";

                    foreach (ForecastInfo info in forecastInfos)
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@FcstDate", info.FcstDate);
                        cmd.Parameters.AddWithValue("@FcstTime", info.FcstTime);
                        cmd.Parameters.AddWithValue("@BaseDate", info.FcstDate);
                        cmd.Parameters.AddWithValue("@BaseTime", info.FcstTime);
                        cmd.Parameters.AddWithValue("@Nx", info.Nx);
                        cmd.Parameters.AddWithValue("@Ny", info.Ny);
                        cmd.Parameters.AddWithValue("@T1H", info.T1H);
                        cmd.Parameters.AddWithValue("@RN1", info.RN1);
                        cmd.Parameters.AddWithValue("@SKY", info.SKY);
                        cmd.Parameters.AddWithValue("@UUU", info.UUU);
                        cmd.Parameters.AddWithValue("@VVV", info.VVV);
                        cmd.Parameters.AddWithValue("@REH", info.REH);
                        cmd.Parameters.AddWithValue("@PTY", info.PTY);
                        cmd.Parameters.AddWithValue("@LGT", info.LGT);
                        cmd.Parameters.AddWithValue("@VEC", info.VEC);
                        cmd.Parameters.AddWithValue("@WSD", info.WSD);

                        cmd.ExecuteNonQuery();
                    }
                 }
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"DB저장 오류: {ex.ToString()}");
            }
        }
    }
}
