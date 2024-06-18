import flask
import os
import json
from flask_restx import Api, Resource, fields

# flask server with swagger feature
# DEPENDENCE : flask , flask_restx
app = flask.Flask(__name__)
api = Api(app, version='1.0', title='Sample API', description='A sample API', )


# --- ReadData class ---------------------------------------------------------------------------------------------------
rend_data_ns = api.namespace('read_data', description='Rend data operations')
data_model = api.model('Data', {
    'userId': fields.String(required=True, description='The ID of the user', example='12345'),
})
@rend_data_ns.route('/<string:user_id>')
@rend_data_ns.param('user_id', 'The user identifier')
class ReadData(Resource):
    # -- id로 유저 정보 불러오기
    @rend_data_ns.response(200, 'Success')
    @rend_data_ns.response(404, 'Not Found')
    def get(self, user_id):
        json_file_path = f'{user_id}.json'

        if not os.path.exists(json_file_path):
            return flask.jsonify({"status": "error", "message": "No data file found for the given user ID."}), 404

        with open(json_file_path, 'r') as json_file:
            data = json.load(json_file)

        return flask.jsonify({"status": "success", "data": data})


# --- SendData class ---------------------------------------------------------------------------------------------------
send_data_ns = api.namespace('send_data', description='Send data operations')
location_model = api.model('Location', {
    'latitude': fields.Float(required=True, description='The latitude of the user location', example=37.7749),
    'longitude': fields.Float(required=True, description='The longitude of the user location', example=-122.4194)
})

user_data_model = api.model('user_data', {
    'userId': fields.String(required=True, description='The ID of the user', example='12345'),
    'userName': fields.String(required=False, description='The name of the user', example='John Doe'),
    'userEmail': fields.String(required=False, description='The email of the user', example='john.doe@example.com'),
    'location': fields.Nested(location_model, required=False, description='The location of the user'),
    'date': fields.String(required=False, description='The date of the data', example='2023-06-18'),
    'forecast': fields.String(required=False, description='The weather forecast', example='Sunny')
})
api.add_resource(ReadData, '/send_data/<string:user_id>')

@send_data_ns.route('/')
class SendData(Resource):
    @send_data_ns.expect(user_data_model)
    @send_data_ns.response(200, 'Success', model=user_data_model)
    @send_data_ns.response(400, 'Bad Request')
    # -- 유저 정보 쓰기
    def post(self):
        data = flask.request.get_json()

        # 데이터 분해
        user_id = data.get('userId')
        user_name = data.get('userName', "Unknown")  # 기본값 "Unknown"
        user_email = data.get('userEmail', "No email")  # 기본값 "No email"

        location = data.get('location', {})
        latitude = location.get('latitude', 0.0)  # 기본값 0.0
        longitude = location.get('longitude', 0.0)  # 기본값 0.0

        date = data.get('date', "No date provided")  # 기본값 "No date provided"
        forecast = data.get('forecast', "No forecast provided")  # 기본값 "No forecast provided"

        # 필수 데이터 확인
        if not user_id:
            return flask.jsonify({"status": "error", "message": "User ID is required."}), 400

        # 데이터 출력 (디버깅 용도)
        print(f"Received data: {data}")

        # 사용자 ID를 파일 이름으로 설정하여 데이터를 JSON 파일로 저장
        json_file_path = f'{user_id}.json'
        with open(json_file_path, 'w') as json_file:
            json.dump(data, json_file, indent=4)

        return flask.jsonify({"status": "success", "received_data": data})
api.add_resource(SendData, '/send_data')

# --- main -------------------------------------------------------------------------------------------------------------
if __name__ == '__main__':
    app.run(debug=True)
