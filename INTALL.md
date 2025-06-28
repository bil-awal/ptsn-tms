# Panduan Deploy TaskManagementSystem ke Heroku via GitHub

## 1. File-file yang Perlu Ditambahkan

### A. `Procfile` (tanpa ekstensi)
Buat file `Procfile` di root project dengan isi:
```
web: cd $HOME/heroku_output && ./TaskManagementSystem.API
```

### B. `heroku.yml`
Buat file `heroku.yml` di root project:
```yaml
build:
  docker:
    web: Dockerfile
```

### C. `Dockerfile`
Buat file `Dockerfile` di root project:
```dockerfile
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

# Copy published files
COPY --from=build /app/publish .

# Set environment variable
ENV ASPNETCORE_URLS=http://+:$PORT

# Run the application
CMD ["dotnet", "TaskManagementSystem.API.dll"]
```

### D. `.dockerignore`
Buat file `.dockerignore`:
```
**/bin/
**/obj/
.git/
.vs/
.vscode/
*.user
*.suo
*.cache
*.log
.DS_Store
```

## 2. Modifikasi Program.cs

Update file `TaskManagementSystem.API/Program.cs` untuk mendukung PORT environment variable dari Heroku:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Tambahkan konfigurasi untuk Heroku
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://+:{port}");

// ... rest of your existing configuration ...

var app = builder.Build();

// Remove atau comment ini untuk production:
// app.UseHttpsRedirection();

// ... rest of your existing middleware ...
```

## 3. Update appsettings.json

Tambahkan konfigurasi untuk production di `TaskManagementSystem.API/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  }
}
```

## 4. Buat appsettings.Production.json

Buat file `TaskManagementSystem.API/appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Serilog": "Information"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## 5. Setup di Heroku

### A. Buat aplikasi Heroku:
1. Login ke Heroku Dashboard
2. Click "New" → "Create new app"
3. Pilih nama app (misal: `ptsn-tms`)
4. Pilih region terdekat

### B. Connect GitHub:
1. Di tab "Deploy" aplikasi Heroku
2. Pilih "GitHub" sebagai deployment method
3. Connect ke akun GitHub Anda
4. Search dan connect repository `ptsn-tms`

### C. Configure Stack (PENTING):
1. Di Settings → Buildpacks
2. Add buildpack: `heroku/buildpacks:heroku-buildpack-docker`
3. Atau jalankan via Heroku CLI: `heroku stack:set container -a your-app-name`

### D. Environment Variables:
Di Settings → Config Vars, tambahkan:
- `ASPNETCORE_ENVIRONMENT`: `Production`
- Variables lain yang dibutuhkan aplikasi

### E. Enable Automatic Deploys:
1. Di tab Deploy
2. Enable "Automatic deploys" dari branch main/master
3. Atau manual deploy dengan click "Deploy Branch"

## 6. Alternative: Tanpa Docker (Menggunakan Buildpack)

Jika tidak ingin menggunakan Docker, gunakan buildpack .NET:

### A. Hapus Dockerfile dan heroku.yml

### B. Buat file `global.json` di root:
```json
{
  "sdk": {
    "version": "8.0.100",
    "rollForward": "latestFeature"
  }
}
```

### C. Update Procfile:
```
web: cd $HOME/heroku_output && ./TaskManagementSystem.API
```

### D. Di Heroku, set buildpack:
```bash
heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
```

## 7. Commit dan Push

```bash
git add .
git commit -m "Add Heroku deployment configuration"
git push origin main
```

## 8. Monitoring

Setelah deploy:
- Check logs: `heroku logs --tail -a your-app-name`
- Akses app: `https://your-app-name.herokuapp.com/swagger`

## Tips Tambahan

1. **Database**: Karena menggunakan InMemory database, data akan hilang setiap restart. Untuk production, pertimbangkan menggunakan Heroku Postgres.

2. **CORS**: Pastikan CORS configuration sudah sesuai dengan frontend URL Anda.

3. **Health Check**: Tambahkan health check endpoint untuk monitoring:
```csharp
app.MapGet("/health", () => "OK");
```

4. **Logs**: Heroku otomatis capture console output, jadi Serilog console sink sudah cukup.

## Troubleshooting

Jika ada masalah:
1. Check build logs di Heroku Activity tab
2. Pastikan semua file config sudah di-commit
3. Verify PORT environment variable handling
4. Check runtime logs dengan `heroku logs`