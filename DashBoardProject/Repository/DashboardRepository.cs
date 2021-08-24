using DashBoardProject.Dtos;
using DashBoardProject.Extensions;
using DashBoardProject.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;

namespace DashBoardProject.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        public async Task<DashboardCardDataDto> GetDomesticPendingOrderData(string channelID, string fromDate, string toDate, int customerType)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            SqlParameter[] arrSqlParam = new SqlParameter[7]
            {
                new SqlParameter()
                {
                    ParameterName = "@reportType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "TotalAmount"
                },
                new SqlParameter()
                {
                    ParameterName = "@channelId",
                    SqlDbType = SqlDbType.VarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@columns",
                    SqlDbType = SqlDbType.VarChar,
                    Value = ""
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = toDate
                },
                new SqlParameter()
                {
                    ParameterName = "@isPrintCurSymbol",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "no"
                },
                new SqlParameter()
                {
                    ParameterName = "@customerType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = customerType
                }
            };
            string sql = string.Empty;
            sql = "sp_Qry1016_PendingDomesticOrder";

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                var documentValue = (await dal.GetSingleValue(sql, CommandType.StoredProcedure, arrSqlParam));
                if (string.IsNullOrEmpty(documentValue))
                {
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentSumValue = Math.Round(Convert.ToDecimal(documentValue) / 10000000, 2);
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentSumValue = 0;
            }

            arrSqlParam = new SqlParameter[7]
            {
                new SqlParameter()
                {
                    ParameterName = "@reportType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "InvoiceCount"
                },
                new SqlParameter()
                {
                    ParameterName = "@channelId",
                    SqlDbType = SqlDbType.VarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@columns",
                    SqlDbType = SqlDbType.VarChar,
                    Value = ""
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = toDate
                },
                new SqlParameter()
                {
                    ParameterName = "@isPrintCurSymbol",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "no"
                },
                new SqlParameter()
                {
                    ParameterName = "@customerType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = customerType
                }
            };
            sql = "sp_Qry1016_PendingDomesticOrder";
            try
            {
                var documentCount = (await dal.GetSingleValue(sql, CommandType.StoredProcedure, arrSqlParam));
                if (string.IsNullOrEmpty(documentCount))
                {
                    dashboardCardDataDto.DocumentCount = "0";
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = documentCount;
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentCount = "0";
            }

            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetDomesticSalesData(string channelID, string fromDate, string toDate, int customerType)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            if (customerType == 1)
            {
                sql = "select BillNum As DocumentNo,sum(ProdAssessableValue) AS DocumentValue from DomasticSalesReport " +
                " where AccTransDate >= CONVERT(date, @fromDate, 103) " +
                " AND AccTransDate <= CONVERT(date, @toDate, 103) " +
                " AND ChannelID in(select value from udf_SplitString(@channelID,',')) " +
                " AND ContactId not in(select CustomerID from EllementryGroupCustomer) " +
                " GROUP BY BillNum ";
            }

            if (customerType == 2)
            {
                sql = "select BillNum As DocumentNo,sum(ProdAssessableValue) AS DocumentValue from DomasticSalesReport " +
                " where AccTransDate >= CONVERT(date, @fromDate, 103) " +
                " AND AccTransDate <= CONVERT(date, @toDate, 103) " +
                " AND ChannelID in(select value from udf_SplitString(@channelID,',')) " +
                " AND ContactId in(select CustomerID from EllementryGroupCustomer where CustomerType=2) " +
                " GROUP BY BillNum ";
            }

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "0";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentCount = "0";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetGITData(string channelID, string fromDate, string toDate, bool isPrevious)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();

            string dateCondition = string.Empty;
            string toDateString = string.Empty;
            if (isPrevious)
            {
                toDate = fromDate;
                fromDate = "01/04/2018";
                toDateString = "CONVERT(date, @toDate, 103)";
            }
            else
            {
                dateCondition = " OR BillLadingDate > CONVERT(date, @toDate, 103)";
                toDateString = "Dateadd(DAY, 1, CONVERT(date, @toDate, 103))";
            }

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;

            sql = "select InvoiceNo As DocumentNo,SUM(ProdDocValueINR) as DocumentValue from GITData " +
            "where ChannelID IN(select value from udf_SplitString(@channelID,',')) AND LRDate >= CONVERT(date, @fromDate, 103) " +
            "AND LRDate < " + toDateString + " AND (BillLadingDate IS NULL " + dateCondition + ") and LRDate IS NOT NULL " +
            "group by InvoiceNo, ProdDocValueINR ";

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "0";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                }
            }
            catch
            {
                dashboardCardDataDto.DocumentCount = "0";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetGUDData(string channelID, string fromDate, string toDate, bool isPrevious)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();


            if (isPrevious)
            {
                toDate = fromDate;
                fromDate = "01/04/2018";
            }

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            if (!isPrevious)
            {
                sql = "select InvoiceNo As DocumentNo,SUM(ProdDocValueINR) as DocumentValue from GUDData " +
                "where ChannelID IN (select value from udf_SplitString(@channelID,','))   AND docopendate >= CONVERT(date, @fromDate, 103) " +
                "AND docopendate < Dateadd(DAY, 1, CONVERT(date, @toDate, 103)) AND (isnull(LRNum, '') = '' " +
                "OR LrDate> CONVERT(date, @toDate, 103)) and BillLadingDate is null group BY InvoiceNo ";
            }
            else
            {
                sql = "select InvoiceNo As DocumentNo,SUM(ProdDocValueINR) as DocumentValue from GUDData " +
                "where ChannelID IN (select value from udf_SplitString(@channelID,','))   AND docopendate >= CONVERT(date, @fromDate, 103) " +
                "AND docopendate < CONVERT(date, @toDate, 103) AND isnull(LRNum, '')='' and BillLadingDate is null " +
                "group BY InvoiceNo ";
            }

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "0";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                }
            }
            catch
            {
                dashboardCardDataDto.DocumentCount = "0";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetPCLedgerBalanceData(string channelID, string fromDate, string toDate)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            sql = "select Convert(Decimal(19,2),Sum(AccountTransaction.Amount)) DocumentValue,convert(varchar, max(AccTransDate), 103) as DocumentNo " +
                " from dileep.dbo.Accounts " +
                " inner join dileep.dbo.AccountTransaction on Accounts.AccountID = AccountTransaction.AccountID " +
                " where AccountCode = 'CBPCL' and Accounts.ChannelID in ('6359740391431204241') group by AccountName"; ;

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "Dr";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    //dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                    if (dashboardCardDataDto.DocumentSumValue < 0)
                    {
                        dashboardCardDataDto.DocumentCount = "Cr";
                    }
                    else
                    {
                        dashboardCardDataDto.DocumentCount = "Dr";
                    }
                    dashboardCardDataDto.AdditionalString = lstdashboardCardDetailsDto[0].DocumentNo.ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Abs(dashboardCardDataDto.DocumentSumValue);
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentCount = "Dr";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetPendingExportOrder(string channelID, string fromDate, string toDate, bool isPrevious)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();

            if (isPrevious)
            {
                fromDate = "01/04/2018";
                toDate = fromDate;
            }

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            if (!isPrevious)
            {
                sql = "select OrderNo As DocumentNo, SUM(PendingOrderValueINR) As DocumentValue from PendingExportOrderData " +
                "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND (docduedate>= CONVERT(date, @fromDate, 103) AND docduedate<Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) ) group by OrderNo";
            }
            else
            {
                sql = "select OrderNo As DocumentNo, SUM(PendingOrderValueINR) As DocumentValue from PendingExportOrderData " +
                "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND (docduedate>= CONVERT(date, @fromDate, 103) AND docduedate < CONVERT(date, @toDate, 103) ) group by OrderNo";
            }

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "0";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentCount = "0";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<DashboardCardDataDto> GetPendingExportSales(string channelID, string fromDate, string toDate)
        {
            DashboardCardDataDto dashboardCardDataDto = new DashboardCardDataDto();
            List<DashboardCardDetailsDto> lstdashboardCardDetailsDto = new List<DashboardCardDetailsDto>();

            SqlParameter[] arrSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            sql = "select InvoiceNo As DocumentNo,sum(ProdDocValueINR) AS DocumentValue " +
            "from ExportSalesData " +
            "where BILLLADINGDate >= CONVERT(date, @fromDate, 103) AND BILLLADINGDate<Dateadd(DAY, 1, CONVERT(date, @toDate, 103)) " +
            "AND (ChannelID in(select value from udf_SplitString(@channelID,',')) OR DelChannelID in(select value from udf_SplitString(@channelID,','))) " +
            "GROUP BY InvoiceNo ";

            DAL<DashboardCardDetailsDto> dal = new DAL<DashboardCardDetailsDto>();
            try
            {
                lstdashboardCardDetailsDto = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if (lstdashboardCardDetailsDto.Count() == 0)
                {
                    dashboardCardDataDto.DocumentCount = "0";
                    dashboardCardDataDto.DocumentSumValue = 0;
                }
                else
                {
                    dashboardCardDataDto.DocumentCount = lstdashboardCardDetailsDto.GroupBy(x => x.DocumentNo).Count().ToString();
                    dashboardCardDataDto.DocumentSumValue = Math.Round(lstdashboardCardDetailsDto.Sum(x => x.DocumentValue) / 10000000, 2);
                }
            }
            catch (Exception ex)
            {
                dashboardCardDataDto.DocumentCount = "0";
                dashboardCardDataDto.DocumentSumValue = 0;
            }
            return dashboardCardDataDto;
        }

        public async Task<object> GetColumnWiseSalesData(string channelID, string fromDate, string toDate, string dynamicColumns)
        {
            try
            {
                SqlParameter[] arrSqlParam = new SqlParameter[3]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@channelID",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = channelID
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@fromDate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = fromDate
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@toDate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = toDate
                    }
                };

                string sql = string.Empty;
                sql = "select #COLUMNS# ,convert(varchar(12), cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 4))) + 'Cr' AS Value " +
                "from ExportSalesData " +
                "where BILLLADINGDate >= CONVERT(date, @fromDate, 103) AND BILLLADINGDate < Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) " +
                "AND (ChannelID in(select value from udf_SplitString(@channelID,',')) " +
                "OR DelChannelID in(select value from udf_SplitString(@channelID,','))) " +
                "GROUP BY #COLUMNS# having cast(sum(ProdDocValueINR/ 10000000) AS decimal(19, 4))>0 ORDER BY #COLUMNS#";

                sql = sql.Replace("#COLUMNS#", dynamicColumns);

                DynamicClassBuilder dynamicClassBuilder = new DynamicClassBuilder("ExportSaleColumnWise");

                // Hardcoaded value is adding as it is specified in the sql with dynamic columns
                string columns = dynamicColumns + ",Value";

                object[] arrParamObject = new object[3] { sql, CommandType.Text, arrSqlParam };

                var records = await dynamicClassBuilder.CallMethodByReflection("GetRecords", columns, arrParamObject);

                return records;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<DashboardCardInfoDetailsDto>> GetDashboardCardDetailsByCardType(string channelID, string fromDate, string toDate, int cardType)
        {
            SqlParameter[] arrSqlParam = new SqlParameter[3]
                {
                    new SqlParameter()
                    {
                        ParameterName = "@channelID",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = channelID
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@fromDate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = fromDate
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@toDate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = toDate
                    }
                };

            SqlParameter[] arrChildSqlParam = new SqlParameter[3]
            {
                new SqlParameter()
                {
                    ParameterName = "@channelID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = channelID
                },
                new SqlParameter()
                {
                    ParameterName = "@fromDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = fromDate
                },
                new SqlParameter()
                {
                    ParameterName = "@toDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = toDate
                }
            };

            string sql = string.Empty;
            if (cardType == 1)
            {
                sql = "select CompanyName, convert(varchar(15),cast((sum(PendingOrderValueINR)/10000000) as decimal(19,2))) + 'Cr' as Value " +
                    "from(select CompanyName, OrderNo, DocOpenDate as OrderDate, BuyersOrderNo, BuyersOrderDate, CustomerName, " +
                    "MerchantName, Country, ProductNo, ProductName, BuyersProductNo, BuyersProductName, OrderQty, InvoicedQty, " +
                    "PendingQty, PAChannel, PAQty, DNQty, GUDQty, DocDueDate as DueDate, OrderValueFC, OrderValueINR, " +
                    "InvoicedValueFC, PendingOrderValueFC, PendingOrderValueINR, Currency, Status, CurrentExRate " +
                    "from PendingExportOrderData where ChannelID IN(select value from udf_SplitString(@channelID,',')) " +
                    "AND (docduedate>= CONVERT(date, @fromDate, 103) " +
                    "AND docduedate <Dateadd(DAY, 1, CONVERT(date, @toDate, 103)) )) as Main " +
                    "group by CompanyName having cast((sum(PendingOrderValueINR)/10000000) as decimal(19,2))>0";
            }
            else if (cardType == 5)
            {
                sql = "select DomasticSalesReport.ChannelID,ChannelName as CompanyName, " +
                    "convert(varchar(12),cast((sum(ProdAssessableValue)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value " +
                    "from DomasticSalesReport " +
                    "inner join Channels on DomasticSalesReport.ChannelID = Channels.ChannelID " +
                    "where DomasticSalesReport.ChannelID IN(select value from udf_SplitString(@channelID,',')) " +
                    "AND(AccTransDate >= CONVERT(date, @fromDate, 103) AND AccTransDate <= CONVERT(date, @toDate, 103)) " +
                    "AND ContactId not in(select CustomerID from EllementryGroupCustomer) " +
                    "GROUP BY DomasticSalesReport.ChannelID,ChannelName " +
                    "having cast((sum(ProdAssessableValue) / 10000000) AS decimal(19, 2)) > 0";
            }
            else if (cardType == 6)
            {
                sql = "select [SubChannel] as CompanyName,convert(varchar(12),cast(sum(a1.[Pending Domestic Orders]) as decimal(19,2))) + 'Cr' AS Value " +
                    "from (select isnull(b.ChannelName,Y.ChannelName) as [MainChannel],Y.ChannelName as [SubChannel],a.docid, " +
                    "a.DocNum,a.InvoiceDate,a.channelid,a.[Pending Domestic Orders],a.GrossAmount,a.docamount  from PendingDomesticOrder a " +
                    "inner join DileepSubDB.dbo.ChannelsHirarchy Y on rtrim(cast(a.ChannelId as nvarchar(22)))=rtrim(Y.ChannelId) " +
                    "left join Channels b on rtrim(cast(b.ChannelID as nvarchar(22)))=rtrim(Y.ChannelParentID) " +
                    "WHERE b.ChannelID in (select value from udf_SplitString(@channelID,',')) " +
                    "and convert(datetime,a.InvoiceDate,103) between CONVERT(date, @fromDate, 103) and CONVERT(date, @toDate, 103) " +
                    " AND ContactId not in(select CustomerID from EllementryGroupCustomer) " +
                    "group by b.ChannelName,Y.ChannelName,Y.ChannelName,a.docid,a.DocNum,a.InvoiceDate,a.channelid, " +
                    "a.[Pending Domestic Orders],a.GrossAmount,a.docamount ) a1 group by [SubChannel] ";
            }
            else if (cardType == 8)
            {
                sql = "select DomasticSalesReport.ChannelID,ChannelName as CompanyName, " +
                    "convert(varchar(12),cast((sum(ProdAssessableValue)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value " +
                    "from DomasticSalesReport " +
                    "inner join Channels on DomasticSalesReport.ChannelID = Channels.ChannelID " +
                    "where DomasticSalesReport.ChannelID IN(select value from udf_SplitString(@channelID,',')) " +
                    "AND(AccTransDate >= CONVERT(date, @fromDate, 103) " +
                    "AND AccTransDate <= CONVERT(date, @toDate, 103)) " +
                    "AND ContactId in(select CustomerID from EllementryGroupCustomer where CustomerType=2) " +
                    "GROUP BY DomasticSalesReport.ChannelID,ChannelName " +
                    "having cast((sum(ProdAssessableValue) / 10000000) AS decimal(19, 2)) > 0";
            }
            else if (cardType == 9)
            {
                sql = "select [SubChannel] as CompanyName,convert(varchar(12),cast(sum(a1.[Pending Domestic Orders]) as decimal(19,2))) + 'Cr' AS Value " +
                    "from (select isnull(b.ChannelName,Y.ChannelName) as [MainChannel],Y.ChannelName as [SubChannel],a.docid, " +
                    "a.DocNum,a.InvoiceDate,a.channelid,a.[Pending Domestic Orders],a.GrossAmount,a.docamount  from PendingDomesticOrder a   " +
                    "inner join DileepSubDB.dbo.ChannelsHirarchy Y on rtrim(cast(a.ChannelId as nvarchar(22)))=rtrim(Y.ChannelId) " +
                    "left join Channels b on rtrim(cast(b.ChannelID as nvarchar(22)))=rtrim(Y.ChannelParentID)   " +
                    "WHERE b.ChannelID in (select value from udf_SplitString(@channelID,','))  " +
                    "and convert(datetime,a.InvoiceDate,103) between CONVERT(date, @fromDate, 103) and CONVERT(date, @toDate, 103) " +
                    " AND ContactId in(select CustomerID from EllementryGroupCustomer where CustomerType=2) " +
                    "group by b.ChannelName,Y.ChannelName,Y.ChannelName,a.docid,a.DocNum,a.InvoiceDate,a.channelid, " +
                    "a.[Pending Domestic Orders],a.GrossAmount,a.docamount ) a1 group by [SubChannel] ";
            }
            else if (cardType == 2)
            {
                sql = "select ChannelID,CompanyName,convert(varchar(12), " +
                    "cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value, " +
                    "ChildChannel.ChildChannelCount As hasChildChannel  from GUDData " +
                    "Left Join(Select ChannelParentID, Count(*) As ChildChannelCount From Channels " +
                    "Group By ChannelParentID) ChildChannel On GUDData.ChannelID = ChildChannel.ChannelParentID " +
                    "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND docopendate >= CONVERT(date, @fromDate, 103) " +
                    "AND docopendate <Dateadd(DAY, 1, CONVERT(date, @toDate, 103)) AND (isnull(LRNum, '')='' OR LrDate> CONVERT(date, @toDate, 103)) and BillLadingDate is null " +
                    "group BY ChannelID,CompanyName, ChildChannel.ChildChannelCount " +
                    "having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))>0";
            }
            else if(cardType == 3)
            {
                sql = "select ChannelID,CompanyName,convert(varchar(12),cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value, "+
                    "ChildChannel.ChildChannelCount As hasChildChannel from GITData " +
                    "Left Join(Select ChannelParentID, Count(*) As ChildChannelCount From Channels " +
                    "Group By ChannelParentID) ChildChannel On GITData.ChannelID = ChildChannel.ChannelParentID " +
                    "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND LRDate >= CONVERT(date, @fromDate, 103) " +
                    "AND LRDate < Dateadd(DAY, 1, CONVERT(date, @toDate, 103)) AND (BillLadingDate IS NULL OR BillLadingDate > CONVERT(date, @toDate, 103)) and LRDate IS NOT NULL " +
                    "group BY ChannelID,CompanyName,ChildChannel.ChildChannelCount having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2)) > 0";
            }
            else if(cardType == 4)
            {
                sql = "select ChannelID,CompanyName, convert(varchar(12),cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value, " +
                "ChildChannel.ChildChannelCount As hasChildChannel from ExportSalesData " +
                "Left Join(Select ChannelParentID, Count(*) As ChildChannelCount From Channels " +
                "Group By ChannelParentID) ChildChannel On ISNULL(ExportSalesData.ChannelID,ExportSalesData.DelChannelID) = ChildChannel.ChannelParentID " +
                "where BILLLADINGDate >= CONVERT(date, @fromDate, 103) AND BILLLADINGDate < Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) " +
                "AND (ChannelID in(select value from udf_SplitString(@channelID,',')) OR DelChannelID in(select value from udf_SplitString(@channelID,','))) " +
                "GROUP BY ChannelID,CompanyName,ChildChannel.ChildChannelCount having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2)) > 0";

            }

            DAL<DashboardCardInfoDetailsDto> dal = new DAL<DashboardCardInfoDetailsDto>();
            try
            {
                var result = (await dal.GetRecords(sql, CommandType.Text, arrSqlParam)).ToList();

                if ((cardType == 2 || cardType == 3 || cardType == 4) && result.Count > 0)
                {
                    List<DashboardCardMoreInfoDetailsDto> childResult = new List<DashboardCardMoreInfoDetailsDto>();
                    if (cardType == 2)
                    {
                        sql = "select ChannelID, UnitName, convert(varchar(12), cast((sum(ProdDocValueINR) / 10000000) AS decimal(19, 2))) +'Cr' as Value from GUDData " +
                            "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND docopendate >= CONVERT(date, @fromDate, 103) " +
                            "AND docopendate < Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) AND (isnull(LRNum, '') = '' OR LrDate > CONVERT(date, @toDate, 103)) and BillLadingDate is null " +
                            "group BY ChannelID, UnitName having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))>0";


                    }
                    else if (cardType == 3)
                    {
                        sql = "select ChannelID, UnitName,convert(varchar(12),cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value from GITData " +
                            "where ChannelID IN (select value from udf_SplitString(@channelID,',')) AND LRDate >= CONVERT(date, @fromDate, 103) " +
                            "AND LRDate < Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) AND (BillLadingDate IS NULL OR BillLadingDate > CONVERT(date, @toDate, 103)) and LRDate IS NOT NULL " +
                            "group BY ChannelID,UnitName having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2)) > 0";
                    }
                    else if (cardType == 4)
                    {
                        sql = "select ISNULL(ChannelID,DelChannelID) As ChannelID, UnitName, convert(varchar(12),cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))) + 'Cr' as  Value " +
                            "from ExportSalesData " +
                            "where BILLLADINGDate >= CONVERT(date, @fromDate, 103) AND BILLLADINGDate<Dateadd(DAY, 1,CONVERT(date, @toDate, 103)) " +
                            "AND (ChannelID in(select value from udf_SplitString(@channelID,',')) OR DelChannelID in(select value from udf_SplitString(@channelID,','))) " +
                            "GROUP BY ISNULL(ChannelID,DelChannelID),UnitName having cast((sum(ProdDocValueINR)/ 10000000) AS decimal(19, 2))>0";
                    }
                    

                    DAL<DashboardCardMoreInfoDetailsDto> dalChild = new DAL<DashboardCardMoreInfoDetailsDto>();
                    childResult = (await dalChild.GetRecords(sql, CommandType.Text, arrChildSqlParam)).ToList();

                    foreach (var row in result)
                    {
                        if (row.hasChildChannel)
                        {
                            var childRows = childResult.Where(x => x.ChannelId == row.ChannelId).ToList();
                            if (childResult != null)
                            {
                                row.childInfo = childRows;
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}