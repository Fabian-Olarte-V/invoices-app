# Invoices App

Full-stack technical project built to demonstrate backend engineering fundamentals, SQL Server integration, and a reproducible local environment with Docker.

## What This Project Demonstrates

- Layered backend structure inspired by Clean Architecture
- Command/query separation with `MediatR`
- Input validation with `FluentValidation`
- Centralized exception handling with custom middleware
- SQL Server integration through `Stored Procedures`
- Automatic database creation, schema setup, and seed data loading at startup
- End-to-end local setup with `Docker Compose`
- Functional Angular frontend focused on consuming the API and completing the invoice flow

## Scope

The application supports:

- Listing clients
- Listing products
- Creating invoices
- Searching invoices by invoice number
- Searching invoices by client

## Architecture Overview

### Backend

The backend is split into four projects:

- `Domain`: core entities and domain contracts
- `Application`: use cases, DTOs, validation, and application behaviors
- `Infrastructure`: SQL Server access, stored procedure execution, database initialization, and technical wiring
- `WebApp`: HTTP API, middleware, and application bootstrap

This is not presented as a strict textbook implementation of Clean Architecture. The goal was to apply the main ideas in a pragmatic way:

- separation between business flow and infrastructure concerns
- inward-facing dependencies
- persistence behind abstractions
- composition rooted in the API layer

### Frontend

The frontend was intentionally kept simple.

The goal was not to build a highly abstracted UI architecture or advanced client-side state management. The focus was:

- API integration
- reactive forms
- invoice creation flow
- clear, working UI

This was a deliberate trade-off to keep the project centered on backend design and end-to-end delivery.

## Database and Stored Procedures

The project uses `SQL Server` and relies on `Stored Procedures` for data access.

On application startup, the backend:

1. checks the database connection
2. creates the target database if it does not exist
3. runs the table creation script
4. runs the stored procedure scripts
5. inserts seed data

This allows the project to be started from scratch without manual database setup.

## Tech Stack

- `.NET 8`
- `ASP.NET Core Web API`
- `Angular`
- `SQL Server 2022`
- `Docker`
- `Docker Compose`
- `MediatR`
- `FluentValidation`

## Running the Project

### Requirements

- Docker
- Docker Compose

### Start

From the repository root:

```bash
docker compose up --build
```

Exposed services:

- Frontend: `http://localhost`
- Backend: `http://localhost:8080`
- SQL Server: `localhost:1433`

## Environment Setup

The `.env` file contains the database host, port, credentials, and database name used by `docker compose`.

## Notes

- The database is recreated on startup while `ResetOnStartup` is enabled
- This project is intended as a technical showcase, not a production-ready system
- The strongest emphasis is on backend structure, SQL Server integration, and environment automation
- The frontend is intentionally lightweight and functional rather than architecturally complex