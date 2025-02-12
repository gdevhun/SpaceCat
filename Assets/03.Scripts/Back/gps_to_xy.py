import math

PI = math.pi
RE = 6371.00877  # 지구 반경(km)
GRID = 5.0  # 격자 간격(km)
SLAT1 = 30.0  # 투영 위도1(degree)
SLAT2 = 60.0  # 투영 위도2(degree)
OLON = 126.0  # 기준점 경도(degree)
OLAT = 38.0  # 기준점 위도(degree)
XO = 43  # 기준점 X좌표(GRID)
YO = 136  # 기준점 Y좌표(GRID)

def toXY(code, v1, v2):
    DEGRAD = PI / 180.0
    RADDEG = 180.0 / PI

    re = RE / GRID
    slat1 = SLAT1 * DEGRAD
    slat2 = SLAT2 * DEGRAD
    olon = OLON * DEGRAD
    olat = OLAT * DEGRAD

    sn = math.tan(PI * 0.25 + slat2 * 0.5) / math.tan(PI * 0.25 + slat1 * 0.5)
    sn = math.log(math.cos(slat1) / math.cos(slat2)) / math.log(sn)
    sf = math.tan(PI * 0.25 + slat1 * 0.5)
    sf = math.pow(sf, sn) * math.cos(slat1) / sn
    ro = math.tan(PI * 0.25 + olat * 0.5)
    ro = re * sf / math.pow(ro, sn)

    rs = {}
    if code == 'toXY':
        rs['lat'] = v1
        rs['lng'] = v2
        ra = math.tan(PI * 0.25 + (v1) * DEGRAD * 0.5)
        ra = re * sf / math.pow(ra, sn)
        theta = v2 * DEGRAD - olon
        if theta > PI:
            theta -= 2.0 * PI
        if theta < -PI:
            theta += 2.0 * PI
        theta *= sn
        rs['x'] = math.floor(ra * math.sin(theta) + XO + 0.5)
        rs['y'] = math.floor(ro - ra * math.cos(theta) + YO + 0.5)
    else:
        rs['x'] = v1
        rs['y'] = v2
        xn = v1 - XO
        yn = ro - v2 + YO
        ra = math.sqrt(xn * xn + yn * yn)
        if sn < 0.0:
            ra = -ra
        alat = math.pow((re * sf / ra), (1.0 / sn))
        alat = 2.0 * math.atan(alat) - PI * 0.5

        if abs(xn) <= 0.0:
            theta = 0.0
        else:
            if abs(yn) <= 0.0:
                theta = PI * 0.5
                if xn < 0.0:
                    theta = -theta
            else:
                theta = math.atan2(xn, yn)
        alon = theta / sn + olon
        rs['lat'] = alat * RADDEG
        rs['lng'] = alon * RADDEG

    return rs


