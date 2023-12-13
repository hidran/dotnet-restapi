
using Microsoft.EntityFrameworkCore;

using Task = PmsApi.Models.Task;
using PmsApi.Models;

namespace PmsApi.DataContexts;

public class PmsContext : DbContext
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

    public DbSet<Role> Roles { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    public DbSet<User> Users { get; set; }
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
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

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
        ConfigureUser(modelBuilder);
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


    private void populateDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
               new Role { RoleId = 1, RoleName = "Admin" },
            new Role { RoleId = 2, RoleName = "Manager" },
            new Role { RoleId = 3, RoleName = "Developer" },
            new Role { RoleId = 4, RoleName = "Designer" },
            new Role { RoleId = 5, RoleName = "Tester" },
            new Role { RoleId = 6, RoleName = "HR" },
            new Role { RoleId = 7, RoleName = "Sales" },
            new Role { RoleId = 8, RoleName = "Marketing" },
            new Role { RoleId = 9, RoleName = "Support" },
            new Role { RoleId = 10, RoleName = "Customer Service" },
            new Role { RoleId = 11, RoleName = "Finance" },
            new Role { RoleId = 12, RoleName = "Legal" },
            new Role { RoleId = 13, RoleName = "Public Relations" },
            new Role { RoleId = 14, RoleName = "Operations" },
            new Role { RoleId = 15, RoleName = "IT" },
            new Role { RoleId = 16, RoleName = "Product Manager" },
            new Role { RoleId = 17, RoleName = "Data Analyst" },
            new Role { RoleId = 18, RoleName = "Business Analyst" },
            new Role { RoleId = 19, RoleName = "Executive" },
            new Role { RoleId = 20, RoleName = "Intern" }
        );
        modelBuilder.Entity<User>().HasData(
           new User { UserId = 1, Username = "user1", FirstName = "Emma", LastName = "Stone", Password = "pass123", Email = "emma.stone@mail.com", RoleId = 1 },
           new User { UserId = 2, Username = "user2", FirstName = "Liam", LastName = "Smith", Password = "pass123", Email = "liam.smith@mail.com", RoleId = 2 },
           new User { UserId = 3, Username = "user3", FirstName = "Olivia", LastName = "Jones", Password = "pass123", Email = "olivia.jones@mail.com", RoleId = 3 },
           new User { UserId = 4, Username = "user4", FirstName = "Noah", LastName = "Brown", Password = "pass123", Email = "noah.brown@mail.com", RoleId = 4 },

           new User
           {
               UserId = 5,
               Username = "user5",
               FirstName = "Jacob",
               LastName = "Williams",
               Password = "pass123",
               Email = "jacob.williams@mail.com",
               RoleId = 5
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



        modelBuilder.Entity<Project>().HasData(
    new Project { ProjectId = 1, ProjectName = "Progetto Alfa", Description = "Descrizione del Progetto Alfa", StartDate = new DateOnly(2021, 1, 1), EndDate = new DateOnly(2021, 6, 30), CategoryId = 1, ManagerId = 1 },
    new Project { ProjectId = 2, ProjectName = "Progetto Beta", Description = "Descrizione del Progetto Beta", StartDate = new DateOnly(2021, 7, 1), EndDate = new DateOnly(2021, 12, 31), CategoryId = 2, ManagerId = 2 },
new Project { ProjectId = 3, ProjectName = "Progetto Gamma", Description = "Descrizione del Progetto Gamma", StartDate = new DateOnly(2022, 1, 1), EndDate = new DateOnly(2022, 6, 30), CategoryId = 3, ManagerId = 3 },
new Project { ProjectId = 4, ProjectName = "Progetto Delta", Description = "Descrizione del Progetto Delta", StartDate = new DateOnly(2022, 7, 1), EndDate = new DateOnly(2022, 12, 31), CategoryId = 4, ManagerId = 4 },
new Project { ProjectId = 5, ProjectName = "Progetto Epsilon", Description = "Descrizione del Progetto Epsilon", StartDate = new DateOnly(2023, 1, 1), EndDate = new DateOnly(2023, 6, 30), CategoryId = 5, ManagerId = 5 },
new Project { ProjectId = 6, ProjectName = "Progetto Zeta", Description = "Descrizione del Progetto Zeta", StartDate = new DateOnly(2023, 7, 1), EndDate = new DateOnly(2023, 12, 31), CategoryId = 6, ManagerId = 5 },
new Project { ProjectId = 7, ProjectName = "Progetto Eta", Description = "Descrizione del Progetto Eta", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 6, 30), CategoryId = 7, ManagerId = 4 },
new Project { ProjectId = 8, ProjectName = "Progetto Theta", Description = "Descrizione del Progetto Theta", StartDate = new DateOnly(2024, 7, 1), EndDate = new DateOnly(2024, 12, 31), CategoryId = 8, ManagerId = 3 },
new Project { ProjectId = 9, ProjectName = "Progetto Iota", Description = "Descrizione del Progetto Iota", StartDate = new DateOnly(2025, 1, 1), EndDate = new DateOnly(2025, 6, 30), CategoryId = 9, ManagerId = 5 },
new Project { ProjectId = 10, ProjectName = "Progetto Kappa", Description = "Descrizione del Progetto Kappa", StartDate = new DateOnly(2025, 7, 1), EndDate = new DateOnly(2025, 12, 31), CategoryId = 10, ManagerId = 4 }
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
        modelBuilder.Entity<Task>().HasData(
            new Task { TaskId = 1, Title = "Task 1", Description = "Descrizione Task 1", PriorityId = 1, StatusId = 1, DueDate = new DateOnly(2021, 6, 30), CreatedDate = new DateOnly(2021, 1, 1), ProjectId = 1, AssignedUserId = 1 },
            new Task { TaskId = 2, Title = "Task 2", Description = "Descrizione Task 2", PriorityId = 2, StatusId = 2, DueDate = new DateOnly(2021, 7, 30), CreatedDate = new DateOnly(2021, 2, 1), ProjectId = 2, AssignedUserId = 2 },
            new Task { TaskId = 3, Title = "Task 3", Description = "Descrizione Task 3", PriorityId = 3, StatusId = 3, DueDate = new DateOnly(2021, 8, 30), CreatedDate = new DateOnly(2021, 3, 1), ProjectId = 3, AssignedUserId = 3 }
        );
        modelBuilder.Entity<TaskAttachment>().HasData(
            new TaskAttachment { Id = 1, FileName = "Attachment1.pdf", TaskId = 1 },
            new TaskAttachment { Id = 2, FileName = "Attachment2.pdf", TaskId = 1 },
            new TaskAttachment { Id = 3, FileName = "Attachment3.pdf", TaskId = 2 },
            new TaskAttachment { Id = 4, FileName = "Attachment4.pdf", TaskId = 2 }
        );

    }
}
