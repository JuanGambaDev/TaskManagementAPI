using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI;

public class TaskManagementContext : DbContext
{
    public DbSet<Task> Tasks {get; set;}

    public TaskManagementContext (DbContextOptions<TaskManagementContext> options) : base (options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(task=>
        {
            task.ToTable("tasks");
            task.HasKey(p => p.IdTask);

            task.Property(p => p.Name).IsRequired().HasMaxLength(200);

            task.Property(p => p.Description).IsRequired(false);

            task.Property(p => p.Icon).IsRequired();

            task.Property(p => p.Status).IsRequired();

            task.Property(p => p.CreationDate).IsRequired();

            task.Property(p => p.Deadline).IsRequired();;
        });
    }

    

}
