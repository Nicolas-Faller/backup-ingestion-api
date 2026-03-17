<div align="center">

# Backup Ingestion API

A .NET 8 Web API for ingesting backup execution data from multiple sources, normalizing it into a single model, storing it in SQLite, and exposing query, summary, and Excel export endpoints.

</div>

---

## Overview

**Backup Ingestion API** is a backend-focused portfolio project built to simulate a real-world operational scenario where backup execution data arrives in different formats and needs to be consolidated into a single, queryable source of truth.

The API currently supports:

- JSON import
- CSV import
- HTML import
- persisted storage with SQLite
- filtered and paginated queries
- aggregated summary endpoint
- Excel export

This project was designed to highlight practical backend skills such as data ingestion, parsing, normalization, persistence, filtering, aggregation, and file generation.

---

## Features

- Import backup executions from **JSON**
- Import backup executions from **CSV**
- Import backup executions from **HTML**
- Normalize all sources into a common domain model
- Store data using **Entity Framework Core** with **SQLite**
- Query backups with filters and pagination
- Retrieve a single backup execution by id
- Get aggregated summary data
- Export filtered backup data to **Excel (.xlsx)**

---

## Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite**
- **CsvHelper**
- **Html Agility Pack**
- **ClosedXML**
- **Swagger / OpenAPI**

---

## Project Structure

```text
backup-ingestion-api/
├── src/
│   ├── BackupIngestion.Api/
│   ├── BackupIngestion.Application/
│   ├── BackupIngestion.Domain/
│   └── BackupIngestion.Infrastructure/
├── tests/
│   ├── BackupIngestion.UnitTests/
│   └── BackupIngestion.IntegrationTests/
├── samples/
│   ├── json/
│   ├── csv/
│   └── html/
└── README.md
```

## Architecture

### Domain
Core entities and enums.

### Application
Contracts, DTOs, and use-case-oriented services.

### Infrastructure
Persistence, parsers, and Excel export implementation.

### API
HTTP endpoints, request/response contracts, and Swagger configuration.

---

## Main Use Case

Different operational systems may expose backup execution data in different formats.  
This API ingests those heterogeneous inputs, transforms them into a unified structure, and makes them available for querying and reporting.

This allows consumers to:

- consolidate execution data from multiple sources
- filter results by client, status, source type, and date range
- inspect summary metrics
- export operational data to Excel

---

## Domain Model

The central entity is `BackupExecution`.

Main fields include:

- `Id`
- `ClientName`
- `JobName`
- `SourceType`
- `Status`
- `StartedAtUtc`
- `FinishedAtUtc`
- `DurationSeconds`
- `DataSizeBytes`
- `BackupSizeBytes`
- `TransferredSizeBytes`
- `Message`
- `ImportedAtUtc`

---

## API Endpoints

### Import

#### Import JSON
`POST /api/Import/json`

Uploads a JSON file and imports backup execution records.

#### Import CSV
`POST /api/Import/csv`

Uploads a CSV file and imports backup execution records.

#### Import HTML
`POST /api/Import/html`

Uploads an HTML file and imports backup execution records.

---

### Query

#### Get backups
`GET /api/Backups`

Supports filtering and pagination.

Available query parameters:

- `clientName`
- `status`
- `sourceType`
- `startDate`
- `endDate`
- `page`
- `pageSize`

Example:

```http
GET /api/Backups?status=Success&page=1&pageSize=10
```

#### Get backup by id
`GET /api/Backups/{id}`

Returns a single backup execution by identifier.

#### Get summary
`GET /api/Backups/summary`

Returns aggregated information such as:

- total backups
- total unique clients
- total transferred bytes
- average duration
- counts by status
- counts by source type

---

### Export

#### Export Excel
`GET /api/Exports/excel`

Exports backup data to an Excel file.

Supports the same filtering parameters used by `GET /api/Backups`.

Example:

```http
GET /api/Exports/excel?status=Success&sourceType=Html
```

---

## Sample Files

Sample files are available under the `samples/` directory:

- `samples/json/sample-backups.json`
- `samples/csv/sample-backups.csv`
- `samples/html/sample-backups.html`

These can be used directly in Swagger for testing the import endpoints.

---

## Running the Project

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Build the solution

```bash
dotnet build
```

### 3. Apply database migrations

```bash
dotnet ef database update \
  --project src/BackupIngestion.Infrastructure \
  --startup-project src/BackupIngestion.Api
```

### 4. Run the API

#### Standard
```bash
dotnet run --project src/BackupIngestion.Api
```

#### WSL-friendly
```bash
dotnet run --project src/BackupIngestion.Api --urls "http://0.0.0.0:5013"
```

---

## Swagger

When running locally, Swagger is available at:

```text
/swagger
```

Examples:

- `http://localhost:5013/swagger`
- `http://<your-host>:5013/swagger`

---

## Example Workflow

1. Import a JSON sample
2. Import a CSV sample
3. Import an HTML sample
4. Query all backups through `GET /api/Backups`
5. Filter by `status` or `sourceType`
6. Check aggregated data through `GET /api/Backups/summary`
7. Export filtered results to Excel through `GET /api/Exports/excel`

---

## Example Summary Response

```json
{
  "totalBackups": 6,
  "totalClients": 6,
  "totalTransferredBytes": 11274289152,
  "averageDurationSeconds": 577.5,
  "statusCounts": [
    { "status": "Success", "count": 3 },
    { "status": "Warning", "count": 2 },
    { "status": "Failed", "count": 1 }
  ],
  "sourceTypeCounts": [
    { "sourceType": "Json", "count": 2 },
    { "sourceType": "Csv", "count": 2 },
    { "sourceType": "Html", "count": 2 }
  ]
}
```

---

## Why This Project

This project was built to demonstrate practical backend development skills in a realistic scenario, including:

- API design
- file ingestion workflows
- parsing structured and semi-structured inputs
- data normalization
- relational persistence
- filtering and pagination
- aggregation queries
- file export generation

---

## Future Improvements

Possible next steps for this project:

- deduplication rules during import
- validation layer for uploaded files
- unit and integration test coverage
- Docker support
- authentication and authorization
- PostgreSQL support
- background processing for large imports
- import batch tracking and history

---

## Repository Goals

This repository is part of a portfolio focused on:

- backend development with C# / .NET
- APIs and integrations
- operational data processing
- automation-oriented software solutions

---

## Author

Built by **Nicolas** as part of a backend portfolio in **C# / .NET**.
