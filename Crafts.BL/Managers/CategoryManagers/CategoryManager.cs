using Crafts.Api.Controllers;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CategoryRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
            if (category != null)
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
            if (category != null)
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

        public void AddImage([FromForm] CategoryImgAddDto categoryImgAddDto, int id)
        {
            if (categoryImgAddDto.Image != null)
            {
                var categoryToEdit = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == id);
                if (categoryToEdit != null)
                {
                    if (categoryImgAddDto.Image != null && !IsSupportedImageFormat(categoryImgAddDto.Image))
                    {
                        throw new ArgumentException("Image file must be in JPEG or PNG or JPG or WEBP format.");
                    }

                    var imgURL = upload.UploadImageOnCloudinary(categoryImgAddDto.Image);

                    categoryToEdit.Image = imgURL;
                    _categoryRepo.Update(categoryToEdit);
                    _categoryRepo.SaveChanges();

                    //var fileResult = _fileService.SaveImage(categoryImgAddDto.Image);

                    ////When Item1 in Tuple = 1 --> this means image saved successfully
                    ////When Item1 in Tuple = 0 --> this means image not saved successfully
                    //if (fileResult.Item1 == 1)
                    //{
                    //    categoryToEdit.Image = fileResult.Item2;
                    //    _categoryRepo.Update(categoryToEdit);
                    //    _categoryRepo.SaveChanges();
                    //}
                }
            }
        }
        private bool IsSupportedImageFormat(IFormFile file)
        {
            return file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/jpg"|| file.ContentType == "image/webp";
        }

        public async Task Add([FromForm] CategoryAddDto categoryAddDto)
        {

            //if (fileResult.Item1 == 1)
            //{
            // All code was here
            //}
            // fileResult = _fileService.SaveImage(categoryAddDto.Image);

            if (categoryAddDto.Image != null && !IsSupportedImageFormat(categoryAddDto.Image))
            {
                throw new ArgumentException("Image file must be in JPEG or PNG or JPG or WEBP format.");
            }

            var x = upload.UploadImageOnCloudinary(categoryAddDto.Image);

            Category categoryToAdd = new Category
            {
                Title = categoryAddDto.Title,
                Image = x
                //Image = fileResult.Item2
            };

            await _categoryRepo.Add(categoryToAdd);
            _categoryRepo.SaveChanges();

            //var categoryTitle = _categoryRepo.GetByName(categoryAddDto.Title);
            //if (categoryTitle != null)
            //{
            //    if (categoryTitle.Title.Equals(categoryAddDto.Title) && categoryTitle != null)
            //    {
            //        throw new ArgumentException($"Category Name '{categoryAddDto.Title}' Exists");
            //    }
            //}
            //else{
                
            //}
        }

        public void Edit(CategoryEditDto categoryEditDto, int id)
        {
            var categoryToEdit = _categoryRepo.GetById(id);
            //var categoryTitle = _categoryRepo.GetByName(categoryEditDto.Title);

            if (categoryToEdit != null)
            {
                categoryToEdit!.Title = categoryEditDto.Title;
                _categoryRepo.Update(categoryToEdit);
                _categoryRepo.SaveChanges();

                //if (categoryEditDto.Title.Equals(categoryToEdit.Title))
                //{
                //    //throw new ArgumentException($"Category Name '{categoryEditDto.Title}' Exists");

                //    categoryToEdit!.Title = categoryToEdit.Title;
                //    _categoryRepo.Update(categoryToEdit);
                //    _categoryRepo.SaveChanges();
                //}
                //else
                //{
                //}
            }
            else
            {
                throw new ArgumentException($"Category with id {id} is not found");
            }
        }

        public void Delete(int id)
        {
            var categoryToDelete = _categoryRepo.GetById(id);

            if (categoryToDelete != null)
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
