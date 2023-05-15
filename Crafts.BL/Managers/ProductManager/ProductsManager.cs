using Crafts.BL.Dtos.CouponDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.ProductsRepo;
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

        public ProductsManager(IProductRepo productRepo) {
            _productRepo = productRepo;
        }
        public async Task Add(ProductAddDto productDto)
        {
            var product = new Product
            {
                Title = productDto.Title,
                Price = productDto.Price,
                Rating = productDto.Rating,
                //Image = productDto.Image,
                Quantity = productDto.Quantity,
                IsSale = productDto.IsSale,
                Description = productDto.Description
            };
            await _productRepo.Add(product);
            _productRepo.SaveChanges();
        }
        public List<ProductReadDto> GetAll()
        {
            List<Product> productsFromDB = _productRepo.GetAll();
            return productsFromDB
                .Select(p => new ProductReadDto 
                { Title = p.Title,
                  Price = p.Price,
                  Rating = p.Rating,
                 // Image = p.Image, 
                  Quantity = p.Quantity, 
                  IsSale = p.IsSale, 
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
                   // Image = p.Image,
                    Quantity = p.Quantity,
                    IsSale = p.IsSale,
                    Description = p.Description
                }).FirstOrDefault(p => p.Id == id);
        }
    }
}
