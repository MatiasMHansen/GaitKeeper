import os
from App import create_app

app = create_app()

if __name__ == "__main__":
    port = int(os.getenv("PORT", 5001))  # Brug Aspire's port, ellers fallback til 5001
    app.run(host="0.0.0.0", port=port)