
using Microsoft.EntityFrameworkCore;

using Task = PmsApi.Models.Task;
using PmsApi.Models;

namespace PmsApi.DataContexts;

public partial class PmsContext : DbContext
{
    private string connectionString = String.Empty;
    public PmsContext()
    {
        connectionString = " server = localhost; port = 3307; database = pms; user = pms; password = dotnet8;";
    }
    public PmsContext(string connectionString)
    {
        this.connectionString = connectionString;
    }


    public PmsContext(DbContextOptions<PmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskAttachment> TaskAttachments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.
        UseMySql(connectionString,
         ServerVersion.AutoDetect(connectionString)
         );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");
        ConfigureProject(modelBuilder);
        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(e => e.PriorityId).HasName("PRIMARY");
            entity.HasIndex(e => e.PriorityName).IsUnique();
            entity.Property(e => e.PriorityName)
            .HasMaxLength(60)
            .IsRequired();



        });

        modelBuilder.Entity<Status>(entity =>
               {

                   entity.HasIndex(e => e.StatusName).IsUnique();
                   entity.Property(e => e.StatusName)
                   .HasMaxLength(60)
                   .IsRequired();



               });

        modelBuilder.Entity<ProjectCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");


            entity.Property(e => e.CategoryName)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");



            entity.Property(e => e.RoleName)
                .HasMaxLength(255);
        });

        ConfigureTask(modelBuilder);

        modelBuilder.Entity<TaskAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");


            entity.HasIndex(e => e.TaskId);

            entity.Property(e => e.FileData)
                .HasColumnType("blob");
            entity.Property(e => e.FileName)
                .HasMaxLength(255);

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAttachments)
                .HasForeignKey(d => d.TaskId);
        });

        ConfigureUser(modelBuilder);

        OnModelCreatingPartial(modelBuilder);


    }

    static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
       {
           entity.HasKey(e => e.UserId).HasName("PRIMARY");


           entity.HasIndex(e => e.RoleId);
           entity.HasIndex(e => e.Email).IsUnique();
           entity.Property(e => e.Email)
               .HasMaxLength(100).IsRequired();

           entity.Property(e => e.FirstName)
               .HasMaxLength(50).IsRequired(); ;
           entity.Property(e => e.LastName)
               .HasMaxLength(50).IsRequired(); ;
           entity.Property(e => e.Password)
               .HasMaxLength(255).IsRequired(); ;
           entity.Property(e => e.Username)
               .HasMaxLength(50);

           entity.HasOne(d => d.Role).WithMany(p => p.Users)
               .HasForeignKey(d => d.RoleId).IsRequired();
       });
    }
    static void ConfigureTask(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PRIMARY");



            entity.HasIndex(e => e.AssignedUserId);
            entity.Property(e => e.AssignedUserId).IsRequired();

            entity.HasIndex(e => e.ProjectId);

            entity.Property(e => e.ProjectId).IsRequired();

            entity.Property(e => e.Description)
                .HasColumnType("text");



            entity.Property(e => e.Title)
                .HasMaxLength(255);

            entity.HasOne(d => d.AssignedUser).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.AssignedUserId).IsRequired();

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId).IsRequired();

            entity.Property(e => e.DueDate).IsRequired();

            entity.Property(e => e.CreatedDate).IsRequired();

        });
    }
    private static void ConfigureProject(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PRIMARY");


            entity.HasIndex(e => e.CategoryId);

            entity.HasIndex(e => e.ManagerId);
            entity.HasIndex(e => e.ProjectName)
            .IsUnique();
            entity.Property(e => e.Description)
                .HasColumnType("text");

            entity.Property(e => e.ProjectName)
                .HasMaxLength(255);
            entity.Property(e => e.StartDate)
                           .IsRequired();
            entity.Property(e => e.EndDate)
          .IsRequired();
            entity.HasOne(d => d.Category).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CategoryId).IsRequired();


            entity.HasOne(d => d.Manager).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ManagerId).IsRequired();

        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
