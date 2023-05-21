using Crafts.BL.Dtos.CategoryDtos;
using Crafts.BL.Dtos.ProductDtos;
using Crafts.BL.Dtos.WishListDto;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.IdentityRepo;
using Crafts.DAL.Repos.ProductsRepo;
using Crafts.DAL.Repos.WishListRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static System.Net.Mime.MediaTypeNames;

namespace Crafts.BL.Managers.WishListManager;

public class WishListManager : IWishListManager
{
    private readonly IWishListRepo _wishListRepo;
    private readonly IProductRepo _productRepo;
    private readonly IUserRepo _userRepo;

    public WishListManager(IWishListRepo wishListRepo,
        IProductRepo productRepo,
        IUserRepo userRepo)
    {
        _wishListRepo = wishListRepo;
        _productRepo = productRepo;
        _userRepo = userRepo;
    }

    //add product to wishlist
    public void AddProductToWishlistAsync(int productId, int wishlistId)
    {

        var wishlist = _wishListRepo.GetById(wishlistId);
        var product = _productRepo.GetById(productId);

        if (wishlist is not null)
        {
            if (wishlist.Products.Any(p => p.Id == productId))
            {
                throw new ArgumentException("Product already exists in wishlist");
            }
        }

        wishlist.Products.Add(product);
        _wishListRepo.Update(wishlist);
        _wishListRepo.SaveChanges();

    }
    //Remove Product From WishList      //=> Don't Delete from DB =>
    public void DeleteProductFromWishListAsync(int productId, int wishlistId)
    {
        var wishlist = _wishListRepo.GetById(wishlistId);
       // var product = _productRepo.GetById(productId);

        if (wishlist is not null)
        {
            var product = wishlist.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                wishlist.Products.Remove(product);
                _wishListRepo.Update(wishlist);
                _wishListRepo.SaveChanges();
            }
        }
    }
    //add wishlist to user
    public async Task Add(WishListAddDto wishListAddDto)
    {
        var userId = wishListAddDto.UserId;
        Wishlist? wishlist = _wishListRepo.GetUserWishList(userId);
        User? user = _userRepo.GetUserById(userId);

        if (user is null)
        {
            throw new ArgumentException($"There is no user with id {userId}");
        }
        if (wishlist is null)
        {
            Wishlist newWishList = new Wishlist
            {
                CreatedAt = DateTime.Now,
                UserId = wishListAddDto.UserId
            };
            await _wishListRepo.Add(newWishList);
        }
        else
        {
            throw new ArgumentException($"This user has already a wishList with Id {wishlist.Id}");
        }
        _wishListRepo.SaveChanges();
    }

    //delete wishlist by id
    public void Delete(int id)
    {
        var wishListToDelete = _wishListRepo.GetById(id);

        if (wishListToDelete != null)
        {
            _wishListRepo.Delete(wishListToDelete);
            _wishListRepo.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"WishList with id {id} is not found");
        }
    }

    //get Wishlist by id
    public WishListReadDto GetById(int id)
    {
        var wishList = _wishListRepo.GetById(id);
        if (wishList != null)
        {
            return new WishListReadDto
            {
                Id = wishList.Id,
                CreatedAt = wishList.CreatedAt,
                UserId = wishList.UserId
            };
        }
        else
        {
            throw new ArgumentException($"WishList with id {id} is not found");
        }


    }

    //Get WishList with it's Products
    public WishlistWithProductsDto GetByIdWithProducts(int id)
    {
        Wishlist? wishList = _wishListRepo.GetByIdWithProducts(id);
        if (wishList != null)
        {
            return new WishlistWithProductsDto
            {
                Id = wishList.Id,
                userId = wishList.UserId,
                Products = wishList.Products.Select(w=> new ProductReadDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Price = w.Price,
                    Rating = w.Rating,
                    Image = w.Image,
                    Quantity = w.Quantity,
                    IsSale = w.IsSale,
                    Description = w.Description,
                    CategoryId = w.CategoryId
                }).ToList()
            };
        }
        else
        {
            throw new ArgumentException($"WishList with id {id} is not found");
        }


    }
    //get all wishLists             //=> Don't get all products =>
    public List<WishListReadDto> GetAll()
    {
   
        List<Wishlist> WishListsFromDb = _wishListRepo.GetAll();
        return WishListsFromDb.Select(w => new WishListReadDto
        {
            Id = w.Id,
            UserId = w.UserId,
            CreatedAt = w.CreatedAt
           // Products= w.Products.Select(p => new ProductReadDto { Id = p.Id, CategoryId = p.CategoryId }).ToList(),
        }).ToList();
    }
    //get user wishList
    public WishListReadDto GetUserWishList(string userId)
    {
        var wishlist = _wishListRepo.GetUserWishList(userId);
        if (wishlist != null)
        {
            return new WishListReadDto
            {
                Id = wishlist.Id,
                CreatedAt = wishlist.CreatedAt,
                UserId = wishlist.UserId,
                Products = wishlist.Products.Select(p => new Product
                {
                    Id = p.Id
                }).ToList()
            };
        }
        else
        {
            throw new ArgumentException($"user with id {userId} dosen't have a wishlist");
        }
    }

  

    }
