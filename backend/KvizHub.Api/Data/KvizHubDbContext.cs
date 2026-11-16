using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Data
{
    public class KvizHubDbContext(DbContextOptions<KvizHubDbContext> options) : DbContext(options)
    {
    }
}
