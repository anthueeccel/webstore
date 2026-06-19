![.Net](http://img.shields.io/badge/-v10.0-008999?style=flat-square&logo=.net&logoColor=ffffff) [![build-test-and-deploy](https://github.com/anthueeccel/webstore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/anthueeccel/webstore/actions/workflows/dotnet.yml) ![last_commit](https://img.shields.io/github/last-commit/anthueeccel/webstore) ![license](https://img.shields.io/github/license/anthueeccel/webstore)

## Overview

WebStore Management Tool (back-end) — a study project focused on **CI/CD with GitHub Actions (build-test-deploy)** and **Spec-Driven Development (AI-Assisted)**.

### Spec-Driven Development (AI-Assisted)

The API is developed using a **spec-first** approach. **Bruno API collections** (`WebStore-Bruno.zip`) serve as the contract and specification for each endpoint. Before writing implementation code, endpoints are defined, tested, and validated through Bruno — ensuring the API contract is clear and testable from the start.

AI assists throughout the workflow by:

- Generating and evolving API specs alongside the code.
- Validating that implementations match the defined contract.
- Accelerating test writing and edge-case discovery based on the spec.

This approach keeps the API contract as the single source of truth, making development more predictable and collaboration more effective.

### CI/CD with GitHub Actions

The project uses a unified CI/CD pipeline defined in `.github/workflows/dotnet.yml` (Terraform), triggered on every pull request to `main`.

#### Continuous Integration & Continous Delivery Details

| Stage                 | Description                                                                      |
| --------------------- | -------------------------------------------------------------------------------- |
| **Build**             | Restore dependencies and build the solution                                      |
| **Unit Tests**        | Run unit tests (filter: `Category=Unit`)                                         |
| **Integration Tests** | Run against a SQL Server 2022 container (filter: `Category=Integration`)         |
| **E2E Tests**         | Run end-to-end tests against the containerized database (filter: `Category=E2E`) |
| **Create Artifact**   | Publish the .NET build as a deployment artifact                                  |
| **Deploy to Azure**   | Deploy the application to **Azure App Service**                                  |


### Technology Stack

- **.NET 10** — ASP.NET Core Minimal API
- **Entity Framework Core** — ORM and data access
- **FluentValidation** — Input validation
- **PostgreSQL** — database 
- **Github Actions** — CI/CD
- **Bruno** — Endpoint testing
- **Azure App Service** — Deployment target

---

## Architecture

### Vertical Slice Architecture

This API follows the principles of **Vertical Slice Architecture**, organizing code by feature rather than by layer. Each feature (e.g., `CreateWebStore`, `GetProduct`) is self-contained within its own folder, containing its endpoint, handler, validator, and response. This approach keeps the codebase modular, testable, and scalable while also aligning with Clean Architecture principles at the project level.

#### Design Patterns

- **CQRS (Command Query Responsibility Segregation)**: The application implements the CQRS pattern to decouple read and write operations. Commands handle data mutations (create, update, delete) while Queries handle data retrieval, keeping business logic encapsulated within specific handlers.
- **Vertical Slices**: Features are organized vertically — each slice runs through all layers (API → Handler → Domain → Infrastructure) within a single folder, making features easy to add, modify, or remove independently.

```
└── API/
    ├── Program.cs (Minimal API endpoint registration)
    ├── Features/
    │   ├── WebStore/
    │   │   ├── CreateWebStore/ (Endpoint, Handler, Validator, Response)
    │   │   ├── UpdateWebStore/
    │   │   ├── GetWebStore/
    │   │   ├── GetAllWebStores/
    │   │   └── DeleteWebStore/
    │   ├── Product/
    │   │   ├── CreateProduct/
    │   │   ├── UpdateProduct/
    │   │   ├── GetProduct/
    │   │   ├── GetProductsByWebStore/
    │   │   └── DeleteProduct/
    │   ├── Category/
    │   │   ├── GetAllCategories/
    │   │   └── GetCategoryById/
    │   └── Brand/
    │       ├── GetAllBrands/
    │       └── GetBrandById/
    └── Shared/
        ├── Dtos/
        ├── Extensions/ (DI + endpoint registration)
        └── Validators/ (AddressValidator)
├── Domain/
│   └── Entities/
│       ├── Address.cs
│       ├── BaseEntity.cs
│       ├── Brand.cs
│       ├── Category.cs
│       ├── Product.cs
│       └── WebStore.cs
├── Infrastructure/
│   ├── Extensions/
│   ├── Migrations/
│   ├── Persistence/
│   └── Seeders/
└── Tests/
    ├── Features/
    │   ├── Brand/
    │   ├── Category/
    │   ├── Product/
    │   └── WebStore/
    └── IntegrationTests/
```

#### Why does this matter?

Modularity and the separation of concerns ensure that the code is easy to understand, test, and expand, making the system ready to evolve as business needs grow. Vertical Slices in particular make it easy to navigate the codebase and add new features without touching unrelated code.

#### Main Entities

The Entity Model maps to database tables and defines how data is organized, including properties and relationships between different data elements (entities).

- `WebStore` - model for core of the application, the register of the store containing its main data.
- `Address` - store and user/clients address
- `Product` - the items available in the store
- `Brand` - brand of the product
- `Category` - category of the product

#### DTO - Data Transfer Object

A DTO (Data Transfer Object) is a simple object used to transfer data between application layers. It typically holds data without business logic, acting as a container to pass information efficiently.

## Tools

- **GitHub Actions** — CI/CD pipeline (build, test, and deploy)
- **Azure App Service** — Cloud deployment target
- **Bruno** — Endpoint testing
- **VS Code** — using Cline extension as AI assistant for Spec-Driven Development
- **Neon** — After deployment database hosted in [Neon](https://neon.com/)

## Helpful CLI

- Add nuget package: `dotnet add package <package-name>`
- Create migrations: `dotnet ef migrations add <name>`
- Create Db in Docker: `docker run --name webstore-postgres-local -e POSTGRES_DB=<DbName> -e POSTGRES_USER=<DbAdminUser> -e POSTGRES_PASSWORD=<DbPassword> -p 5432:5432 -d postgres:16-alpine`
- Create Github secret for Azure deployment: `az ad sp create-for-rbac --name "GitHub-Actions-App-Deploy" --role contributor --scopes /subscriptions/<YourAppId>/resourceGroups/<ResourceGroupName> --json-auth`
