using System;
using System.Collections.Generic;

public static class GPStoXY
{
    // 위도 경도 좌표 변환
    private const double RE = 6371.00877; // 지구 반경(km)
    private const double GRID = 5.0; // 격자 간격(km)
    private const double SLAT1 = 30.0; // 투영 위도1(degree)
    private const double SLAT2 = 60.0; // 투영 위도2(degree)
    private const double OLON = 126.0; // 기준점 경도(degree)
    private const double OLAT = 38.0; // 기준점 위도(degree)
    private const double XO = 43; // 기준점 X좌표(GRID)
    private const double YO = 136; // 기준점 Y좌표(GRID)

    public static Dictionary<string, double> dfs_xy_conf(string code, double v1, double v2)
    {
        double DEGRAD = System.Math.PI / 180.0;
        double RADDEG = 180.0 / System.Math.PI;

        double re = RE / GRID;
        double slat1 = SLAT1 * DEGRAD;
        double slat2 = SLAT2 * DEGRAD;
        double olon = OLON * DEGRAD;
        double olat = OLAT * DEGRAD;

        double sn = System.Math.Tan((System.Math.PI * 0.25f + slat2 * 0.5f)) / System.Math.Tan(System.Math.PI * 0.25f + slat1 * 0.5f);
        sn = System.Math.Log(System.Math.Cos(slat1) / System.Math.Cos(slat2)) / System.Math.Log(sn);
        double sf = System.Math.Tan(System.Math.PI * 0.25f + slat1 * 0.5f);
        sf = System.Math.Pow(sf, sn) * System.Math.Cos(slat1) / sn;
        double ro = System.Math.Tan(System.Math.PI * 0.25f + olat * 0.5f);
        ro = re * sf / System.Math.Pow(ro, sn);

        Dictionary<string, double> rs = new Dictionary<string, double>();
        double ra, theta;

        if (code == "toXY")
        {
            rs["lat"] = v1;
            rs["lng"] = v2;
            ra = System.Math.Tan(System.Math.PI * 0.25f + (v1) * DEGRAD * 0.5f);
            ra = re * sf / System.Math.Pow(ra, sn);
            theta = v2 * DEGRAD - olon;
            if (theta > System.Math.PI) theta -= 2.0f * System.Math.PI;
            if (theta < -System.Math.PI) theta += 2.0f * System.Math.PI;
            theta *= sn;
            rs["x"] = System.Math.Floor(ra * System.Math.Sin(theta) + XO + 0.5f);
            rs["y"] = System.Math.Floor(ro - ra * System.Math.Cos(theta) + YO + 0.5f);
        }
        else
        {
            rs["x"] = v1;
            rs["y"] = v2;
            double xn = v1 - XO;
            double yn = ro - v2 + YO;
            ra = System.Math.Sqrt(xn * xn + yn * yn);
            if (sn < 0.0f) ra = -ra;
            double alat = System.Math.Pow((re * sf / ra), (1.0f / sn));
            alat = 2.0f * System.Math.Atan(alat) - System.Math.PI * 0.5f;

            if (System.Math.Abs(xn) <= 0.0)
            {
                theta = 0.0f;
            }
            else
            {
                if (System.Math.Abs(yn) <= 0.0)
                {
                    theta = System.Math.PI * 0.5f;
                    if (xn < 0.0f) theta = -theta;
                }
                else theta = System.Math.Atan2(xn, yn);
            }
            double alon = theta / sn + olon;
            rs["lat"] = alat * RADDEG;
            rs["lng"] = alon * RADDEG;
        }
        return rs;
    }
}
