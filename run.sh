#!/usr/bin/env sh
set -eu
[ -f .env ] || cp .env.example .env
docker compose -f build/Docker/docker-compose.yml up --build -d
echo "Swagger:  http://localhost:8080/swagger"
echo "RabbitMQ: http://localhost:15672"
echo "Qdrant:   http://localhost:6333/dashboard"
