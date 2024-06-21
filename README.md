 <h1> 전북대학교 오픈소스소프트웨어 팀 프로젝트 </h1>

## 개발 기간 🗓
- 2024.04 ~ 2024.06

## 역할 분담 🧑‍💻
### 개발 인원 : 5명
| 이름 | 개인 역할 | 담당 역할 및 기능 |
| ------ | ---------- | ------ |
| 원동훈 | PM, Developer | PM, 기획, 프론트엔드, MBTI Test 구현 |
| 김유림 | Developer | UI/UX, 프론트엔드, MBTI Test 구현, OpenAI 파인튜닝 |
| 신광철 | Developer | 백엔드, Firebase 로그인 기능 구현, 서버 통신|
| 김솔래 | Developer | 백엔드, REST API 로그인 구현|
| 주효돈 | Developer | 백엔드, Firebase API, 날씨&지도 API, 서버 통신 |

<br/>

## 기술 스택 💻
<img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=Unity&logoColor=black">
<img src="https://img.shields.io/badge/csharp-512BD4?style=for-the-badge&logo=csharp&logoColor=white">
<img src="https://img.shields.io/badge/javascript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black"/>
<img src="https://img.shields.io/badge/python-3776AB?style=for-the-badge&logo=python&logoColor=white"/>
<img src="https://img.shields.io/badge/firebase-1D9FD7?style=for-the-badge&logo=firebase&logoColor=FFCA28"/> 
<img src="https://img.shields.io/badge/OpenAI-000000?style=for-the-badge&logo=openai&logoColor=white"/>
<img src="https://img.shields.io/badge/amazonwebservices-232F3E?style=for-the-badge&logo=amazonwebservices&logoColor=white"/>
<img src="https://img.shields.io/badge/flask-000000?style=for-the-badge&logo=flask&logoColor=white"/>

(Version : Unity 2022.3.22f1 - LTS)
<br/>


## 협업 가이드 및 규칙 Tool - 프로젝트 스케줄 📅
#### 노션 Notion
- https://www.notion.so/ab5763575e0940c09e21e68cd2c7b464

- https://unmarred-deer-17b.notion.site/ab5763575e0940c09e21e68cd2c7b464?pvs=4


## 설치 및 빌드

<details>
  <summary><b>키 값 입력</b></summary>
  - Assets/Firebase/google-services.json에 19번 줄 부분에 'current_key'에 notion에 있는 google서비스 키 값 입력
  - Assets/03.Scripts/AI/OpenAI-Fine-Tuning/auth.json 에 있는 'api-key'와 'organization'에 notion에 있는 키 값을 각각 입력 해줄 것 


</details>
<details>
  <summary><b>유니티 설치</b></summary>
  https://unity.com/kr/download

  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/d11b0bae-848d-4ac7-b4e8-ff56573d04f8" alt="image 1">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/dd58f8f4-4a8c-459e-b165-44df613cdb3b" alt="image 2">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/92f2fffd-7624-4202-8b63-e581ac34315a" alt="image 3">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/8b72ac3d-ef12-4798-9a8e-e1e4671eac32" alt="image 4">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/49909002-eee5-4b45-a4bc-094de4c1b1e0" alt="image 5">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/827b8afe-be15-425f-bef8-145652a24edf" alt="image 6">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/89b2ab44-ccb3-4150-af08-ae5027463fba" alt="image 7">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/e98cb372-9c3a-42d4-8ea2-cea533e83327" alt="image 8">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/b0bb768f-7fff-4484-b5b6-375154905fa4" alt="image 9">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/760468ac-512e-4c69-9938-9f5913428ad3" alt="image 10">
  <img src="https://github.com/gdevhun/SpaceCat/assets/83668266/6d507e63-ca0e-4bfb-b1e8-c6772d21af4a" alt="image 11">

  
  - 상단에 Windows-Package Manger 클릭
  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/578c8762-75a9-4137-82e0-58f52a2ab4f3)
  
  - 좌측상단에 + 버튼을 클릭 후 Add package from git URL 클릭
  아래 URL 입력 후 Add
  https://github.com/srcnalt/OpenAI-Unity.git
  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/48924abc-2f29-4962-baf8-5bf98d62b863)

  - OpenAI Unity 찾은 후 Install 버튼 클릭
    
  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/ff7cf3a5-b2ad-4484-b76e-24b256be96ed)


</details>


<details>
  <summary><b>유니티 실행</b></summary>
  https://unity.com/kr/download![image](https://github.com/gdevhun/SpaceCat/assets/83668266/75938ffb-242a-4f80-851f-05ed9053d0f4)

  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/c77408be-d1ee-44c2-9971-a89c85c36eca)
  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/b70160b6-1fff-4c9d-99cf-5129d0dd0354)
  ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/7634a7a5-e76c-43a2-be80-7bdbf95059f5)

</details>


## 참고 자료
<details>
<summary><b>OpenAI Fine-Tuning</b></summary>



### MBTI 특성 정리

