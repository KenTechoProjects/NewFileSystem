



using Domain.Entities;

namespace Persistence
{
    public class AppDbContext:DbContext, IAppDbContext
    {
      

        #region Contructors
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region DbSet
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        #endregion

        #region Overrides

        #endregion





        public IDbContextTransaction Begin()
        {
            var trans = this.Database.CurrentTransaction;
            if (this.Database.CurrentTransaction == null)
            {
                trans = this.Database.BeginTransaction();
            }
            return trans;
        }
        public async Task CommitAsync()
        {
            var trans = Begin();

            if (trans != null)
            {

                await trans.CommitAsync();
            }

        }
        public async Task RollbackAsync()
        {
            var trans = Begin();

            if (trans != null)
            {
                await trans.RollbackAsync();
            }

        }


        public DbContext GetAppDbContext()
        {
            return this;
        }




       
    }
}
