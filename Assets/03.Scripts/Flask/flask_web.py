# pip install flask

from flask import Flask, render_template

app = Flask(__name__)

@app.route('/') 
def home():
   return 'This is Home!'

if __name__ == '__main__' :   
  app.run(host='127.0.0.1', port=8000, debug=False)

# app = Flask(__name__)

# @app.route('/')
# def hello():
#     return 'hello'

# @app.route('/coder')
# def coder():
#     return 'I am AutoCoder'

# # .py가 실행되는 경로 안에 'templates' 폴더 생성 후 그 안에 test.html 파일 넣어두기
# @app.route('/autocoder')
# def autocoder():
#     return render_template("test.html")

# # 127.0.0.1 → 로컬만 들어오게, debug=True이면 에러 메시지를 페이지 랜더링에서 보여줌
# def main():
#     app.run(host='127.0.0.1', debug=False, port=80)

# if __name__ == '__main__':
#     main()
