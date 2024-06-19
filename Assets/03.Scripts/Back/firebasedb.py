import pyrebase
import random

firebaseConfig = {
    "apiKey": "AIzaSyCPI4_03fyAqPilT2cq3lQhRYhtm0BMsUs",
    "authDomain": "ossteamproject-a4ea0.firebaseapp.com",
    "databaseURL": "https://ossteamproject-a4ea0-default-rtdb.firebaseio.com",
    "projectId": "ossteamproject-a4ea0",
    "storageBucket": "ossteamproject-a4ea0.appspot.com",
    "messagingSenderId": "794886672080",
    "appId": "1:794886672080:web:310a69b7fd0915c7de98d5",
    "measurementId": "G-5RP0M42BY7"
}

# Firebase 초기화
firebase = pyrebase.initialize_app(firebaseConfig)

# 데이터베이스 참조 가져오기
db = firebase.database()

# 데이터 읽어오기 함수
def read_data(path):
    try:
        snapshot = db.child(path).get()
        if snapshot.val():
            print("def_db:", snapshot.val())
            return snapshot.val()
        else:
            print(f"No data available at {path}")
            return None
    except Exception as error:
        print('Error reading data:', error)
        return None

# 무작위 번호 생성 함수
def get_random_number(numbers):
    return random.choice(numbers)

def read_weather_data(user_id,user_name, weather_data):
    try:
        user_mbti_path = f"/USER/{user_id}/{user_name}/mbti"
        user_mbti_snapshot = db.child(user_mbti_path).get()
        user_mbti = user_mbti_snapshot.val()

        T1H = float(weather_data.get('T1H', 'N/A'))
        RN1 = float(weather_data.get('RN1', 'N/A'))
        PTY = float(weather_data.get('PTY', 'N/A'))
        WSD = float(weather_data.get('WSD', 'N/A'))
        print('User_id:', user_id)
        ISTJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ISTJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ISTP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ISTP_out = [4,5,12,15,18,19,20,22,26,27,29]
        ISFJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ISFJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ISFP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ISFP_out = [4,5,12,15,18,19,20,22,26,27,29]
        INTJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        INTJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        INTP_in = [1,2,4,5,18,19,21,25,26,27,28]
        INTP_out = [4,5,12,15,18,19,20,22,26,27,29]
        INFJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        INFJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        INFP_in = [1,2,4,5,18,19,21,25,26,27,28]
        INFP_out = [4,5,12,15,18,19,20,22,26,27,29]
        ESTJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ESTJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ESTP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ESTP_out = [4,5,12,15,18,19,20,22,26,27,29]
        ESFJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ESFJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ESFP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ESFP_out = [4,5,12,15,18,19,20,22,26,27,29]
        ENTJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ENTJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ENTP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ENTP_out = [4,5,12,15,18,19,20,22,26,27,29]
        ENFJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        ENFJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        ENFP_in = [1,2,4,5,18,19,21,25,26,27,28]
        ENFP_out = [4,5,12,15,18,19,20,22,26,27,29]
        

        if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
            random_index = get_random_number(INFJ_in)
            formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
            path = f"/Hobby/Entertainment/In/{formatted_index}"
        else:
            random_index = get_random_number(INFJ_out)
            formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
            path = f"/Hobby/Entertainment/Out/{formatted_index}"

        result = read_data(path)
        return result
    except Exception as error:
        print('Error reading weather data:', error)
        return None



