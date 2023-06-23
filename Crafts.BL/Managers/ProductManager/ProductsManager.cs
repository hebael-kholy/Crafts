using Crafts.Api.Controllers;
using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.IdentityDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CategoryRepo;
using Crafts.DAL.Repos.ProductsRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ProductManager
{
    public class ProductsManager : IProductsManager
    {
        private readonly IProductRepo _productRepo;
        private readonly IFileService _fileService;
        private readonly ICategoryRepo _categoryRepo;

        public ProductsManager(IProductRepo productRepo,
            IFileService fileService, ICategoryRepo categoryRepo) {
            _productRepo = productRepo;
            _fileService = fileService;
            _categoryRepo = categoryRepo;
        }
        public async Task Add([FromForm] ProductAddDto productDto)
        {
            if (productDto.Image != null && !IsSupportedImageFormat(productDto.Image))
            {
                throw new ArgumentException("Image file must be in JPEG or PNG or JPG or WEBP format.");
            }

            // Check if the category with the given ID exist
            var category = _categoryRepo.GetById(productDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with id {productDto.CategoryId} is not found");
            }

            var x = upload.UploadImageOnCloudinary(productDto.Image);

            var product = new Product
            {
                Title = productDto.Title,
                Price = productDto.Price,
                Rating = 1,
                Image = x,
                Quantity = productDto.Quantity,
                IsSale = false,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId
            };

            await _productRepo.Add(product);
            _productRepo.SaveChanges();
        }

        public void Update(ProductUpdateDto productUpdateDto, int id)
        {
            var category = _categoryRepo.GetById(productUpdateDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with id {productUpdateDto.CategoryId} is not found");
            }

            Product? ProductToEdit =  _productRepo.GetById(id);  //await
    
            if (ProductToEdit != null) 
            {

                ProductToEdit.Title = productUpdateDto.Title;
                ProductToEdit.Price = productUpdateDto.Price;
                ProductToEdit.Quantity = productUpdateDto.Quantity;
                ProductToEdit.IsSale = productUpdateDto.IsSale;
                ProductToEdit.Description = productUpdateDto.Description;
                ProductToEdit.CategoryId = productUpdateDto.CategoryId;

                _productRepo.Update(ProductToEdit);     //await
                _productRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Product with id {id} is not found");
            }
        }

        public void Delete(int id)
        {
            Product? product = _productRepo.GetById(id);

            if (product != null) 
            { 
                _productRepo.Delete(product);
                _productRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Product with id {id} is not found");
            }
        }

        public List<ProductReadDto> GetAll()
        {
            List<Product> productsFromDB = _productRepo.GetAll();

            List<ProductReadDto> productReadDtos = new List<ProductReadDto>();

            foreach (var p in productsFromDB)
            {
                var category = _categoryRepo.GetById(p.CategoryId);

                var productReadDto = new ProductReadDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Rating = p.Rating,
                    Image = p.Image,
                    Quantity = p.Quantity,
                    IsSale = p.IsSale,
                    CategoryId = p.CategoryId,
                    Description = p.Description,
                    CategoryName = category!.Title
                };

                productReadDtos.Add(productReadDto);
            }
            return productReadDtos;
        }

        public List<ProductReadDto> GetProductwithSale()
        {
            List<Product> productsFromDB = _productRepo.GetProductsWithSale();
            if(productsFromDB != null)
            {
                return productsFromDB
               .Select(p => new ProductReadDto
               {
                   Id = p.Id,
                   Title = p.Title,
                   Price = p.Price,
                   Rating = p.Rating,
                   Image = p.Image,
                   Quantity = p.Quantity,
                   IsSale = p.IsSale,
                   CategoryId = p.CategoryId,
                   Description = p.Description
               }).ToList();
            }
            else
            {
                throw new ArgumentException($"Products With Sale are not found");
            }
        }
        public ProductReadDto GetById(int id)
        {
            var productFromDB = _productRepo.GetById(id);

            if(productFromDB != null)
            {
                var category = _categoryRepo.GetById(productFromDB.CategoryId);
                return new ProductReadDto
                {
                    Id = productFromDB.Id,
                    Title = productFromDB.Title,
                    Price = productFromDB.Price,
                    Rating = productFromDB.Rating,
                    Image = productFromDB.Image,
                    Quantity = productFromDB.Quantity,
                    IsSale = productFromDB.IsSale,
                    Description = productFromDB.Description,
                    CategoryId = productFromDB.CategoryId,
                    CategoryName = category!.Title
                };
            }
            else
            {
                throw new ArgumentException($"Product with id {id} is not found");
            }
        }
        private bool IsSupportedImageFormat(IFormFile file)
        {
            return file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/jpg" || file.ContentType == "image/webp";
        }

        public void AddImage([FromForm] ProductImgAddDto productImgAddDto, int id)
        {
            if (productImgAddDto.Image != null)
            {
                var productToEdit = _productRepo.GetAll().FirstOrDefault(c => c.Id == id);
                if (productToEdit != null)
                {
                    if (productImgAddDto.Image != null && !IsSupportedImageFormat(productImgAddDto.Image))
                    {
                        throw new ArgumentException("Image file must be in JPEG or PNG or JPG or WEBP format.");
                    }

                    var imgURL = upload.UploadImageOnCloudinary(productImgAddDto.Image);

                    productToEdit.Image = imgURL;
                    _productRepo.Update(productToEdit);
                    _productRepo.SaveChanges();
                }
            }
        }
    }
}
