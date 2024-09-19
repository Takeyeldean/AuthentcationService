using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthentcationServiceForTradingMarket.Model
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>{
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
