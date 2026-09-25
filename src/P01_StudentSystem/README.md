# Student System

An EF Core 8 Code First console application backed by SQL Server. It models students, courses, enrollments, resources, and homework submissions. The `InitialCreate` migration includes deterministic sample data.

## Requirements

- .NET 8 SDK
- SQL Server (SQL Server LocalDB works on Windows)

## Run

The application uses `STUDENT_SYSTEM_CONNECTION_STRING` when it is set. Otherwise, it uses a SQL Server LocalDB database named `StudentSystem`.

```bash
export STUDENT_SYSTEM_CONNECTION_STRING='Server=localhost,1433;Database=StudentSystem;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True'
dotnet restore
dotnet run
```

On startup, the console app applies pending migrations, then prints courses with enrolled students and students with their courses.

## Migration commands

The initial migration is committed under `Data/Migrations`. To recreate it:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations remove
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
dotnet ef database update
```
