using Crafts.DAL.Context;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace Crafts.DAL.Repos.ReviewRepo
{
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {
        private readonly CraftsContext _context;
        public ReviewRepo(CraftsContext context) : base(context)
        {
            _context = context;
        }

        public Review GetReviewWithProductAndUser(int id)
        {
            return _context.Set<Review>()
                .Include(r => r.ProductId)
                .Include(r => r.UserId)
                .FirstOrDefault(r => r.Id == id)!;
        }
    }
}
