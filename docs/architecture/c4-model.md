# C4 Model

## Context
InvoiceFlow automates invoice intake, AI extraction, fraud-risk analysis, approval/rejection and semantic search.

## Containers
- Web API (.NET 9)
- Worker service
- PostgreSQL + pgvector
- Redis
- RabbitMQ
- Qdrant
- Optional external document intelligence / LLM providers

## Components
Domain -> Application/CQRS -> Infrastructure/AI -> Web API/Workers.
