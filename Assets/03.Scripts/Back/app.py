from flask import Flask, request, jsonify, Response
import json
import os
from weather import start

app = Flask(__name__)

# 데이터 받기
@app.route('/send_data', methods=['POST'])
def send_data():
    data = request.get_json()

    # 데이터 분해
    user_id = data.get('userId')
    user_name = data.get('userName', "Unknown")  # 기본값 "Unknown"
    user_email = data.get('userEmail', "No email")  # 기본값 "No email"

    location = data.get('location', {})
    latitude = location.get('latitude', 0.0)  # 기본값 0.0
    longitude = location.get('longitude', 0.0)  # 기본값 0.0

    datetime = data.get('datetime', "No datetime provided")  # 기본값 "No date provided"
    forecast = data.get('forecast', "No forecast provided")  # 기본값 "No forecast provided"

    # 필수 데이터 확인
    if not user_id:
        return jsonify({"status": "error", "message": "User ID is required."}), 400

    # 데이터기반 결과 받는 함수
    det = start(user_id, user_name, latitude, longitude, datetime)

    print("change_det", det)

    # 데이터 출력 (디버깅 용도)
    print(f"Received data: {data}")

    # "det" 값을 data에 추가
    data['det'] = det


    # 사용자 ID를 파일 이름으로 설정하여 데이터를 JSON 파일로 저장
    json_file_path = f'{user_id}.json'
    with open(json_file_path, 'w', encoding='utf-8') as json_file:
        json.dump(data, json_file, indent=4, ensure_ascii=False)

    print("success to save")

    response_data = {"status": "success", "received_data": data, "det": det}
    response_json = json.dumps(response_data, ensure_ascii=False)
    return Response(response_json, content_type='application/json; charset=utf-8')

# 저장된 JSON 파일을 읽어오기
@app.route('/home/weather/<user_id>.json', methods=['GET'])
def read_data(user_id):
    print("I received signal")
    json_file_path = f'{user_id}.json'

    if not os.path.exists(json_file_path):
        return jsonify({"status": "error", "message": "No data file found for the given user ID."}), 404

    with open(json_file_path, 'r', encoding='UTF-8') as json_file:
        data = json.load(json_file)

    response_data = {"status": "success", "data": data}
    response_json = json.dumps(response_data, ensure_ascii=False)

    print("Response:", response_json)

    return Response(response_json, content_type='application/json; charset=utf-8')

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)


