# app.py
from flask import Flask, request, jsonify
import json
import os

app = Flask(__name__)

# 데이터 받기
@app.route('/send_data', methods=['POST'])
def send_data():
    data = request.get_json()
    user_id = data.get('userId')

    if not user_id:
        return jsonify({"status": "error", "message": "User ID is required."}), 400

    print(f"Received data: {data}")

    # 사용자 ID를 파일 이름으로 설정하여 데이터를 JSON 파일로 저장
    json_file_path = f'{user_id}.json'
    with open(json_file_path, 'w') as json_file:
        json.dump(data, json_file, indent=4)

    return jsonify({"status": "success", "received_data": data})

# 저장된 JSON 파일을 읽어오기
@app.route('/read_data/<user_id>', methods=['GET'])
def read_data(user_id):
    json_file_path = f'{user_id}.json'
    
    if not os.path.exists(json_file_path):
        return jsonify({"status": "error", "message": "No data file found for the given user ID."}), 404

    with open(json_file_path, 'r') as json_file:
        data = json.load(json_file)

    return jsonify({"status": "success", "data": data})

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)
