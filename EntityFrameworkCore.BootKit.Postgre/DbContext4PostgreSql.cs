using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.BootKit.DbContexts;

public class DbContext4PostgreSql : DataContext
{
    public DbContext4PostgreSql(DbContextOptions options, IServiceProvider serviceProvider)
        : base(options, serviceProvider) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SetLog(optionsBuilder);
        optionsBuilder.UseNpgsql(ConnectionString,
            x =>
            {
                x.UseNetTopologySuite();
                if (enableRetryOnFailure)
                {
                    x.EnableRetryOnFailure();
                }
            });
        base.OnConfiguring(optionsBuilder);
    }
}

public class DbContext4PostgreSql2 : DataContext
{
    public DbContext4PostgreSql2(DbContextOptions options, IServiceProvider serviceProvider)
        : base(options, serviceProvider) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SetLog(optionsBuilder);
        optionsBuilder.UseNpgsql(ConnectionString,
            x =>
            {
                x.UseNetTopologySuite();
                if (enableRetryOnFailure)
                {
                    x.EnableRetryOnFailure();
                }
            });
        base.OnConfiguring(optionsBuilder);
    }
}

public class DbContext4PostgreSql3 : DataContext
{
    public DbContext4PostgreSql3(DbContextOptions options, IServiceProvider serviceProvider)
        : base(options, serviceProvider) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SetLog(optionsBuilder);
        optionsBuilder.UseNpgsql(ConnectionString,
            x =>
            {
                x.UseNetTopologySuite();
                if (enableRetryOnFailure)
                {
                    x.EnableRetryOnFailure();
                }
            });
        base.OnConfiguring(optionsBuilder);
    }
}
