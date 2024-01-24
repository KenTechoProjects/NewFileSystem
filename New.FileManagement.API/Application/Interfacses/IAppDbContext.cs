




namespace Application.Interfacses
{
    public interface IAppDbContext
    {
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        IDbContextTransaction Begin();
        Task CommitAsync();
        Task RollbackAsync();
        DbContext GetAppDbContext();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

    }
}
