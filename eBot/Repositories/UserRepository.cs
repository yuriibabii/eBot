using System;
using System.Threading.Tasks;
using eBot.Data.Domain;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace eBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public UserRepository(IServiceProvider serviceProvider)
        {
            serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
        }

        public async Task<User> GetUserAsync(long chatId)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var userDb = await studyContext.Users.FindAsync(chatId);
            var user = userDb.Map(studyContext);
            return user;
        }

        public async Task SaveUserAsync(User user)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            var userDb = user.Map();
            var oldUserDb = await studyContext.Users.FirstOrNullAsync(user => user.Id == userDb.Id);
            if (oldUserDb != null)
            {
                studyContext.Users.Remove(oldUserDb);
            }

            studyContext.Users.Add(userDb);
            await studyContext.SaveChangesAsync();
        }

        public async Task SaveNewUserAsync(long chatId)
        {
            var user = new User(chatId);
            await SaveUserAsync(user);
        }
    }
}
