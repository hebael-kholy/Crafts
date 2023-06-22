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
        public async Task Add(ProductAddDto productDto)
        {
            // Check if the category with the given ID exist
            var category = _categoryRepo.GetById(productDto.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with id {productDto.CategoryId} is not found");
            }

            var product = new Product
            {
                Title = productDto.Title,
                Price = productDto.Price,
                Rating = productDto.Rating,
                Image = "https://epin-sam.s3.ap-south-1.amazonaws.com/media/images/category/default.png",
                Quantity = productDto.Quantity,
                IsSale = productDto.IsSale,
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
    
            if (ProductToEdit == null) { return; }

            ProductToEdit.Title = productUpdateDto.Title == "" ? ProductToEdit.Title : productUpdateDto.Title; 
            ProductToEdit.Price = productUpdateDto.Price != 0 ? productUpdateDto.Price : ProductToEdit.Price;
            ProductToEdit.Quantity = productUpdateDto.Quantity != 0 ? productUpdateDto.Quantity : ProductToEdit.Quantity;
            if (productUpdateDto.IsSale == true) { ProductToEdit.IsSale = productUpdateDto.IsSale; }
            else { ProductToEdit.IsSale = ProductToEdit.IsSale; }
            ProductToEdit.Description = productUpdateDto.Description != "" ? productUpdateDto.Description : ProductToEdit.Description; ;
            ProductToEdit.CategoryId = productUpdateDto.CategoryId != 0 ? productUpdateDto.CategoryId : ProductToEdit.CategoryId;

             _productRepo.Update(ProductToEdit);     //await
            _productRepo.SaveChanges();
        }

        public void Delete(int id)
        {
            Product? product = _productRepo.GetById(id);

            if (product is null) { return; }

            _productRepo.Delete(product);
            _productRepo.SaveChanges();
        }

        public List<ProductReadDto> GetAll()
        {
            List<Product> productsFromDB = _productRepo.GetAll();
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
                  Description = p.Description }).ToList();
        }
        public List<ProductReadDto> GetProductwithSale()
        {
            List<Product> productsFromDB = _productRepo.GetProductsWithSale();
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
        public ProductReadDto? GetById(int id)
        {
            List<Product> productFromDB = _productRepo.GetAll();
            return productFromDB
                .Select(p => new ProductReadDto
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
                }).FirstOrDefault(p => p.Id == id);
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
