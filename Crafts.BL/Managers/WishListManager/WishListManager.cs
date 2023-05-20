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
    
    public List<WishListReadDto> GetAll()
    {
        List<Wishlist> WishListsFromDb = _wishListRepo.GetAll();
        return WishListsFromDb.Select(w => new WishListReadDto
        {
            Id = w.Id,
            CreatedAt= w.CreatedAt,
            UserId = w.UserId,
        }).ToList();
    }
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
