using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CategoryRepo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CategoryManagers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepo _categoryRepo;

        private readonly IFileService _fileService;

        public CategoryManager(ICategoryRepo categoryRepo, IFileService fileService) 
        {
            _categoryRepo = categoryRepo;
            _fileService = fileService;
        }

        public List<CategoryReadDto> GetAll()
        {
            List<Category> categoriesFromDb = _categoryRepo.GetAll();
            return categoriesFromDb.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Image = c.Image,
                Title = c.Title,
            }).ToList();
        }

        public CategoryReadDto GetById(int id)
        {
            var category = _categoryRepo.GetById(id);
            if(category != null)
            {
                return new CategoryReadDto
                {
                    Id = category.Id,
                    Image = category.Image,
                    Title = category.Title,
                };
            }
            else
            {
                throw new ArgumentException($"Category with id {id} is not found");
            }
        }

        public CategoryWithProductsDto GetByIdWithProducts(int id)
        {
            var category = _categoryRepo.GetByIdWithProducts(id);
            if(category != null)
            {
                return new CategoryWithProductsDto
                {
                    Id = category.Id,
                    Image = category.Image,
                    Title = category.Title,
                    //We used Select To convert from list of products to list of ProductsReadDto
                    Products = category.Products.Select(p => new ProductReadDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Price = p.Price,
                        Rating = p.Rating,
                        Image = p.Image,
                        Quantity = p.Quantity,
                        IsSale = p.IsSale,
                        Description = p.Description,
                        CategoryId = p.CategoryId,
                    }).ToList()
                };
            }
            else
            {
                throw new ArgumentException($"Category with id {id} is not found");
            }
        }

        public void AddImage([FromForm]CategoryImgAddDto categoryImgAddDto, int id)
        {
            if(categoryImgAddDto.Image != null)
            {
                var categoryToEdit = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == id);
                if (categoryToEdit != null)
                {
                    var fileResult = _fileService.SaveImage(categoryImgAddDto.Image);

                    //When Item1 in Tuple = 1 --> this means image saved successfully
                    //When Item1 in Tuple = 0 --> this means image not saved successfully
                    if (fileResult.Item1 == 1)
                    {
                        categoryToEdit.Image = fileResult.Item2;
                        _categoryRepo.Update(categoryToEdit);
                        _categoryRepo.SaveChanges();
                    }
                }
            }
        }

        public async Task Add(CategoryAddDto categoryAddDto)
        {
            Category categoryToAdd = new Category
            {
                Title = categoryAddDto.Title,
                Image = "https://epin-sam.s3.ap-south-1.amazonaws.com/media/images/category/default.png"
            };
            await _categoryRepo.Add(categoryToAdd);
            _categoryRepo.SaveChanges();
        }

        public void Edit(CategoryEditDto categoryEditDto, int id)
        {
            var categoryToEdit = _categoryRepo.GetById(id);
            if (categoryToEdit != null)
            {
                categoryToEdit.Title = categoryEditDto.Title;
                _categoryRepo.Update(categoryToEdit);
                _categoryRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Category with id {id} is not found");
            }
        }

        public void Delete(int id)
        {
            var categoryToDelete = _categoryRepo.GetById(id);

            if(categoryToDelete != null)
            {
                _categoryRepo.Delete(categoryToDelete);
                _categoryRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Category with id {id} is not found");
            }
        }
    }
}
