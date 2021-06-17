
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isNumber(evt, field) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (evt.keyCode == 46) {
        var patt1 = new RegExp("\\.");
        var ch = patt1.exec(field);
        if (ch == ".") {
            return false;

        }
    }
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


function DeleteRowOnUI(elementId, rowId) {
    document.getElementById(elementId).value = true;
    document.getElementById(rowId).style.display = 'none';
}