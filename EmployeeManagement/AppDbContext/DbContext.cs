using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using EmployeeManagement.NewFolder;

namespace EmployeeManagement.NewFolder;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<State> States => Set<State>();
}