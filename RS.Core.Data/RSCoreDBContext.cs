namespace RS.Core.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using RS.Core.Domain;

    public class RSCoreDBContext : DbContext
    {
        public RSCoreDBContext()
            : base("name=RSCoreDBContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        /// <summary> 
        /// ToDo: Translate - DataSet objeleri test tarafýnda ezilip in-memory olarak kullanýlacaðý için virtual olarak tanýmlanmýþtýr.
        /// </summary>
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<AutoCode> AutoCode { get; set; }
        public virtual DbSet<AutoCodeLog> AutoCodeLog { get; set; }
    }
}