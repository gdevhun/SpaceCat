import pyrebase
import random

firebaseConfig = {
    "apiKey": "Key is in the Notion",
    "authDomain": "ossteamproject-a4ea0.firebaseapp.com",
    "databaseURL": "https://ossteamproject-a4ea0-default-rtdb.firebaseio.com",
    "projectId": "ID is in the Notion",
    "storageBucket": "ossteamproject-a4ea0.appspot.com",
    "messagingSenderId": "ID is in the Notion",
    "appId": "ID is in the Notion",
    "measurementId": "ID is in the Notion"
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

        ISTJ_in = [1,2,4,5,10,12,13,18,19,21,25,28,30]
        ISTJ_out = [5,8,10,15,18,19,20,21,22,25,26,27,29]
        ISTP_in = [1,2,3,4,5,12,13,18,19,21,25,26,27,28,29]
        ISTP_out = [4,5,8,9,12,15,18,19,20,22,25,26,27,29,30]
        ISFJ_in = [1,2,3,4,5,9,12,16,18,19,21,22,25,26,27,28,29]
        ISFJ_out = [12,15,20,22,26,27,29]
        ISFP_in = [1,2,3,4,5,7,13,14,16,18,19,20,21,23,27,28,30]
        ISFP_out = [4,5,7,8,9,10,11,13,15,17,19,21,22,25,26,27,29]
        INTJ_in = [1,2,3,4,14,16,18,19,20,21,22,23,28]
        INTJ_out = [4,5,7,11,12,13,17,18,19,21,22,25,26,27,29,30]
        INTP_in = [1,2,5,7,16,18,19,20,21,23,26,28,29]
        INTP_out = [4,10,12,15,18,20,22,24,27,29,30]
        INFJ_in = [1,2,4,5,18,19,21,25,26,27,28]
        INFJ_out = [4,5,12,15,18,19,20,22,26,27,29]
        INFP_in = [1,2,12,13,18,19,21,25,26,27,28]
        INFP_out = [4,10,15,20,26,29,30]
        ESTJ_in = [1,2,3,4,5,6,7,8,9,11,13,20,24,27,28,29,30]
        ESTJ_out = [1,2,6,12,14,16,20,23,27,28,30]
        ESTP_in = [2,4,7,8,10,12,14,16,17,20,24,26,27,29,30]
        ESTP_out = [1,2,3,4,6,12,13,14,15,16,17,23,24,27,28,30]
        ESFJ_in = [1,3,4,5,6,7,9,11,13,20,24,27,28,29,30]
        ESFJ_out = [6,8,10,12,13,14,16,18,19,23,27,28,30]
        ESFP_in = [2,4,5,6,7,10,11,14,16,17,19,22,23,24,26,27,30]
        ESFP_out = [1,2,3,6,12,13,14,16,17,18,20,23,24,26,27,28,30]
        ENTJ_in = [2,4,5,6,7,8,10,14,16,17,18,19,20,23,24,27,29]
        ENTJ_out = [1,2,3,6,8,9,11,13,14,16,17,18,22,23,24,26,27,28,30]
        ENTP_in = [2,4,6,8,10,12,14,16,17,18,19,22,24,26,27]
        ENTP_out = [4,6,8,12,13,14,16,17,18,22,23,27,28,29,30]
        ENFJ_in = [1,3,4,5,6,7,8,9,11,13,18,20,21,24,29]
        ENFJ_out = [1,2,3,5,6,12,14,16,18,19,21,23,25,27,28,30]
        ENFP_in = [2,4,6,8,11,12,13,15,18,19,20,22,23,24,25,27]
        ENFP_out = [4,5,6,7,8,9,12,14,16,17,18,19,21,22,23,27,28,29,30]

        if user_mbti == "ISTJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ISTJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ISTJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ISFJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ISFJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ISFJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ISTP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ISTP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ISTP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ISFP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ISFP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ISFP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "INTJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(INTJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(INTJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "INFJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(INFJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(INFJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "INTP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(INTP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(INTP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "INFP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(INFP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(INFP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ESTJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ESTJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ESTJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ESTP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ESTP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ESTP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ESFJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ESFJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ESFJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ESFP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ESFP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ESFP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"
    
        if user_mbti == "ENTJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ENTJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ENTJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ENTP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ENTP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ENTP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ENFJ":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ENFJ_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ENFJ_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"

        if user_mbti == "ENFP":
            if T1H > 28 or PTY != 0 or RN1 > 1 or WSD > 20:
                random_index = get_random_number(ENFP_in)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/In/{formatted_index}"
            else:
                random_index = get_random_number(ENFP_out)
                formatted_index = str(random_index).zfill(3)  # 001, 002 형식으로 변환
                path = f"/Hobby/Entertainment/Out/{formatted_index}"
        
        result = read_data(path)
        return result
    except Exception as error:
        print('Error reading weather data:', error)
        return None



