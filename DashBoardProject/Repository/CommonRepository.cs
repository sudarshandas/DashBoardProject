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
    public class CommonRepository : ICommonRepository
    {
        public async Task<List<Channels>> GetAllChannels(string filter)
        {
            List<Channels> channels = new List<Channels>();

            string sql = "Select * from Channels where ChannelCode in('DileepIND','DPPL','DTCE','SMILEINTE') " + filter + " order by ChannelID";

            DAL<Channels> dal = new DAL<Channels>();

            try
            {
                channels = (await dal.GetRecords(sql, CommandType.Text, null)).ToList();
            }
            catch (Exception ex)
            {

            }
            return channels;
        }

        public async Task<string> GetChildCompany(string parentChannelID)
        {
            string channels = string.Empty;
            SqlParameter[] arrSqlParam = new SqlParameter[1]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = parentChannelID
                }
            };

            string sql = "SELECT ',' + cast(ChannelID AS nvarchar) " +
            " FROM Channels where ChannelParentID in (select value from udf_SplitString(@channelID,',')) " +
            " and ChannelCode in('DIPL-I','DIPL-II','DIPL-III','DIPL-IV','DIPL-V','DIPL-VI','DIPL-VII','DTCD','DTC') " +
            " ORDER BY LEN(ChannelID) " +
            " FOR XML PATH('') ";

            DAL<CommonRepository> dal = new DAL<CommonRepository>();

            try
            {
                channels = await dal.GetSingleValue(sql, CommandType.Text, arrSqlParam);
                if (!string.IsNullOrEmpty(channels))
                {
                    channels = channels.Substring(1);
                }
            }
            catch (Exception ex)
            {

            }
            return channels;
        }

        public async Task<string> GetParentChildCompany(string parentChannelID)
        {
            string channels = string.Empty;
            string childChannelID = await GetChildCompany(parentChannelID);

            if (!string.IsNullOrEmpty(childChannelID))
            {
                channels = parentChannelID + "," + childChannelID; ;
            }
            else
            {
                channels = parentChannelID;
            }

            return channels;
        }
    }
}