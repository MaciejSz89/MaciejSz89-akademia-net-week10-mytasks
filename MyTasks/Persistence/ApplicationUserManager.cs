using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using System.Configuration;

namespace MyTasks.Persistence
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _dbContext;
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, IConfiguration configuration,
        IApplicationDbContext dbContext) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var defaultCategories = _configuration.GetSection("DefaultCategories").Get<string[]>();

            foreach (var category in defaultCategories)
            {
                user.Categories.Add(new Category { Name = category });
            }
       

            // Wywołaj metodę bazową do utworzenia użytkownika
            var result = await base.CreateAsync(user);

            // Zwróć wynik metody bazowej
            return result;
        }
    }
}
