FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManagementSystem.Web/TaskManagementSystem.Web.csproj", "TaskManagementSystem.Web/"]
RUN dotnet restore "TaskManagementSystem.Web/TaskManagementSystem.Web.csproj"
COPY . .
WORKDIR "/src/TaskManagementSystem.Web"
RUN dotnet build "TaskManagementSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementSystem.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ApiSettings__BaseUrl=http://api:80/
ENTRYPOINT ["dotnet", "TaskManagementSystem.Web.dll"]
