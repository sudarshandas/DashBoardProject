function GetSelectedCompany(controlid) {
    var items = "";
    var x = document.getElementById(controlid);
    for (var i = 0; i < x.options.length; i++) {
        if (x.options[i].selected) {
            if (items == "") {
                items = x.options[i].value;
            }
            else {
                items += "," + x.options[i].value;
            }

        }
    }
    return items;
}

function GetSelectedColumns(controlid) {
    var items = "";
    var x = document.getElementById(controlid);
    for (var i = 0; i < x.options.length; i++) {
        if (x.options[i].selected) {
            if (items == "") {
                items = x.options[i].value;
            }
            else {
                items += "," + x.options[i].value;
            }

        }
    }
    return items;
}

function GetDateRangeValue(controlid) {
    return $('#' + controlid + '').val();;
}

function capitalizeFirstLetter(string) {
    return (string.charAt(0).toUpperCase() + string.slice(1)).replace(/([A-Z])/g, ' $1').trim();
}

//Bind dash board data
function BindDashBoardData() {
    debugger;
    var selectedcompany = GetSelectedCompany('ChannelID');
    var daterange = GetDateRangeValue('spndaterangevalue');

    //var parameter = { "company": selectedcompany, "daterange": daterange };
    $('#divloading').addClass('loading');
    $.ajax({
        type: "GET",
        url: "/Home/DashboardCards",
        async: true,
        data: { company: selectedcompany, daterange: daterange },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            $('#divloading').addClass('loading');
            //console.log(response);

            var exportOrder = response.filter(obj => {
                return obj.CardType === 1 && obj.IsPreviousFY == false
            })
            $('#pendingExportOrderValue').html(exportOrder[0].DocumentSumValue);
            if (exportOrder[0].DocumentSumValue != "0") {
                $('#pendingExportOrderValue').append('<sup>Cr</sup>');
            }
            $('#pendingExportOrderCount').html(exportOrder[0].DocumentCount);

            var prevexportOrder = response.filter(obj => {
                return obj.CardType === 1 && obj.IsPreviousFY == true
            })
            $('#prevpendingExportOrderValue').html(prevexportOrder[0].DocumentSumValue);
            if (prevexportOrder[0].DocumentSumValue != "0") {
                $('#prevpendingExportOrderValue').append('<sup>Cr</sup>');
            }
            $('#pendingExportOrderCount').html(exportOrder[0].DocumentCount);

            var GUDData = response.filter(obj => {
                return obj.CardType === 2 && obj.IsPreviousFY == false
            })
            $('#GUDValue').html(GUDData[0].DocumentSumValue);
            if (GUDData[0].DocumentSumValue != "0") {
                $('#GUDValue').append('<sup>Cr</sup>');
            }
            $('#GUDCount').html(GUDData[0].DocumentCount);

            var prevGUDData = response.filter(obj => {
                return obj.CardType === 2 && obj.IsPreviousFY == true
            })
            $('#prevGUDValue').html(prevGUDData[0].DocumentSumValue);
            if (prevGUDData[0].DocumentSumValue != "0") {
                $('#prevGUDValue').append('<sup>Cr</sup>');
            }
            $('#prevGUDCount').html(prevGUDData[0].DocumentCount);

            var GITData = response.filter(obj => {
                return obj.CardType === 3 && obj.IsPreviousFY == false
            })
            $('#GITValue').html(GITData[0].DocumentSumValue);
            if (GITData[0].DocumentSumValue != "0") {
                $('#GITValue').append('<sup>Cr</sup>');
            }
            $('#GITCount').html(GITData[0].DocumentCount);

            var prevGITData = response.filter(obj => {
                return obj.CardType === 3 && obj.IsPreviousFY == true
            })
            $('#prevGITValue').html(prevGITData[0].DocumentSumValue);
            if (prevGITData[0].DocumentSumValue != "0") {
                $('#prevGITValue').append('<sup>Cr</sup>');
            }
            $('#prevGITCount').html(prevGITData[0].DocumentCount);

            var ExportSales = response.filter(obj => {
                return obj.CardType === 4 && obj.IsPreviousFY == false
            })
            $('#ExportSalesValue').html(ExportSales[0].DocumentSumValue);
            if (ExportSales[0].DocumentSumValue != "0") {
                $('#ExportSalesValue').append('<sup>Cr</sup>');
            }
            $('#ExportSalesCount').html(ExportSales[0].DocumentCount);

            var DomesticSales = response.filter(obj => {
                return obj.CardType === 5 && obj.IsPreviousFY == false
            })
            $('#DomesticSalesValue').html(DomesticSales[0].DocumentSumValue);
            if (DomesticSales[0].DocumentSumValue != "0") {
                $('#DomesticSalesValue').append('<sup>Cr</sup>');
            }
            $('#DomesticSalesCount').html(DomesticSales[0].DocumentCount);

            var DomesticPendingOrder = response.filter(obj => {
                return obj.CardType === 6 && obj.IsPreviousFY == false
            })
            $('#DomesticPendingOrderValue').html(DomesticPendingOrder[0].DocumentSumValue);
            if (DomesticPendingOrder[0].DocumentSumValue != "0") {
                $('#DomesticPendingOrderValue').append('<sup>Cr</sup>');
            }
            $('#DomesticPendingOrderCount').html(DomesticPendingOrder[0].DocumentCount);

            var PCLedgerBalance = response.filter(obj => {
                return obj.CardType === 7 && obj.IsPreviousFY == false
            })
            $('#PCLedgerBalanceValue').html(PCLedgerBalance[0].DocumentSumValue);
            if (PCLedgerBalance[0].DocumentSumValue != "0") {
                $('#PCLedgerBalanceValue').append('<sup>Cr</sup>');
            }
            $('#PCLedgerBalanceCount').html(PCLedgerBalance[0].DocumentCount);
            $('#spnPCLedgerLastTransDate').html(PCLedgerBalance[0].AdditionalString);

            var DomesticSaleEllementry = response.filter(obj => {
                return obj.CardType === 8 && obj.IsPreviousFY == false
            })
            $('#DomesticSaleEllementryValue').html(DomesticSaleEllementry[0].DocumentSumValue);
            if (DomesticSaleEllementry[0].DocumentSumValue != "0") {
                $('#DomesticSaleEllementryValue').append('<sup>Cr</sup>');
            }
            $('#DomesticSaleEllementryCount').html(DomesticSaleEllementry[0].DocumentCount);

            var DomesticPendingOrderEllementry = response.filter(obj => {
                return obj.CardType === 9 && obj.IsPreviousFY == false
            })
            $('#DomesticPendingOrderEllementryValue').html(DomesticPendingOrderEllementry[0].DocumentSumValue);
            if (DomesticPendingOrderEllementry[0].DocumentSumValue != "0") {
                $('#DomesticPendingOrderEllementryValue').append('<sup>Cr</sup>');
            }
            $('#DomesticPendingOrderEllementryCount').html(DomesticPendingOrderEllementry[0].DocumentCount);
            //onSuccessBindDashBoardData(response.d, "spnPendingOrderValue", "spnPendingOrderCount", "ContentPlaceHolderBody_hfPendingOrderValue", "ContentPlaceHolderBody_hfPendingOrderCount");

            $('#divloading').removeClass('loading');
            DisplayDashBoardSumValue();
        },
        error: function (response) {
            //ShowPopup('Error in Operation', 'Error in Operation - ' + response, 'PopUpMessage');
            //console.log('Error in Operation - ' + response);
            $('#divloading').removeClass('loading');
        },
        complete: function () {
            //console.log("Order Data Fetching Complete: " + new Date($.now()));
            //handleComplete();
        }
    });



    //$('#ExportSales').empty();
    //$('#ExportSales tbody').empty();
    $('#divloading').addClass('loading');
    var selectedColumns = GetSelectedColumns('columns-selection');
    var selectedchannelId = GetSelectedCompany('ExportSalesChannelID');
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: '/Home/ColumnWiseSalesData',
        data: { company: selectedchannelId, daterange: daterange, columns: selectedColumns },
        success: function (data) {
            $('#divloading').addClass('loading');
            if ($.fn.DataTable.fnIsDataTable($('#ExportSales'))) {
                $('#ExportSales').DataTable().destroy();
                $('#ExportSales').find('thead').remove();
                $('#ExportSales').find('tbody').remove();

            }
            //console.log(data);

            var columns = [];
            //data = JSON.parse(data);
            var columnNames = Object.keys(data.data[0]);
            //console.log(columnNames);
            for (var i in columnNames) {
                columns.push({
                    data: columnNames[i],
                    title: capitalizeFirstLetter(columnNames[i])
                });
            }
            //console.log(columns);
            //console.log(data.data);
            $('#ExportSales').DataTable({
                fixedHeader: true,
                data: data.data,
                columns: columns
            });

            $('#ExportSales').find('thead').addClass("bg-orange");

            $('#divloading').removeClass('loading');
        }
    });
}

