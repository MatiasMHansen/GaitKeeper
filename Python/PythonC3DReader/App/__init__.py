from flask import Flask
from flask_injector import FlaskInjector
from injector import Binder, singleton
from App.interfaces import IC3DParserService
from App.services import C3DParserService

def configure(binder: Binder):
    binder.bind(IC3DParserService, to=C3DParserService, scope=singleton)

def create_app():
    app = Flask(__name__)

    from App.controllers import register_routes
    register_routes(app)

    FlaskInjector(app=app, modules=[configure])

    return app
