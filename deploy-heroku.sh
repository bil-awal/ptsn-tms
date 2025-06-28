#!/bin/bash

# Deploy script for TaskManagementSystem to Heroku
# Usage: ./deploy-heroku.sh

echo "🚀 Starting Heroku deployment process..."

# Check if Heroku CLI is installed
if ! command -v heroku &> /dev/null; then
    echo "❌ Heroku CLI is not installed. Please install it first."
    echo "Visit: https://devcenter.heroku.com/articles/heroku-cli"
    exit 1
fi

# Configuration
APP_NAME="PT SN - Task Management System"
GITHUB_REPO="ptsn-tms"

echo "📋 Deployment Configuration:"
echo "   App Name: $APP_NAME"
echo "   GitHub Repo: $GITHUB_REPO"
echo ""

# Function to check if app exists
check_app_exists() {
    heroku apps:info -a $APP_NAME &> /dev/null
    return $?
}

# Step 1: Login to Heroku
echo "1️⃣ Logging in to Heroku..."
heroku login

# Step 2: Create app if it doesn't exist
if check_app_exists; then
    echo "✅ App $APP_NAME already exists"
else
    echo "2️⃣ Creating Heroku app..."
    heroku create $APP_NAME
fi

# Step 3: Set stack to container
echo "3️⃣ Setting stack to container..."
heroku stack:set container -a $APP_NAME

# Step 4: Add git remote if not exists
if ! git remote | grep heroku > /dev/null; then
    echo "4️⃣ Adding Heroku git remote..."
    heroku git:remote -a $APP_NAME
else
    echo "✅ Heroku remote already exists"
fi

# Step 5: Set environment variables
echo "5️⃣ Setting environment variables..."
heroku config:set ASPNETCORE_ENVIRONMENT=Production -a $APP_NAME

# Step 6: Check required files
echo "6️⃣ Checking required files..."
REQUIRED_FILES=("Dockerfile" ".dockerignore" "heroku.yml" "Procfile")
MISSING_FILES=()

for file in "${REQUIRED_FILES[@]}"; do
    if [ ! -f "$file" ]; then
        MISSING_FILES+=("$file")
    fi
done

if [ ${#MISSING_FILES[@]} -ne 0 ]; then
    echo "❌ Missing required files:"
    printf '   - %s\n' "${MISSING_FILES[@]}"
    echo "Please create these files before deploying."
    exit 1
else
    echo "✅ All required files present"
fi

# Step 7: Build and test locally
echo "7️⃣ Building Docker image locally for testing..."
docker build -t $APP_NAME-test . || {
    echo "❌ Docker build failed. Please fix errors before deploying."
    exit 1
}

echo "✅ Docker build successful"

# Step 8: Commit changes
echo "8️⃣ Checking git status..."
if [[ -n $(git status -s) ]]; then
    echo "📝 Uncommitted changes detected. Committing..."
    git add .
    git commit -m "Update Heroku deployment configuration"
fi

# Step 9: Deploy to Heroku
echo "9️⃣ Deploying to Heroku..."
echo "This may take several minutes..."
git push heroku main || git push heroku master

# Step 10: Check deployment status
echo "🔍 Checking deployment status..."
heroku ps -a $APP_NAME

# Step 11: Open app
echo "🌐 Opening app in browser..."
heroku open -a $APP_NAME

# Step 12: Show logs
echo "📜 Showing recent logs..."
heroku logs --tail -n 50 -a $APP_NAME

echo ""
echo "✅ Deployment process complete!"
echo ""
echo "📌 Useful commands:"
echo "   - View logs: heroku logs --tail -a $APP_NAME"
echo "   - Restart app: heroku restart -a $APP_NAME"
echo "   - Check status: heroku ps -a $APP_NAME"
echo "   - Open app: heroku open -a $APP_NAME"
echo "   - Run bash: heroku run bash -a $APP_NAME"
echo ""
echo "🔗 Your app URL: https://$APP_NAME.herokuapp.com"