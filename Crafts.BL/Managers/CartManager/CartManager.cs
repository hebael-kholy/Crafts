using Crafts.BL.Dtos.CartDtos;
using Crafts.BL.Dtos.CartItemsDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CartRepo;
using Crafts.DAL.Repos.CouponRepo;
using Crafts.DAL.Repos.IdentityRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Crafts.BL.Managers.CartManager
{
    public class CartManager : ICartManager
    {
        private readonly ICartRepo _cartRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICouponRepo _couponRepo;

        public CartManager(ICartRepo cartRepo, IUserRepo userRepo, ICouponRepo couponRepo)
        {
            _cartRepo = cartRepo;
            _userRepo = userRepo;
            _couponRepo = couponRepo;


        }
        #region GetAll
        public List<CartReadDto> GetAll()
        {
            List<Cart> cartFromDB = _cartRepo.GetAll();
            return cartFromDB.Select(c => new CartReadDto
            {
                Id = c.Id,
                UserId = c.UserId

            }).ToList();
        }
        #endregion

        #region GetById(Depricated)
        public CartReadDto GetById(int id)
        {
            var cart = _cartRepo.GetById(id);
            if (cart != null)
            {
                return new CartReadDto
                {
                    Id = id,
                    UserId = cart.UserId
                };
            }
            else
            {
                throw new ArgumentException($"Cart with id {id} is not found");
            }
        }
        #endregion

        #region Add
        public async Task Add(CartAddDto cart)
        {
            var user = _userRepo.GetUserById(cart.UserId);
            if (user is null)
            {
                throw new ArgumentException($"User Cart is not found");
            }
            Cart cartToAdd = new Cart
            {
                UserId = cart.UserId,

            };
            await _cartRepo.Add(cartToAdd);
            _cartRepo.SaveChanges();
        }

        public void Delete(int id)
        {
            var cartToDelete = _cartRepo.GetById(id);

            if (cartToDelete is not null)
            {
                _cartRepo.Delete(cartToDelete);
                _cartRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Cart with id {id} is not found");
            }
        }
        #endregion

        #region GetByIdWithCartItems
        public CartWithCartItemsReadDto GetByIdWithCartItems(int id)
        {
            Cart? cart = _cartRepo.GetByIdWithCartItems(id);
            if (cart is null)
            {
                throw new ArgumentException($"There is no Cart with this Id");
            }
            cart.Quantity = cart.CartItems.Count;
            cart.TotalPrice = cart.CalculateTotalPrice();
            return new CartWithCartItemsReadDto
            {
                Id = id,
                Quantity = cart.Quantity,
                TotalPrice = cart.TotalPrice,
                TotalPriceAfterDiscount = cart.TotalPriceAfterDiscount,
                CartItems = cart.CartItems.Select(c => new CartItemsChildReadDto
                {
                    Id = c.Id,
                    Quantity = c.Quantity,

                }).ToList()
            };

        }
        #endregion

        #region ApplyCouponForDiscount
        public void ApplyCouponForDiscount(int cartId, int couponId)
        {
            var cart = _cartRepo.GetByIdWithCartItems(cartId);
            var coupon = _couponRepo.GetById(couponId);

            if (cart is null)
            {
                throw new ArgumentException($"There is no cart with id {cartId}");
            }
            if (coupon is null)
            {
                throw new ArgumentException($"There is no cart with id {couponId}");
            }
            var totalPrice = cart.CalculateTotalPrice();
            var totalAfterDiscount = (totalPrice - (totalPrice * coupon.Discount / 100));
            cart.TotalPriceAfterDiscount = totalAfterDiscount;
            _cartRepo.SaveChanges();
        }
        #endregion


    }
}