function DisplayDashBoardSumValue() {
    var dblSumValue = 0;
    var strValue = "";
    debugger;

    if (document.getElementById("chkPendingExportOrder").checked == true) {
        if ($('#pendingExportOrderValue').text() != "0") {
            strValue = $('#' + "pendingExportOrderValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkGUD").checked == true) {
        if ($('#GUDValue').text() != "0") {
            strValue = $('#' + "GUDValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkGIT").checked == true) {
        if ($('#GITValue').text() != "0") {
            strValue = $('#' + "GITValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkExportSales").checked == true) {
        if ($('#ExportSalesValue').text() != "0") {
            strValue = $('#' + "ExportSalesValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkDomesticSales").checked == true) {
        if ($('#DomesticSalesValue').text() != "0") {
            strValue = $('#' + "DomesticSalesValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkDomesticPendingOrder").checked == true) {
        if ($('#DomesticPendingOrderValue').text() != "0") {
            strValue = $('#' + "DomesticPendingOrderValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkDomesticSaleEllementry").checked == true) {
        if ($('#DomesticSaleEllementryValue').text() != "0") {
            strValue = $('#' + "DomesticSaleEllementryValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (document.getElementById("chkDomesticPendingOrderEllementry").checked == true) {
        if ($('#DomesticPendingOrderEllementryValue').text() != "0") {
            strValue = $('#' + "DomesticPendingOrderEllementryValue").text();
            dblSumValue += parseFloat(strValue);
        }
    }

    if (dblSumValue > 0) {
        document.getElementById("dashBoardTotal").innerText = "Total Value: " + dblSumValue.toFixed(2) + "Cr";
    }
    else {
        document.getElementById("dashBoardTotal").innerText = "";
    }

}

function ShowDetailsByCardType(cardType, modalTitleText) {
    $('#divloading').addClass('loading');
    var selectedcompany = GetSelectedCompany('ChannelID');
    var daterange = GetDateRangeValue('spndaterangevalue');
    $.ajax({
        type: "GET",
        url: "/Home/DashboardDataDetails",
        data: { company: selectedcompany, daterange: daterange, cardType: cardType },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //console.log(response);
            if ($.fn.DataTable.fnIsDataTable($('#moreInfo'))) {
                $('#moreInfo').DataTable().destroy();
                $('#moreInfo').find('tbody').remove();
            }

            if ($.fn.DataTable.fnIsDataTable($('#moreInfo-with-child'))) {
                $('#moreInfo-with-child').DataTable().destroy();
                $('#moreInfo-with-child').find('tbody').remove();
            }

            if (cardType == 2 || cardType == 3 || cardType == 4) {
                $("#moreInfo").addClass('d-none');
                $("#moreInfo-with-child").removeClass('d-none');

                var table = $("#moreInfo-with-child").DataTable({
                    searching: false,
                    paging: false,
                    data: response,
                    columns: [
                        {
                            "className": 'details-control',
                            "orderable": false,
                            'data': null,
                            "defaultContent": ''
                        },
                        { 'data': 'CompanyName' },
                        { 'data': 'Value' }
                    ],
                    "order": [[1, 'asc']]
                });
                
                $('#moreInfo-with-child tbody td.details-control').each(function () {
                    debugger;
                    var tr = $(this).closest('tr');
                    var row = table.row(tr);

                    if (row.data().hasChildChannel == false) {
                        tr.removeClass('details-control');
                        tr.children('td:first').removeClass('details-control');
                    }
                });

                $('#moreInfo-with-child tbody').on('click', 'td.details-control', function () {
                    debugger;
                    var tr = $(this).closest('tr');
                    var row = table.row(tr);

                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('shown');
                    }
                    else {
                        // Open this row
                        var rowid = row.data().ChannelId;
                        row.child(format(rowid)).show();
                        tr.addClass('shown');

                        var childData = row.data().childInfo;
                        $('table#' + rowid).DataTable({
                            data: childData,
                            paging: false,
                            info: false,
                            searching: false,
                            "columns": [
                                { "data": "UnitName" },
                                { "data": "Value" },
                            ]
                        });

                        $('#' + rowid).find('thead').addClass("bg-info text-white");
                    }
                });

                $('#moreInfo-with-child').find('thead').addClass("bg-primary text-white");
            }
            else {
                $("#moreInfo").removeClass('d-none');
                $("#moreInfo-with-child").addClass('d-none');

                $("#moreInfo").DataTable({
                    paging: false,
                    data: response,
                    columns: [
                        { 'data': 'CompanyName' },
                        { 'data': 'Value' }
                    ],
                    "order": [[1, 'asc']]
                });
            }

            $('#moreInfo').find('thead').addClass("bg-primary text-white");
        },
        failure: function (response) {
            $('#divloading').removeClass('loading');
            //alert(response);
        },
        error: function (response) {
            $('#divloading').removeClass('loading');
            //alert(response);
        },
        complete: function () {
            $('#divloading').removeClass('loading');
            $('#moreInfoModal').modal('show');
            var moreInfoModal = document.getElementById('moreInfoModal')

            var modalTitle = moreInfoModal.querySelector('.modal-title');
            modalTitle.textContent = modalTitleText;
        }
    });
}

function format(table_id) {
    return '<table id="' + table_id + '" class="display table table-bordered table-striped table-hover jQuery-datatable border-info" border="0" style="width:100%;">' +
        '<thead>' +
        '<th>Unit Name</th>' +
        '<th>Value</th>' +
        '</thead>' +
        '</table>';
}