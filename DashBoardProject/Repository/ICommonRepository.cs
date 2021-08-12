using DashBoardProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardProject.Repository
{
    public interface ICommonRepository
    {
        Task<string> GetChildCompany(string parentChannelID);
        Task<string> GetParentChildCompany(string parentChannelID);
        Task<List<Channels>> GetAllChannels(string filter);
    }
}
