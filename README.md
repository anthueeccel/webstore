![.Net](http://img.shields.io/badge/-v10.0-008999?style=flat-square&logo=.net&logoColor=ffffff) ![last_commit](https://img.shields.io/github/last-commit/anthueeccel/webstore) ![license](https://img.shields.io/github/license/anthueeccel/webstore)

## Overview
WebStore Management Tool (back-end).

### Clean Architecture
This API follows the principles of Clean Architecture to ensure it is modular, testable, and scalable.

#### Design Patterns
CQRS (Command Query Responsibility Segregation): The application implements the CQRS pattern using MediatR to decouple read and write operations. This ensures that the application layer remains clean and that business logic is encapsulated within specific Commands and Queries.

```
Project/
├── API/    
│   └── Controllers/
├── Application/
│   ├── Commands/
│	├── Dtos/
│	├── Extensions/
│   └── Services/
├── Domain/
│	├── Entities/
│	└── Repositories/ (interfaces)
├── Infrastructure/
│   ├── Extensions/
│	├── Migrations/
│	├── Persistence/
│	├── Repositories/
│   └── Seeders/
└── Tests/    
```

#### Why does this matter?
Modularity and the separation of concerns ensure that the code is easy to understand, test, and expand, making the system ready to evolve as business needs grow.

## Technologies
* .NET 10
* Entity Framework
* FluentValidation
* MediatR

#### Main Entities
The Entity Model maps to database tables and defines how data is organized, including properties and relationships between different data elements (entities).
* `WebStore` - model for core of the application, the register of the store containing its main data. 
* `Address` - store and user/clients address
* `Product` - the items available in the store
* `Brand` - brand of the product
* `Category` - category of the product

#### DTO - Data Transfer Object
A DTO (Data Transfer Object) is a simple object used to transfer data between application layers. It typically holds data without business logic, acting as a container to pass information efficiently.
* `WebStoreDto` - fetch data for http get endpoint
* `ProductDto` - fetch data for http get endpoint
* `WebStoreCreateDto` - Webstore posting endpoint
* `ProductCreateDto` - Product posting endpoint
* `WebStoreUpdateDto` - Webstore updating endpoint
* `ProductUpdateDto` - Product updating endpoint

## Tools
* Github Actions to perform validations and run the tests.
* Visual Studio 2026

## Helpful CLI
* Add nuget package: `dotnet add package <package-name>`
* Create migrations: `dotnet ef migrations add <name>`
