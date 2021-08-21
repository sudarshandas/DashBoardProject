using DashBoardProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardProject.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardCardDataDto> GetPendingExportOrder(string channelID, string fromDate, string toDate, bool isPrevious);
        Task<DashboardCardDataDto> GetPendingExportSales(string channelID, string fromDate, string toDate);
        Task<DashboardCardDataDto> GetGUDData(string channelID, string fromDate, string toDate, bool isPrevious);
        Task<DashboardCardDataDto> GetGITData(string channelID, string fromDate, string toDate, bool isPrevious);
        Task<DashboardCardDataDto> GetDomesticSalesData(string channelID, string fromDate, string toDate, int customerType);
        Task<DashboardCardDataDto> GetDomesticPendingOrderData(string channelID, string fromDate, string toDate, int customerType);
        Task<DashboardCardDataDto> GetPCLedgerBalanceData(string channelID, string fromDate, string toDate);
        Task<object> GetColumnWiseSalesData(string channelID, string fromDate, string toDate, string dynamicColumns);
    }
}
