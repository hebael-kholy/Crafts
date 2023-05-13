using Crafts.BL.Dtos.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers;

public interface IClientManager
{
     UserReadDto? GetById(string id);
    Task Update(UpdateUserDto userDto);
}
