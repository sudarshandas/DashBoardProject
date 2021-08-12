using DashBoardProject.Dtos;
using DashBoardProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardProject.Repository
{
    public interface IAccountRepository
    {
        Task<Users> GetUserByUserNameAndPassword(string userName, string password);
        Task<List<UserMenuDto>> GetUserMenu(long userid);
    }
}
