var handsontableGetdata = {
    //"GetData": function (hotobj) {
    //    var dt1 = hotobj.getSourceData();
    //    //var xd = {};
    //    var xd = dt1.filter(function (item) {
    //        for (var key in dt1) {
    //            if (item[key] === null) { return false; }
    //            if (item[key] === '') { return 0; }
    //        }
    //        return true;
    //    });
    //    //$.each(dt1, function (rowKey, object) {
    //    //    if (!hotobj.isEmptyRow(rowKey)) xd[rowKey] = object;
    //    //});
       
    //    var ret = JSON.stringify(xd);
    //    console.log(ret);
    //    if ($.isEmptyObject(ret)) {
    //        return "";
    //    } else {
    //        return ret;
    //    }
    //}
};

function removeAllBlankOrNull(JsonObj) {
    $.each(JsonObj, function (key, value) {
        if (value === "" || value === null) {
            delete JsonObj[key];
        } else if (typeof (value) === "object") {
            JsonObj[key] = removeAllBlankOrNull(value);
        }
    });
    return JsonObj;
}
