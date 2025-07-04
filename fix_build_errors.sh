#!/bin/bash

# Fix all TaskStatus ambiguity issues in the project

echo "Fixing TaskStatus ambiguity issues..."

# Fix in Controllers
echo "Fixing HomeController.cs..."
sed -i '' 's/TaskStatus\.Todo/TaskManagementSystem.Web.Models.TaskStatus.Todo/g' TaskManagementSystem.Web/Controllers/HomeController.cs
sed -i '' 's/TaskStatus\.InProgress/TaskManagementSystem.Web.Models.TaskStatus.InProgress/g' TaskManagementSystem.Web/Controllers/HomeController.cs
sed -i '' 's/TaskStatus\.Completed/TaskManagementSystem.Web.Models.TaskStatus.Completed/g' TaskManagementSystem.Web/Controllers/HomeController.cs
sed -i '' 's/TaskStatus\.Cancelled/TaskManagementSystem.Web.Models.TaskStatus.Cancelled/g' TaskManagementSystem.Web/Controllers/HomeController.cs

# Fix in Views
echo "Fixing Views..."
find TaskManagementSystem.Web/Views -name "*.cshtml" -type f -exec sed -i '' 's/TaskStatus\.Todo/TaskManagementSystem.Web.Models.TaskStatus.Todo/g' {} \;
find TaskManagementSystem.Web/Views -name "*.cshtml" -type f -exec sed -i '' 's/TaskStatus\.InProgress/TaskManagementSystem.Web.Models.TaskStatus.InProgress/g' {} \;
find TaskManagementSystem.Web/Views -name "*.cshtml" -type f -exec sed -i '' 's/TaskStatus\.Completed/TaskManagementSystem.Web.Models.TaskStatus.Completed/g' {} \;
find TaskManagementSystem.Web/Views -name "*.cshtml" -type f -exec sed -i '' 's/TaskStatus\.Cancelled/TaskManagementSystem.Web.Models.TaskStatus.Cancelled/g' {} \;

echo "All TaskStatus ambiguity issues fixed!"
