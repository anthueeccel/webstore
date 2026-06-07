![.Net](http://img.shields.io/badge/-v10.0-008999?style=flat-square&logo=.net&logoColor=ffffff) ![last_commit](https://img.shields.io/github/last-commit/anthueeccel/webstore) ![license](https://img.shields.io/github/license/anthueeccel/webstore)

## Overview

WebStore Management Tool (back-end).

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
        ├── Validators/ (AddressValidator)
        └── Extensions/ (DI + endpoint registration)
├── Domain/
│   ├── Entities/
│   └── Repositories/ (interfaces)
├── Infrastructure/
│   ├── Extensions/
│   ├── Migrations/
│   ├── Persistence/
│   ├── Repositories/
│   └── Seeders/
└── Tests/
    └── Features/
        └── WebStore/
```

#### Why does this matter?

Modularity and the separation of concerns ensure that the code is easy to understand, test, and expand, making the system ready to evolve as business needs grow. Vertical Slices in particular make it easy to navigate the codebase and add new features without touching unrelated code.

## Technologies

- .NET 10
- Entity Framework
- FluentValidation

#### Main Entities

The Entity Model maps to database tables and defines how data is organized, including properties and relationships between different data elements (entities).

- `WebStore` - model for core of the application, the register of the store containing its main data.
- `Address` - store and user/clients address
- `Product` - the items available in the store
- `Brand` - brand of the product
- `Category` - category of the product

#### DTO - Data Transfer Object

A DTO (Data Transfer Object) is a simple object used to transfer data between application layers. It typically holds data without business logic, acting as a container to pass information efficiently.

- `WebStoreDto` - fetch data for http get endpoint
- `ProductDto` - fetch data for http get endpoint
- `WebStoreCreateDto` - Webstore posting endpoint
- `ProductCreateDto` - Product posting endpoint
- `WebStoreUpdateDto` - Webstore updating endpoint
- `ProductUpdateDto` - Product updating endpoint

## Tools

- Github Actions to perform validations and run the tests.
- Visual Studio 2026

## Helpful CLI

- Add nuget package: `dotnet add package <package-name>`
- Create migrations: `dotnet ef migrations add <name>`
