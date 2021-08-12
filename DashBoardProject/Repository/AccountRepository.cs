using DashBoardProject.Dtos;
using DashBoardProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DashBoardProject.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Users> GetUserByUserNameAndPassword(string userName, string password)
        {
            Users users = new Users();

            SqlParameter[] arrSqlParam = new SqlParameter[2]
            {
                new SqlParameter()
                {
                    ParameterName = "@userName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = userName
                },
                new SqlParameter()
                {
                    ParameterName = "@password",
                    SqlDbType = SqlDbType.VarChar,
                    Value = password
                }
            };

            string sql = "Select UserPersonName,UserName,UserPassword,UserID from tblUsers " +
                "where upper(rtrim(UserName))=@userName " +
                "and rtrim(convert(varchar(100), DecryptByPassPhrase('dileep2020', UserPassword_1)))=@password " +
                "and UserActive=1";

            DAL<Users> dal = new DAL<Users>();
            try
            {
                users = (await dal.GetRecord(sql, CommandType.Text, arrSqlParam));
                return users;
            }
            catch
            {
                return null;
            }


        }

        public async Task<List<UserMenuDto>> GetUserMenu(long userid)
        {
            IEnumerable<UserMenuDto> lstuserMenuDto;

            SqlParameter[] arrSqlParam = new SqlParameter[1]
            {
                new SqlParameter()
                {
                    ParameterName = "@userid",
                    SqlDbType = SqlDbType.BigInt,
                    Value = userid
                }
            };

            string sql = "SELECT c.MenuName,c.ParentMenuName,c.DisplayName,c.ControllerName,c.ActionName FROM tblUserGroupAndMenu a " +
                "INNER JOIN tblUserAndGroup b ON a.GroupId = b.GroupId INNER JOIN tblMenus c ON a.MenuId = c.Id "+
                "WHERE c.IsActive = 1 AND(ISNULL(c.ParentMenuName,'') <> '') AND b.UserID = @userid " +
                "GROUP BY c.ParentMenuName,c.DisplayName,c.ControllerName,c.Id,c.MenuName,ActionName ORDER BY c.Id";

            DAL<UserMenuDto> dal = new DAL<UserMenuDto>();
            try
            {
                lstuserMenuDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam));
                return lstuserMenuDto.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}