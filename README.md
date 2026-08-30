# InvoiceFlow Enterprise

> Enterprise-style, AI-assisted invoice processing platform built with .NET 9, Clean Architecture, Domain-Driven Design, CQRS, PostgreSQL, Redis, background processing, vector search, observability, Docker, and CI/CD.

InvoiceFlow Enterprise is a portfolio/reference implementation of a modern invoice-processing backend designed to demonstrate how enterprise application architecture, AI-assisted document processing, asynchronous workflows, observability, automated testing, and containerized infrastructure can be combined in a single .NET solution.

The application accepts invoice documents, extracts structured invoice data, applies domain rules, evaluates basic fraud-risk signals, persists invoices, generates embeddings for semantic search, and exposes workflows for reviewing, approving, and rejecting invoices.

The repository is intentionally structured around replaceable abstractions. The default development setup uses deterministic local AI fallbacks so the core workflow can run without paid OpenAI or Azure credentials, while the architecture contains integration points for production-grade external services.

---

## Table of Contents

- [Project Goals](#project-goals)
- [Key Features](#key-features)
- [End-to-End Processing Flow](#end-to-end-processing-flow)
- [Architecture](#architecture)
- [Solution Structure](#solution-structure)
- [Technology Stack](#technology-stack)
- [Domain Model](#domain-model)
- [Application Layer and CQRS](#application-layer-and-cqrs)
- [AI and Intelligent Processing](#ai-and-intelligent-processing)
- [Persistence and Infrastructure](#persistence-and-infrastructure)
- [Transactional Outbox](#transactional-outbox)
- [Background Workers](#background-workers)
- [Observability](#observability)
- [API Overview](#api-overview)
- [Local Development](#local-development)
- [Running with Docker](#running-with-docker)
- [Running with the .NET CLI](#running-with-the-net-cli)
- [Configuration](#configuration)
- [Testing Strategy](#testing-strategy)
- [CI/CD](#cicd)
- [Architecture Decision Records](#architecture-decision-records)
- [Security and Production Considerations](#security-and-production-considerations)
- [Current Implementation Notes](#current-implementation-notes)
- [Potential Future Improvements](#potential-future-improvements)

---

## Project Goals

The project was created to demonstrate several concepts commonly found in larger backend and distributed-system codebases:

- clear separation of business rules from infrastructure concerns;
- Clean Architecture and dependency inversion;
- Domain-Driven Design building blocks;
- CQRS with MediatR;
- structured validation and request pipeline behaviors;
- PostgreSQL persistence with Entity Framework Core;
- asynchronous and event-oriented processing patterns;
- transactional outbox concepts;
- AI-assisted document extraction;
- embedding generation and semantic search;
- fraud-risk analysis;
- background jobs and worker services;
- Redis-based caching abstractions;
- OpenTelemetry and structured logging;
- Docker-based local infrastructure;
- unit, integration, AI, and architecture testing;
- GitHub Actions for automated build, test, and container publishing.

The main objective is not only to provide a working invoice workflow, but also to show how a maintainable enterprise solution can be decomposed into independent layers and replaceable services.

---

## Key Features

### Invoice ingestion

Invoice PDFs can be submitted through the Web API. The uploaded document is passed to the application layer through a MediatR command rather than processed directly inside the HTTP endpoint.

### AI-assisted extraction

The document extraction abstraction converts an uploaded invoice into structured information such as:

- invoice number;
- vendor name;
- total amount;
- currency;
- extraction confidence.

The repository includes provider-oriented implementations and local fallback behavior, allowing development without external AI credentials.

### Domain-driven invoice lifecycle

The `Invoice` aggregate controls its own lifecycle and business state. It supports transitions such as:

- pending extraction;
- pending review;
- approved;
- rejected.

Business behavior is kept inside the domain model rather than being spread across controllers or infrastructure services.

### Confidence-based review

Extraction confidence is represented by a dedicated value object. The domain contains rules that support human-review decisions for low-confidence AI results.

### Fraud-risk assessment

The AI infrastructure contains a fraud-analysis implementation that evaluates invoice signals such as high invoice value and low extraction confidence. Fraud findings are represented through domain objects such as `RiskAssessment`, `RiskLevel`, and `FraudFlag`.

### Semantic search

Invoice information is converted into a vector representation and indexed through the `IVectorStore` abstraction. The API exposes semantic search functionality for retrieving invoices using natural-language-style queries.

### Approval and rejection workflows

Finance-oriented workflows are exposed for:

- listing pending invoices;
- retrieving invoice details;
- approving an invoice;
- rejecting an invoice with a reason.

### Background processing

A separate Worker project demonstrates processing that should not block HTTP requests. Quartz is used to schedule recurring background work, including outbox processing.

### Observability

The solution contains dedicated observability infrastructure for:

- structured application logging;
- OpenTelemetry tracing;
- ASP.NET Core instrumentation;
- outbound HTTP instrumentation;
- Entity Framework Core instrumentation;
- LLM/AI tracing extension points.

### Automated quality checks

The test suite contains separate projects for:

- domain unit tests;
- application unit tests;
- infrastructure integration tests;
- AI integration tests;
- architecture tests.

---

## End-to-End Processing Flow

A typical invoice-processing request follows this path:

```text
Client
  |
  | POST /api/invoices/upload
  v
InvoiceFlow.WebApi
  |
  | MediatR command
  v
ProcessInvoicePdfCommandHandler
  |
  +--> IDocumentExtractor
  |      |
  |      +--> structured invoice data
  |
  +--> Invoice aggregate
  |      |
  |      +--> domain validation
  |      +--> extraction confidence
  |      +--> domain event
  |
  +--> PostgreSQL / EF Core
  |
  +--> IFraudDetectionModel
  |
  +--> IEmbeddingService
  |
  +--> IVectorStore
  |
  v
Invoice available for review and semantic search
```

In code, the main orchestration happens in `ProcessInvoicePdfCommandHandler`. The handler depends only on abstractions defined by the Application layer:

- `IApplicationDbContext`;
- `IDocumentExtractor`;
- `IFraudDetectionModel`;
- `IEmbeddingService`;
- `IVectorStore`.

This keeps the use case independent from specific database, AI, or vector-database technologies.

---

## Architecture

InvoiceFlow follows a Clean Architecture-inspired structure.

```text
Presentation
    |
    v
Application
    |
    v
Domain

Infrastructure --------> Application abstractions
AI Infrastructure -----> Application abstractions
Observability ----------> Host / infrastructure integration
```

The central design rule is that business logic should not depend on frameworks or external providers.

### Domain

The Domain project contains the core business concepts and has no dependency on the Web API, database implementation, Redis, RabbitMQ, AI providers, or other infrastructure details.

### Application

The Application layer defines use cases and ports/interfaces required by those use cases. It coordinates domain behavior but does not know how external services are implemented.

### Infrastructure

The Infrastructure layer implements persistence, caching, identity-related services, email/storage adapters, outbox processing, and other technical concerns.

### AI Infrastructure

The AI project implements document extraction, embedding generation, fraud analysis, vector storage, and AI-oriented agents.

### Presentation

The Presentation layer contains the Minimal API host and background Worker host. It is responsible for receiving requests, dependency injection, application startup, and mapping endpoints.

### Observability

Observability is kept in a dedicated project to centralize telemetry and logging setup rather than scattering instrumentation configuration across the application.

---

## Solution Structure

```text
InvoiceFlow.Enterprise/
|
+-- .github/
|   +-- ISSUE_TEMPLATE/
|   +-- workflows/
|       +-- build-and-test.yml
|       +-- deploy-prod.yml
|
+-- build/
|   +-- Build.cs
|   +-- Docker/
|       +-- docker-compose.yml
|       +-- docker-compose.override.yml
|       +-- postgres-init.sql
|
+-- docs/
|   +-- api/
|   |   +-- openapi-spec.json
|   +-- architecture/
|   |   +-- adr/
|   |   +-- c4-model.md
|   +-- prompts/
|
+-- src/
|   +-- Core/
|   |   +-- InvoiceFlow.Domain/
|   |   +-- InvoiceFlow.Application/
|   |
|   +-- Infrastructure/
|   |   +-- InvoiceFlow.Infrastructure/
|   |   +-- InvoiceFlow.AI/
|   |   +-- InvoiceFlow.Observability/
|   |
|   +-- Presentation/
|       +-- InvoiceFlow.WebApi/
|       +-- InvoiceFlow.Workers/
|
+-- tests/
|   +-- InvoiceFlow.Domain.UnitTests/
|   +-- InvoiceFlow.Application.UnitTests/
|   +-- InvoiceFlow.IntegrationTests/
|   +-- InvoiceFlow.AI.IntegrationTests/
|   +-- InvoiceFlow.ArchitectureTests/
|
+-- Directory.Build.props
+-- Directory.Packages.props
+-- global.json
+-- InvoiceFlow.sln
+-- run.ps1
+-- run.sh
+-- .env.example
+-- README.md
+-- LICENSE
```

---

## Technology Stack

### Platform and backend

- **.NET 9**
- **C#**
- **ASP.NET Core Minimal APIs**
- **MediatR 12**
- **FluentValidation**

### Data and persistence

- **Entity Framework Core 9**
- **PostgreSQL 17**
- **Npgsql**
- **pgvector-enabled PostgreSQL image**
- **Redis**

### Messaging and background processing

- **Quartz.NET**
- **MassTransit RabbitMQ package / messaging extension point**
- **RabbitMQ development infrastructure**
- **Transactional Outbox pattern**

### AI and search

- document extraction abstractions;
- Azure Document Intelligence adapter/fallback path;
- OpenAI Vision-oriented extractor/fallback path;
- deterministic local 384-dimensional embeddings;
- Qdrant-oriented vector store abstraction;
- pgvector-oriented vector store abstraction;
- fraud-risk analysis;
- ONNX Runtime dependency and model integration point;
- AI agent classes for validation and executive summaries.

### Observability

- **Serilog**
- **OpenTelemetry**
- ASP.NET Core instrumentation
- HTTP instrumentation
- Entity Framework Core instrumentation
- OTLP exporter support

### Testing

- **xUnit**
- **Moq**
- **FluentAssertions**
- **Testcontainers for PostgreSQL**
- **NetArchTest.Rules**

### DevOps

- **Docker**
- **Docker Compose**
- **GitHub Actions**
- **GitHub Container Registry (GHCR)**

---

## Domain Model

The Domain layer demonstrates several Domain-Driven Design patterns.

### Aggregate Root

`Invoice` is modeled as an aggregate root. State-changing behavior is implemented through methods instead of allowing arbitrary public property mutation.

Examples include:

```csharp
invoice.ApplyExtraction(...);
invoice.Approve();
invoice.Reject(reason);
```

This makes domain invariants easier to protect.

### Entities

Important entities include:

- `Invoice`;
- `InvoiceLineItem`;
- `Vendor`;
- `VendorAddress`.

### Value Objects

Value objects model concepts that require stronger semantics than primitive values:

- `Money`;
- `ConfidenceScore`;
- `TaxNumber`.

### Domain Events

Invoice state changes can raise domain events such as:

- `InvoiceExtractedViaAiEvent`;
- `InvoiceApprovedDomainEvent`;
- `InvoiceRejectedDomainEvent`.

Domain events allow side effects to be separated from the aggregate itself.

### Business Rules

The project contains explicit domain-rule classes, including:

- positive invoice amount validation;
- low-confidence human-review rules.

### Fraud-analysis domain types

Fraud evaluation is modeled using dedicated domain concepts:

- `RiskAssessment`;
- `RiskLevel`;
- `FraudFlag`.

This avoids representing risk as an unstructured numeric value with no business meaning.

---

## Application Layer and CQRS

The Application project organizes invoice use cases into Commands and Queries.

### Commands

Commands represent operations that change application state.

Implemented command workflows include:

- `ProcessInvoicePdfCommand`;
- `ApproveInvoiceCommand`;
- `RejectInvoiceCommand`.

Each command is handled through MediatR.

### Queries

Queries represent read-only operations.

Implemented queries include:

- `GetInvoiceByIdQuery`;
- `GetPendingInvoicesQuery`;
- `SemanticSearchQuery`.

DTOs are used to keep external API contracts separate from domain entities.

### Application abstractions

The Application layer defines interfaces for infrastructure capabilities such as:

```text
IApplicationDbContext
ICacheService
ICurrentUserService
IDateTimeProvider
IEmailService
IDocumentExtractor
IEmbeddingService
IFraudDetectionModel
IVectorStore
```

Infrastructure projects implement these interfaces at runtime.

### MediatR pipeline behaviors

Cross-cutting request concerns are modeled through behaviors:

- `ValidationBehavior`;
- `LoggingBehavior`;
- `TransactionBehavior`;
- `AiRateLimitingBehavior`.

This pattern avoids duplicating validation, transaction, and logging code in individual handlers.

---

## AI and Intelligent Processing

The AI layer is designed around interfaces so implementations can be replaced without changing business use cases.

### Document extraction

`IDocumentExtractor` provides the abstraction used by the application workflow.

The repository contains:

- `OpenAiVisionExtractor`;
- `AzureDocumentIntelligenceExtractor`.

In the current local implementation, the OpenAI-oriented extractor acts as a deterministic fallback. It hashes the uploaded document bytes and produces predictable demo invoice data. This makes the project runnable without external API calls or credentials.

The Azure adapter currently delegates to the fallback implementation, providing an architectural integration point for replacing it with the Azure Document Intelligence SDK in a production implementation.

### Embeddings

`FastEmbedService` creates a deterministic 384-dimensional vector representation of invoice text.

The local algorithm:

1. tokenizes text;
2. hashes tokens with SHA-256;
3. maps token hashes into vector positions;
4. accumulates vector values;
5. normalizes the resulting vector.

This is intentionally lightweight and dependency-free for local demonstration purposes.

### Vector search

`IVectorStore` allows semantic search to remain independent of a specific vector database.

Available implementations include:

- `QdrantVectorStore`;
- `PgVectorStore`.

The current Qdrant-oriented implementation is an in-memory fallback using cosine similarity. This lets semantic search work locally even when a remote Qdrant server is not used by the running application.

Docker Compose nevertheless provisions Qdrant so the architecture can be extended to a real remote vector-store implementation.

### Fraud detection

`IFraudDetectionModel` defines the fraud-analysis contract.

`OnnxFraudDetector` currently performs deterministic risk scoring using application/domain signals. For example:

- invoices above the demo high-value threshold increase risk;
- low AI extraction confidence increases risk.

The project also contains an ONNX model artifact and references `Microsoft.ML.OnnxRuntime`, providing a natural extension point for replacing the deterministic scoring path with real model inference.

### AI agents

The project includes agent-oriented classes such as:

- `InvoiceValidationAgent`;
- `ExecutiveSummaryAgent`.

Versioned prompt assets are stored under:

```text
docs/prompts/
```

Keeping prompts under source control supports review, traceability, and future prompt-version evaluation.

---

## Persistence and Infrastructure

### Entity Framework Core

`ApplicationDbContext` provides relational persistence through EF Core and PostgreSQL.

Entity configurations are separated into dedicated classes for:

- invoices;
- vendors;
- outbox messages.

This keeps database mapping details out of domain entities.

### Migrations

The repository contains an initial EF Core migration and model snapshot under:

```text
src/Infrastructure/InvoiceFlow.Infrastructure/Persistence/Migrations/
```

### Persistence interceptors

The Infrastructure layer includes EF Core interception points such as:

- `AuditableEntityInterceptor`;
- `PublishDomainEventsInterceptor`.

They demonstrate how auditing and domain-event handling can be integrated with the persistence lifecycle.

### Redis caching

`RedisCacheService` implements the application cache abstraction. Redis is provisioned by Docker Compose for local development.

### Storage and external-service adapters

Infrastructure service adapters include:

- `AzureBlobStorageService`;
- `SmtpEmailService`;
- `DateTimeProvider`;
- `CurrentUserService`;
- `KeycloakAuthService`.

These isolate external-service concerns from domain and application code.

---

## Transactional Outbox

The project contains a Transactional Outbox implementation to demonstrate reliable event-processing architecture.

The basic concept is:

1. a business transaction modifies domain state;
2. an integration/event message is stored in the same database transaction;
3. a background processor retrieves unprocessed outbox messages;
4. messages are processed/published asynchronously;
5. successful messages are marked as processed.

Relevant classes include:

```text
OutboxMessage
OutboxMessageConfiguration
OutboxProcessor
ProcessOutboxMessagesJob
```

The current `OutboxProcessor` marks pending messages as processed and logs their processing. A production implementation can extend this step to deserialize and publish events through a real message bus with retry/idempotency handling.

---

## Background Workers

`InvoiceFlow.Workers` provides a separate host for asynchronous workloads.

Quartz.NET is configured to execute the outbox-processing job every 10 seconds.

Worker-oriented components include:

- `ProcessOutboxMessagesJob`;
- `BatchEmbeddingIndexingJob`;
- `InvoiceUploadedConsumer`;
- `InvoiceApprovedConsumer`.

Separating workers from the API makes it possible to scale HTTP traffic and background processing independently in a larger deployment.

---

## Observability

The `InvoiceFlow.Observability` project centralizes instrumentation and logging configuration.

It contains:

- `LoggingExtensions`;
- `OpenTelemetrySetup`;
- `LLMTracingExtensions`.

The solution references OpenTelemetry packages for:

- ASP.NET Core request tracing;
- outbound HTTP tracing;
- EF Core tracing;
- OTLP telemetry export.

Structured console logging is configured through Serilog in the default application settings.

For a production environment, telemetry can be exported to platforms such as Grafana Tempo, Jaeger, Azure Monitor, Datadog, New Relic, or another OTLP-compatible backend.

---

## API Overview

Swagger UI is available in the development setup and provides interactive documentation for the API.

### Health

```http
GET /health
```

Returns the current Web API health response.

### Upload invoice

```http
POST /api/invoices/upload
Content-Type: multipart/form-data
```

Example:

```bash
curl -F "file=@invoice.pdf" http://localhost:8080/api/invoices/upload
```

The endpoint returns `202 Accepted` with the generated invoice identifier.

### List pending invoices

```http
GET /api/invoices/pending
```

Example:

```bash
curl http://localhost:8080/api/invoices/pending
```

### Get invoice by ID

```http
GET /api/invoices/{id}
```

### Approve invoice

```http
POST /api/invoices/{id}/approve
```

### Reject invoice

```http
POST /api/invoices/{id}/reject
Content-Type: application/json
```

Example body:

```json
{
  "reason": "Invoice requires manual vendor verification."
}
```

### Semantic search

```http
GET /api/ai/search?q={query}
```

Example:

```bash
curl "http://localhost:8080/api/ai/search?q=vendor%20invoice"
```

### Explain risk

```http
GET /api/ai/explain?invoiceNumber={invoiceNumber}
```

The current endpoint returns a demonstration explanation of the risk signals considered by the application.

### Development login

```http
POST /api/auth/demo-login
```

This returns a development-only placeholder token. It is intentionally not a production authentication mechanism.

---

## Local Development

### Prerequisites

For the full local environment:

- **Docker Desktop** or compatible Docker Engine/Compose installation.

For direct .NET development:

- **.NET 9 SDK**;
- PostgreSQL;
- Redis, if exercising cache-backed functionality.

The repository pins the SDK family through `global.json`:

```json
{
  "sdk": {
    "version": "9.0.100",
    "rollForward": "latestFeature"
  }
}
```

---

## Running with Docker

### Windows helper script

From PowerShell:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\run.ps1
```

### Linux/macOS helper script

```bash
chmod +x run.sh
./run.sh
```

### Manual Docker Compose startup

First create the local environment file:

```bash
cp .env.example .env
```

Then start the stack:

```bash
docker compose -f build/Docker/docker-compose.yml up --build -d
```

### Local services

After the stack starts, the main development endpoints are:

| Service | Address |
|---|---|
| InvoiceFlow API | `http://localhost:8080` |
| Swagger UI | `http://localhost:8080/swagger` |
| Health endpoint | `http://localhost:8080/health` |
| PostgreSQL | `localhost:5432` |
| Redis | `localhost:6379` |
| RabbitMQ | `localhost:5672` |
| RabbitMQ Management | `http://localhost:15672` |
| Qdrant HTTP / Dashboard | `http://localhost:6333` |
| Qdrant gRPC | `localhost:6334` |

### Stop the stack

```bash
docker compose -f build/Docker/docker-compose.yml down
```

To also remove the PostgreSQL volume:

```bash
docker compose -f build/Docker/docker-compose.yml down -v
```

> Removing volumes deletes locally persisted development data.

---

## Running with the .NET CLI

Restore packages:

```bash
dotnet restore InvoiceFlow.sln
```

Build the solution:

```bash
dotnet build InvoiceFlow.sln
```

Run all tests:

```bash
dotnet test InvoiceFlow.sln
```

Run the Web API:

```bash
dotnet run --project src/Presentation/InvoiceFlow.WebApi
```

Run the Worker host:

```bash
dotnet run --project src/Presentation/InvoiceFlow.Workers
```

---

## Configuration

Copy the example file before starting the Docker environment:

```bash
cp .env.example .env
```

The example configuration contains development defaults for:

```text
POSTGRES_DB
POSTGRES_USER
POSTGRES_PASSWORD
POSTGRES_PORT
REDIS_PORT
RABBITMQ_PORT
RABBITMQ_MANAGEMENT_PORT
QDRANT_PORT
ConnectionStrings__Postgres
ConnectionStrings__Redis
RabbitMq__Host
Qdrant__Host
Qdrant__Port
AI__Provider
AI__OpenAiApiKey
AI__AzureDocumentIntelligenceEndpoint
AI__AzureDocumentIntelligenceKey
AI__FraudModelPath
```

### AI configuration

The default mode is:

```text
AI__Provider=local
```

No OpenAI or Azure key is required for the current deterministic local fallback implementation.

Do not commit real secrets to source control. For production deployments, use environment-specific secret management such as:

- GitHub Actions secrets;
- Kubernetes Secrets combined with an external secret manager;
- Azure Key Vault;
- AWS Secrets Manager;
- HashiCorp Vault;
- another managed secret store.

---

## Testing Strategy

The project intentionally separates different types of automated tests.

### Domain unit tests

Project:

```text
tests/InvoiceFlow.Domain.UnitTests
```

Focuses on business rules and value objects without external infrastructure.

Examples include:

- invoice rule validation;
- value-object behavior.

### Application unit tests

Project:

```text
tests/InvoiceFlow.Application.UnitTests
```

Tests application handlers and pipeline behavior with dependencies replaced by mocks or test doubles.

Examples include:

- approval command handling;
- PDF-processing command handling;
- validation pipeline behavior.

### Integration tests

Project:

```text
tests/InvoiceFlow.IntegrationTests
```

Uses PostgreSQL Testcontainers to test persistence behavior against a real database engine rather than relying only on EF Core in-memory behavior.

### AI integration tests

Project:

```text
tests/InvoiceFlow.AI.IntegrationTests
```

Contains invoice extraction tests and sample PDF fixtures under:

```text
TestData/
```

### Architecture tests

Project:

```text
tests/InvoiceFlow.ArchitectureTests
```

Uses `NetArchTest.Rules` to enforce architectural dependency boundaries and protect the intended Clean Architecture structure from accidental coupling.

### Run the entire test suite

```bash
dotnet test InvoiceFlow.sln -c Release
```

---

## CI/CD

The repository includes two GitHub Actions workflows.

### Build and test

File:

```text
.github/workflows/build-and-test.yml
```

Runs on pushes and pull requests.

The pipeline:

1. checks out the repository;
2. installs .NET 9;
3. restores NuGet packages;
4. builds the solution in Release mode;
5. runs the automated test suite.

Equivalent commands:

```bash
dotnet restore InvoiceFlow.sln
dotnet build InvoiceFlow.sln -c Release --no-restore
dotnet test InvoiceFlow.sln -c Release --no-build --verbosity normal
```

### Production container publishing

File:

```text
.github/workflows/deploy-prod.yml
```

This workflow is manually triggered with `workflow_dispatch`.

It:

1. checks out the repository;
2. authenticates with GitHub Container Registry;
3. builds the Web API Docker image;
4. pushes the image to GHCR;
5. tags the image with the Git commit SHA.

Image pattern:

```text
ghcr.io/<repository-owner>/invoiceflow-api:<commit-sha>
```

The workflow currently performs container publication. Deployment of that container to Kubernetes, Azure Container Apps, AWS ECS/EKS, a VM, or another runtime can be added as the next delivery stage.

---

## Architecture Decision Records

Important architectural choices are documented as ADRs under:

```text
docs/architecture/adr/
```

Current ADRs include:

- `ADR-001-Clean-Architecture.md`;
- `ADR-002-CQRS-MediatR.md`;
- `ADR-003-Transactional-Outbox.md`;
- `ADR-004-VectorDb-Selection.md`.

The project also contains a C4 architecture model:

```text
docs/architecture/c4-model.md
```

Using ADRs is useful in larger projects because it records not only *what* was built, but also *why* architectural decisions were made.

---

## Security and Production Considerations

This repository is designed primarily as an enterprise-style reference and learning/portfolio project. Before using a similar system for real financial processing, several areas should be hardened.

### Authentication and authorization

The current `/api/auth/demo-login` endpoint returns a development placeholder token.

A production deployment should use a real identity provider such as:

- Keycloak;
- Microsoft Entra ID;
- Auth0;
- another OpenID Connect/OAuth 2.0 provider.

Role and policy-based authorization should protect approval, rejection, administration, and AI endpoints.

### Secrets

Credentials and API keys should never be committed to the repository. Production secrets should be stored in a managed secret system.

### File-upload security

Invoice uploads should be hardened with:

- MIME/type validation;
- file-size limits;
- malware scanning;
- PDF parser hardening;
- encrypted blob storage;
- retention policies.

### Financial controls

For real payment workflows, high-risk actions should include:

- human approval;
- supplier-master validation;
- duplicate invoice detection;
- segregation of duties;
- immutable audit trails;
- approval thresholds;
- idempotency controls.

### Infrastructure

Production infrastructure should use:

- TLS everywhere;
- managed PostgreSQL and Redis where appropriate;
- secured RabbitMQ/Qdrant networks;
- backups and restore testing;
- network policies/firewalls;
- horizontal scaling;
- rate limiting;
- health/readiness checks;
- monitoring and alerting.

### AI safety and evaluation

AI-generated or AI-extracted financial data should not automatically authorize payments.

A production AI pipeline should include:

- provider-specific evaluation datasets;
- extraction confidence thresholds;
- human review;
- structured output validation;
- prompt/version tracking;
- hallucination/error analysis;
- model monitoring;
- PII and data-retention controls.

---

## Current Implementation Notes

To make the repository useful both as a learning project and a runnable local demo, several external-service implementations are intentionally simplified.

### Local document extractor

`OpenAiVisionExtractor` currently acts as a deterministic local fallback rather than calling the OpenAI API. It derives repeatable demo values from the document bytes.

### Azure extractor

`AzureDocumentIntelligenceExtractor` currently delegates to the local fallback. It demonstrates the adapter boundary where the Azure SDK can be connected later.

### Embeddings

`FastEmbedService` uses a deterministic hashing-based embedding algorithm rather than a hosted embedding model.

### Qdrant

`QdrantVectorStore` currently stores vectors in memory and performs cosine similarity locally. Docker Compose provisions Qdrant for development and future real integration.

### pgvector

`PgVectorStore` currently delegates to the fallback vector-store implementation. PostgreSQL is nevertheless provisioned with the pgvector-enabled image, providing the infrastructure foundation for a database-backed vector implementation.

### Fraud model

`OnnxFraudDetector` currently performs rule-based deterministic scoring. The project contains an ONNX model artifact and ONNX Runtime dependency as an integration point for model inference.

### Messaging

RabbitMQ and the MassTransit package are included in the architecture and local infrastructure. `MassTransitEventBus` is currently an extension-point class rather than a complete production event-bus publisher.

### Authentication

The current authentication endpoint is explicitly development-only. A `KeycloakAuthService` integration boundary exists for future OIDC implementation.

These choices keep the project runnable and understandable while preserving the boundaries needed to replace fallbacks with external production services.

---

## Potential Future Improvements

The architecture supports several natural next steps:

- connect a real OpenAI Vision or Azure Document Intelligence provider;
- add schema-validated structured AI extraction;
- execute the included ONNX fraud model through ONNX Runtime;
- persist fraud assessments and expose them through the API;
- replace the in-memory vector store with a real Qdrant client;
- implement native pgvector persistence and similarity queries;
- publish outbox events through MassTransit/RabbitMQ;
- add retry, dead-letter, and idempotent message processing;
- implement real Keycloak/OIDC authentication and authorization;
- add multi-tenant data isolation;
- implement encrypted document storage in Azure Blob Storage or S3;
- add duplicate invoice detection;
- add supplier/vendor master-data matching;
- implement approval thresholds and role-based approval workflows;
- add API versioning;
- add pagination/filtering specifications to larger invoice queries;
- publish OpenTelemetry data to a real observability backend;
- add Prometheus/Grafana dashboards;
- add Kubernetes manifests or Helm charts;
- add database migration execution to deployment pipelines;
- add end-to-end tests against the Docker Compose environment;
- add performance/load testing;
- add AI evaluation and regression tests;
- add code coverage reporting and quality gates.

---

## Why This Project Is Structured This Way

InvoiceFlow Enterprise intentionally goes beyond a basic CRUD example.

The project demonstrates how a backend can evolve when requirements include:

- complex business rules;
- external AI providers;
- multiple databases or storage technologies;
- asynchronous processing;
- reliable messaging;
- testability;
- traceability;
- independent deployment/scaling concerns.

By keeping the Domain and Application layers independent from provider-specific code, external technologies can be replaced with limited impact on business logic.

For example:

```text
IDocumentExtractor
        |
        +--> local deterministic extractor
        +--> OpenAI Vision implementation
        +--> Azure Document Intelligence implementation

IVectorStore
        |
        +--> in-memory development implementation
        +--> Qdrant implementation
        +--> pgvector implementation
```

This dependency-inversion approach is one of the main architectural ideas demonstrated by the repository.

---

## License

See the [`LICENSE`](LICENSE) file included in the repository for licensing information.

---

## Summary

InvoiceFlow Enterprise demonstrates a modern .NET backend architecture for AI-assisted financial document processing. It combines DDD, Clean Architecture, CQRS, PostgreSQL, Redis, vector-search abstractions, AI integration boundaries, background jobs, transactional outbox concepts, observability, automated testing, Docker, and GitHub Actions in one structured solution.

The project is designed to be understandable locally with deterministic fallbacks while still showing how the same architecture can evolve toward real cloud AI services, distributed messaging, vector databases, stronger authentication, and production deployment infrastructure.
