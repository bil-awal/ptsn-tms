# Heroku Docker Deployment Setup

## Prerequisites
1. GitHub repository terhubung dengan Heroku
2. Heroku app sudah dibuat (ptsn-task-management-system-88f1ef338f0f)

## Setup Stack Container di Heroku

### Via Heroku CLI:
```bash
heroku stack:set container --app ptsn-task-management-system-88f1ef338f0f
```

### Via Heroku Dashboard:
1. Buka https://dashboard.heroku.com/apps/ptsn-task-management-system-88f1ef338f0f/settings
2. Di bagian "Stack", ubah ke "Container"
3. Save changes

## Deployment Process

1. **Commit changes ke GitHub:**
   ```bash
   git add .
   git commit -m "Switch to Docker deployment"
   git push origin main
   ```

2. **Heroku akan otomatis deploy dari GitHub**
   - Jika automatic deploys aktif, deployment akan langsung jalan
   - Jika tidak, klik "Deploy Branch" di Heroku Dashboard

## Monitoring

Check logs:
```bash
heroku logs --tail --app ptsn-task-management-system-88f1ef338f0f
```

Check dyno status:
```bash
heroku ps --app ptsn-task-management-system-88f1ef338f0f
```

## Environment Variables

Set di Heroku Dashboard atau via CLI:
```bash
heroku config:set ASPNETCORE_ENVIRONMENT=Production --app ptsn-task-management-system-88f1ef338f0f
```

## Troubleshooting

1. **Jika app tidak start:**
   - Check logs untuk error messages
   - Pastikan PORT environment variable digunakan dengan benar
   - Verify Docker image build berhasil

2. **Jika build gagal:**
   - Check heroku.yml syntax
   - Verify Dockerfile path benar
   - Check memory limits (max 512MB untuk free dyno)

## URL Aplikasi
https://ptsn-task-management-system-88f1ef338f0f.herokuapp.com/
