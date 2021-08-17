using DashBoardProject.Dtos;
using DashBoardProject.Repository;
using DashBoardProject.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DashBoardProject.Controllers
{
    [DashboardAuthorize]
    public class HomeController : Controller
    {
        private readonly IDashboardRepository dashboardRepo;
        private readonly ICommonRepository commonRepo;
        public HomeController(IDashboardRepository dashboardRepository, ICommonRepository commonRepository)
        {
            dashboardRepo = dashboardRepository;
            commonRepo = commonRepository;
        }
        // GET: Home
        public async Task<ActionResult> Index()
        {
            var lstChannels = await commonRepo.GetAllChannels("and ShowInDashBoard = 1");

            var selectList = new SelectList(lstChannels, "ChannelID", "ChannelName", null);

            ViewBag.Channels = selectList;


            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DashboardCards(string company, string daterange)
        {
            List<DashboardCardDataDto> dashboardCardDataDtos = new List<DashboardCardDataDto>();
            string strFromDate = string.Empty;
            string strTodate = string.Empty;
            string strParentChildCompany = string.Empty;
            string[] arrdaterange;
            string fromDate = string.Empty;
            string toDate = string.Empty;

            if (string.IsNullOrEmpty(company))
            {
                return null;
            }
            strParentChildCompany = await commonRepo.GetParentChildCompany(company);

            arrdaterange = daterange.Split(new string[] { "-" }, StringSplitOptions.None);
            fromDate = arrdaterange[0].Trim();
            toDate = arrdaterange[1].Trim();

            // Export Order of current FY
            var exportOrder = await dashboardRepo.GetPendingExportOrder(strParentChildCompany, fromDate, toDate, false);
            exportOrder.CardType = 1;
            exportOrder.IsPreviousFY = false;
            dashboardCardDataDtos.Add(exportOrder);

            // Export order of previous FYs
            var previousExportOrder = await dashboardRepo.GetPendingExportOrder(strParentChildCompany, fromDate, toDate, true);
            previousExportOrder.CardType = 1;
            previousExportOrder.IsPreviousFY = true;
            dashboardCardDataDtos.Add(previousExportOrder);

            //GUD of current FY
            var GUD = await dashboardRepo.GetGUDData(strParentChildCompany, fromDate, toDate, false);
            GUD.CardType = 2;
            GUD.IsPreviousFY = false;
            dashboardCardDataDtos.Add(GUD);

            //GUD of Previous FY
            var previousGUD = await dashboardRepo.GetGUDData(strParentChildCompany, fromDate, toDate, true);
            previousGUD.CardType = 2;
            previousGUD.IsPreviousFY = true;
            dashboardCardDataDtos.Add(previousGUD);

            //GIT of current FY
            var GIT = await dashboardRepo.GetGITData(strParentChildCompany, fromDate, toDate, false);
            GIT.CardType = 3;
            GIT.IsPreviousFY = false;
            dashboardCardDataDtos.Add(GIT);

            //GIT of Previous FY
            var previousGIT = await dashboardRepo.GetGITData(strParentChildCompany, fromDate, toDate, true);
            previousGIT.CardType = 3;
            previousGIT.IsPreviousFY = true;
            dashboardCardDataDtos.Add(previousGIT);

            //PendingExportSales of current FY
            var PendingExportSales = await dashboardRepo.GetPendingExportSales(strParentChildCompany, fromDate, toDate);
            PendingExportSales.CardType = 4;
            PendingExportSales.IsPreviousFY = false;
            dashboardCardDataDtos.Add(PendingExportSales);

            //DomesticSalesData of current FY
            var DomesticSalesData = await dashboardRepo.GetDomesticSalesData(strParentChildCompany, fromDate, toDate, 1);
            DomesticSalesData.CardType = 5;
            DomesticSalesData.IsPreviousFY = false;
            dashboardCardDataDtos.Add(DomesticSalesData);

            //DomesticPending Order
            string fromdate1 = arrdaterange[0].Trim().Substring(3, 2) + "/" + arrdaterange[0].Trim().Substring(0, 2) + "/" + arrdaterange[0].Trim().Substring(6, 4);   // "04/01/2021";
            string todate1 = arrdaterange[1].Trim().Substring(3, 2) + "/" + arrdaterange[1].Trim().Substring(0, 2) + "/" + arrdaterange[1].Trim().Substring(6, 4);  // "03/31/2022";

            var DomesticPendingOrder = await dashboardRepo.GetDomesticPendingOrderData(company, fromdate1, todate1, 1);
            DomesticPendingOrder.CardType = 6;
            DomesticPendingOrder.IsPreviousFY = false;
            dashboardCardDataDtos.Add(DomesticPendingOrder);

            //PC Ledger
            var PCLedgerBalance = await dashboardRepo.GetPCLedgerBalanceData(company, fromdate1, todate1);
            PCLedgerBalance.CardType = 7;
            PCLedgerBalance.IsPreviousFY = false;
            dashboardCardDataDtos.Add(PCLedgerBalance);

            //DomesticSale Ellementry
            var DomesticSaleEllementry = await dashboardRepo.GetDomesticSalesData(strParentChildCompany, fromDate, toDate, 2);
            DomesticSaleEllementry.CardType = 8;
            DomesticSaleEllementry.IsPreviousFY = false;
            dashboardCardDataDtos.Add(DomesticSaleEllementry);

            //DomesticPending Order Ellementry
            var DomesticPendingOrderEllementry = await dashboardRepo.GetDomesticPendingOrderData(strParentChildCompany, fromdate1, todate1, 2);
            DomesticPendingOrderEllementry.CardType = 9;
            DomesticPendingOrderEllementry.IsPreviousFY = false;
            dashboardCardDataDtos.Add(DomesticPendingOrderEllementry);

            return Json(dashboardCardDataDtos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ColumnWiseSalesData(string company, string daterange, string columns)
        {
            List<DashboardCardDataDto> dashboardCardDataDtos = new List<DashboardCardDataDto>();
            string strFromDate = string.Empty;
            string strTodate = string.Empty;
            string parentChildCompany = string.Empty;
            string[] arrdaterange;
            string fromDate = string.Empty;
            string toDate = string.Empty;

            if (string.IsNullOrEmpty(company))
            {
                return null;
            }
            parentChildCompany = await commonRepo.GetParentChildCompany(company);

            arrdaterange = daterange.Split(new string[] { "-" }, StringSplitOptions.None);
            fromDate = arrdaterange[0].Trim();
            toDate = arrdaterange[1].Trim();

            var result = dashboardRepo.GetColumnWiseSalesData(parentChildCompany, fromDate, toDate, columns);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}