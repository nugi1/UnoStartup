namespace UnoAccess.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class UnoContextFactory : IDesignTimeDbContextFactory<UnoContext>
{
    public UnoContext CreateDbContext(string[] args)
    {
        return new UnoContext();
    }
}