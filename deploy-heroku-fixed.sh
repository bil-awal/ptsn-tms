#!/bin/bash

# Heroku Deployment Script for TaskManagementSystem
# Usage: ./deploy-heroku-fixed.sh [api|web]

PROJECT_TYPE=${1:-api}
APP_NAME="ptsn-task-management-system"

echo "Deploying TaskManagementSystem ($PROJECT_TYPE) to Heroku..."

# Ensure we're in the right directory
if [ ! -f "TaskManagementSystem.sln" ]; then
    echo "Error: Must run from TaskManagementSystem root directory"
    exit 1
fi

# Configure Procfile based on deployment type
if [ "$PROJECT_TYPE" = "web" ]; then
    echo "Configuring for Web deployment..."
    cp Procfile.web Procfile
    echo "Note: Make sure the API is deployed separately or update appsettings.Production.json with the correct API URL"
else
    echo "Configuring for API deployment..."
    # Use the default Procfile which is already configured for API
fi

# Add all changes
git add -A

# Commit changes
git commit -m "Configure for $PROJECT_TYPE deployment to Heroku" || echo "No changes to commit"

# Push to Heroku
echo "Pushing to Heroku..."
git push heroku main

# Scale the web dyno
echo "Scaling web dyno..."
heroku ps:scale web=1 --app $APP_NAME

# Check deployment status
echo "Checking deployment status..."
heroku ps --app $APP_NAME

# Show logs
echo "Recent logs:"
heroku logs --tail -n 50 --app $APP_NAME

# Open the app
if [ "$PROJECT_TYPE" = "api" ]; then
    echo "API deployed. Swagger UI should be available at: https://$APP_NAME-88f1ef338f0f.herokuapp.com/"
else
    echo "Web app deployed at: https://$APP_NAME-88f1ef338f0f.herokuapp.com/"
fi

echo "Deployment complete!"