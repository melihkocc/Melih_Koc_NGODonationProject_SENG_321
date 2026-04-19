using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using NgoDonationSystem.Models;

namespace NgoDonationSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<AidRequest> AidRequests { get; set; }
        public DbSet<AidDistribution> AidDistributions { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AuditLog>()
                .Property(a => a.OldValues)
                .HasColumnType("jsonb");

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.NewValues)
                .HasColumnType("jsonb");


            modelBuilder.Entity<User>()
                .HasOne(u => u.Donor)
                .WithOne(d => d.User)
                .HasForeignKey<Donor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AidRequest>()
                .HasOne(a => a.AidDistribution)
                .WithOne(ad => ad.AidRequest)
                .HasForeignKey<AidDistribution>(ad => ad.AidRequestId)
                .OnDelete(DeleteBehavior.Cascade);


            
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.CreatedBy)
                .WithMany(u => u.DonationsCreated)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.ExpensesCreated)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AidRequest>()
                .HasOne(ar => ar.CreatedBy)
                .WithMany(u => u.AidRequestsCreated)
                .HasForeignKey(ar => ar.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AidRequest>()
                .HasOne(ar => ar.AssignedWorker)
                .WithMany(u => u.AssignedAidRequests)
                .HasForeignKey(ar => ar.AssignedWorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AidDistribution>()
                .HasOne(ad => ad.DeliveredBy)
                .WithMany(u => u.AidDistributionsDelivered)
                .HasForeignKey(ad => ad.DeliveredById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalWorkflow>()
                .HasOne(aw => aw.RequestedBy)
                .WithMany(u => u.ApprovalWorkflowsRequested)
                .HasForeignKey(aw => aw.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalWorkflow>()
                .HasOne(aw => aw.Approver)
                .WithMany(u => u.ApprovalWorkflowsApproved)
                .HasForeignKey(aw => aw.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AidRequest>()
                .HasOne(ar => ar.InventoryItem)
                .WithMany(i => i.AidRequests)
                .HasForeignKey(ar => ar.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ReceiptDocument)
                .WithMany(d => d.Expenses)
                .HasForeignKey(e => e.ReceiptDocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = new List<AuditLog>();
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out var userId);

            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var tableName = entry.Entity.GetType().Name;
                var action = entry.State switch
                {
                    EntityState.Added => "INSERT",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => null
                };

                if (action == null) continue;

                string oldValues = "{}";
                string newValues = "{}";

                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    var oldDict = new Dictionary<string, object>();
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified || entry.State == EntityState.Deleted)
                        {
                            oldDict[prop.Metadata.Name] = prop.OriginalValue;
                        }
                    }
                    oldValues = JsonSerializer.Serialize(oldDict);
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var newDict = new Dictionary<string, object>();
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified || entry.State == EntityState.Added)
                        {
                            newDict[prop.Metadata.Name] = prop.CurrentValue;
                        }
                    }
                    newValues = JsonSerializer.Serialize(newDict);
                }

                auditEntries.Add(new AuditLog
                {
                    TableName = tableName,
                    Action = action,
                    UserId = userId > 0 ? userId : 1,
                    Timestamp = DateTime.UtcNow,
                    OldValues = oldValues,
                    NewValues = newValues
                });
            }


            var result = await base.SaveChangesAsync(cancellationToken);


            if (auditEntries.Any())
            {
                AuditLogs.AddRange(auditEntries);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
