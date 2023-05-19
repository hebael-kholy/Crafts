using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Managers.Services;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.ProductsRepo;
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

        public ProductsManager(IProductRepo productRepo,
            IFileService fileService) {
            _productRepo = productRepo;
            _fileService = fileService;
        }
        public async Task Add(ProductAddDto productDto)
        {
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
            Product? ProductToEdit =  _productRepo.GetById(id);  //await
    
            if (ProductToEdit == null) { return; }

            ProductToEdit.Title = productUpdateDto.Title != "string" ? productUpdateDto.Title : ProductToEdit.Title;
            ProductToEdit.Price = productUpdateDto.Price != 0 ? productUpdateDto.Price : ProductToEdit.Price;
            ProductToEdit.Quantity = productUpdateDto.Quantity != 0 ? productUpdateDto.Quantity : ProductToEdit.Quantity;
            ProductToEdit.IsSale = productUpdateDto.IsSale;
            ProductToEdit.Description = productUpdateDto.Description != "string" ? productUpdateDto.Description : ProductToEdit.Description; ;
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

        public void AddImage([FromForm] ProductImgAddDto productImgAddDto, int id)
        {
            if (productImgAddDto.Image != null)
            {
                var categoryToEdit = _productRepo.GetAll().FirstOrDefault(c => c.Id == id);
                if (categoryToEdit != null)
                {
                    var fileResult = _fileService.SaveImage(productImgAddDto.Image);

                    //When Item1 in Tuple = 1 --> this means image saved successfully
                    //When Item1 in Tuple = 0 --> this means image not saved successfully
                    if (fileResult.Item1 == 1)
                    {
                        categoryToEdit.Image = fileResult.Item2;
                        _productRepo.Update(categoryToEdit);
                        _productRepo.SaveChanges();
                    }
                }
            }
        }
    }
}
