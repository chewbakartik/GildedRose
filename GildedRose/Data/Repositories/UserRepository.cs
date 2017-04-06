using GildedRose.Data.Abstract;
using GildedRose.Entities;

namespace GildedRose.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(ApiContext context) : base(context) {}

        public User GetByAuthId(string authId)
        {
            return this.Get(x => x.AuthId == authId);
        }
    }
}
