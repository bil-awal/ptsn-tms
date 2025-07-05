# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TaskManagementSystem.sln ./
COPY TaskManagementSystem.Domain/*.csproj ./TaskManagementSystem.Domain/
COPY TaskManagementSystem.Application/*.csproj ./TaskManagementSystem.Application/
COPY TaskManagementSystem.Infrastructure/*.csproj ./TaskManagementSystem.Infrastructure/
COPY TaskManagementSystem.API/*.csproj ./TaskManagementSystem.API/
COPY TaskManagementSystem.Tests/*.csproj ./TaskManagementSystem.Tests/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build the application
RUN dotnet publish TaskManagementSystem.API/TaskManagementSystem.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=build /app/publish .

# Heroku assigns a dynamic port
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

# The PORT environment variable is set by Heroku
EXPOSE $PORT

# Run the application
CMD dotnet TaskManagementSystem.API.dll --urls http://+:$PORT
