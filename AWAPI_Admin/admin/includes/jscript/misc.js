Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);



function beginRequestHandler(sender, args) {
    var dv = document.getElementById("ajaxLoaderContainer");
    if (dv != null)
        dv.style.display = "";

}

function endRequestHandler(sender, args) {
    //Handler Error
    document.getElementById("ajaxError").style.display = "none";
    if (args.get_error() != undefined) {
        var errorMessage;
        if (args.get_response().get_statusCode() == '200') {
            errorMessage = args.get_error().message;
        }
        else {
            // Error occurred somewhere other than the server page.
            errorMessage = 'An unspecified error occurred. ';
        }
        args.set_errorHandled(true);
        //alert(errorMessage);
        document.getElementById("ajaxError").style.display = "";
        document.getElementById("ajaxErrorMessage").innerHTML = errorMessage;

        //ToggleAlertDiv('visible');
        //$get(messageElem).innerHTML = errorMessage;
    }

    //Close Ajax Loader
    try {
        var dv = document.getElementById("ajaxLoaderContainer");
        if (dv != null)
            dv.style.display = "none";

        //setLightboxToLinks();
        initTinyMCE();

        tinyMCE.triggerSave(false, true);

        if (args.get_error() != undefined) {
            var eMsg = args.get_error().message;
            args.set_errorHandled(true);
            $get(messageElem).innerHTML = eMsg;
        }
    }
    catch (e) {
    }
}


function closeColorBox(refresh) {
    if (refresh)
        document.location = document.location;
    else
        $.fn.colorbox.close();
}

function showHideDiv(divId) {
    var obj = document.getElementById(divId);
    if (obj == null)
        return;

    if (obj.style.display == "")
        obj.style.display = "none";
    else
        obj.style.display = "";
}

function StartsWith(source, find) {
    return (source.match("^" + find) == find);
}

function EndsWith(source, find) {
    return (source.match(find + "$") == find)
}

function TagAutoComplete(values, targetId, seperator) {
    if (seperator == null)
        seperator = ",";
        
    var data = values.split(seperator);
    $('#' + targetId).autocomplete(data, 
        { width: 320, max: 10, multiple: true, multipleSeparator: seperator, scroll: true, scrollHeight: 300 });
}


function AddTag(tagName, targetId, seperator) {
    var currentValue = jQuery.trim($("#" + targetId).val().toLowerCase());
    tagName = jQuery.trim(tagName.toLowerCase());

    if (currentValue == "") {
        $("#" + targetId).val(tagName);
        return;
    }

    if (seperator == null || seperator == "undefined")
        seperator = ",";
    var tags = currentValue.split(seperator);
    var n = 0;
    var alreadyExist = false;
    for (n = 0; n < tags.length; n++) {
        if (jQuery.trim(tags[n]) == tagName) {
            alreadyExist = true;
            break;
        }
    }
    if (!alreadyExist)
        if (EndsWith(currentValue, ","))
            currentValue += tagName;
        else
            currentValue += "," + tagName;

    $("#" + targetId).val(currentValue);
}

function setElementFromCheckBoxList(spanName, listId, targetId, seperator) {
    var tagNames = $("span[name=" + spanName + "]");
    var target = document.getElementById(targetId);
    var sep = ";";

    if (seperator != null)
        sep = seperator;

    if (tagNames == null || tagNames.length == 0 || tagNames == null)
        return;

    target.value = "";
    var n = 0;

    for (n = 0; n < tagNames.length; n++) {
        var cbId = listId + "_" + n;
        var cb = document.getElementById(cbId);
        if (cb != null && cb.checked)
            target.value += tagNames[n].getAttribute("tag") + seperator;
    }
}

//Update alias text ------------------------------------------------------
//$(document).ready(function() {
//    $("input[rel=alias]").change(function() {
//        alert(1);
//        var text = "";
//        var arr = $(this).val()
//        $.each(arr, function(i) {
//            var c = arr.charCodeAt(i);
//            if ((c == ' ') ||
//                (c == '"') ||
//                (c == '\'') ||
//                (c == '&') ||
//                (c == '?')) {
//                text += '_';
//            }
//            else
//                text += arr.charAt(i);
//        });
//        $(this).val(text);
//    });
//});


//SET ALIAS
//alias doesn't except spaces or non alphanumberic characters
//Replaces non alphanumeric values with dash
//Also, if alias is empty then set the alias from the title,,,
function aliasNameTrigger(alias, title) {
    var aliasId = "#" + alias;
    var titleId = "#" + title;

    if ($(aliasId).length) {
        if ($(titleId).length) {
            $(titleId).change(function() {
                if ($(aliasId).val() == null || $(aliasId).val() == '') {
                    var Text = jQuery.trim($(this).val());
                    Text = Text.toLowerCase();
                    Text = Text.replace(/[^a-zA-Z0-9]+/g, '-');
                    $(aliasId).val(Text);
                }
            });
        }

        $(aliasId).keyup(function() {
            var Text = $(this).val();
            Text = Text.toLowerCase();
            Text = Text.replace(/[^a-zA-Z0-9]+/g, '-');
            $(aliasId).val(Text);
        });
    }
}