[MBTI Personality Types 500 Dataset](https://www.kaggle.com/datasets/zeyadkhalid/mbti-personality-types-500-dataset/data)
![image](https://github.com/gdevhun/SpaceCat/assets/83668266/3350c6b3-3617-4daa-94c1-164556c10629)



### OpenAI 파인튜닝
1. OpenAI에서 `gpt-3.5-turbo`로 데이터셋 제작.
   - [Create_MBTI_Data_Openai_api.ipynb](https://github.com/YBIGTA/24th-project-mbti-prediction/blob/main/task2/Create_MBTI_Data_Openai_api.ipynb)
     ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/483818ae-a2bc-459c-bcd0-e4215c037611)
   - 데이터셋 변환 중 발생한 오류: [ChatGPT 솔루션](https://chatgpt.com/share/fee22987-b773-4913-8e80-2e319dfb1514)

2. OpenAI ‘gpt-3.5-turbo-1106’ 모델을 베이스로 파인튜닝
   - [OpenAI Fine-tuning](https://platform.openai.com/docs/guides/fine-tuning)
     ![image](https://github.com/gdevhun/SpaceCat/assets/83668266/296496e4-2d40-4e48-a64c-9a31f5d4bc89)



### Unity에 파인튜닝된 모델 적용

[How To Make ChatGPT NPC In Unity - Tutorial](https://youtu.be/lYckk570Tqw?si=L7pjwiSJ9_HQQla2)
</details>

<details>
  <summary><b>날씨 및 지도 API</b></summary>
 
  기상청 날씨예보 받아오는 샘플코드
  https://www.data.go.kr/tcs/dss/selectApiDataDetailView.do?publicDataPk=15084084
 
  사용자 위치정보 가져오는 코드
  https://velog.io/@jm450_/Unity-AR-GPS%EC%97%90-%EB%94%B0%EB%A5%B8-%EC%9C%84%EC%B9%98-%EC%9D%B4%EB%8F%99
  
  기상청 좌표 xy로 변환
  https://gist.github.com/fronteer-kr/14d7f779d52a21ac2f16

</details>
<details>
  <summary><b>Firebase API</b></summary>
 
  firebase sdk 사용법
  https://firebase.google.com/docs/web/setup?hl=ko&authuser=0&_gl=1*vrvf6k*_ga*MTU4OTg2ODE2LjE3MTg4OTQ3Mjk.*_ga_CW55HF8NVT*MTcxODk0NzI0NS41LjEuMTcxODk0NzM3Ny40Ni4wLjA.
 firebase R/W 활용
 https://chatgpt.com/c/f8a2acc9-5e1f-4004-952f-a01bcc2880af


</details>
<details>
  <summary><b>서버</b></summary>
 
  [app.py]
  서버에 데이터 주고 파일로 저장 및 데이터 유저에게 전달 (.send_data), 
  서버에 저장된 파일 데이터 유저에게 전달 (.read_data)
  데이터 정의 및 직렬화, 서버와의 통신
  
  참고 자료
  // https://github.com/AakashGD890/FirebaseStarterProject
  // https://firebase.google.com/docs/auth/unity/start?hl=ko)&_gl=1*2pq1it*_up*MQ..*_ga*NDUxNzQ2NTQ0LjE3MTQwMDYzODg.*_ga_CW55HF8NVT*MTcxNDAwNjM4OC4xLjAuMTcxNDAwNjM4OC4wLjAuMA.. (firebase 공식 docs)



</details>
<details>
  <summary><b>Swagger 해설</b></summary>
 
  설명 : API개발에 도움이 되도록 API통신을 쉽게 테스트해볼 수 있는 여건을 제공해준다.
https://swagger.io/
  
  테스트 링크 : (swagger UI editor 기본예제인 Petstore API)
  https://petstore.swagger.io/?_gl=1*1jcnyq5*_gcl_au*MzAyNjY4OTguMTcxODAyNzI4MA..&_ga=2.38522087.518992663.1718952823-1509199943.1718027280
  
  flask_restx 레퍼런스 : https://flask-restx.readthedocs.io/en/latest/
  
  적용 방법 : (flask 서버 + python 이용) (python으로 만들어진 코드를 Swagger 정의로 자동변환함)
      1.	기존에 서버로 사용되었던 파일에 flask_restx 를 설치해 불러옵니다.
      2.	Swagger API로 사용되는 변수를 추가합니다
      api = Api(app, version='1.0', title='Sample API', description='A sample API', )
      3.	api.amespace를 통해 기능을 대분류로 나눕니다.
      4.	api.model를 통해 사용할 데이터 구조를 정의합니다. (자료형, 필수입력여부 등)
      5.	각 기능을 구현할 class를 구현합니다. 클래스는 flask_restx의 Resource 클래스로부터 상속받습니다.
      6.	클래스 상단에 @<구현한 namespace 이름>.route()로 실제 api 접속 주소를, .param()으로 이 기능에서 사용될 입력을 추가합니다.
      7.	각 클래스에서 get, post, put 등의 이름을 가진 메서드를 구현합니다. 리턴값이 곧 응답 결과물 입니다. (swagger는 Resoure에 오버라이딩 하면 알아서 인식해 표시합니다)
      8.	각 메서드 상단에는 @<구현한 namespace 이름>.expect()를 통해 입력 데이터 구조를 정의하고, .response(code, text)를 통해 응답을 설정해 줄 수 있습니다.


</details>

<br/>
.... 
