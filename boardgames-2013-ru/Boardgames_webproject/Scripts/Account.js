
$(document).ready(function()
{
    $("#Terms-And-Conditions-Div").draggable();
    $("#Contact-Info-Div").draggable();

    $("#Terms-And-Conditions").click(function ()
    {
        $("#Terms-And-Conditions-Div").show();
    });

    $("#Close-Terms-Link").click(function ()
    {
        $("#Terms-And-Conditions-Div").hide();

    });


    $("#Contact-Info-Link").click(function ()
    {
        $("#Contact-Info-Div").show();
    });

    $("#Close-Info-Link").click(function ()
    {
        $("#Contact-Info-Div").hide();

    });





    });

