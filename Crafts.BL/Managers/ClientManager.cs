using Crafts.BL.Dtos.IdentityDtos;
using Crafts.DAL.Repos.IdentityRepo;
using Microsoft.AspNetCore.Hosting;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers;

public class ClientManager : IClientManager
{
    private readonly IUserRepo _UserRepo;
    private readonly IWebHostEnvironment _env;
    public ClientManager(IUserRepo UserRepo, IWebHostEnvironment env)
    {
        _UserRepo = UserRepo;
        _env = env;
    }

    public UserReadDto? GetById(string id)
    {
        List<User> UserFromDB = _UserRepo.GetAll();

        return UserFromDB
            .Select(U => new UserReadDto
            {
                Id = U.Id,
                Email = U.Email,
                Gender = U.Gender,
                UserName = U.UserName,
                Image = U.Image
            })
            .FirstOrDefault(c => c.Id == id);
    }

    public async Task Update([FromForm]UpdateUserDto userDto)
    {

        //var imagePath = Path.Combine(_env.WebRootPath, "images", userDto.Image.FileName);

        //var stream = new FileStream(imagePath, FileMode.Append);
        //userDto.Image.CopyTo(stream);

        //var existingUser = _UserRepo.GetUserByEmail(userDto.Email);

        //if (existingUser is null)
        //{

        //}
        //var User = new User
        //{
        // Email = userDto.Email,
        //Image = imagePath,
        //UserName = userDto.UserName
        //  };
        
        //await _UserRepo.Update(User);
        //_UserRepo.SaveChanges();
    }

}
