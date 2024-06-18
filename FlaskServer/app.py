import flask
import os
import json
from flask_restx import Api, Resource, fields

# --- description strings ----------------------------------------------------------------------------------------------
# 문서 작성에 사용될 문자열 입니다. 여러줄 텍스트가 필요할시 사용하기 위해서 추가하였습니다.
STR_api_description = """
전북대학교 오픈소스소프트웨어 팀 프로젝트 - SpaceCat에 사용되는 API 문서입니다.
"""
STR_ReadData_description = "유저 데이터를 불러오는 기능입니다"
STR_SendData_description = "유저 데이터를 서버에 작성하는 기능입니다"


# --- init -------------------------------------------------------------------------------------------------------------
# flask server with swagger feature
# DEPENDENCE : flask , flask_restx
app = flask.Flask(__name__)
api = Api(app, version='1.0', title='SpaceCat API', description=STR_api_description, )
api.namespaces.clear()   # delete 'default' namespace, adding other namespace after this


# namespace read_data --------------------------------------------------------------------------------------------------
read_data_ns = api.namespace('read_data', description=STR_ReadData_description)
read_data_model = api.model('user_data(read)', {
    'user_id': fields.String(required=True, description='사용자 ID 입니다.', example='abcdefgTNtaY7mOY0Rm123456789'),
})


@read_data_ns.route('/<string:user_id>')
@read_data_ns.param('user_id', '사용자 ID를 입력해 주세요.')
class ReadData(Resource):
    # -- id로 유저 정보 불러오기
    @read_data_ns.response(200, 'Success', model=read_data_model)
    @read_data_ns.response(404, 'Not Found')
    def get(self, user_id):
        json_file_path = f'{user_id}.json'

        if not os.path.exists(json_file_path):
            return flask.jsonify({"status": "error", "message": "No data file found for the given user ID."}), 404

        with open(json_file_path, 'r') as json_file:
            data = json.load(json_file)

        return flask.jsonify({"status": "success", "data": data})


api.add_resource(ReadData, '/send_data/<string:user_id>')

# namespace send_data --------------------------------------------------------------------------------------------------
send_data_ns = api.namespace('send_data', description=STR_SendData_description)

# user_data_model에서 사용하는 모델의 하위모델
location_model = api.model('Location', {
    'latitude': fields.Float(required=True, description='사용자 위치의 위도입니다.', example=37.7749),
    'longitude': fields.Float(required=True, description='사용자 위치의 경도입니다.', example=-122.4194)
})
user_data_model = api.model('user_data(send)', {
    'userId': fields.String(required=True, description='사용자 ID입니다.', example='abcdefgTNtaY7mOY0Rm123456789'),
    'userName': fields.String(required=False, description='사용자의 이름입니다.', example='홍길동'),
    'userEmail': fields.String(required=False, description='사용자의 email 주소입니다.', example='hgd0123@korea.com'),
    'location': fields.Nested(location_model, required=False, description='사용자의 위치입니다.'),
    'date': fields.String(required=False, description='날짜입니다.', example='1901-01-23'),
    'forecast': fields.String(required=False, description='일기예보입니다.', example='맑음')
})


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
