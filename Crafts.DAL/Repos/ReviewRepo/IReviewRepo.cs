using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.DAL.Repos.ReviewRepo
{
    public interface IReviewRepo : IGenericRepo<Review>
    {
        Review GetReviewWithProductAndUser(int id);
        List<Review> GetReviewsByProductId(int id);
    }
}
