using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.IdentityRepo;

public interface IUserRepo : IGenericRepo<User>
{
    User? GetUserById(string id);
    public User GetUserByEmail(string email);
}
