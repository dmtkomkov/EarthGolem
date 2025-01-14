using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options)
{
    public DbSet<Step> Steps { get; set; }
}