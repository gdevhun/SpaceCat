
import requests
import time as tm
from gps_to_xy import toXY  # Assuming you have a module named gps_to_xy with a function toXY
from firebasedb import read_weather_data

SERVICE_KEY = "Key is in the Notion"
BASE_URL = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"

def get_weather(user_id, user_name, date, time, nx, ny):
    url = f"{BASE_URL}?ServiceKey={SERVICE_KEY}&pageNo=1&numOfRows=1000&dataType=JSON&base_date={date}&base_time={time}&nx={nx}&ny={ny}"
    print(url)
    try:
        response = requests.get(url)
        response.raise_for_status()
        result = parse_weather_data(user_id, user_name, response.json())
        print("get_def:",result)
        return result
    except requests.RequestException as error:
        print(f"Error fetching weather data: {error}")
        return None

def parse_weather_data(user_id, user_name, data):
    items = data['response']['body']['items']['item']

    weather_data = {}

    for item in items:
        category = item['category']
        value = item['obsrValue']
        weather_data[category] = value

        if category == "PTY":
            print("현재 강수형태:", value)

        print(f"Category: {category}, Value: {value}")

    result = read_weather_data(user_id, user_name, weather_data)
    print("parse_def:",result)
    return result

def get_xy_location(lat, lon):
    location = toXY('toXY', lat, lon)
    return location['x'], location['y']

def start(user_id, user_name, latitude, longitude, datetime):
    nx, ny = get_xy_location(latitude, longitude)

    date = datetime[:8]
    time_str = datetime[8:]

    print("x:", nx)
    print("y:", ny)
    print("Date:", date)
    print("Time:", time_str)

    result = get_weather(user_id, user_name, date, time_str, nx, ny)
    print("start_result:", result)
    return result

if __name__ == "__main__":
    input_userid = "1jkh39oh9asg"
    input_username = "Test"
    input_latitude = 35.8510  # 예시 좌표
    input_longitude = 127.1263  # 예시 좌표
    input_datetime = "202406161200"  # yyyyMMddHHmm 형식의 시간 데이터

    start(input_userid, input_username, input_latitude, input_longitude, input_datetime)

