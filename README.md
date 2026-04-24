# baby-monitor-backend

.NET 8 Web API backend for **Luna** — a self-hosted baby tracking application.
Exposes a REST API consumed by the Angular PWA in
[`baby-monitor-frontend`](../baby-monitor-frontend) and persists data in a
PostgreSQL database. Everything runs locally via Docker Desktop.

---

## Tech stack

| Layer             | Technology                        |
| ----------------- | --------------------------------- |
| Framework         | .NET 8 / ASP.NET Core Web API     |
| ORM               | Entity Framework Core 8           |
| Database          | PostgreSQL 16                     |
| Auth              | ASP.NET Core Identity + JWT       |
| API docs          | Swagger / OpenAPI                 |
| Containerisation  | Docker + Docker Compose           |
| Logging           | Serilog (planned)                 |

## Architecture

The solution follows a lightweight **Clean Architecture** split:

```
BabyMonitor.API/             # ASP.NET Core entry point, controllers, DI wiring
BabyMonitor.Core/            # Entities, enums, DTOs, interfaces (no deps)
BabyMonitor.Infrastructure/  # EF Core DbContext, repositories, services, migrations
```

PostgreSQL runs as a sibling container — no separate repo is needed. Its data
is persisted to a named Docker volume (`postgres_data`) on your host, so it
survives container restarts and rebuilds.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (with the
  WSL2 backend enabled on Windows)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (only if you want to run
  the API outside Docker during development)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet):
  `dotnet tool install --global dotnet-ef`

## Getting started

> The project scaffold is not committed yet — this is the initial repository
> skeleton. Commands below reflect the planned setup.

### 1. Configure environment

Copy `.env.example` to `.env` and fill in at least:

```env
POSTGRES_DB=babymonitor
POSTGRES_USER=babymonitor
POSTGRES_PASSWORD=change-me
JWT_SECRET=please-generate-a-long-random-value
```

### 2. Run everything with Docker Compose

```bash
docker compose up --build
```

This starts:

- `api`      → http://localhost:5000  (Swagger at `/swagger`)
- `db`       → PostgreSQL on `localhost:5432`

### 3. Apply database migrations

On first run (or after pulling new migrations):

```bash
docker compose exec api dotnet ef database update
```

### 4. Stop everything

```bash
docker compose down           # stop containers, keep data
docker compose down -v        # stop containers AND wipe the DB volume
```

## Running the API outside Docker (dev loop)

```bash
cd BabyMonitor.API
dotnet restore
dotnet run
```

Make sure PostgreSQL is reachable (either via `docker compose up db` or a
local install) and that `ConnectionStrings__Default` points to it.

## `docker-compose.yml` (overview)

```yaml
services:
  api:
    build: .
    ports: ["5000:8080"]
    depends_on: [db]
    environment:
      - ConnectionStrings__Default=Host=db;Port=5432;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
      - Jwt__Secret=${JWT_SECRET}

  db:
    image: postgres:16
    restart: unless-stopped
    environment:
      - POSTGRES_DB=${POSTGRES_DB}
      - POSTGRES_USER=${POSTGRES_USER}
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports: ["5432:5432"]

volumes:
  postgres_data:
```

## Project structure (planned)

```
baby-monitor-backend/
├── BabyMonitor.API/
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
├── BabyMonitor.Core/
│   ├── Entities/
│   ├── Enums/
│   ├── Interfaces/
│   └── Dtos/
├── BabyMonitor.Infrastructure/
│   ├── Data/               # AppDbContext, migrations
│   ├── Repositories/
│   └── Services/
├── docker-compose.yml
├── Dockerfile
├── .env.example
└── README.md
```

## API surface (planned)

- `POST /api/auth/register` / `POST /api/auth/login`
- `GET|POST|PUT|DELETE /api/babies`
- `GET|POST|PUT|DELETE /api/babies/{babyId}/feedings`
- `GET|POST|PUT|DELETE /api/babies/{babyId}/sleep`
- `GET|POST|PUT|DELETE /api/babies/{babyId}/diapers`
- `GET|POST|PUT|DELETE /api/babies/{babyId}/growth`
- `GET|POST|PUT|DELETE /api/babies/{babyId}/medications`
- `GET        /api/babies/{babyId}/reports/pdf`

## Data backup

Because PostgreSQL data lives in the `postgres_data` Docker volume, you can
back it up with:

```bash
docker run --rm \
  -v baby-monitor-backend_postgres_data:/data \
  -v $PWD:/backup alpine \
  tar czf /backup/babymonitor-$(date +%F).tar.gz -C /data .
```

## Contributing

This is a personal project, but suggestions and pull requests are welcome.
Please open an issue before starting any significant work.

## License

MIT — see [LICENSE](./LICENSE).
