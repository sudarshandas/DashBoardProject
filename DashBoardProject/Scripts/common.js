function SetDateRangePicker(datecontrolID, IsPostBack) {
    var start = moment().subtract(29, 'days').format("DD/MM/YYYY");
    var end = moment().format("DD/MM/YYYY");
    var quarter = moment().quarter();
    var selecteddaterange;

    var date = new Date();
    var year = new Date().getFullYear();

    if (date.getMonth() <= 2) {
        year = date.getFullYear() - 1;
    }


    var financialYearStartDate = moment("04/01/" + year);// + new Date().getFullYear());//+new Date().getFullYear()
    var lastfinYearDate = moment(financialYearStartDate);
    if (IsPostBack == 0) {
        start = moment(financialYearStartDate, "DD/MM/YYYY"); //moment().quarter(quarter).startOf('quarter');
        end = moment(financialYearStartDate.add(1, 'year').subtract(1, 'days'), "DD/MM/YYYY"); //moment().quarter(quarter).endOf('quarter');
    }
    else {
        var str = $('#' + datecontrolID).val();
        selecteddaterange = str.split("-");
        start = moment(selecteddaterange[0], "DD/MM/YYYY");
        end = moment(selecteddaterange[1], "DD/MM/YYYY");
    }

    function cb(start, end) {
        $('#' + datecontrolID).val(start.format('DD/MM/YYYY') + '-' + end.format('DD/MM/YYYY'));
    }

    $('#' + datecontrolID + '').daterangepicker({
        startDate: start,
        endDate: end,
        "locale": {
            "format": "DD/MM/YYYY"
        },
        "opens": "center",
        "drops": "auto",
        ranges: {

            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'This Quarter': [moment().quarter(quarter).startOf('quarter'), moment().quarter(quarter).endOf('quarter')],
            'Last Quarter': [moment().subtract(1, 'quarter').startOf('quarter'), moment().subtract(1, 'quarter').endOf('quarter')],
            'This Financial Year': [moment(financialYearStartDate), moment(financialYearStartDate.add(1, 'year').subtract(1, 'days'))],
            'Last Financial Year': [moment(lastfinYearDate.subtract(1, 'year')), moment(lastfinYearDate.add(1, 'year').subtract(1, 'days'))]
        }
    }, cb);

    cb(start, end);
}
