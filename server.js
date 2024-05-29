import express from 'express';     // Express  모듈을 가져옴
import jwt from 'jsonwebtoken';    // jwt 모듈을 가져옴
import bodyParser from 'body-parser';
import fetch from 'node-fetch';    // node-fetch 패키지를 사용하여 HTTP 요청을 보냄
const app = express();      // Express 애플리케이션을 생성
const port = 8080;           // 서버가 리슨할 포트를 설정

const CLIENT_ID = '1077253468932-j3e7ushv5trp8cgo0ss0dumh0l8j6518.apps.googleusercontent.com';
const CLIENT_SECRET = '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';
const REDIRECT_URI = 'http://localhost:8080';

app.use(bodyParser.json());

app.get('/', (req, res) => {
    const code = req.query.code;               // Google에서 받은 인증 코드
    if (!code) {
        res.send('No code provided');
        return;
    }

    console.log(`Received code: ${code}`);  // 인증 코드를 터미널에 출력

    // 교환 코드 → 토큰
    const tokenEndpoint = 'https://oauth2.googleapis.com/token';
    const params = new URLSearchParams();
    params.append('code', code);
    params.append('client_id', CLIENT_ID);
    params.append('client_secret', CLIENT_SECRET);
    params.append('redirect_uri', REDIRECT_URI);
    params.append('grant_type', 'authorization_code');

    fetch(tokenEndpoint, {
        method: 'POST',
        body: params,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                res.send(`Error: ${data.error}`);
                return;
            }

            const idToken = data.id_token;
            console.log(`ID Token: ${idToken}`);

            // JWT 생성
            const jwtToken = jwt.sign({ idToken }, 'your_jwt_secret', { expiresIn: '1h' });

            // Unity로 리디렉션
            /*
             *  브라우저의 localStorage에 인증 코드를 저장
             *  커스텀 스키마를 사용하여 Unity 애플리케이션으로 리디렉션함
             */
            res.send(`<script>
                        console.log('Redirecting to unity:auth');
                        localStorage.setItem('jwt_token', '${jwtToken}');
                        window.location = 'unity:auth/${jwtToken}';
                      </script>`);
        })
        .catch(err => {
            console.error(err);
            res.send('Error exchanging code for token');
        });
    /*res.send(`<script>window.location = "unity:auth/${code}";</script>`);*/
});

app.listen(port, () => {
    console.log(`Server listening at http://localhost:${port}`);
});
