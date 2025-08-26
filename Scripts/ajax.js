function GetAjaxArray(argsdata, filename,fieldname) {
    var myArray = [];

    $.ajax({
        type: 'POST',
        url: "data.asmx/" + filename,
        dataType: "json",
        data: argsdata,
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var mydata = $.parseJSON(data.d);

            $.each(mydata, function (i, j) {
                //console.log(j[fieldname]);
                myArray.push(j[fieldname]);

            });

        },
        error: function (jqxhr, status, error) {

            alert(error);
        }
    });

    return myArray;

}

function GetAjaxValue(argsdata, funcname, argname) {
    var retval = -1;
    var adata = "{'" + argname + "' : '" + argsdata + "'}";
    $.ajax({
        type: 'POST',
        url: "data.asmx/" + funcname,
        dataType: "json",
        data: adata,
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var mydata = $.parseJSON(data.d);
            retval = mydata;

        },
        error: function (jqxhr, status, error) {

            alert(error);
            retval = -1;

        }
    });
 //   alert(retval);
    return retval;

}