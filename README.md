![.Net](http://img.shields.io/badge/-9-008999?style=flat-square&logo=.net&logoColor=ffffff) ![last_commit](https://img.shields.io/github/last-commit/anthueeccel/webstore) ![license](https://img.shields.io/github/license/anthueeccel/webstore)

## Overview
WebStore Management Tool

## Clean Architecture
This API follows the principles of Clean Architecture to ensure it is modular, testable, and scalable.

```
Project/
├── Domain/
└── Application/
    ├── Infraestructure/
    └── API/
```

### Why does this matter?
Modularity and the separation of concerns ensure that the code is easy to understand, test, and expand, making the system ready to evolve as business needs grow.

## More Technical Information
AutoMapper was not used here once it is going commercial.
Entity data validations are mannually once FluentValidation is going commercial.

### Technologies Used:
* .NET 9
* Entity Framework

### Main Entities
The Entity Model maps to database tables and defines how data is organized, including properties and relationships between different data elements (entities).
* WebStore --> model for core of the application, the register of the store containing its main data. 
* Address --> store and user/clients address
* Product --> the items available in the store
* Brand --> brand of the product
* Category --> category of the product

### DTO - Data Transfer Object
A DTO (Data Transfer Object) is a simple object used to transfer data between application layers. It typically holds data without business logic, acting as a container to pass information efficiently.
* WebStoreDto - fetch data for http get endpoint
* ProductDto - fetch data for http get endpoint
* WebStoreCreateDto - Webstore posting endpoint

### Helpful CLI
* Add nuget package: `dotnet add package <package-name>`
* Create migrations: `dotnet ef migrations add <name>`


### Exceptions Handled Globally

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
public async ValueTask<bool> TryHandleAsync(
HttpContext httpContext,
Exception, exception,
CanellationToken cancellationToken)
{
var exceptionMessage = exception.Message;
logger.LogError(exception, "Error Message: {@ExceptionMessage}", exceptionMessage);

var problemDetails = new ProblmeDetails
{
Status = statusCodes.Status500InternalServerError,
Title = "Server Error: " + exceptionMessage,
Instance = httpContext.Request.Path,
};

httpContext.Response.StatusCode = problemDetails.Status.Value;

await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

return true;
}
}
