using Crafts.BL.Dtos.CartItemsDtos;
using Crafts.BL.Dtos.ReviewDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.CartItemsRepo;
using Crafts.DAL.Repos.CartRepo;
using Crafts.DAL.Repos.CouponRepo;
using Crafts.DAL.Repos.IdentityRepo;
using Crafts.DAL.Repos.ProductsRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.CartItemsManager
{
    public class CartItemsManager : ICartItemsManager
    {
        private readonly ICartItemRepo _cartItemRepo;
        private readonly ICartRepo _cartRepo;
        private readonly IProductRepo _productRepo;



        public CartItemsManager(ICartItemRepo cartItemRepo, ICartRepo cartRepo, IProductRepo productRepo )
        {
            _cartItemRepo = cartItemRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        #region GetAll
        public List<CartItemReadDto> GetAll()
        {
            List<CartItem> cartItemFromDB = _cartItemRepo.GetAll();
            return cartItemFromDB.Select(c => new CartItemReadDto
            {
                Id = c.Id,
                Quantity = c.Quantity, 
                CartId = c.CartId,
                ProductId = c.ProductId

            }).ToList();
        }
        #endregion

        #region GetById
        public CartItemReadDto GetById(int id)
        {
            var cartItem = _cartItemRepo.GetById(id);
            if (cartItem != null)
            {
                return new CartItemReadDto
                {
                    Id = id,
                    Quantity = cartItem.Quantity,
                    CartId = cartItem.CartId,
                    ProductId= cartItem.ProductId

                };
            }
            else
            {
                throw new ArgumentException($"CartItem with id {id} is not found");
            }
        }
        #endregion

        #region Add

        public async Task Add(CartItemAddDto cartItem)
        {

            var cart = _cartRepo.GetById(cartItem.CartId);
            if (cart is null)
            {
                throw new ArgumentException($"There is no Cart with this Id");
            }
            var product = _productRepo.GetById(cartItem.ProductId);

            if (product is null)
            {
                throw new ArgumentException($"There is no product with this Id");
            }

            CartItem cartItemToAdd = new CartItem
            {
                CartId = cartItem.CartId,   
                ProductId = cartItem.ProductId, 
            
            };
            var existingCartItem = _cartItemRepo.GetByCartIdAndProductId(cartItem.CartId, cartItem.ProductId);
            Console.WriteLine(existingCartItem);
            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity >= product.Quantity)
                {
                    throw new ArgumentException("Requested quantity is greater than the available quantity of the product");
                }
                existingCartItem.Quantity++;
                _cartItemRepo.Update(existingCartItem);
               
            }
            else
            {
                await _cartItemRepo.Add(cartItemToAdd);
            }
            _cartItemRepo.SaveChanges();

        }
        #endregion

        #region Edit

        public void Edit(CartItemEditDto cartItemEditDto, int id)
        {
            // Check if the product, user with the given ID exist
            var cartItemToEdit = _cartItemRepo.GetById(id);

            if (cartItemToEdit is null)
            {
                throw new ArgumentException($"Cart Item with id {id} is not found");
            }

            else
            {
                var product = _productRepo.GetById(cartItemToEdit.ProductId);

                cartItemToEdit.Quantity = cartItemEditDto.Quantity;
                if (cartItemToEdit.Quantity > product.Quantity)
                {
                    throw new ArgumentException("Requested quantity is greater than the available quantity of the product");
                }

                _cartItemRepo.Update(cartItemToEdit);
                _cartItemRepo.SaveChanges();
            }

        }
        #endregion

        #region Delete
        public void Delete(int id)
        {
            var cartItemToDelete = _cartItemRepo.GetById(id);

            if (cartItemToDelete is not null)
            {
                _cartItemRepo.Delete(cartItemToDelete);
                _cartItemRepo.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"CartItem with id {id} is not found");
            }
        }
        #endregion

        #region DeleteAllCartItemsByCartId
        public void DeleteAllCartItemsByCartId(int cartId)
        {
            var cart = _cartRepo.GetById(cartId);
            if (cart != null)
            {
                _cartItemRepo.DeleteAllCartItemsByCartId(cartId);
                _cartItemRepo.SaveChanges();

            }
            else
            {
                throw new ArgumentException($"CartItem with id {cartId} is not found");
            }

        }
# endregion

    }
}


