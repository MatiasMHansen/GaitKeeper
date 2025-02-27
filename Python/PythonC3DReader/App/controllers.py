from flask import Blueprint, request, jsonify
from injector import inject, singleton
from App.interfaces import IC3DParserService
from App.dtos import MetadataDTO

def register_routes(app):
    api = Blueprint('api', __name__)

    # Simpelt GET-endpoint for test.
    @api.route('/', methods=['GET'])
    def home():
        return jsonify({"message": "Hello from PythonC3DReader"}), 200

    # API endpoint til at parse en C3D-fil -> Metadata.
    @api.route('/metadata/<file_name>', methods=['GET'])
    @inject
    def metadata(file_name: str, c3d_service: IC3DParserService):
        try:
            data = c3d_service.get_metadata(file_name)          # data: MetadataDTO = c3d_service.get_metadata(file_name)
            return jsonify(data), 200                           # return jsonify(data.model_dump()), 200  # Brug Pydantic's `model_dump`
        except Exception as e:
            return jsonify({"error": str(e)}), 500

    # API endpoint til at parse en C3D-fil -> Pointdata.
    @api.route('/pointdata/<file_name>', methods=['GET'])
    @inject
    def pointdata(file_name: str, c3d_service: IC3DParserService):
        try:
            data = c3d_service.get_point_data(file_name)
            return jsonify(data), 200
        except Exception as e:
            return jsonify({"error": str(e)}), 500

    app.register_blueprint(api)