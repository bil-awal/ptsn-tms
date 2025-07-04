#!/bin/bash

echo "==================================="
echo "Security Vulnerability Check"
echo "==================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null
then
    echo -e "${RED}dotnet CLI is not installed!${NC}"
    exit 1
fi

echo "Checking for vulnerable packages..."
echo ""

# Function to check vulnerabilities in a project
check_project() {
    local project_path=$1
    local project_name=$(basename "$project_path" .csproj)
    
    echo -e "${YELLOW}Checking $project_name...${NC}"
    
    cd $(dirname "$project_path")
    
    # Run vulnerability check
    output=$(dotnet list package --vulnerable --include-transitive 2>&1)
    
    if echo "$output" | grep -q "no vulnerable packages"; then
        echo -e "${GREEN}✓ No vulnerabilities found${NC}"
    else
        echo -e "${RED}✗ Vulnerabilities detected:${NC}"
        echo "$output" | grep -A 10 "Top-level Package"
    fi
    
    echo ""
    cd - > /dev/null
}

# Find all project files
echo "Scanning all projects in the solution..."
echo ""

for proj in $(find . -name "*.csproj" -not -path "*/bin/*" -not -path "*/obj/*"); do
    check_project "$proj"
done

echo "==================================="
echo "Outdated Packages Check"
echo "==================================="
echo ""

# Check for outdated packages
for proj in $(find . -name "*.csproj" -not -path "*/bin/*" -not -path "*/obj/*"); do
    project_name=$(basename "$proj" .csproj)
    echo -e "${YELLOW}Checking outdated packages in $project_name...${NC}"
    
    cd $(dirname "$proj")
    dotnet list package --outdated
    echo ""
    cd - > /dev/null
done

echo "==================================="
echo "Security Recommendations:"
echo "==================================="
echo "1. Update all vulnerable packages immediately"
echo "2. Review and update outdated packages"
echo "3. Run 'dotnet restore' after updates"
echo "4. Run 'dotnet test' to ensure compatibility"
echo "5. Commit changes to version control"
echo ""
echo "To update all packages in a project:"
echo "  dotnet add package [PackageName] --version [LatestVersion]"
echo ""
