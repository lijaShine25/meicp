var SaveFunc = {
    'SaveObj': function () {
        var obj = new Object();
        obj.sale_dt = $('#sale_dt').val();
        obj.consideration = $('#consideration').val();
        obj.brokerage = $('#brokerage').val();
        obj.tds_receivable = $('#tds_receivable').val();
        obj.pay_mode = $('#pay_mode').val();
        obj.regn_date = $('#regn_date').val();
        obj.docs_provided = $('#docs_provided').val();
        obj.buyer_details = $('#buyer_details').val();

       
        var hot1 = $container1.data('handsontable');
        var hotData = hot1.getData();
        if (hotData === '[]')
            hotData = "";
        obj.payment_details = hotData;
        console.log(hotData);
        var frmdata = new FormData();
        frmdata.append("maindata", JSON.stringify(obj));
        frmdata.append("payment_dtls", JSON.stringify(hotData));

        return frmdata;
    }
};