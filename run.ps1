$ErrorActionPreference = "Stop"
if (-not (Test-Path ".env")) {
    Copy-Item ".env.example" ".env"
    Write-Host "Created .env from .env.example"
}
docker compose -f build/Docker/docker-compose.yml up --build -d
Write-Host ""
Write-Host "InvoiceFlow started:"
Write-Host "Swagger:  http://localhost:8080/swagger"
Write-Host "Health:   http://localhost:8080/health"
Write-Host "RabbitMQ: http://localhost:15672"
Write-Host "Qdrant:   http://localhost:6333/dashboard"
