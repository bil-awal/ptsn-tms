# Task Management System - ASP.NET MVC Frontend

## Overview

This is the modern web frontend for the Task Management System, built with ASP.NET Core MVC. It provides a beautiful and responsive user interface for managing tasks and users.

## Features

- **Modern UI/UX Design**
  - Responsive design that works on all devices
  - Dark/Light theme support
  - Smooth animations and transitions
  - Clean and intuitive interface

- **Dashboard**
  - Overview of task statistics
  - Recent tasks display
  - Quick access to important metrics

- **Task Management**
  - Create, view, edit, and delete tasks
  - Filter tasks by status and priority
  - Search functionality
  - Task assignment to users
  - Priority levels: Low, Medium, High, Urgent
  - Status tracking: Todo, In Progress, Completed, Cancelled

- **User Management**
  - Create and view users
  - See user-specific task assignments
  - User task statistics

## Prerequisites

- .NET 8.0 SDK
- The backend API running (TaskManagementSystem.API)

## Getting Started

1. **Start the Backend API**
   ```bash
   cd ../TaskManagementSystem.API
   dotnet run
   ```
   The API should be running on `https://localhost:5001`

2. **Configure API URL**
   If your API is running on a different port, update the `appsettings.json`:
   ```json
   "ApiSettings": {
     "BaseUrl": "https://localhost:5001/"
   }
   ```

3. **Run the Web Application**
   ```bash
   cd TaskManagementSystem.Web
   dotnet run
   ```
   The web application will start on `https://localhost:7002`

4. **Open in Browser**
   Navigate to `https://localhost:7002` in your web browser

## Technology Stack

- **ASP.NET Core 8.0 MVC**
- **Bootstrap 5** - CSS Framework
- **Font Awesome** - Icons
- **jQuery** - JavaScript Library
- **Chart.js** - For future dashboard charts
- **Custom CSS** - Modern UI styling

## Project Structure

```
TaskManagementSystem.Web/
├── Controllers/          # MVC Controllers
├── Models/              # View Models
├── Services/            # API Service Clients
├── Views/               # Razor Views
│   ├── Home/           # Dashboard views
│   ├── Tasks/          # Task management views
│   ├── Users/          # User management views
│   └── Shared/         # Shared layouts and partials
├── wwwroot/            # Static files
│   ├── css/           # Stylesheets
│   └── js/            # JavaScript files
└── Program.cs          # Application entry point
```

## Key Features Implementation

### Theme Support
- Toggle between light and dark themes
- Theme preference saved in browser localStorage

### Responsive Design
- Mobile-first approach
- Adaptive layouts for different screen sizes
- Touch-friendly interface

### Real-time Feedback
- Loading states for all actions
- Success/error notifications
- Form validation feedback

### Enhanced UX
- Smooth animations
- Hover effects
- Interactive elements
- Tooltips for better guidance

## Development Tips

1. **Adding New Views**
   - Follow the existing pattern in Views folder
   - Use the shared layout for consistency
   - Implement responsive design principles

2. **Styling**
   - Custom CSS variables in `site.css`
   - Bootstrap utility classes for quick styling
   - Consistent color scheme using CSS variables

3. **API Integration**
   - Services are registered in `Program.cs`
   - All API calls go through service classes
   - Error handling implemented in services

## Future Enhancements

- Real-time updates using SignalR
- Advanced filtering and sorting
- Bulk operations on tasks
- Export functionality (PDF, Excel)
- Calendar view for tasks
- Notifications system
- User authentication and authorization
- Activity logs and audit trails

## Troubleshooting

- **API Connection Issues**: Ensure the backend API is running and the URL in `appsettings.json` is correct
- **Theme Not Saving**: Check if browser localStorage is enabled
- **Validation Errors**: Ensure all required fields are filled correctly

## License

This project is part of the Task Management System and follows the same license terms.
