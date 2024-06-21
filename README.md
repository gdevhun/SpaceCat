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
<summary><b>OpenAI Fine-Tuning</b></summary>



### 유니티 설치
<details>
  <summary><b>유니티 설치</b></summary>
  https://unity.com/kr/download![image](https://github.com/gdevhun/SpaceCat/assets/83668266/75938ffb-242a-4f80-851f-05ed9053d0f4)

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
</details>


<details>
  <summary><b> 유니티 설치</b></summary>
  <div markdown="1">
    <ul>
      1. 유니티 홈페이지 다운로드
     
      2. 설치 동의
      
      3. 로그인
      
      4. 버전은 2022.3.22f1 다운로드합니다.
      
      5. 라이선스 획득
      
      6. 라이선스 획득 후 화면
      
      7. 원하는 모듈 추후 설치 가능(continue)
      
      8.
      
      9. 깃 허브 브랜치에서 프로젝트 파일을 다운로드합니다.

         Add project from disk를 통해서 다운로드한 파일을 적용합니다.
         
      10. 클릭하면 프로젝트 파일이 열립니다.



      
      <li> https://unity.com/kr/download</li>
      <img src="./docs/주요_기능/포토스팟_콜렉션/1.gif" width=70%>
      <img src="./docs/주요_기능/포토스팟_콜렉션/2.gif" width=70%>
      <li>콜렉션에 있는 포토스팟 리스트를 확인하고 해당 포토스팟을 클릭하면 해당 좌표로 이동</li>
      <img src="./docs/주요_기능/포토스팟_콜렉션/3.gif" width=70%>
      <li>마음에 드는 콜렉션에 나의 반응을 표현할 수 있는 좋아요 기능</li>
      <img src="./docs/주요_기능/포토스팟_콜렉션/4.gif" width=70%>
      <li>포토스팟에 등록된 사진을 모아보고 비슷한 사진을 추천하는 기능</li>
      <img src="./docs/주요_기능/포토스팟_콜렉션/5.gif" width=70%>
    </ul>
  </div>
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

<br/>
.... 
