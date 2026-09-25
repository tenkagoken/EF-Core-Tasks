# EF Core Tasks

A single .NET 8 repository containing complete Code First solutions for the two exercises in `EF-Core-Exercises.pdf`. Each exercise is an independent console project with its own model, `DbContext`, seed data, and checked-in EF Core migrations.

## Exercises

### 1. Student System

`P01_StudentSystem` models a learning system with:

- students and courses connected through the `StudentCourse` join entity;
- course resources categorized as video, presentation, document, or other;
- homework submissions associated with both a student and a course;
- the required field lengths, Unicode rules, optional fields, and relationships;
- sample students, courses, resources, enrollments, and homework submissions.

The console application displays the seeded course and student information from the database. The optional bonus task of accepting new course and student information through the console is not implemented.

### 2. Sales Database

`P02_SalesDatabase` models products, customers, stores, and sales. It includes generated sample data and the three required migration stages:

1. `InitialCreate` creates the original schema.
2. `ProductsAddColumnDescription` adds the product description with a maximum length of 250 and a default value of `No description`.
3. `SalesAddDateDefault` configures the sale date to use SQL Server's `GETDATE()` database function, rather than an application-side `DateTime.Now` value.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or another Docker Compose-compatible runtime
- The EF Core CLI tool (needed only for migration commands). A pinned local tool manifest is included, so restore it from the repository root:

  ```bash
  dotnet tool restore
  ```

## Start SQL Server with Docker

The Compose file runs SQL Server 2022 Developer Edition and stores its data in a named Docker volume. Choose a strong password that satisfies SQL Server's password policy; do not commit it.

```bash
export MSSQL_SA_PASSWORD="$(openssl rand -base64 24)Aa1!"
docker compose up -d
```

Port `1433` is published by default. If that port is already in use, set `SQLSERVER_PORT` before starting the container, for example `export SQLSERVER_PORT=14330`.

To stop SQL Server without deleting its databases:

```bash
docker compose down
```

The named volume is intentionally retained. Running `docker compose down --volumes` deletes it and all databases stored in it.

## Configure the connections

Both projects read their connection string from environment variables (`STUDENT_SYSTEM_CONNECTION_STRING` and `SALES_DB_CONNECTION`). This keeps credentials out of source control. In the same shell where `MSSQL_SA_PASSWORD` is set, configure:

```bash
export STUDENT_SYSTEM_CONNECTION_STRING="Server=localhost,${SQLSERVER_PORT:-1433};Database=StudentSystem;User Id=sa;Password=${MSSQL_SA_PASSWORD};Encrypt=True;TrustServerCertificate=True"
export SALES_DB_CONNECTION="Server=localhost,${SQLSERVER_PORT:-1433};Database=SalesDatabase;User Id=sa;Password=${MSSQL_SA_PASSWORD};Encrypt=True;TrustServerCertificate=True"
```

These values exist only in the current shell session. For another SQL Server instance, provide equivalent connection strings through the same variables. Local secret files and development settings are ignored by Git; no password is included in this repository.

## Restore and build

From the repository root:

```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build
```

## Apply the included migrations

Migrations are already checked in, so a fresh checkout only needs `database update`:

```bash
dotnet ef database update --project src/P01_StudentSystem/P01_StudentSystem.csproj --startup-project src/P01_StudentSystem/P01_StudentSystem.csproj
dotnet ef database update --project src/P02_SalesDatabase/P02_SalesDatabase.csproj --startup-project src/P02_SalesDatabase/P02_SalesDatabase.csproj
```

The commands create/update the separate `StudentSystem` and `SalesDatabase` databases. Run them after the Docker container is ready and the connection-string variables are available.

When a model changes, create a clearly named migration in the affected project and then apply it:

```bash
# Student System
dotnet ef migrations add <MigrationName> --project src/P01_StudentSystem/P01_StudentSystem.csproj --startup-project src/P01_StudentSystem/P01_StudentSystem.csproj --output-dir Data/Migrations
dotnet ef database update --project src/P01_StudentSystem/P01_StudentSystem.csproj --startup-project src/P01_StudentSystem/P01_StudentSystem.csproj

# Sales Database
dotnet ef migrations add <MigrationName> --project src/P02_SalesDatabase/P02_SalesDatabase.csproj --startup-project src/P02_SalesDatabase/P02_SalesDatabase.csproj --output-dir Migrations
dotnet ef database update --project src/P02_SalesDatabase/P02_SalesDatabase.csproj --startup-project src/P02_SalesDatabase/P02_SalesDatabase.csproj
```

Never edit an already-applied migration to represent a new schema change.

## Run the applications

```bash
dotnet run --project src/P01_StudentSystem/P01_StudentSystem.csproj
dotnet run --project src/P02_SalesDatabase/P02_SalesDatabase.csproj
```

Each application applies the checked-in migrations, seeds data safely, and prints a summary that can be inspected from the terminal.

## Repository structure

```text
.
├── .config/dotnet-tools.json            # Repository-local EF Core CLI version
├── .github/workflows/dotnet.yml         # Restore, build, and test CI
├── src/
│   ├── P01_StudentSystem/              # Student System models, context, seed, migrations, console app
│   └── P02_SalesDatabase/              # Sales models, context, seed, migrations, console app
├── tests/EFCoreExercises.Tests/         # Model, constraints, defaults, and migration checks
├── Directory.Build.props               # Shared .NET 8 compiler settings
├── docker-compose.yml                  # Local SQL Server 2022 Developer container
├── EFCoreExercises.sln                 # Solution containing both projects
└── README.md
```

The projects remain independent but are grouped in one solution and repository, as requested by the final upload exercise. GitHub Actions restores, builds, and tests the solution on pushes to `main` and on pull requests.
