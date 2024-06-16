# app/logout.py

from flask import Blueprint, jsonify

logout_bp = Blueprint('logout', __name__)

@logout_bp.route('/logout', methods=['POST'])
def logout():
    return jsonify({"status": "success", "message": "Logged out successfully"}), 200
