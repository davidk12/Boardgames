
$(document).ready(function ()
{
    
    $("#Board-Box").corner("100px");
    //fartscroll(1);
    $("#Image-Div").corner("1000px");
    var lAudioNote = document.getElementById("calvin");
    $("#Sekrit-Div").hover(function ()
    {
        
        lAudioNote.play();
        $("#Image-Div").show("pulse");

    },
    function ()
    {
        lAudioNote.pause();
        //alert("lol");
        //document.getElementById('calvin').currentTime = 0;
        
        $("#Image-Div").hide("slow");

    });

    $("#Exit-Options-Div").click(function ()
    {
        $("#Options-Div").hide("fast");
    });
    

    $("#Options-Button").click(function ()
    {
        if ($("#Options-Div").is(':visible'))
        {
            $("#Options-Div").hide("fade");
            $("#Options-Button").attr('value', 'Show Options');
        }
        else
        {
            //$("#Chat-Box").hide("fade");
            
            $("#Options-Div").show("fade");
            $("#Options-Button").attr('value', 'Hide Options');
        }
    });

    $(".Board-Cell").hover(function ()
    {
        $(this).css("background-color", "red");
        if ($(this).val() == "")
        {
            $(this).css("cursor", "pointer");
        }

    }, function ()
    {
        $(this).css("cursor", "default");
        $(this).css("background-color", "pink");
    });
    
   


    $("#Options-Div").draggable();

});



var rotation = function ()
{
    $("#fruit").rotate({
        angle: 0,
        animateTo: 360,
        callback: rotation,
        easing: function (x, t, b, c, d)
        {        // t: current time, b: begInnIng value, c: change In value, d: duration
            return c * (t / d) + b;
        }
    });
}


rotation();
