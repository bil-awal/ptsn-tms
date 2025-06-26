# Task Management System

A clean architecture implementation of a Task Management System built with .NET 8, following SOLID principles.

## Features

- Create, update, delete, and retrieve tasks
- Assign tasks to users
- Task validation (due dates, required fields)
- Comprehensive logging
- Unit tests
- RESTful API
- In-memory database for easy testing

## Architecture

The solution follows Clean Architecture principles with the following layers:

- **Domain**: Core business entities and interfaces
- **Application**: Business logic, DTOs, validators, and services
- **Infrastructure**: Data access implementation, repositories
- **API**: RESTful endpoints, dependency injection configuration

## Running the Application

1. Ensure you have .NET 8 SDK or later installed
2. Run the setup script: `./setup.sh`
3. The API will start on `https://localhost:7001` and `http://localhost:5000`

## API Endpoints

### Tasks
- `POST /api/tasks` - Create a new task
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get a specific task
- `GET /api/tasks/user/{userId}` - Get tasks by user
- `PUT /api/tasks/{id}` - Update a task
- `DELETE /api/tasks/{id}` - Delete a task

### Users
- `POST /api/users` - Create a new user
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get a specific user

## Testing

Run unit tests with: `dotnet test`

## SOLID Principles Implementation

- **Single Responsibility**: Each class has a single, well-defined purpose
- **Open/Closed**: Services and repositories use interfaces for extensibility
- **Liskov Substitution**: Repository implementations can be substituted
- **Interface Segregation**: Specific interfaces for different concerns
- **Dependency Inversion**: High-level modules depend on abstractions

## Technologies Used

- .NET 8
- Entity Framework Core (In-Memory)
- Serilog for logging
- FluentValidation
- xUnit for testing
- Moq for mocking
- FluentAssertions
