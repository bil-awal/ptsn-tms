#!/bin/bash

echo "Building the solution..."
dotnet build

if [ $? -eq 0 ]; then
    echo "Build successful!"
    
    # Function to start API in background
    start_api() {
        echo "Starting the API on https://localhost:5001..."
        cd TaskManagementSystem.API
        dotnet run &
        API_PID=$!
        cd ..
    }
    
    # Function to start Web in background
    start_web() {
        echo "Starting the Web application on https://localhost:7002..."
        cd TaskManagementSystem.Web
        dotnet run &
        WEB_PID=$!
        cd ..
    }
    
    # Start both applications
    start_api
    sleep 5  # Give API time to start
    start_web
    
    echo ""
    echo "======================================"
    echo "Task Management System is running!"
    echo "API: https://localhost:5001"
    echo "Web: https://localhost:7002"
    echo "Press Ctrl+C to stop both applications"
    echo "======================================"
    echo ""
    
    # Wait for user to press Ctrl+C
    trap "echo 'Stopping applications...'; kill $API_PID $WEB_PID; exit" INT
    wait
else
    echo "Build failed!"
    exit 1
fi
