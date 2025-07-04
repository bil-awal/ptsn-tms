#!/bin/bash
echo "=== Fixing TaskManagementSystem Build Errors ==="
echo ""

# Navigate to project directory
cd "/Users/bilawalrizky/Documents/2025 - Project/SAT/TaskManagementSystem"

echo "1. Running Python script to fix all TaskStatus references..."
python3 fix_all_errors.py

echo ""
echo "2. Updating NuGet packages to fix security vulnerability..."
dotnet add TaskManagementSystem.Web/TaskManagementSystem.Web.csproj package Refit --version 7.2.22

echo ""
echo "3. Cleaning and rebuilding the solution..."
dotnet clean
dotnet restore
dotnet build

echo ""
echo "=== Build Error Fixes Complete ==="
echo ""
echo "Summary of fixes applied:"
echo "- Fixed Razor syntax error in Views/Users/Create.cshtml"
echo "- Fixed all TaskStatus ambiguity errors by using fully qualified names"
echo "- Updated Refit package from 7.2.0 to 7.2.22 to fix security vulnerability"
echo ""
echo "Please run 'dotnet build' to verify all errors are resolved."
