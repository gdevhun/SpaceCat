# app/__init__.py

from flask import Flask
from flask_cors import CORS
import pyrebase
from .config import Config

def create_app():
    app = Flask(__name__)
    CORS(app)

    config = Config.FIREBASE_CONFIG
    firebase = pyrebase.initialize_app(config)
    auth = firebase.auth()

    app.config['firebase'] = firebase
    app.config['auth'] = auth

    from .login import login_bp
    from .logout import logout_bp
    from .signup import signup_bp

    app.register_blueprint(login_bp)
    app.register_blueprint(logout_bp)
    app.register_blueprint(signup_bp)

    return app
