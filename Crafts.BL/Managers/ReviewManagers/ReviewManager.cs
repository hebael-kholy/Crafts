using Crafts.BL.Dtos.ReviewDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CategoryRepo;
using Crafts.DAL.Repos.ReviewRepo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ReviewManagers
{
    public class ReviewManager : IReviewManager
    {
        private readonly IReviewRepo _reviewRepo;

        public ReviewManager(IReviewRepo reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public List<ReviewReadDto> GetAll()
        {
            List<Review> reviewsFromDb = _reviewRepo.GetAll();
            return reviewsFromDb.Select(r => new ReviewReadDto
            {
                Id = r.Id,
                Content = r.Content,
                ProductId = r.ProductId,
                UserId = r.UserId,
            }).ToList();
        }

        public ReviewReadDto GetById(int id)
        {
            var reviewFromDb = _reviewRepo.GetReviewWithProductAndUser(id);
            if(reviewFromDb is null)
            {
                return null!;
            }
            return new ReviewReadDto
            {
                Id = id,
                Content = reviewFromDb.Content,
                ProductId = reviewFromDb.ProductId,
                UserId = reviewFromDb.UserId
            };
        }

        public async Task Add(ReviewAddDto reviewAddDto, int productId, int userId)
        {
            //// Check if the product with the given ID exists
            //var product = _productRepo.GetById(productId);
            //if (product == null)
            //{
            //    throw new ArgumentException($"Product with ID {productId} not found");
            //}
            //var user = _userRepo.GetById(userId);
            //if (user == null)
            //{
            //    throw new ArgumentException($"User with ID {userId} not found");
            //}
            Review reviewToAdd = new Review
            {
                Content = reviewAddDto.Content,
                ProductId = productId,
                UserId = userId,
            };
            await _reviewRepo.Add(reviewToAdd);
            _reviewRepo.SaveChanges();
        }
    }
}
