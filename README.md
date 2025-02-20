
## Overview

MyApp is a robust and scalable application designed using modern design patterns and best practices. It leverages a clean architecture to ensure separation of concerns, maintainability, and testability. The application is built with **.NET 8** and **C# 12.0**, and it includes features such as product management, user authentication, health checks, and global exception handling.

---

## Features

- **Product Management**: CRUD operations for products.
- **User Authentication**: User registration and login functionality.
- **Health Checks**: Custom health checks for monitoring the application's health.
- **Global Exception Handling**: Middleware for handling exceptions globally.

---

## Architecture

The application follows a **Clean Architecture** pattern, which is divided into several layers:

1. **Core**: Contains the domain entities, interfaces, and specifications.
2. **Application**: Contains the business logic, including commands, queries, and handlers.
3. **Infrastructure**: Contains the data access logic, including the database context, repositories, and configurations.
4. **API**: Contains the presentation layer, including controllers and middleware.

---

### Core Layer

- **Entities**: Defines the domain entities such as `Product` and `BaseEntity`.
- **Interfaces**: Defines the repository interfaces such as `IAsyncBaseRepository` and `IProductRepository`.
- **Specifications**: Defines the specifications for querying data, such as `PaginatedSpecsParams` and `Pagination`.

---

### Application Layer

- **Commands**: Defines the commands for creating, updating, and deleting products, such as `CreateProductCommand` and `UpdateProductCommand`.
- **Queries**: Defines the queries for retrieving products, such as `GetAllProductQuery` and `GetProductByIdQuery`.
- **Handlers**: Implements the command and query handlers, such as `CreateProductCommandHandler` and `GetProductByIdQueryHandler`.
- **Exceptions**: Defines custom exceptions, such as `NotFoundException`.

---

### Infrastructure Layer

- **Data**: Contains the database context (`AppDbContext`) and seed data (`AppDbContextSeed`).
- **Repositories**: Implements the repository pattern for data access, such as `ProductRepository` and `RepositoryBase`.
- **Configuration**: Contains the entity configurations and application settings.
- **Utils**: Contains utility classes, such as health checks (`DatabaseHealthCheck`).

---

### API Layer

- **Controllers**: Implements the API controllers for handling HTTP requests, such as `ProductController` .
- **Middleware**: Implements middleware for global exception handling (`GlobalExceptionHandling`).
- **Extensions**: Contains extension methods for configuring services and middleware.

---

## Design Patterns and Best Practices

- **Repository Pattern**: Used for data access to abstract the database operations.
- **CQRS (Command Query Responsibility Segregation)**: Separates the read and write operations to improve scalability and maintainability.
- **Dependency Injection**: Used to inject dependencies and manage the application's services.
- **Exception Handling**: Global exception handling middleware to manage exceptions consistently.
- **Health Checks**: Custom health checks to monitor the application's health.

---
