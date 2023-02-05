using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTraining1101Demo.Authorization.Delegation;
using MyTraining1101Demo.Authorization.Roles;
using MyTraining1101Demo.Authorization.Users;
using MyTraining1101Demo.Chat;
using MyTraining1101Demo.Customer;
using MyTraining1101Demo.Editions;
using MyTraining1101Demo.Friendships;
using MyTraining1101Demo.LIMS.Library.Container;
using MyTraining1101Demo.LIMS.Library.Customers.Source;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax;
using MyTraining1101Demo.LIMS.Library.Personnel;
using MyTraining1101Demo.LIMS.Library.Tests.Application;
using MyTraining1101Demo.LIMS.Library.Tests.StandardReference;
using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark;
using MyTraining1101Demo.LIMS.Library.Tests.SubApplications;
using MyTraining1101Demo.LIMS.Library.Tests.Technique;
using MyTraining1101Demo.LIMS.Library.Tests.Unit;
using MyTraining1101Demo.MultiTenancy;
using MyTraining1101Demo.MultiTenancy.Accounting;
using MyTraining1101Demo.MultiTenancy.Payments;
using MyTraining1101Demo.PhoneBook;
using MyTraining1101Demo.Storage;

namespace MyTraining1101Demo.EntityFrameworkCore
{
    public class MyTraining1101DemoDbContext : AbpZeroDbContext<Tenant, Role, User, MyTraining1101DemoDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<Person> Persons { get; set; }

        //public virtual DbSet<customer> customers { get; set; }

        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<Applications> Applications { get; set; }

        public virtual DbSet<StandardRemark> StandardRemarks { get; set; }

        public virtual DbSet<StandardReference> StandardReferences { get; set; }

        public virtual DbSet<Technique> Techniques { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

        public virtual DbSet<Source> Sources { get; set; }

        public virtual DbSet<Bank> Banks { get; set; }

        public virtual DbSet<Charges> Charges { get; set; }

        public virtual DbSet<Tax> Tax { get; set; }

        public virtual DbSet<Personnel> Personnels { get; set; }

        public virtual DbSet<Container> Containers { get; set; }

        public virtual DbSet<SubApplication> SubApplications { get; set; }

        public MyTraining1101DemoDbContext(DbContextOptions<MyTraining1101DemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
