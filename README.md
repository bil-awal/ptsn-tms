# Task Management System

A full-stack Task Management System built with .NET 8, featuring a clean architecture backend API and a modern ASP.NET MVC frontend with responsive UI/UX design.

## Features

### Backend (API)
- Create, update, delete, and retrieve tasks
- Assign tasks to users
- Task validation (due dates, required fields)
- Comprehensive logging with Serilog
- Unit tests with xUnit
- RESTful API with Swagger documentation
- In-memory database for easy testing

### Frontend (Web)
- Modern, responsive UI design
- Dark/Light theme support
- Real-time task management
- Interactive dashboard with statistics
- Advanced filtering and search
- User-friendly task creation and editing
- Mobile-responsive design

## Architecture

The solution follows Clean Architecture principles with the following projects:

### Backend
- **Domain**: Core business entities and interfaces
- **Application**: Business logic, DTOs, validators, and services
- **Infrastructure**: Data access implementation, repositories
- **API**: RESTful endpoints, dependency injection configuration

### Frontend
- **Web**: ASP.NET MVC application with modern UI/UX
  - Controllers for handling requests
  - Services for API communication
  - Responsive views with Bootstrap 5
  - Custom CSS for modern styling

## Running the Application

### Prerequisites
- .NET 8 SDK or later
- Visual Studio 2022 or VS Code (optional)

### Running the Backend API
1. Navigate to the API directory: `cd TaskManagementSystem.API`
2. Run: `dotnet run`
3. The API will start on `https://localhost:5001`

### Running the Frontend Web Application
1. Navigate to the Web directory: `cd TaskManagementSystem.Web`
2. Run: `dotnet run`
3. Open your browser and navigate to `https://localhost:7002`

### Quick Start
Run the setup script to start both applications: `./setup.sh`

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

### Backend
- .NET 8
- Entity Framework Core (In-Memory)
- Serilog for logging
- FluentValidation
- xUnit for testing
- Moq for mocking
- FluentAssertions
- Swagger/OpenAPI

### Frontend
- ASP.NET Core MVC
- Bootstrap 5
- Font Awesome icons
- jQuery
- Custom modern CSS
- Responsive design principles
