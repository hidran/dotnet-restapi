
using Microsoft.EntityFrameworkCore;

using Task = PmsApi.Models.Task;
using PmsApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PmsApi.DataContexts;

public class PmsContext : IdentityDbContext
{
    private readonly string connectionString = String.Empty;
    public PmsContext()
    {
    }
    public PmsContext(string connectionString)
    {
        this.connectionString = connectionString;
    }


    public PmsContext(DbContextOptions<PmsContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectCategory> ProjectCategories { get; set; }

    public new DbSet<Role> Roles { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    public new DbSet<User> Users { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<ProjectCategory> Categories { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (connectionString != String.Empty)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


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


        ConfigureProject(modelBuilder);
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


        populateDatabase(modelBuilder);


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


    private void populateDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
               new Role { Name = "Admin", NormalizedName = "ADMIN" },
            new Role
            {
                Name = "Editor",
                NormalizedName = "Editor"
            },
            new Role
            {
                Name = "User",
                NormalizedName = "User"
            }


        );
        modelBuilder.Entity<User>().HasData(
           new User { UserName = "user1", FirstName = "Emma", LastName = "Stone", Email = "emma.stone@mail.com" },
           new User { UserName = "user2", FirstName = "Liam", LastName = "Smith", Email = "liam.smith@mail.com" },
           new User { UserName = "user3", FirstName = "Olivia", LastName = "Jones", Email = "olivia.jones@mail.com" },
           new User { UserName = "user4", FirstName = "Noah", LastName = "Brown", Email = "noah.brown@mail.com" },

           new User
           {

               UserName = "user5",
               FirstName = "Jacob",
               LastName = "Williams"
              ,
               Email = "jacob.williams@mail.com"

           });
        modelBuilder.Entity<ProjectCategory>().HasData(
            new ProjectCategory { CategoryId = 1, CategoryName = "Software Development" },
            new ProjectCategory { CategoryId = 2, CategoryName = "Web Development" },
            new ProjectCategory { CategoryId = 3, CategoryName = "Database Systems" },
            new ProjectCategory { CategoryId = 4, CategoryName = "System Integration" },
            new ProjectCategory { CategoryId = 5, CategoryName = "Testing" },
            new ProjectCategory { CategoryId = 6, CategoryName = "Maintenance" },
            new ProjectCategory { CategoryId = 7, CategoryName = "Research and Development" },
            new ProjectCategory { CategoryId = 8, CategoryName = "IT Support" },
            new ProjectCategory { CategoryId = 9, CategoryName = "Marketing Campaigns" },
            new ProjectCategory { CategoryId = 10, CategoryName = "Product Launch" },
            new ProjectCategory { CategoryId = 11, CategoryName = "Event Management" },
            new ProjectCategory { CategoryId = 12, CategoryName = "Content Creation" },
            new ProjectCategory { CategoryId = 13, CategoryName = "Social Media Management" },
            new ProjectCategory { CategoryId = 14, CategoryName = "Customer Relations" },
            new ProjectCategory { CategoryId = 15, CategoryName = "Sales Strategies" },
            new ProjectCategory { CategoryId = 16, CategoryName = "Market Research" },
            new ProjectCategory { CategoryId = 17, CategoryName = "Financial Planning" },
            new ProjectCategory { CategoryId = 18, CategoryName = "Budget Management" },
            new ProjectCategory { CategoryId = 19, CategoryName = "Legal Compliance" },
            new ProjectCategory { CategoryId = 20, CategoryName = "Environmental Projects" }
        );



        modelBuilder.Entity<Priority>().HasData(
            new Priority { PriorityId = 1, PriorityName = "Alta" },
            new Priority { PriorityId = 2, PriorityName = "Media" },
            new Priority { PriorityId = 3, PriorityName = "Bassa" },
            new Priority { PriorityId = 4, PriorityName = "Urgente" }


        );
        modelBuilder.Entity<Status>().HasData(
            new Status { StatusId = 1, StatusName = "In Attesa" },
            new Status { StatusId = 2, StatusName = "In Corso" },
            new Status { StatusId = 3, StatusName = "Completato" }


        );


    }
}
