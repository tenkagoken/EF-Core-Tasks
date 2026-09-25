using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace P01_StudentSystem.Data;

public class StudentSystemContextFactory : IDesignTimeDbContextFactory<StudentSystemContext>
{
    public StudentSystemContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("STUDENT_SYSTEM_CONNECTION_STRING")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=StudentSystem;Trusted_Connection=True;TrustServerCertificate=True;";

        var options = new DbContextOptionsBuilder<StudentSystemContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new StudentSystemContext(options);
    }
}
