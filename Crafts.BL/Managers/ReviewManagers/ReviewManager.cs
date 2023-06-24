using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CategoryRepo;
using Crafts.DAL.Repos.IdentityRepo;
using Crafts.DAL.Repos.ProductsRepo;
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
        private readonly IProductRepo _productRepo;
        private readonly IUserRepo _userRepo;

        public ReviewManager(IReviewRepo reviewRepo, IProductRepo productRepo, IUserRepo userRepo)
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
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
        public List<ReviewReadDto> GetReviewsByProductId(int id)
        {
            List<Review> reviewsFromDb = _reviewRepo.GetReviewsByProductId(id);
            return reviewsFromDb.Select(r => new ReviewReadDto
            {
                Id = r.Id,
                Content = r.Content,
                ProductId = r.ProductId,
                UserId = r.UserId,
                Image = r.User.Image,
                UserName = r.User.UserName
            }).ToList();
        }

        public ReviewReadDto GetById(int id)
        {
            var review = _reviewRepo.GetReviewWithProductAndUser(id);
            if(review != null)
            {
                return new ReviewReadDto
                {
                    Id = id,
                    Content = review.Content,
                    ProductId = review.ProductId,
                    UserId = review.UserId
                };
            }
            else
            {
                throw new ArgumentException($"Review with id {id} is not found");
            }
        }

        public async Task Add(ReviewAddDto reviewAddDto)
        {
            // Check if the product, user with the given ID exist
            var product = _productRepo.GetById(reviewAddDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with id {reviewAddDto.ProductId} is not found");
            }

            var user = _userRepo.GetUserById(reviewAddDto.UserId.ToString());
            if (user == null)
            {
                throw new ArgumentException($"User with id {reviewAddDto.UserId} is not found");
            }

            Review reviewToAdd = new Review
            {
                Content = reviewAddDto.Content,
                ProductId = reviewAddDto.ProductId,
                UserId = reviewAddDto.UserId,
            };
            await _reviewRepo.Add(reviewToAdd);
            _reviewRepo.SaveChanges();
        }

        public void Edit(ReviewEditDto reviewEditDto, int id)
        {
            // Check if the product, user with the given ID exist
            var product = _productRepo.GetById(reviewEditDto.ProductId);

            if (product == null)
            {
                throw new ArgumentException($"Product with id {reviewEditDto.ProductId} is not found");
            }

            var user = _userRepo.GetUserById(reviewEditDto.UserId.ToString());
            if (user == null)
            {
                throw new ArgumentException($"User with id {reviewEditDto.UserId} is not found");
            }

            var reviewToEdit = _reviewRepo.GetById(id);
            if (reviewToEdit != null)
            {
                reviewToEdit.Content = reviewEditDto.Content;
                reviewToEdit.ProductId = reviewEditDto.ProductId;
                reviewToEdit.UserId = reviewEditDto.UserId;

                _reviewRepo.Update(reviewToEdit);
                _reviewRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Review with id {id} is not found");
            }
        }
        public void Delete(int id)
        {
            var reviewToDelete = _reviewRepo.GetById(id);

            if (reviewToDelete != null)
            {
                _reviewRepo.Delete(reviewToDelete);
                _reviewRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Review with id {id} is not found");
            }
        }
    }
}
